using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Management.Smo;

namespace TestJamesDbCreator
{
    internal static class SqlHelper
    {
        internal static string SqlFkReferences => @"SELECT    s1.name AS from_schema
	, o1.Name AS         from_table
	, s2.name AS         to_schema
	, o2.Name AS         to_table
	  FROM sys.foreign_keys fk
		   INNER JOIN sys.objects o1
				ON fk.parent_object_id = o1.object_id
		   INNER JOIN sys.schemas s1
				ON o1.schema_id = s1.schema_id
		   INNER JOIN sys.objects o2
				ON fk.referenced_object_id = o2.object_id
		   INNER JOIN sys.schemas s2
				ON o2.schema_id = s2.schema_id

/******************************************************
For the purposes of finding dependency hierarchy       
		we're not worried about self-referencing tables
******************************************************/

	  WHERE    NOT    (    s1.name = s2.name
					   AND o1.name = o2.name)";

        internal static string SqlTableStructureQuery => @"WITH fk_tables
	AS ( 
	" + SqlFkReferences + @"
	)
, ordered_tables
	AS (        SELECT    s.name AS schemaName
				, t.name AS         tableName
				, 2 AS              Level
				  FROM sys.tables t
					   INNER JOIN sys.schemas s
							ON t.schema_id = s.schema_id
					   LEFT OUTER JOIN fk_tables fk
							ON s.name = fk.from_schema
						   AND t.name = fk.from_table
				  WHERE    fk.from_schema IS NULL
                            AND is_ms_shipped = 0
				UNION    ALL
				SELECT    fk.from_schema
				, fk.from_table
				, ot.Level + 2
				  FROM fk_tables fk
					   INNER JOIN ordered_tables ot
							ON fk.to_schema = ot.schemaName
						   AND fk.to_table = ot.tableName
	)
, ViewReferences
	AS (
	SELECT DISTINCT s.name schemaName, t.name tableName
	  FROM sys.tables t
		   INNER JOIN sys.schemas s
				ON s.schema_id = t.schema_id
		   INNER JOIN sys.sql_expression_dependencies d
				ON d.referenced_id = t.object_id
		   INNER JOIN sys.objects o
				ON d.referencing_id = o.object_id
			   AND o.type = 'V'
	)
, maxLevel(schemaName, tableName, maxLevel)
	AS (
	SELECT    schemaName
	, tableName
	, MAX(Level) maxLevel
	  FROM ordered_tables
	  GROUP    BY schemaName, tableName
	), result
	AS (
	SELECT    DISTINCT    ot.schemaName
	, ot.tableName
	, ot.Level
	, CASE WHEN vr.tableName IS NULL THEN ot.Level
		  ELSE ot.Level - 1
	  END FixedLevel
	  FROM ordered_tables ot
		   INNER JOIN maxLevel mx
				ON ot.schemaName = mx.schemaName
			   AND ot.tableName = mx.tableName
			   AND mx.maxLevel = ot.Level
		   LEFT JOIN ViewReferences vr
				ON vr.tableName = ot.tableName
			   AND vr.schemaName = ot.schemaName
	)
	SELECT schemaName, tableName, FixedLevel level
	  FROM result
	  ORDER BY FixedLevel;";

        internal static SqlConnection GetDestinationConnection(string? dbName = null)
        {
            var scsb = new SqlConnectionStringBuilder(
                ConfigurationManager.ConnectionStrings["SnapDestination"].ConnectionString);
            if (null != dbName)
            {
                scsb.InitialCatalog = dbName;
            }

            return new SqlConnection(scsb.ConnectionString);
        }
        internal static SqlConnection GetSourceConnection(string? dbName = null)
        {
            var scsb = new SqlConnectionStringBuilder(
                ConfigurationManager.ConnectionStrings["JamesSource"].ConnectionString);
            if (null != dbName)
            {
                scsb.InitialCatalog = dbName;
            }

            return new SqlConnection(scsb.ConnectionString);
        }

        internal static string GetDestinationDbName()
        {
            var scsb = new SqlConnectionStringBuilder(
                ConfigurationManager.ConnectionStrings["SnapDestination"].ConnectionString);
            return scsb.InitialCatalog;
        }
        internal static string GetDestinationServerName()
        {
            var scsb = new SqlConnectionStringBuilder(
                ConfigurationManager.ConnectionStrings["SnapDestination"].ConnectionString);
            return scsb.DataSource;
        }

