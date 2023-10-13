using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace James.Data.Server.Migrations
{
    /// <inheritdoc />
    public partial class RawSqlAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //TODO:Create new NewNumber functions in Sql
            migrationBuilder.Sql(@"IF OBJECT_ID('dbo.NewObligeeNum') IS null
EXECUTE ('
CREATE FUNCTION [dbo].[NewObligeeNum]
()
RETURNS VARCHAR(7)
AS
BEGIN
    DECLARE @newNumber INT,
            @mostSignificantDigit INT;
    WITH AllPreviousObligeeNumbers
    AS (SELECT ObligeeNum
        FROM dbo.Obligee
        UNION ALL
        SELECT PrintStatusLetter
        FROM dbo.AdditionalObligee)
    SELECT @newNumber = MAX(CAST(RIGHT(ObligeeNum, 6) AS INT)) + 1
    FROM dbo.AllPreviousObligeeNumbers
    WHERE ObligeeNum NOT LIKE ''%[^0-9]%'';
    SELECT @mostSignificantDigit = CAST(s.Value AS INT)
    FROM Config.Setting s
    WHERE s.Name = ''MostSignificantDigit'';
    DECLARE @possibleError NVARCHAR(255)
        = N''""MostSignificantDigit"" is not defined in Config.Setting as a single digit positive integer'',
            @i INT;
    IF ISNULL(@mostSignificantDigit, -1) NOT
       BETWEEN 0 AND 9
        --HACK: The illegal case below is the only way to return a semi-meaningful error from a function
        SET @i = CAST(@possibleError AS INT);
    RETURN CAST(@newNumber + 1000000 * @mostSignificantDigit AS VARCHAR(8));
END;');
ALTER TABLE [dbo].[AdditionalObligee] ADD  CONSTRAINT [DF_AdditionalObligee_ObligeeNum]  DEFAULT ([dbo].[NewObligeeNum]()) FOR [ObligeeNum];
ALTER TABLE [dbo].[Obligee] ADD  CONSTRAINT [DF_Obligee_ObligeeNum]  DEFAULT ([dbo].[NewObligeeNum]()) FOR [ObligeeNum];");
            
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //TODO:Remove Default Values
            //TODO:Drop NewNumber functions
            migrationBuilder.Sql(@"IF EXISTS(SELECT 1 FROM sys.default_constraints WHERE name = 'DF_Obligee_ObligeeNum')
ALTER TABLE dbo.Obligee
DROP CONSTRAINT DF_Obligee_ObligeeNum;
IF EXISTS(SELECT 1 FROM sys.default_constraints WHERE name = 'DF_AdditionalObligee_ObligeeNum')
ALTER TABLE dbo.AdditionalObligee
DROP CONSTRAINT DF_AdditionalObligee_ObligeeNum;
IF OBJECT_ID('dbo.NewObligeeNum') IS NOT null
DROP FUNCTION [dbo].[NewObligeeNum]");
        }
    }
}
