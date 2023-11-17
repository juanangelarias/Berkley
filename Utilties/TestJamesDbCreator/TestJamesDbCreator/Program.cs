// See https://aka.ms/new-console-template for more information

using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Security.AccessControl;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Smo;
using TestJamesDbCreator;
using static TestJamesDbCreator.WindowsShareHelper;
using static TestJamesDbCreator.SqlHelper;

bool SetupShare = false,
    backupDb = false,
    restoreDb = true,
    updateForeignKeys = true,
    deleteData = true,
    scriptData = true;

Console.WriteLine("Hello, World!  We are going to create a new, minimal JamesDb for testing.");

Debug.Assert(ConfigurationManager.AppSettings["LocalRestoreFolder"] != null, "App.config must contain \"LocalRestoreFolder\" key.");
Debug.Assert(ConfigurationManager.AppSettings["BackupNetworkFolder"] != null, "App.config must contain \"BackupNetworkFolder\" key.");

const string BackupFileName = "testdb.bak";
var backupFileDir = new DirectoryInfo(ConfigurationManager.AppSettings["LocalRestoreFolder"]!);
var networkShare = ConfigurationManager.AppSettings["BackupNetworkFolder"]!;
var shareName = networkShare.Split('\\').Last();
var snapDbName = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["SnapDestination"].ConnectionString)
    .InitialCatalog;
var serverSmo = new Server(GetDestinationServerName());
//Set up share directory for backup to be made to
//NOTE:Must be run as admin of local machine
if (SetupShare)
{
    //UNDONE:  Wasn't able to get this to work quickly, so just moved on.
    backupFileDir.Create();
    var accessControl = backupFileDir.GetAccessControl();

    if (null == WindowsShare.GetShareByName(shareName))
        ShareFolder(backupFileDir.FullName, shareName, "Temporary share to snapshot database.");
}

if (backupDb)
{
    BackupSourceDb(networkShare, BackupFileName);
}

if (restoreDb)
{
    Console.WriteLine("Beginning Db restore to mssqllocaldb.");
    RestoreSnapDb(backupFileDir, snapDbName);
}
if (updateForeignKeys)
{
    Console.WriteLine("Beginning to update foreign keys to ON DELETE CASCADE.");
    UpdateForeignKeys(serverSmo);
}

if (deleteData)
{
    Console.WriteLine("Shrinking dataset");

    DeleteExcessData();
}

if (scriptData)
    ScriptData(serverSmo);

void BackupSourceDb(string fileshare, string backupFileName)
{
    //Backup James Dev
    using var sourceConnection = GetSourceConnection("Master");
    using var backupCommand =
        new SqlCommand($"BACKUP DATABASE {GetSourceDbName()} TO DISK = '{fileshare}\\{backupFileName}' WITH INIT;",
            sourceConnection);
    sourceConnection.Open();
    backupCommand.ExecuteNonQuery();
}