        internal static string GetSourceDbName()
        {

            var scsb = new SqlConnectionStringBuilder(
                ConfigurationManager.ConnectionStrings["JamesSource"].ConnectionString);
            return scsb.InitialCatalog;
        }

        internal static string SqlReduceAccounts = @"DECLARE @acctList TABLE
(
    newnessRank INT,
    AccountNum VARCHAR(8)
);
DECLARE @NeededSetCount INT,
        @containedSetCount INT;
DECLARE @allData TABLE
(
    AccountNum VARCHAR(8),
    AccountStatus VARCHAR(24),
    Division CHAR(4),
    HasBonds BIT,
    HasBids BIT,
    HasFinancials BIT,
    HasProgram BIT,
    HasLoa BIT,
    HasAccountRates INT
);
DECLARE @Dataset TABLE
(
    AccountNum VARCHAR(8),
    AccountStatus VARCHAR(24),
    Division CHAR(4),
    HasBonds BIT,
    HasBids BIT,
    HasFinancials BIT,
    HasProgram BIT,
    HasLoa BIT,
    HasAccountRates INT
);

DECLARE @fullSet BIT = 0,
        @newnessRank INT,
        @accountNum INT;

WITH CurrentStatusDate
AS (SELECT asl.AccountNum,
           MAX(asl.Effective) Effective
    FROM dbo.AccountStatusLog asl
    GROUP BY asl.AccountNum),
     AccountStatus
AS (SELECT asl.AccountNum,
           asl.AccountStatus
    FROM dbo.AccountStatusLog asl
        INNER JOIN CurrentStatusDate csm
            ON csm.AccountNum = asl.AccountNum
               AND csm.Effective = asl.Effective)
INSERT INTO @acctList
SELECT ROW_NUMBER() OVER (PARTITION BY AccountStatus, Division ORDER BY a.AccountNum DESC) NewnessRank,
       a.AccountNum
FROM Account a
    LEFT JOIN AccountStatus s
        ON s.AccountNum = a.AccountNum
ORDER BY 1,
         a.AccountNum DESC;

WITH CurrentStatusDate
AS (SELECT asl.AccountNum,
           MAX(asl.Effective) Effective
    FROM dbo.AccountStatusLog asl
    GROUP BY asl.AccountNum),
     AccountStatus
AS (SELECT asl.AccountNum,
           asl.AccountStatus
    FROM dbo.AccountStatusLog asl
        INNER JOIN CurrentStatusDate csm
            ON csm.AccountNum = asl.AccountNum
               AND csm.Effective = asl.Effective),
     BondCount
AS (SELECT AccountNum,
           COUNT(*) BondCount
    FROM dbo.Bond b
    GROUP BY b.AccountNum),
     BidCount
AS (SELECT AccountNum,
           COUNT(*) BidCount
    FROM dbo.BondRequest b
    GROUP BY b.AccountNum),
     FinancialCount
AS (SELECT AccountNum,
           COUNT(*) FinancialCount
    FROM dbo.FinancialRatio
    GROUP BY AccountNum),
     ProgramCount
AS (SELECT AccountNum,
           COUNT(*) ProgramCount
    FROM dbo.AccountProgram
    GROUP BY AccountNum),
     LoaCount
AS (SELECT AccountNum,
           COUNT(*) LoaCount
    FROM dbo.LineOfAuthorityLog
    GROUP BY AccountNum),
     AccountRateAttachmentCount
AS (SELECT a.AccountNum,
           COUNT(*) AttachmentCount
    FROM dbo.Account a
        INNER JOIN dbo.AccountRate ar
            ON a.AccountNum = ar.CoNum
        INNER JOIN dbo.AccountRateAttachment ara
            ON ara.AccountRateId = ar.Id
    GROUP BY a.AccountNum)
INSERT INTO @allData
(
    AccountNum,
    AccountStatus,
    Division,
    HasBonds,
    HasBids,
    HasFinancials,
    HasProgram,
    HasLoa,
    HasAccountRates
)
SELECT a.AccountNum,
       s.AccountStatus,
       a.Division,
       CASE
           WHEN ISNULL(BondCount, 0) > 1 THEN
               1
           ELSE
               0
       END HasBonds,
       CASE
           WHEN ISNULL(BidCount, 0) > 1 THEN
               1
           ELSE
               0
       END HasBids,
       CASE
           WHEN ISNULL(FinancialCount, 0) > 1 THEN
               1
           ELSE
               0
       END HasFinancials,
       CASE
           WHEN ISNULL(ProgramCount, 0) > 1 THEN
               1
           ELSE
               0
       END HasProgram,
       CASE
           WHEN ISNULL(LoaCount, 0) > 1 THEN
               1
           ELSE
               0
       END HasLoa,
       CASE
           WHEN ISNULL(ac.AttachmentCount, 0) > 1 THEN
               2
           WHEN ar.CoNum IS NOT NULL THEN
               1
           ELSE
               0
       END HasAccountRates
FROM Account a
    LEFT JOIN AccountStatus s
        ON s.AccountNum = a.AccountNum
    LEFT JOIN BondCount bc
        ON bc.AccountNum = a.AccountNum
    LEFT JOIN BidCount bic
        ON bic.AccountNum = a.AccountNum
    LEFT JOIN FinancialCount fc
        ON fc.AccountNum = a.AccountNum
    LEFT JOIN ProgramCount pc
        ON pc.AccountNum = a.AccountNum
    LEFT JOIN LoaCount lc
        ON lc.AccountNum = a.AccountNum
    LEFT JOIN dbo.AccountRate ar
        ON a.AccountNum = ar.CoNum
    LEFT JOIN AccountRateAttachmentCount ac
        ON ac.AccountNum = a.AccountNum
ORDER BY 1,
         2,
         3,
         4,
         5;

INSERT INTO @Dataset
(
    AccountNum,
    AccountStatus,
    Division,
    HasBonds,
    HasBids,
    HasFinancials,
    HasProgram,
    HasLoa,
    HasAccountRates
)
SELECT ad.*
FROM @allData ad
    INNER JOIN @acctList al
        ON al.AccountNum = ad.AccountNum
WHERE al.newnessRank = 1;

WITH NeededCategories
AS (SELECT DISTINCT
           HasBonds,
           HasBids,
           HasFinancials,
           HasProgram,
           HasLoa,
           HasAccountRates
    FROM @allData)
SELECT @NeededSetCount = COUNT(*)
FROM NeededCategories;

--UNDONE:  Change the logic for making sure account rates are included to add directly from the AccountRate and AccountRate Attachment tables.
DECLARE @accountsSearched INT = 0;
DECLARE AcctCursur CURSOR FOR
SELECT *
FROM @acctList
WHERE newnessRank > 1
ORDER BY newnessRank,
         AccountNum DESC;
OPEN AcctCursur;
FETCH NEXT FROM AcctCursur
INTO @newnessRank,
     @accountNum;
WHILE @@FETCH_STATUS = 0 AND @fullSet = 0
BEGIN
    SET @accountsSearched = @accountsSearched + 1;
    WITH CurrentCategories
    AS (SELECT DISTINCT
               HasBonds,
               HasBids,
               HasFinancials,
               HasProgram,
               HasLoa,
               HasAccountRates
        FROM @Dataset)
    SELECT @containedSetCount = COUNT(*)
    FROM CurrentCategories;
    IF @containedSetCount < @NeededSetCount
        INSERT INTO @Dataset
        (
            AccountNum,
            AccountStatus,
            Division,
            HasBonds,
            HasBids,
            HasFinancials,
            HasProgram,
            HasLoa,
            HasAccountRates
        )
        SELECT ad.*
        FROM @allData ad
        WHERE ad.AccountNum = @accountNum
              AND NOT EXISTS
        (
            SELECT 1
            FROM @Dataset ds
            WHERE ds.HasBonds = ad.HasBonds
                  AND ds.HasBids = ad.HasBids
                  AND ds.HasFinancials = ad.HasFinancials
                  AND ds.HasProgram = ad.HasProgram
                  AND ds.HasLoa = ad.HasLoa
                  AND ds.HasAccountRates = ad.HasAccountRates
        );
    ELSE
        SET @fullSet = 1;
    IF @accountsSearched % 500 = 0
        PRINT (STR(@accountsSearched) + ' accounts searched');
    FETCH NEXT FROM AcctCursur
    INTO @newnessRank,
         @accountNum;
END;
CLOSE AcctCursur;
DEALLOCATE AcctCursur;

DELETE bt
FROM dbo.BondTransaction bt 
WHERE AccountNum NOT IN
      (
          SELECT AccountNum FROM @Dataset
      );
DELETE FROM dbo.Bond
WHERE AccountNum NOT IN
      (
          SELECT AccountNum FROM @Dataset
      );
DELETE FROM dbo.Account
WHERE AccountNum NOT IN
      (
          SELECT AccountNum FROM @Dataset
      );";

