using System.Diagnostics;
using System.Reflection;
using James.Shared;
using James.Shared.Model;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace James.Data.Server.Model
{
    public partial class JamesDatabaseContext
    {
        private bool _seedSnapshot = false;
        private bool _seedTestData = false;
        //private string _connectionString 
        //    = ConfigurationHelper.ConfigGetConnectionStringByName("James");

        //HACK:  Moving this to private for use in pooled connections
        //TODO:  Figure out how to use this with unit test.
        //UNDONE:  Getting connection pooling to work has probably broken this
        //NOTE:  Commenting this out during POC work 2/1/24, knowing this breaks our plan for unit testing.  DO NOT REMOVE
        //protected JamesDatabaseContext(string testDbName, bool seedSnapshot, bool seedTestData) :
        //    this((DbContextOptionsBuilder)( option =>
        //    {
        //        option.EnableDetailedErrors();
        //        option.UseSqlServer(new SqlConnectionStringBuilder
        //        {
        //            DataSource = "(localdb)\\mssqllocaldb",
        //            InitialCatalog = testDbName,
        //            IntegratedSecurity = true,
        //            MultipleActiveResultSets = true
        //        }.ConnectionString);
        //    }))
        //{
        //    this._seedSnapshot = seedSnapshot;
        //    this._seedTestData = seedTestData;
        //    ////TODO: Decide if the below is still needed for unit test database generator
        //    //var scsb = new SqlConnectionStringBuilder
        //    //{
        //    //    DataSource = "(localdb)\\mssqllocaldb",
        //    //    InitialCatalog = testDbName,
        //    //    IntegratedSecurity = true,
        //    //    MultipleActiveResultSets = true
        //    //};
        //    //_connectionString = scsb.ConnectionString;
        //}
        //NOTE:  End of portion commented out 2/1/24

        //Order of loading is determined by foreign key constraints
        //It can be queried with the following SQL:
        //WITH fk_tables
        //	AS ( 
        //		SELECT    s1.name AS from_schema
        //		, o1.Name AS         from_table
        //		, s2.name AS         to_schema
        //		, o2.Name AS         to_table
        //		  FROM sys.foreign_keys fk
        //			   INNER JOIN sys.objects o1
        //					ON fk.parent_object_id = o1.object_id
        //			   INNER JOIN sys.schemas s1
        //					ON o1.schema_id = s1.schema_id
        //			   INNER JOIN sys.objects o2
        //					ON fk.referenced_object_id = o2.object_id
        //			   INNER JOIN sys.schemas s2
        //					ON o2.schema_id = s2.schema_id

        //	/******************************************************
        //	For the purposes of finding dependency hierarchy       
        //			we're not worried about self-referencing tables
        //	******************************************************/

        //		  WHERE    NOT    (    s1.name = s2.name
        //						   AND o1.name = o2.name)
        //	)
        //, ordered_tables
        //	AS (        SELECT    s.name AS schemaName
        //				, t.name AS         tableName
        //				, 2 AS              Level
        //				  FROM sys.tables t
        //					   INNER JOIN sys.schemas s
        //							ON t.schema_id = s.schema_id
        //					   LEFT OUTER JOIN fk_tables fk
        //							ON s.name = fk.from_schema
        //						   AND t.name = fk.from_table
        //				  WHERE    fk.from_schema IS NULL
        //				UNION    ALL
        //				SELECT    fk.from_schema
        //				, fk.from_table
        //				, ot.Level + 2
        //				  FROM fk_tables fk
        //					   INNER JOIN ordered_tables ot
        //							ON fk.to_schema = ot.schemaName
        //						   AND fk.to_table = ot.tableName
        //	)
        //, ViewReferences
        //	AS (
        //	SELECT DISTINCT s.name schemaName, t.name tableName
        //	  FROM sys.tables t
        //		   INNER JOIN sys.schemas s
        //				ON s.schema_id = t.schema_id
        //		   INNER JOIN sys.sql_expression_dependencies d
        //				ON d.referenced_id = t.object_id
        //		   INNER JOIN sys.objects o
        //				ON d.referencing_id = o.object_id
        //			   AND o.type = 'V'
        //	)
        //, maxLevel(schemaName, tableName, maxLevel)
        //	AS (
        //	SELECT    schemaName
        //	, tableName
        //	, MAX(Level) maxLevel
        //	  FROM ordered_tables
        //	  GROUP    BY schemaName, tableName
        //	), result
        //	AS (
        //	SELECT    DISTINCT    ot.schemaName
        //	, ot.tableName
        //	, ot.Level
        //	, CASE WHEN vr.tableName IS NULL THEN ot.Level
        //		  ELSE ot.Level - 1
        //	  END FixedLevel
        //	  FROM ordered_tables ot
        //		   INNER JOIN maxLevel mx
        //				ON ot.schemaName = mx.schemaName
        //			   AND ot.tableName = mx.tableName
        //			   AND mx.maxLevel = ot.Level
        //		   LEFT JOIN ViewReferences vr
        //				ON vr.tableName = ot.tableName
        //			   AND vr.schemaName = ot.schemaName
        //	)
        //	SELECT schemaName, tableName, FixedLevel level
        //	  FROM result
        //	  WHERE result.schemaName IN ('dbo','beta')
        //    ORDER BY FixedLevel, tableName;
        public void AddFunctionsForDefaultValues(ModelBuilder modelBuilder)
        {
            //UNDONE:
        }

        public async Task SeedSnapshotData(ModelBuilder modelBuilder)
        {
            //TODO:  Locate InsertData.sql, load and execute.
            var curDir = Assembly.GetExecutingAssembly().Location;
            var diCurrent = new DirectoryInfo(curDir);
            var diMigrations = diCurrent.GetDirectories().First(di =>
                string.Equals(di.Name, "Migrations", StringComparison.InvariantCultureIgnoreCase));
            var fiInsertSqlFile = diMigrations.GetFiles("InsertData.sql").First();
            //NOTE: Must have sqlcmd installed on machine for this to work.
            var sqlCmdProc = new Process{StartInfo = new ProcessStartInfo("SqlCmd", " -i "+ fiInsertSqlFile.FullName)
            {
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = true, WindowStyle = ProcessWindowStyle.Hidden
            }};
            var srError = sqlCmdProc.StandardError;
            var srOutput = sqlCmdProc.StandardOutput;
            if (!sqlCmdProc.Start())
            {
                Debug.WriteLine("SqlCmd didn't start.  Probably not installed properly.");
                throw new Exception("SqlCmd did not start");
            }

            await sqlCmdProc.WaitForExitAsync();
            var CheckOutputTasks = new Task<string>[]{ srError.ReadToEndAsync(), srOutput.ReadToEndAsync()};
            Task.WaitAll(CheckOutputTasks);
            var error = CheckOutputTasks[0].Result;
            var output = CheckOutputTasks[1].Result;
            if (Environment.UserInteractive)
                Console.WriteLine(output);
            else
                Debug.WriteLine(output);
            if (!string.IsNullOrWhiteSpace(error))
            {
                var forOutput = "SqlCmd returned the following Error:\r\n" + error;
                if (Environment.UserInteractive)
                    Console.WriteLine(forOutput);
                else 
                    Debug.WriteLine(output);
                throw new Exception(forOutput);
            }
            
        }
        public void SeedTestData(ModelBuilder modelBuilder) 
        {
            //Free conums obtained by the following query:
            //SELECT TOP 25 Number
            //FROM DataObject.Number n
            //	left JOIN Basis.Company c ON c.conum = n.number
            //	WHERE Number>1001 AND c.conum IS null
            //	ORDER BY Number
            var freeConums = new int[] {1007, 1010, 1015, 1016, 1017,
                1019, 1026, 1031, 1033, 1036, 1042, 1046, 1048, 1061,
                1064, 1065, 1072, 1073, 1074, 1077, 1079, 1080, 1115,
                1122, 1125};
            modelBuilder.Entity<AccountRate>().HasData(
                new AccountRate
                {
                    CoNum = freeConums[0].ToString(),
                    AgentContactInfo = "Bill Jackson, bJackson3244@gmail.com, 515-555-6543",
                    CommissionRateCalculation = "20%",
                    PremiumRateCalculation = "$10 per $1000",
                    SpecialProcessingInstructions = "Call Bill first",
                    InvoiceEmail = "bJackson3244@gmail.com"
                },
                new AccountRate
                {
                    CoNum = freeConums[1].ToString(),
                    AgentContactInfo = "Barb Jackson, bJackson3474@gmail.com, 515-555-2843",
                    CommissionRateCalculation = "19%",
                    PremiumRateCalculation = "$10.50 per $1000",
                    SpecialProcessingInstructions = "Call Barb first",
                    InvoiceEmail = "bJackson3474@gmail.com"
                });
        }
    }
}