void RestoreSnapDb(DirectoryInfo directoryInfo, string snapDbName1)
{
    string s;
    var sqlDataFileDir = ConfigurationManager.AppSettings["SqlDataFilesDirectory"]!;
    Directory.CreateDirectory(sqlDataFileDir);
    //TODO:Restore locally
    using var destinationConnection = GetDestinationConnection("Master");
    var restoreFilename = Path.Combine(directoryInfo.FullName, BackupFileName);
    using var dropConnectionsCmd = new SqlCommand(@"DECLARE @kill varchar(8000) = '';  
SELECT @kill = @kill + 'kill ' + CONVERT(varchar(5), session_id) + ';'  
FROM sys.dm_exec_sessions
WHERE database_id  = db_id('" + snapDbName1 + @"')
EXEC(@kill);", destinationConnection);
    destinationConnection.Open();
    dropConnectionsCmd.ExecuteNonQuery();
    using var restoreCommand = new SqlCommand($"RESTORE FILELISTONLY FROM DISK='{restoreFilename}';", destinationConnection);
    var dbFiles = new Dictionary<string, string>();
    using (var sdr = restoreCommand.ExecuteReader())
        while (sdr.Read())
            dbFiles[sdr.GetString(0)] = sdr.GetString(1);

    Func<string, string> newFileLocation =
        oldFileLocation => Path.Combine(sqlDataFileDir, Path.GetFileName(oldFileLocation));
    restoreCommand.CommandText =
        $"RESTORE DATABASE {snapDbName1} FROM DISK ='{restoreFilename}' WITH {string.Join(", ", dbFiles.Select(kvp => $"MOVE '{kvp.Key}' TO '{newFileLocation(kvp.Value)}'"))}, REPLACE";
    restoreCommand.ExecuteNonQuery();
}

void UpdateForeignKeys(Server serverSmo)
{
    //Convert all FK to delete cascade
    //var serverSmo = new Server(GetDestinationServerName());
    var dbSmo = new Database(serverSmo, GetDestinationDbName());
    dbSmo.Refresh();
    var relevantSchemas = new string[] { "dbo", "beta", "deleted", "basis" };
    var tablesSmo = dbSmo.Tables.OfType<Table>()
        .Where(t => relevantSchemas.Contains(t.Schema, StringComparer.InvariantCultureIgnoreCase)).ToArray();
    Console.WriteLine($"Found {tablesSmo.Length} tables");
    var foreignKeysSmo = tablesSmo.SelectMany(t => t.ForeignKeys.OfType<ForeignKey>()
        //Ignore self referencing table foreign keys and any foreign keys already set as on delete cascade
        .Where(fk => fk.DeleteAction != ForeignKeyAction.Cascade && fk.Parent.Name != fk.ReferencedTable)
        .Cast<ForeignKey>().Select(fk =>
        {
            fk.DeleteAction = ForeignKeyAction.Cascade;
            return fk;
        })).ToArray();
    var scriptOptions = new ScriptingOptions() { ScriptDrops = true, SchemaQualify = true };
    Console.WriteLine($"Starting scripting of {foreignKeysSmo.Length} foreign keys");
    var scripter = new ScriptMaker(serverSmo);
    var scripts = scripter.Script(foreignKeysSmo).OfType<string>().ToArray();

    var scriptsToShow = 10;
    Console.WriteLine($"First {scriptsToShow} scripts:\r\n{string.Join("\r\n\r\n", scripts.Take(scriptsToShow))}");
    Console.WriteLine("---------------------------------");
    Debug.Assert(foreignKeysSmo.Length * 2 == scripts.Length);
    using var sqlConn = GetDestinationConnection();
    sqlConn.Open();
    for (var i = 0; i < foreignKeysSmo.Length; i++)
    {
        var fk = foreignKeysSmo[i];
        if (fk.Name == "FK_Account_LawEntity_LawFirm")
        {
            continue;
        }

        fk.Drop();
        //fkNew.DeleteAction = ForeignKeyAction.Cascade;
        var createFk = new SqlCommand(scripts[i * 2] + "\r\nON DELETE CASCADE;\r\n" + scripts[i * 2 + 1], sqlConn);
        try
        {
            createFk.ExecuteNonQuery();
            Console.WriteLine("Added ON DELETE CASCADE to " + fk.Name);
        }
        catch (SqlException ex) when (ex.Message.Contains("may cause cycles or multiple cascade paths. "))
        {
            Console.WriteLine($"Skipping {fk.Name} to prevent a cascade path.");
            createFk.CommandText = scripts[i * 2] + ";\r\n" + scripts[i * 2 + 1];
            createFk.ExecuteNonQuery();
        }
    }
}

void DeleteExcessData()
{

    using var sqlConn = GetDestinationConnection();
    //Remove tables from unneeded schemas
    Console.WriteLine("Removing unneeded schemas and unneeded metadata");
    var unneededSchemas = new[] { "Stage", "Data", "Sync" };
    using var deleteSchemaTablesCmd = new SqlCommand(SqlDeleteSchemaTables, sqlConn);
    deleteSchemaTablesCmd.Parameters.Add("@SchemaName", SqlDbType.NVarChar, 128);
    if (sqlConn.State != ConnectionState.Open) sqlConn.Open();
    foreach (var schema in unneededSchemas)
    {
        deleteSchemaTablesCmd.Parameters["@SchemaName"].Value = schema;
        deleteSchemaTablesCmd.ExecuteNonQuery();
    }

    //Remove Basis table metadata
    using var removeBasisMetadataCmd = new SqlCommand("DELETE FROM Basis.DatabaseInfo", sqlConn);
    removeBasisMetadataCmd.ExecuteNonQuery();

    //Prune Accounts
    Console.WriteLine("Pruning Accounts");
    using var accountPruneCmd = new SqlCommand(SqlHelper.SqlReduceAccounts, sqlConn)
    { CommandType = CommandType.Text, CommandTimeout = 90 };
    accountPruneCmd.ExecuteNonQuery();

    //Prune Bonds
    Console.WriteLine("Pruning bonds");
    using var bondPruneCmd = new SqlCommand(SqlHelper.SqlReduceBonds, sqlConn) { CommandType = CommandType.Text };
    //if (sqlConn.State != ConnectionState.Open) sqlConn.Open();
    bondPruneCmd.ExecuteNonQuery();

    //TODO:Can get rid of after this is added into the backup file
    Console.WriteLine("Adding IdTable type");
    using var createIdTableTypeCmd = new SqlCommand(SqlCreateIdTableType, sqlConn)
    { CommandType = CommandType.Text };
    createIdTableTypeCmd.ExecuteNonQuery();


    Console.WriteLine("Pruning agents and agencies");
    using var agentAgencyPruneCmd = new SqlCommand(SqlPruneAgentAgency, sqlConn) { CommandType = CommandType.Text };
    agentAgencyPruneCmd.ExecuteNonQuery();

    Console.Write("Pruning attorneys and law firms");
    //NOTE: This is untested because attorneys and law firms aren't being ported over, yet.
    using var lawEntityPruneCmd = new SqlCommand(SqlPruneLawEntities, sqlConn) { CommandType = CommandType.Text };
    lawEntityPruneCmd.ExecuteNonQuery();

    Console.WriteLine("Pruning misc tables");
    using var miscPruneCmd = new SqlCommand(SqlPruneMiscData, sqlConn) { CommandType = CommandType.Text };
    miscPruneCmd.ExecuteNonQuery();

    Console.WriteLine("Pruning orphaned legal entities and obligee info");
    using var orphanPruneCmd = new SqlCommand(SqlPruneOrphanedAccounts + "\r\n" + SqlPruneObligeeInfo, sqlConn);
    orphanPruneCmd.ExecuteNonQuery();

    Console.WriteLine("Pruning address and phone number tables");
    using var addressPhonePruneCmd = new SqlCommand(SqlPruneAddressesAndPhoneNumbers, sqlConn)
    { CommandType = CommandType.Text };
    addressPhonePruneCmd.ExecuteNonQuery();
}

void ScriptData(Server serverSmo)
{
    var dbSmo = serverSmo.Databases[GetDestinationDbName()];
    dbSmo.Tables.Refresh();
    //Get table hierarchy
    var hierarchyTable = new DataTable();
    using var sqlConn = GetDestinationConnection();
    using var GetHierarchyCmd = new SqlCommand(SqlTableStructureQuery, sqlConn)
    { CommandType = CommandType.Text };
    using var sda = new SqlDataAdapter(GetHierarchyCmd);
    sda.Fill(hierarchyTable);

    //TODO:Delete down to small dataset
    var hierarchyView = new DataView(hierarchyTable, "schemaName in ('dbo', 'Beta')", "level ASC",
        DataViewRowState.CurrentRows);
    var orderedTables = new List<SqlSmoObject>();
    foreach (DataRowView table in hierarchyView)
        if (table["tableName"].ToString() != "MSchange_tracking_history")
        {
            var tbl = dbSmo.Tables.OfType<Table>().First(t =>
                t.Name == table["tableName"].ToString() && t.Schema == table["schemaName"].ToString());
            tbl.Refresh();
            orderedTables.Add(tbl);
        }
    var scriptDirectory = ConfigurationManager.AppSettings["ScriptsDirectory"]!;
    Directory.CreateDirectory(scriptDirectory);
    var scriptFileName = Path.Combine(scriptDirectory, "InsertData.sql");
    var fi = new FileInfo(scriptFileName);
    if (fi.Exists)
    {
        fi.IsReadOnly = false;
        fi.Delete();
    }
    Console.WriteLine("Data will be scripted to " + scriptFileName);
    var scriptOptions = new ScriptingOptions
    {
        ScriptData = true,
        ScriptBatchTerminator = false,
        ScriptDrops = false,
        ScriptDataCompression = false,
        ScriptForAlter = false,
        ScriptForCreateDrop = false,
        ScriptForCreateOrAlter = false,
        ScriptOwner = false,
        ScriptSchema = false,
        FileName = scriptFileName,
        AppendToFile = true,
        ToFileOnly = false
    };
    var scripter = new ScriptMaker(serverSmo, scriptOptions);
    //Get tables to script data far
    //var schemasToScript = new[] { "dbo", "beta" };
    //var tablesToScript = dbSmo.Tables.OfType<Table>()
    //    .Where(t => schemasToScript.Contains(t.Schema, StringComparer.InvariantCultureIgnoreCase))
    //    .ToArray();

    var tablesScripted = 0;
    var batchSize = 10;
    using (var fs = new FileStream(scriptFileName, FileMode.Create))
    using (var sw = new StreamWriter(fs))
        while (tablesScripted < orderedTables.Count)
        {
            var scripts = scripter.Script(orderedTables.Skip(tablesScripted).Take(batchSize).ToArray())
                .OfType<string>().ToArray();
            foreach (var script in scripts)
                sw.WriteLine(script);
            tablesScripted += batchSize;
        }
    Console.WriteLine("Initial script generation complete.");
    //Console.WriteLine("Scripts:\r\n\r\n" + string.Join("\r\n\r\n", scripts));
    //TODO:Clean up scripts

}