        internal static string SqlReduceBonds = @"DECLARE @bondList TABLE
(
    rank INT,
    bondNumbner VARCHAR(12)
);
DECLARE @NeededSetCount INT,
        @containedSetCount INT;
DECLARE @allData TABLE
(
    BondNumber VARCHAR(12),
    Status VARCHAR(14),
    TerminationType NVARCHAR(20),
    Cancellable BIT,
    BondClass NVARCHAR(20),
    IsDirectBill BIT,
    BondTerm VARCHAR(25),
    HasBids BIT,
    Era VARCHAR(10)
);
DECLARE @dataset TABLE
(
    BondNumber VARCHAR(12),
    Status VARCHAR(14),
    TerminationType NVARCHAR(20),
    Cancellable BIT,
    BondClass NVARCHAR(20),
    IsDirectBill BIT,
    BondTerm VARCHAR(25),
    HasBids BIT,
    Era VARCHAR(10)
);

DECLARE @fullSet BIT = 0,
        @rank INT,
        @bondNumber VARCHAR(12);

WITH BondTerm
AS (SELECT b.BondNumber,
           b.BondMod,
           MAX(DATEDIFF(DAY, b.Effective, b.Expiration)) TermDays
    FROM dbo.BondTransaction b
    GROUP BY b.BondNumber,
             b.BondMod),
     BidCount
AS (SELECT BondNumber,
           COUNT(*) BidCount
    FROM dbo.BondRequest b
    GROUP BY b.BondNumber),
     CompletedDate
AS (SELECT BondNumber,
           MAX(Expiration) FinalDay
    FROM dbo.BondTransaction
    GROUP BY BondNumber),
     IsComplete
AS (SELECT BondNumber,
           CASE
               WHEN FinalDay < DATEADD(DAY, 1, GETDATE()) THEN
                   1
               ELSE
                   0
           END Complete
    FROM CompletedDate),
     BondEra
AS (SELECT BondNumber,
           CASE
               WHEN Effective > DATEADD(YEAR, -2, GETDATE()) THEN
                   'Recent'
               WHEN Effective
                    BETWEEN DATEADD(YEAR, -6, GETDATE()) AND DATEADD(YEAR, -2, GETDATE()) THEN
                   'Medium'
               ELSE
                   'Oldest'
           END Era
    FROM dbo.Bond
    WHERE Effective > DATEADD(YEAR, -10, GETDATE())),
     AddtionalObligee
AS (SELECT BondNumber,
           COUNT(*) AdditionalObligeeCnt
    FROM dbo.AdditionalObligee
    GROUP BY BondNumber),
     AdditionalResponsiblePArty
AS (SELECT BondNumber,
           COUNT(*) AdditionalPartyCount
    FROM dbo.AdditionalRelatedParty arp
        INNER JOIN dbo.Bond b
            ON b.AccountNum = arp.AccountNum
    GROUP BY BondNumber)
INSERT INTO @allData
SELECT bt.BondNumber,
       b.Status,
       b.TerminationType,
       b.Cancellable,
       b.BondClass,
       b.IsDirectBill,
       CASE
           WHEN t.TermDays
                BETWEEN 364 AND 367 THEN
               'One year term'
           WHEN t.TermDays <= 363 THEN
               'Less than a year term'
           WHEN t.TermDays
                BETWEEN 368 AND 720 THEN
               'More than a year term'
           ELSE
               'Two year or longer term'
       END BondTerm,
       CASE
           WHEN ISNULL(BidCount, 0) > 1 THEN
               1
           ELSE
               0
       END HasBids,
       Era
FROM dbo.BondTransaction bt
    INNER JOIN Bond b
        ON b.BondNumber = bt.BondNumber
    LEFT JOIN BondTerm t
        ON t.BondNumber = b.BondNumber
    LEFT JOIN BidCount bic
        ON bic.BondNumber = b.BondNumber
    LEFT JOIN IsComplete ic
        ON ic.BondNumber = b.BondNumber
    INNER JOIN BondEra be
        ON be.BondNumber = b.BondNumber
ORDER BY 1,
         2,
         3,
         4,
         5;

INSERT INTO @bondList
SELECT ROW_NUMBER() OVER (PARTITION BY Status, BondClass, Era ORDER BY NEWID()),
       BondNumber
FROM @allData;

INSERT INTO @dataset
SELECT DISTINCT
       ad.*
FROM @allData ad
    INNER JOIN @bondList b
        ON ad.BondNumber = b.bondNumbner
WHERE b.rank = 1;

WITH NeededCategories
AS (SELECT DISTINCT
           TerminationType,
           IsDirectBill,
           BondTerm,
           HasBids,
           Era
    FROM @allData)
SELECT @NeededSetCount = COUNT(*)
FROM NeededCategories;

DECLARE BondCursor CURSOR FOR
SELECT *
FROM @bondList
WHERE @rank > 1
ORDER BY rank;
OPEN BondCursor;
FETCH NEXT FROM BondCursor
INTO @rank,
     @bondNumber;
WHILE @@FETCH_STATUS = 0 AND @fullSet = 0
BEGIN
    WITH CurrentCategories
    AS (SELECT DISTINCT
               TerminationType,
               IsDirectBill,
               BondTerm,
               HasBids,
               Era
        FROM @dataset)
    SELECT @containedSetCount = COUNT(*)
    FROM CurrentCategories;
    IF @containedSetCount < @NeededSetCount
        INSERT INTO @dataset
        SELECT ad.*
        FROM @allData ad
        WHERE BondNumber = @bondNumber
              AND NOT EXISTS
        (
            SELECT 1
            FROM @dataset ds
            WHERE ds.TerminationType = ad.TerminationType
                  AND ds.IsDirectBill = ad.IsDirectBill
                  AND ds.BondTerm = ad.BondTerm
                  AND ds.Era = ad.Era
        );
    ELSE
        SET @fullSet = 1;
    FETCH NEXT FROM BondCursor
    INTO @rank,
         @bondNumber;
END;
CLOSE BondCursor;
DEALLOCATE BondCursor;

--SELECT * FROM @dataset;

DELETE FROM dbo.BondTransaction
WHERE BondNumber NOT IN
      (
          SELECT BondNumber FROM @dataset
      );
DELETE FROM dbo.Bond
WHERE BondNumber NOT IN
      (
          SELECT BondNumber FROM @dataset
      );";

        internal static string SqlDeleteSchemaTables = @"DECLARE @SqlStatement NVARCHAR(MAX);
SELECT @SqlStatement = 
    COALESCE(@SqlStatement, N'') + N'DROP TABLE [' + @SchemaName + '].' + QUOTENAME(TABLE_NAME) + N';' + CHAR(13)
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_SCHEMA = @SchemaName and TABLE_TYPE = 'BASE TABLE';

EXEC (@SqlStatement);";

        internal static string SqlPruneOrphanedAccounts = @"DECLARE @rowsDeleted INT = -1, @passes INT = 0;;
WHILE @rowsDeleted <> 0 AND @passes < 10
BEGIN
	DELETE le
    FROM dbo.LegalEntity le
        LEFT JOIN dbo.Account a
            ON le.ID = a.ID
    WHERE a.Id IS NULL
          AND le.EntityType = 'Company'
          AND NOT EXISTS
    (
        SELECT 1 FROM legalentity le2 WHERE le2.parent = le.id AND le2.parent <> le2.id
    );
    SET @rowsDeleted = @@ROWCOUNT;
	SET @passes = @passes + 1;
END;
DELETE ara
FROM dbo.AccountRateAttachment ara
    INNER JOIN dbo.AccountRate ar
        ON ar.Id = ara.AccountRateId
    LEFT JOIN dbo.Account a
        ON ar.CoNum = a.AccountNum
WHERE a.AccountNum IS NULL;

DELETE ara
FROM dbo.AccountRateAttachment ara
    INNER JOIN dbo.AccountRate ar
        ON ar.Id = ara.AccountRateId
    LEFT JOIN dbo.Account a
        ON ar.CoNum = a.AccountNum
WHERE a.AccountNum IS NULL;
DELETE ar
FROM dbo.AccountRate ar
    LEFT JOIN dbo.Account a
        ON ar.CoNum = a.AccountNum
WHERE a.AccountNum IS NULL;";

        internal static string SqlPruneObligeeInfo = @"DELETE o
FROM dbo.Obligee o LEFT JOIN dbo.Bond b on o.Id = b.ObligeeId
WHERE b.Id IS NULL;
DELETE lea
FROM  dbo.LegalEntityAddress lea
INNER JOIN dbo.LegalEntity le ON le.Id = lea.LegalEntityId
LEFT JOIN dbo.Obligee o on o.Id = lea.LegalEntityId
WHERE o.id IS NULL AND le.EntityType = 'Obligee';
DELETE le
FROM dbo.LegalEntity le
    LEFT JOIN dbo.Obligee o
        ON o.Id = le.Id
WHERE le.EntityType = 'Obligee'
      AND o.Id IS NULL;";

        internal static string SqlPruneMiscData = @"DELETE FROM dbo.RenewalRequest WHERE RenewalCompleted < DATEADD(MONTH, -2, GETDATE());
DELETE FROM dbo.RenewalRequest WHERE CancelReason IS NOT NULL AND Created < DATEADD(MONTH, -2, GETDATE());

DELETE asl
FROM dbo.AccountStatusLog asl
    LEFT JOIN dbo.Account a
        ON a.AccountNum = asl.AccountNum
WHERE a.Id IS NULL;

DELETE cc
FROM dbo.LegalEntity cc 
LEFT JOIN dbo.Account a ON a.CPAContactId = cc.Id 
WHERE cc.EntityType = 'CPAContact' AND a.Id IS NULL
DELeTE cf 
FROM dbo.LegalEntity cf 
LEFT JOIN dbo.Account a ON a.CPAFirmId = cf.Id 
WHERE cf.EntityType = 'CPAFirm' AND a.Id IS NULL

DELETE rp
FROM dbo.LegalEntity rp
    LEFT JOIN dbo.Bond b
        ON b.ResponsiblePartyId = rp.Id
WHERE rp.EntityType = 'ResponsibleParty'
      AND b.Id IS NULL;
DELETE rp
FROM dbo.ResponsibleParty rp
    LEFT JOIN dbo.Bond b
        ON b.ResponsiblePartyId = rp.Id
WHERE b.Id IS NULL;
WITH AgencyRefs
AS (SELECT AgencyId
    FROM dbo.BondTransaction
    UNION
    SELECT AgencyId
    FROM dbo.VoidedBond)
DELETE ag
FROM dbo.Agency ag
    LEFT JOIN dbo.Account a
        ON a.AgencyNumber = ag.AgencyNumber
    LEFT JOIN AgencyRefs ar
        ON ar.AgencyId = ag.Id
WHERE a.AgencyNumber IS NULL
      AND ar.AgencyId IS NULL;

WITH SicRefs
AS (SELECT SICCode
    FROM dbo.Bond
    UNION
    SELECT SICCode
    FROM dbo.BondHold
    UNION
    SELECT Code
    FROM dbo.SICRatio)
DELETE s
FROM dbo.SIC s
    LEFT JOIN SicRefs sr
        ON sr.SICCode = s.Code
WHERE sr.SICCode IS NULL;

WITH SfaaRefs
AS (SELECT SfaaCode
    FROM dbo.BondTransaction
    UNION
    SELECT SFAACode
    FROM deleted.BondTransaction
    UNION
    SELECT SFAACode
    FROM dbo.BondHold
    UNION
    SELECT SFAACode
    FROM dbo.BondRequestCommercial)
--DELETE s
SELECT s.*
FROM dbo.SIC s
    LEFT JOIN SfaaRefs sr
        ON s.Code = sr.SfaaCode
WHERE sr.SfaaCode IS NULL;

DELETE aa
FROM dbo.AgentsInAgency aa
    LEFT JOIN dbo.Agency a
        ON aa.AgentId = a.Id
WHERE a.Id IS NULL;

DELETE a
FROM dbo.Agent a
    LEFT JOIN dbo.AgentsInAgency aa
        ON aa.AgentId = a.Id;";

        internal static string SqlCreateIdTableType = @"IF NOT EXISTS (SELECT 1 FROM sys.types WHERE name='IdTable')
CREATE TYPE IdTable AS TABLE (Id UNIQUEIDENTIFIER);
--IF EXISTS
--(
--    SELECT 1
--    FROM sys.tables t
--        INNER JOIN sys.columns c
--            ON c.object_id = t.object_id
--    WHERE c.name = 'SFAAClassCode'
--          AND t.name = 'BondRequestCommercial'
--)
--    EXEC sp_rename 'dbo.BondRequestCommercial.SFAAClassCode',
--                   'SfaaCode',
--                   'COLUMN';
--IF EXISTS
--(
--    SELECT 1
--    FROM sys.tables t
--        INNER JOIN sys.columns c
--            ON c.object_id = t.object_id
--    WHERE c.name = 'SaaCode'
--          AND t.name = 'BondTransaction'
--          AND SCHEMA_NAME(t.schema_id) = 'deleted'
--)
--    EXEC sp_rename 'deleted.BondTransaction.SaaCode', 'SfaaCode', 'COLUMN';";
        internal static string SqlPruneAgentAgency = @"DECLARE @AgenciesToRemove IdTable,
		@agentsToRemove IdTable,
        @agentsInMultipleAgencies IdTable;
INSERT INTO @agentsInMultipleAgencies
SELECT TOP 1
       AgentId
FROM dbo.AgentsInAgency
GROUP BY AgentId
HAVING COUNT(*) > 1;
INSERT INTO @AgenciesToRemove
SELECT ag.Id
FROM dbo.Agency ag
    LEFT JOIN dbo.Account a
        ON a.AgencyNumber = ag.AgencyNumber
    LEFT JOIN dbo.AgentsInAgency aa
        ON aa.AgencyId = ag.Id
	LEFT JOIN dbo.Bond b ON b.AgencyId = ag.id
	LEFT JOIN dbo.BondTransaction bt ON bt.AgencyId = ag.id
WHERE a.AgencyNumber IS NULL AND b.Id IS NULL AND aa.Id IS NULL AND bt.id IS NULL
		-- keep agencies with a multiple-agency agent
      AND aa.AgentId NOT IN
          (
              SELECT Id FROM @agentsInMultipleAgencies
          );
--Keep a single example of a non-attached agency to simulate a new agency just created
DELETE TOP (1) ar
FROM @AgenciesToRemove ar
INNER JOIN dbo.Agency ag ON ag.Id = ar.Id
WHERE AG.Status IN ('Active');

DELETE aa FROM dbo.AgentsInAgency aa INNER JOIN @AgenciesToRemove ON [@AgenciesToRemove].Id = aa.AgencyId;
DELETE ag
FROM dbo.Agency ag INNER JOIN @AgenciesToRemove ar ON ar.Id = ag.Id;


INSERT INTO @agentsToRemove
SELECT ag.Id FROM dbo.Agent ag LEFT JOIN dbo.AgentsInAgency aa ON aa.AgentId = ag.Id
LEFT JOIN dbo.Bond b ON b.AgentId = ag.Id
LEFT JOIN dbo.Account a ON a.AgentId = ag.id
WHERE aa.AgentId IS NULL AND b.Id IS NULL AND a.Id IS NULL;
--Keep a single example of a non-attached Agent
DELETE TOP (1) ar
FROM @agentsToRemove ar

DELETE ag
FROM dbo.Agent ag INNER JOIN @agentsToRemove ar ON ar.Id = ag.Id

DELETE le
FROM dbo.LegalEntity le
    LEFT JOIN dbo.Agent a
        ON a.Id = le.Id
WHERE le.EntityType = 'Agent'
      AND a.Id IS NULL;
DECLARE @rowsDeleted INT = -1, @passes INT = 0;
WHILE @rowsDeleted <> 0 AND @passes < 10
BEGIN
	DELETE le
	FROM dbo.LegalEntity le
		LEFT JOIN dbo.Agency a
			ON a.Id = le.Id
		LEFT JOIN dbo.LegalEntity child ON child.Parent = le.Id AND child.Parent <> child.id
	WHERE le.EntityType = 'Agency'
		  AND a.Id IS NULL AND child.Id IS NULL;
	SET @rowsDeleted = @@ROWCOUNT;
	SET @passes = @passes + 1;
END";

        internal static string SqlPruneAddressesAndPhoneNumbers = @"WITH AddressRefs
AS (SELECT AddressId
    FROM dbo.LegalEntityAddress
    UNION
    SELECT BranchOfficeAddressId
    FROM dbo.Branch
    UNION
    SELECT DirectBillAddressId
    FROM dbo.Bond)
DELETE a
FROM dbo.Address a
    LEFT JOIN AddressRefs ar
        ON ar.AddressId = a.Id
WHERE ar.AddressId IS NULL;
WITH PhoneRefs
AS (SELECT PhoneNumberId
    FROM dbo.LegalEntityPhone
    UNION
    SELECT PhoneId
    FROM dbo.Branch
    UNION
    SELECT DefaultPhoneNumber
    FROM dbo.Agent
    UNION
    SELECT DefaultCellNumber
    FROM dbo.Agent	)
DELETE p
FROM dbo.PhoneNumber p
    LEFT JOIN PhoneRefs pr
        ON pr.PhoneNumberId = p.Id
WHERE pr.PhoneNumberId IS NULL;";
        internal static string SqlPruneLawEntities = @"DECLARE @LawFirmIds IdTable,
        @attorneyIds IdTable,
        @rowsDeleted INT = -1;
INSERT INTO @LawFirmIds
SELECT DISTINCT
       LawFirmId
FROM dbo.Account
UNION
SELECT DISTINCT
       att.Parent
FROM dbo.Account a
    INNER JOIN dbo.LegalEntity att
        ON a.AttorneyId = att.Id
UNION
SELECT DISTINCT
       att.Parent
FROM dbo.Bond b
    INNER JOIN dbo.LegalEntity att
        ON b.AttorneyInFactId = att.Id;

INSERT INTO @attorneyIds
SELECT DISTINCT
       a.AttorneyId
FROM dbo.Account a
UNION
SELECT DISTINCT
       lf.Id
FROM @LawFirmIds lfi
    INNER JOIN dbo.LegalEntity lf
        ON lf.Parent = lfi.Id
UNION
SELECT DISTINCT
       b.AttorneyInFactId
FROM dbo.Bond b;
--NOTE:If we want an unattached lawyer or law firm, add a row to these tables

DELETE lw
FROM dbo.LawEntity lw
    INNER JOIN dbo.LegalEntity le
        ON lw.Id = le.Id
    LEFT JOIN @attorneyIds ai
        ON ai.Id = lw.Id
WHERE ai.Id IS NULL
      AND le.EntityType = 'Attorney';
DELETE le
FROM dbo.LegalEntity le
    LEFT JOIN @attorneyIds ai
        ON ai.Id = le.Id
WHERE ai.Id IS NULL
      AND le.EntityType = 'Attorney';

WHILE @rowsDeleted <> 0
BEGIN
    DELETE lf
    FROM dbo.LawEntity lf
        INNER JOIN dbo.LegalEntity le
            ON lf.Id = le.Id
        LEFT JOIN dbo.LegalEntity child
            ON child.Parent = le.Id
               AND child.Id <> le.Id
        LEFT JOIN @LawFirmIds li
            ON li.Id = lf.Id
    WHERE li.Id IS NULL
          AND child.Id IS NULL
          AND le.EntityType = 'LawFirm';
    SET @rowsDeleted = @@ROWCOUNT;
    DELETE le
    FROM dbo.LegalEntity le
        LEFT JOIN dbo.LegalEntity child
            ON child.Parent = le.Id
               AND child.Id <> le.Id
        LEFT JOIN @LawFirmIds li
            ON li.Id = le.Id
    WHERE li.Id IS NULL
          AND child.Id IS NULL
          AND le.EntityType = 'LawFirm';
    SET @rowsDeleted = @rowsDeleted + @@ROWCOUNT;
END;";
    }
}
