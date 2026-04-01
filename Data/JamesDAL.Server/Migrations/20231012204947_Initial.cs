using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace James.Data.Server.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Beta");

            migrationBuilder.CreateTable(
                name: "AccountClassDM",
                columns: table => new
                {
                    AccountClass = table.Column<string>(type: "varchar(4)", unicode: false, maxLength: 4, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Name = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    DivisionGeneralLedgerCode = table.Column<int>(type: "int", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((1))"),
                    LineOfAuthorityNotificationGroup = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountClassDM", x => x.AccountClass);
                });

            migrationBuilder.CreateTable(
                name: "AccountProgramEmailNotificationGroups",
                columns: table => new
                {
                    UserName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    SendTo = table.Column<string>(type: "nvarchar(510)", maxLength: 510, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountProgramEmailNotificationGroups", x => x.UserName);
                });

            migrationBuilder.CreateTable(
                name: "AccountProgramStatusDM",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OldId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountProgramStatus", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "AccountProgramStatusHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    OldStatus = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NewStatus = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OldSingle = table.Column<int>(type: "int", nullable: true),
                    NewSingle = table.Column<int>(type: "int", nullable: false),
                    OldAggregate = table.Column<int>(type: "int", nullable: true),
                    NewAggregate = table.Column<int>(type: "int", nullable: false),
                    StatusDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    StatusChangeBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountProgramStatusHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccountProgramUserAuthority",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    User = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    AccountClassId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Effective = table.Column<DateTime>(type: "datetime", nullable: false),
                    Expiration = table.Column<DateTime>(type: "datetime", nullable: false),
                    Single = table.Column<int>(type: "int", nullable: false),
                    Aggregate = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountProgramUserAuthority", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "AccountRate",
                columns: table => new
                {
                    CoNum = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    AgentContactInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommissionRateCalculation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PremiumRateCalculation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpecialProcessingInstructions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InvoiceEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsParent = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountRate", x => x.CoNum);
                    table.UniqueConstraint("AK_AccountRate_Id", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccountStatusDM",
                columns: table => new
                {
                    AccountStatus = table.Column<string>(type: "varchar(24)", unicode: false, maxLength: 24, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Active = table.Column<bool?>(type: "bit", nullable: false, defaultValueSql: "((1))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountStatusDM", x => x.AccountStatus)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "AdditionalObligee",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    ObligeeNum = table.Column<string>(type: "varchar(7)", unicode: false, maxLength: 7, nullable: false),
                    PrintStatusLetter = table.Column<bool>(type: "bit", nullable: false),
                    Interest = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdditionalObligee", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "AddressTypeDM",
                columns: table => new
                {
                    Type = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressType", x => x.Type);
                });

            migrationBuilder.CreateTable(
                name: "AgencyStatusDM",
                columns: table => new
                {
                    Status = table.Column<string>(type: "varchar(24)", unicode: false, maxLength: 24, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgencyStatusDM", x => x.Status)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "AgentSystemDM",
                columns: table => new
                {
                    SystemName = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgentSystemDM", x => x.SystemName)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "AgreementTypeDM",
                columns: table => new
                {
                    Type = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgreementTypeDM", x => x.Type)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "AppUser",
                columns: table => new
                {
                    ActiveDirectoryAccount = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Initials = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((1))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUser", x => x.ActiveDirectoryAccount);
                });

            migrationBuilder.CreateTable(
                name: "BidPercentDM",
                columns: table => new
                {
                    BidPercent = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BidPercentDM", x => x.BidPercent)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "BidResultDM",
                columns: table => new
                {
                    Description = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BidResult", x => x.Description)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "BidRetainageDM",
                columns: table => new
                {
                    Retainage = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BidRetainageDM", x => x.Retainage)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "BidStatusDM",
                columns: table => new
                {
                    Status = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BidStatus", x => x.Status);
                });

            migrationBuilder.CreateTable(
                name: "BondModTransaction",
                columns: table => new
                {
                    BondNumber = table.Column<string>(type: "varchar(12)", unicode: false, maxLength: 12, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    BondMod = table.Column<int>(type: "int", nullable: false),
                    Effective = table.Column<DateTime>(type: "date", nullable: false),
                    Expiration = table.Column<DateTime>(type: "date", nullable: false),
                    ModPremium = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BondModTransaction", x => x.BondNumber)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "BondTypeDM",
                columns: table => new
                {
                    BondType = table.Column<string>(type: "varchar(12)", unicode: false, maxLength: 12, nullable: false),
                    BondClass = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Description = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    Class = table.Column<string>(type: "varchar(3)", unicode: false, maxLength: 3, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((1))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BondType", x => new { x.BondType, x.BondClass });
                    table.UniqueConstraint("AK_BondTypeDM_Id", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BookRatio",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Name = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    LowerQuartileStated = table.Column<double>(type: "float", nullable: false),
                    MedianStated = table.Column<double>(type: "float", nullable: false),
                    UpperQuartileStated = table.Column<double>(type: "float", nullable: false),
                    LowerQuartileAllowed = table.Column<double>(type: "float", nullable: false),
                    MedianAllowed = table.Column<double>(type: "float", nullable: false),
                    UpperQuartileAllowed = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookRatio", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "BusinessTypeClassCodeDM",
                columns: table => new
                {
                    BusinessType = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    ClassCode = table.Column<int>(type: "int", nullable: false),
                    IsContract = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessTypeClassCodeDM", x => x.BusinessType)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "BusinessTypeDM",
                columns: table => new
                {
                    BusinessType = table.Column<string>(type: "varchar(24)", unicode: false, maxLength: 24, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessTypeDM", x => x.BusinessType)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "CoInsurer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    BondNumber = table.Column<string>(type: "varchar(12)", unicode: false, maxLength: 12, nullable: false),
                    InsurerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Percentage = table.Column<double>(type: "float", nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoInsurer", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "CollateralTypeDM",
                columns: table => new
                {
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Active = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((1))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollateralTypeDM", x => x.Type)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "CommercialBondTypeDM",
                columns: table => new
                {
                    CommercialBondType = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommercialBondTypeDM", x => x.CommercialBondType)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "CommercialRegionDM",
                columns: table => new
                {
                    Region = table.Column<string>(type: "varchar(25)", unicode: false, maxLength: 25, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommercialRegionDM", x => x.Region)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "Competition",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Competition", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "CountryDM",
                columns: table => new
                {
                    Code = table.Column<string>(type: "char(2)", unicode: false, fixedLength: true, maxLength: 2, nullable: false),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Name = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "CreditReportDM",
                columns: table => new
                {
                    CreditReport = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditReportDM", x => x.CreditReport)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "DivisionDM",
                columns: table => new
                {
                    DivisionCode = table.Column<string>(type: "char(4)", unicode: false, fixedLength: true, maxLength: 4, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Division = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    LOANotificationGroup = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((1))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DivisionDM", x => x.DivisionCode);
                });

            migrationBuilder.CreateTable(
                name: "DocumentDataMissingAction",
                columns: table => new
                {
                    Action = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentDataMissingAction", x => x.Action);
                    table.UniqueConstraint("AK_DocumentDataMissingAction_Id", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DocumentDefinition",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    IsActiveGIAForm = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentDefinition", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "DocumentRule",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Condition = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    OutputDefinition = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    ForEach = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentRule", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "EmailActionDM",
                columns: table => new
                {
                    Action = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailActionDM", x => x.Action)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "FinancialAccountTypeDM",
                columns: table => new
                {
                    AccountType = table.Column<string>(type: "char(2)", unicode: false, fixedLength: true, maxLength: 2, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Title = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialAccountType", x => x.AccountType);
                });

            migrationBuilder.CreateTable(
                name: "FinancialRatio",
                columns: table => new
                {
                    AccountNum = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: false),
                    StatementDate = table.Column<DateTime>(type: "date", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    CurrentStated = table.Column<double>(type: "float", nullable: true),
                    QuickStated = table.Column<double>(type: "float", nullable: true),
                    FixedAsssets2NetWorthStated = table.Column<double>(type: "float", nullable: true),
                    GAExpenses2SalesStated = table.Column<double>(type: "float", nullable: true),
                    ARAPBalanceStated = table.Column<double>(type: "float", nullable: true),
                    UnderBills2SalesStated = table.Column<double>(type: "float", nullable: true),
                    DaysInCashStated = table.Column<double>(type: "float", nullable: true),
                    CurrentAllowed = table.Column<double>(type: "float", nullable: true),
                    QuickAllowed = table.Column<double>(type: "float", nullable: true),
                    WorkingCapital2AggregateProgram = table.Column<double>(type: "float", nullable: true),
                    FixedAsssets2NetWorthAllowed = table.Column<double>(type: "float", nullable: true),
                    GAExpenses2SalesAllowed = table.Column<double>(type: "float", nullable: true),
                    ARAPBalanceAllowed = table.Column<double>(type: "float", nullable: true),
                    UnderBills2SalesAllowed = table.Column<double>(type: "float", nullable: true),
                    DaysInCashAllowed = table.Column<double>(type: "float", nullable: true),
                    APPayableDays = table.Column<double>(type: "float", nullable: true),
                    ARCollectionDays = table.Column<double>(type: "float", nullable: true),
                    ARCollectionDatsNoReturn = table.Column<double>(type: "float", nullable: true),
                    BillingTurnoverDays = table.Column<double>(type: "float", nullable: true),
                    InventoryTurnoverDays = table.Column<double>(type: "float", nullable: true),
                    Sales2Equity = table.Column<double>(type: "float", nullable: true),
                    Sales2TotalAssets = table.Column<double>(type: "float", nullable: true),
                    Sales2WorkingCapital = table.Column<double>(type: "float", nullable: true),
                    UnderBills2Equity = table.Column<double>(type: "float", nullable: true),
                    UnderBills2WorkingCapital = table.Column<double>(type: "float", nullable: true),
                    TermDebt2Equity = table.Column<double>(type: "float", nullable: true),
                    TermDebt2WorkingCapital = table.Column<double>(type: "float", nullable: true),
                    GrossProfitMargin = table.Column<double>(type: "float", nullable: true),
                    NetProfitMargin = table.Column<double>(type: "float", nullable: true),
                    OperatingProfitMargin = table.Column<double>(type: "float", nullable: true),
                    ReturnOnAssets = table.Column<double>(type: "float", nullable: true),
                    ReturnOnEquity = table.Column<double>(type: "float", nullable: true),
                    ReturnNetRetained = table.Column<double>(type: "float", nullable: true),
                    SingloProgram2LargestJob = table.Column<double>(type: "float", nullable: true),
                    AggregateProgram2LargetBacking = table.Column<double>(type: "float", nullable: true),
                    AggregateProgram2Sales = table.Column<double>(type: "float", nullable: true),
                    AggregateProgram2LargetBacklog = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialRatios", x => new { x.AccountNum, x.StatementDate })
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "HomeOfficeEmailTeam",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Team = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: false),
                    Description = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeOfficeEmailTeam", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "ImagingCategory",
                columns: table => new
                {
                    Category = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImagingCategory", x => x.Category)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "ImagingTab",
                columns: table => new
                {
                    TabName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Description = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImagingTab", x => x.TabName)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "ImagingType",
                columns: table => new
                {
                    Type = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Description = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImagingType", x => x.Type)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "InventoryDocumentDM",
                columns: table => new
                {
                    DocumentType = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryDocumentDM", x => x.DocumentType)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "LegalEntityTypeDM",
                columns: table => new
                {
                    EntityType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegalEntityType", x => x.EntityType);
                });

            migrationBuilder.CreateTable(
                name: "LicenseStatusDM",
                columns: table => new
                {
                    Status = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenseStatusDM", x => x.Status)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "LineOfAuthorityStatusDM",
                columns: table => new
                {
                    Status = table.Column<string>(type: "varchar(12)", unicode: false, maxLength: 12, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LineOfAuthorityStatusDM", x => x.Status)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "LineOfBusinessDM",
                columns: table => new
                {
                    LineOfBusiness = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LineOfBusinessDM", x => x.LineOfBusiness)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "NAICSCode",
                columns: table => new
                {
                    Code = table.Column<string>(type: "varchar(6)", unicode: false, maxLength: 6, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Sector = table.Column<string>(type: "varchar(80)", unicode: false, maxLength: 80, nullable: false),
                    Description = table.Column<string>(type: "varchar(80)", unicode: false, maxLength: 80, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NAICSCode", x => x.Code)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "Notebook",
                schema: "Beta",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    SourceTable = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notebook", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "NotebookEntryTypeDM",
                schema: "Beta",
                columns: table => new
                {
                    Type = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotebookEntryTypeDM", x => x.Type)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "NoteTypeDM",
                columns: table => new
                {
                    NoteType = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteTypeDM", x => x.NoteType)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "ObligeeTypeDM",
                columns: table => new
                {
                    Type = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObligeeTypeDM", x => x.Type)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationTypeDM",
                columns: table => new
                {
                    Type = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationTypeDM", x => x.Type)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "PermissionRole",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Role = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionRole", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "PersonalFinancialStatementTypeDM",
                columns: table => new
                {
                    Type = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalFinancialStatementTypeDM", x => x.Type)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "PhoneNumber",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    CountryCode = table.Column<string>(type: "varchar(3)", unicode: false, maxLength: 3, nullable: false, defaultValueSql: "('1')"),
                    MainNumber = table.Column<string>(type: "varchar(12)", unicode: false, maxLength: 12, nullable: false),
                    Extension = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhoneNumber", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "PhoneTypeDM",
                columns: table => new
                {
                    Type = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhoneType", x => x.Type);
                });

            migrationBuilder.CreateTable(
                name: "PowerOfAttorneyDocumentNameDM",
                columns: table => new
                {
                    Name = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PowerOfAttorneyDocumentTypeDM", x => x.Name)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "PowerOfAttorneyStatusDM",
                columns: table => new
                {
                    Status = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PowerOfAttourneyStatusDM", x => x.Status)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "ProducerInvoiceEmail",
                columns: table => new
                {
                    PNumber = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    InvoiceEmail = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProducerInvoiceEmail", x => x.PNumber);
                });

            migrationBuilder.CreateTable(
                name: "RateGroup",
                columns: table => new
                {
                    RateGroup = table.Column<string>(type: "varchar(4)", unicode: false, maxLength: 4, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Description = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RateGroup", x => x.RateGroup);
                });

            migrationBuilder.CreateTable(
                name: "RateStructureDM",
                columns: table => new
                {
                    RateStructure = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RateStructureDM", x => x.RateStructure);
                });

            migrationBuilder.CreateTable(
                name: "ReferenceTypeDM",
                columns: table => new
                {
                    Type = table.Column<string>(type: "char(3)", unicode: false, fixedLength: true, maxLength: 3, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReferenceTypeDM", x => x.Type);
                });

            migrationBuilder.CreateTable(
                name: "RegionDM",
                columns: table => new
                {
                    Region = table.Column<string>(type: "char(2)", unicode: false, fixedLength: true, maxLength: 2, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Region", x => x.Region);
                });

            migrationBuilder.CreateTable(
                name: "RenewalRequest",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    BasisBondNumber = table.Column<string>(type: "varchar(25)", unicode: false, maxLength: 25, nullable: false),
                    RequestedBy = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    SendEmailTo = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    RenewalStarted = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataUsed = table.Column<string>(type: "xml", nullable: true),
                    EmailSent = table.Column<DateTime>(type: "datetime", nullable: true),
                    RenewalCompleted = table.Column<DateTime>(type: "datetime", nullable: true),
                    CancelReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PolMod = table.Column<int>(type: "int", nullable: false),
                    UnderWriterNotified = table.Column<DateTime>(type: "datetime", nullable: true),
                    RenewalRequestSourceID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BondChangeReportUploaded = table.Column<DateTime>(type: "datetime", nullable: true),
                    UploadError = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BondChangeReportEmailed = table.Column<DateTime>(type: "datetime", nullable: true),
                    EmailError = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BondChangeReportToEmail = table.Column<string>(type: "nvarchar(1048)", maxLength: 1048, nullable: true),
                    AccountName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RenewalRequest", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "RenewalRequestSource",
                columns: table => new
                {
                    Source = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RenewalRequestSource", x => x.Source);
                });

            migrationBuilder.CreateTable(
                name: "ResponsibilityDM",
                columns: table => new
                {
                    Responsibility = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResponsibilityDM", x => x.Responsibility);
                });

            migrationBuilder.CreateTable(
                name: "ResponsiblePartyTypeDM",
                columns: table => new
                {
                    Type = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResponsiblePartyTypeDM", x => x.Type)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "RiskTypeDM",
                columns: table => new
                {
                    RiskType = table.Column<string>(type: "varchar(16)", unicode: false, maxLength: 16, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskTypeDM", x => x.RiskType)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "ScalingDM",
                columns: table => new
                {
                    Scaling = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScalingDM", x => x.Scaling)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "SFAA",
                columns: table => new
                {
                    Code = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    General = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RateClass = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: true),
                    RiskType = table.Column<string>(type: "char(14)", unicode: false, fixedLength: true, maxLength: 14, nullable: true),
                    NMLSClassCode = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SAA", x => x.Code)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "SFAABondTypeDM",
                columns: table => new
                {
                    SFAABondType = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SFAABondType", x => x.SFAABondType)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "SIC",
                columns: table => new
                {
                    Code = table.Column<string>(type: "varchar(4)", unicode: false, maxLength: 4, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    General = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    RiskLevel = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SIC", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "SICRatioTypeDM",
                columns: table => new
                {
                    Type = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SICRatioTypeDM", x => x.Type)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "StatementBasisDM",
                columns: table => new
                {
                    Basis = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatementBasisDM", x => x.Basis)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "StatementQualityDM",
                columns: table => new
                {
                    Quality = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatementQualityDM", x => x.Quality)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "StatementTypeDM",
                columns: table => new
                {
                    Type = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatementTypeDM", x => x.Type)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "SurchargeTypeDM",
                columns: table => new
                {
                    Type = table.Column<string>(type: "varchar(14)", unicode: false, maxLength: 14, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurchargeTypeDM", x => x.Type)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "SystemNameDM",
                columns: table => new
                {
                    SystemName = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemNameDM", x => x.SystemName)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "TaxBasisDM",
                columns: table => new
                {
                    TaxBasis = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxBasisDM", x => x.TaxBasis)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "TicketTasks",
                columns: table => new
                {
                    TicketId = table.Column<int>(type: "int", nullable: false),
                    TicketDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false),
                    refTicket = table.Column<int>(type: "int", nullable: true),
                    TaskCategory = table.Column<int>(type: "int", nullable: true),
                    TaskCategoryTitle = table.Column<int>(type: "int", nullable: true),
                    TaskOrder = table.Column<int>(type: "int", nullable: true),
                    TaskDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaskTime = table.Column<double>(type: "float", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketTasks", x => x.TicketId);
                });

            migrationBuilder.CreateTable(
                name: "UserLayoutColumn",
                schema: "Beta",
                columns: table => new
                {
                    Username = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ColumnOrder = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    PrimaryScreen = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLayoutColumn", x => new { x.Username, x.ColumnOrder })
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "UserMenu",
                schema: "Beta",
                columns: table => new
                {
                    Username = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    MenuName = table.Column<string>(type: "varchar(25)", unicode: false, maxLength: 25, nullable: false),
                    MenuOrder = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Text = table.Column<string>(type: "varchar(35)", unicode: false, maxLength: 35, nullable: false),
                    Icon = table.Column<string>(type: "varchar(35)", unicode: false, maxLength: 35, nullable: true),
                    UrlPattern = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    MinimalDisplay = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMenu", x => new { x.Username, x.MenuName, x.MenuOrder })
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "UserPreference",
                schema: "Beta",
                columns: table => new
                {
                    Username = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Key = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPreference", x => new { x.Username, x.Key })
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "UserProfile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Initials = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(46)", maxLength: 46, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    PrimaryBranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Username = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Underwriter = table.Column<bool>(type: "bit", nullable: false),
                    HomeOfficeApprover = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfile", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.UniqueConstraint("AK_UserProfile_Initials", x => x.Initials);
                });

            migrationBuilder.CreateTable(
                name: "WatchStatusDM",
                columns: table => new
                {
                    WatchStatus = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WatchStatusDM", x => x.WatchStatus)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "WorkInProgressSummary",
                columns: table => new
                {
                    AccountNum = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: false),
                    WIPDate = table.Column<DateTime>(type: "date", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    ContractPrice = table.Column<long>(type: "bigint", nullable: false),
                    EstimatedCost = table.Column<long>(type: "bigint", nullable: false),
                    EstimatedGrossProfit = table.Column<long>(type: "bigint", nullable: false),
                    PercentComplete = table.Column<double>(type: "float", nullable: false),
                    EarnedRevenue = table.Column<long>(type: "bigint", nullable: false),
                    CostToDate = table.Column<long>(type: "bigint", nullable: false),
                    GrossProfit = table.Column<long>(type: "bigint", nullable: false),
                    GrossProfitPercent = table.Column<decimal>(type: "numeric(38,15)", nullable: true, computedColumnSql: "(case when [ContractPrice]=(0) OR [EstimatedCost]=(0) then (0) else ((100.0)*[EstimatedGrossProfit])/[ContractPrice] end)", stored: false),
                    ProgressBillings = table.Column<long>(type: "bigint", nullable: true),
                    UnderBillings = table.Column<long>(type: "bigint", nullable: false),
                    OverBillings = table.Column<long>(type: "bigint", nullable: false),
                    RevenueRemaining = table.Column<long>(type: "bigint", nullable: false),
                    CostToComplete = table.Column<long>(type: "bigint", nullable: false),
                    TotalEstimatedGrossProfit = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkInProgressSummary", x => new { x.AccountNum, x.WIPDate })
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "DefaultGeneralLedgerAccount",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    AccountName = table.Column<string>(type: "varchar(45)", unicode: false, maxLength: 45, nullable: false),
                    AccountClass = table.Column<string>(type: "varchar(4)", unicode: false, maxLength: 4, nullable: false),
                    AccountType = table.Column<string>(type: "char(2)", unicode: false, fixedLength: true, maxLength: 2, nullable: false),
                    FundedDebt = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DefaultGeneralLedgerAccount", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_DefaultGeneralLedgerAccount_AccountClassDM",
                        column: x => x.AccountClass,
                        principalTable: "AccountClassDM",
                        principalColumn: "AccountClass");
                });

            migrationBuilder.CreateTable(
                name: "AccountRateAttachment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    AccountRateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Attachment = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountRateAttachment", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_AccountRateAttachment_AccountRate",
                        column: x => x.AccountRateId,
                        principalTable: "AccountRate",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AccountStatusLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    AccountNum = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: false),
                    AccountStatus = table.Column<string>(type: "varchar(24)", unicode: false, maxLength: 24, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueid", unicode: false, nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountStatusLog", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_AccountStatusLog_AccountStatusDM",
                        column: x => x.AccountStatus,
                        principalTable: "AccountStatusDM",
                        principalColumn: "AccountStatus");
                });

            migrationBuilder.CreateTable(
                name: "State",
                columns: table => new
                {
                    Code = table.Column<string>(type: "char(2)", unicode: false, fixedLength: true, maxLength: 2, nullable: false),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SAACode = table.Column<string>(type: "char(2)", unicode: false, fixedLength: true, maxLength: 2, nullable: true),
                    WRBStateCode = table.Column<string>(type: "char(2)", unicode: false, fixedLength: true, maxLength: 2, nullable: true),
                    CountryCode = table.Column<string>(type: "char(2)", unicode: false, fixedLength: true, maxLength: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_State", x => x.Code);
                    table.ForeignKey(
                        name: "FK_State_Country",
                        column: x => x.CountryCode,
                        principalTable: "CountryDM",
                        principalColumn: "Code");
                });

            migrationBuilder.CreateTable(
                name: "DocumentDefinitionRule",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    DefinitionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RuleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentDefinitionRule", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_DocumentDefinitionRule_DocumentDefinition",
                        column: x => x.DefinitionId,
                        principalTable: "DocumentDefinition",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DocumentDefinitionRule_DocumentRule",
                        column: x => x.RuleId,
                        principalTable: "DocumentRule",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DocumentRuleReplacementMap",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    RuleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Token = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    ValuePath = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    IsMissingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentRuleReplacementMap", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_DocumentRuleReplacementMap_DocumentDataMissingAction",
                        column: x => x.IsMissingId,
                        principalTable: "DocumentDataMissingAction",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DocumentRuleReplacementMap_DocumentRule",
                        column: x => x.RuleId,
                        principalTable: "DocumentRule",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ImagingCategoryTabDivision",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Category = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    TabName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    DivisionCode = table.Column<string>(type: "char(4)", unicode: false, fixedLength: true, maxLength: 4, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImagingCategoryTabDivision", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_ImagingCategoryTabDivision_DivisionDM",
                        column: x => x.DivisionCode,
                        principalTable: "DivisionDM",
                        principalColumn: "DivisionCode");
                    table.ForeignKey(
                        name: "FK_ImagingCategoryTabDivision_ImagingCategory",
                        column: x => x.Category,
                        principalTable: "ImagingCategory",
                        principalColumn: "Category");
                    table.ForeignKey(
                        name: "FK_ImagingCategoryTabDivision_ImagingTab",
                        column: x => x.TabName,
                        principalTable: "ImagingTab",
                        principalColumn: "TabName");
                });

            migrationBuilder.CreateTable(
                name: "LegalEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Parent = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    GivenName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FamilyName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MiddleInitial = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: true),
                    EntityType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Website = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IsIndividual = table.Column<bool>(type: "bit", nullable: false),
                    TaxIdEncrypted = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegalEntity", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_LegalEntity_LegalEntity",
                        column: x => x.Parent,
                        principalTable: "LegalEntity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LegalEntity_LegalEntityType",
                        column: x => x.EntityType,
                        principalTable: "LegalEntityTypeDM",
                        principalColumn: "EntityType");
                });

            migrationBuilder.CreateTable(
                name: "OrganizationTitle",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Type = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Title = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationTitle", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_OrganizationTitle_OrganizationTypeDM",
                        column: x => x.Type,
                        principalTable: "OrganizationTypeDM",
                        principalColumn: "Type");
                });

            migrationBuilder.CreateTable(
                name: "ContractRate",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    RateGroup = table.Column<string>(type: "varchar(4)", unicode: false, maxLength: 4, nullable: false),
                    Class = table.Column<string>(type: "varchar(4)", unicode: false, maxLength: 4, nullable: false),
                    RateType = table.Column<string>(type: "varchar(12)", unicode: false, maxLength: 12, nullable: false),
                    MinumumAmount = table.Column<int>(type: "int", nullable: false),
                    MaximumAmount = table.Column<int>(type: "int", nullable: false),
                    PremiumRate = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractRate", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_ContractRate_RateGroup",
                        column: x => x.RateGroup,
                        principalTable: "RateGroup",
                        principalColumn: "RateGroup");
                });

            migrationBuilder.CreateTable(
                name: "ResponsibleParty",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Type = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    PrintStatusLetter = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DesignatedParty", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_ResponsibleParty_ResponsiblePartyTypeDM",
                        column: x => x.Type,
                        principalTable: "ResponsiblePartyTypeDM",
                        principalColumn: "Type");
                });

            migrationBuilder.CreateTable(
                name: "CommercialRate",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    RateGroup = table.Column<string>(type: "varchar(4)", unicode: false, maxLength: 4, nullable: false),
                    CommercialBondType = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    RiskType = table.Column<string>(type: "varchar(16)", unicode: false, maxLength: 16, nullable: false),
                    MinumumAmount = table.Column<int>(type: "int", nullable: false),
                    MaximumAmount = table.Column<int>(type: "int", nullable: false),
                    AmountPerUnit = table.Column<double>(type: "float", nullable: true),
                    UnitSize = table.Column<int>(type: "int", nullable: true),
                    AnnualMinimum = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommercialRate", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_CommercialRate_CommercialBondTypeDM",
                        column: x => x.CommercialBondType,
                        principalTable: "CommercialBondTypeDM",
                        principalColumn: "CommercialBondType");
                    table.ForeignKey(
                        name: "FK_CommercialRate_RateGroup",
                        column: x => x.RateGroup,
                        principalTable: "RateGroup",
                        principalColumn: "RateGroup");
                    table.ForeignKey(
                        name: "FK_CommercialRate_RiskTypeDM",
                        column: x => x.RiskType,
                        principalTable: "RiskTypeDM",
                        principalColumn: "RiskType");
                });

            migrationBuilder.CreateTable(
                name: "SICRatio",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Code = table.Column<string>(type: "varchar(4)", unicode: false, maxLength: 4, nullable: false),
                    Type = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    UpperQuartileStated = table.Column<double>(type: "float", nullable: false),
                    MedianStated = table.Column<double>(type: "float", nullable: false),
                    LowerQuartileStated = table.Column<double>(type: "float", nullable: true),
                    UpperQuartileAllowed = table.Column<double>(type: "float", nullable: false),
                    MedianAllowed = table.Column<double>(type: "float", nullable: false),
                    LowerQuartileAllowed = table.Column<double>(type: "float", nullable: true),
                    SampleSize = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SICRatio", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_SICRatio_SIC",
                        column: x => x.Code,
                        principalTable: "SIC",
                        principalColumn: "Code");
                    table.ForeignKey(
                        name: "FK_SICRatio_SICRatioTypeDM",
                        column: x => x.Type,
                        principalTable: "SICRatioTypeDM",
                        principalColumn: "Type");
                });

            migrationBuilder.CreateTable(
                name: "UserLayoutWidget",
                schema: "Beta",
                columns: table => new
                {
                    Username = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ColumnOrder = table.Column<int>(type: "int", nullable: false),
                    WidgetOrder = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    WidgetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLayoutWidget", x => new { x.Username, x.ColumnOrder, x.WidgetOrder })
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_UserLayoutWidget_UserLayoutColumn",
                        columns: x => new { x.Username, x.ColumnOrder },
                        principalSchema: "Beta",
                        principalTable: "UserLayoutColumn",
                        principalColumns: new[] { "Username", "ColumnOrder" });
                });

            migrationBuilder.CreateTable(
                name: "AgencyStatusLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    AccountNum = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: false),
                    Effective = table.Column<DateTime>(type: "datetime", nullable: false),
                    OldStatus = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true),
                    NewStatus = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true),
                    ChangedBy = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgencyStatusLog", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_AgencyStatusLog_UserProfile",
                        column: x => x.ChangedBy,
                        principalTable: "UserProfile",
                        principalColumn: "Initials");
                });

            migrationBuilder.CreateTable(
                name: "BondBlock",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Prefix = table.Column<string>(type: "varchar(4)", unicode: false, maxLength: 4, nullable: false),
                    FirstNumber = table.Column<int>(type: "int", nullable: false),
                    LastNumber = table.Column<int>(type: "int", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    AgencyRestricted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((1))"),
                    IssuedBy = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BondBlock", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_BondBlock_UserProfile",
                        column: x => x.IssuedBy,
                        principalTable: "UserProfile",
                        principalColumn: "Initials");
                });

            migrationBuilder.CreateTable(
                name: "LineOAuthorityLog",
                columns: table => new
                {
                    AccountNum = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    SequenceNumber = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Effective = table.Column<DateTime>(type: "datetime", nullable: false),
                    Expiration = table.Column<DateTime>(type: "datetime", nullable: false),
                    LOASingle = table.Column<int>(type: "int", nullable: false),
                    LOAAggregate = table.Column<int>(type: "int", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "varchar(12)", unicode: false, maxLength: 12, nullable: true),
                    ApprovedBy = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    Approved = table.Column<DateTime>(type: "datetime", nullable: true),
                    Division = table.Column<string>(type: "char(4)", unicode: false, fixedLength: true, maxLength: 4, nullable: true),
                    BondType = table.Column<string>(type: "varchar(12)", unicode: false, maxLength: 12, nullable: true),
                    Conditions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HomeOfficeApproved = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LineOFAuthorityLog", x => x.AccountNum)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_LineOfAuthorityLog_DivisionDM",
                        column: x => x.Division,
                        principalTable: "DivisionDM",
                        principalColumn: "DivisionCode");
                    table.ForeignKey(
                        name: "FK_LineOfAuthorityLog_LineOfAuthorityStatusDM",
                        column: x => x.Status,
                        principalTable: "LineOfAuthorityStatusDM",
                        principalColumn: "Status");
                    table.ForeignKey(
                        name: "FK_LineOfAuthorityLog_UserProfile",
                        column: x => x.ApprovedBy,
                        principalTable: "UserProfile",
                        principalColumn: "Initials");
                    table.ForeignKey(
                        name: "FK_LineOfAuthorityLog_UserProfile_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "UserProfile",
                        principalColumn: "Initials");
                });

            migrationBuilder.CreateTable(
                name: "NotebookEntry",
                schema: "Beta",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    NotebookId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(18)", maxLength: 18, nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Completed = table.Column<bool>(type: "bit", nullable: false),
                    Completion = table.Column<DateTime>(type: "datetime", nullable: true),
                    ReminderTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    ColumnName = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotebookEntry", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_NotebookEntry_Notebook",
                        column: x => x.NotebookId,
                        principalSchema: "Beta",
                        principalTable: "Notebook",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NotebookEntry_NotebookEntryTypeDM",
                        column: x => x.Type,
                        principalSchema: "Beta",
                        principalTable: "NotebookEntryTypeDM",
                        principalColumn: "Type");
                    table.ForeignKey(
                        name: "FK_NotebookEntry_UserProfile",
                        column: x => x.CreatedBy,
                        principalTable: "UserProfile",
                        principalColumn: "Initials");
                });

            migrationBuilder.CreateTable(
                name: "ProfitCenter",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    ProfitCenter = table.Column<string>(type: "varchar(3)", unicode: false, maxLength: 3, nullable: false),
                    DivisionCode = table.Column<string>(type: "char(4)", unicode: false, fixedLength: true, maxLength: 4, nullable: false),
                    LineOfBusiness = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Location = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: false),
                    LocationOverview = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: false),
                    BudgetDefault = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Underwriter = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    Effective = table.Column<DateTime>(type: "date", nullable: false),
                    Expiration = table.Column<DateTime>(type: "date", nullable: false),
                    CommercialRegion = table.Column<string>(type: "varchar(25)", unicode: false, maxLength: 25, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfitCenter", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_ProfitCenter_CommercialRegionDM",
                        column: x => x.CommercialRegion,
                        principalTable: "CommercialRegionDM",
                        principalColumn: "Region");
                    table.ForeignKey(
                        name: "FK_ProfitCenter_DivisionDM",
                        column: x => x.DivisionCode,
                        principalTable: "DivisionDM",
                        principalColumn: "DivisionCode");
                    table.ForeignKey(
                        name: "FK_ProfitCenter_LineOfBusinessDM",
                        column: x => x.LineOfBusiness,
                        principalTable: "LineOfBusinessDM",
                        principalColumn: "LineOfBusiness");
                    table.ForeignKey(
                        name: "FK_ProfitCenter_ProfitCenter",
                        column: x => x.BudgetDefault,
                        principalTable: "ProfitCenter",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProfitCenter_UserProfile",
                        column: x => x.Underwriter,
                        principalTable: "UserProfile",
                        principalColumn: "Initials");
                });

            migrationBuilder.CreateTable(
                name: "UserLineOfAuthority",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Single = table.Column<int>(type: "int", nullable: false),
                    Aggregate = table.Column<int>(type: "int", nullable: false),
                    Expiration = table.Column<DateTime>(type: "date", nullable: false),
                    DivisionCode = table.Column<string>(type: "char(4)", unicode: false, fixedLength: true, maxLength: 4, nullable: false),
                    BondType = table.Column<string>(type: "varchar(12)", unicode: false, maxLength: 12, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLineOfAuthority", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_UserLineOfAuthority_UserProfile",
                        column: x => x.UserId,
                        principalTable: "UserProfile",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserLineOfAuthority_UserProfile_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "UserProfile",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserLineOfAuthority_UserProfile_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "UserProfile",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Address1 = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false, defaultValueSql: "('')"),
                    Address2 = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    Address3 = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StateCode = table.Column<string>(type: "char(2)", unicode: false, fixedLength: true, maxLength: 2, nullable: true),
                    PostalCode = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_Address_State",
                        column: x => x.StateCode,
                        principalTable: "State",
                        principalColumn: "Code");
                });

            migrationBuilder.CreateTable(
                name: "Surcharge",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    State = table.Column<string>(type: "char(2)", unicode: false, fixedLength: true, maxLength: 2, nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    Effective = table.Column<DateTime>(type: "date", nullable: false),
                    Expiration = table.Column<DateTime>(type: "date", nullable: false),
                    Exclude = table.Column<bool>(type: "bit", nullable: false),
                    Type = table.Column<string>(type: "varchar(14)", unicode: false, maxLength: 14, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Surcharge", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_Surcharge_State",
                        column: x => x.State,
                        principalTable: "State",
                        principalColumn: "Code");
                    table.ForeignKey(
                        name: "FK_Surcharge_SurchargeTypeDM",
                        column: x => x.Type,
                        principalTable: "SurchargeTypeDM",
                        principalColumn: "Type");
                    table.ForeignKey(
                        name: "FK_Surcharge_UserProfile",
                        column: x => x.CreatedBy,
                        principalTable: "UserProfile",
                        principalColumn: "Initials");
                });

            migrationBuilder.CreateTable(
                name: "AccountReference",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Type = table.Column<string>(type: "char(3)", unicode: false, fixedLength: true, maxLength: 3, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountReference", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_AccountReference_LegalEntity",
                        column: x => x.Id,
                        principalTable: "LegalEntity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AccountReference_ReferenceTypeDM",
                        column: x => x.Type,
                        principalTable: "ReferenceTypeDM",
                        principalColumn: "Type");
                });

            migrationBuilder.CreateTable(
                name: "Agency",
                columns: table => new
                {
                    AgencyNumber = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Branch = table.Column<string>(type: "char(3)", unicode: false, fixedLength: true, maxLength: 3, nullable: true),
                    Status = table.Column<string>(type: "varchar(24)", unicode: false, maxLength: 24, nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    NASBP = table.Column<bool>(type: "bit", nullable: false),
                    W9 = table.Column<bool>(type: "bit", nullable: false),
                    ProfitSharing = table.Column<bool>(type: "bit", nullable: false),
                    ProfitSharingMinimumPremium = table.Column<int>(type: "int", nullable: true),
                    BillingContactId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NationalProducerNumber = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    ErrorsAndOmmissionsCarrier = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    ErrorsAndOmmissionsExpiration = table.Column<DateTime>(type: "datetime", nullable: true),
                    ErrorsAndOmmissionsCoverage = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agency", x => x.AgencyNumber);
                    table.UniqueConstraint("AK_Agency_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Agency_AgencyStatusDM",
                        column: x => x.Status,
                        principalTable: "AgencyStatusDM",
                        principalColumn: "Status");
                    table.ForeignKey(
                        name: "FK_Agency_LegalEntity",
                        column: x => x.Id,
                        principalTable: "LegalEntity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Agency_LegalEntity_BillingContact",
                        column: x => x.BillingContactId,
                        principalTable: "LegalEntity",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Agent",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    NationalProducerNumber = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    DefaultPhoneNumber = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DefaultCellNumber = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DefaultEMail = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agent", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_Agent_LegalEntity",
                        column: x => x.Id,
                        principalTable: "LegalEntity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Agent_PhoneNumber",
                        column: x => x.DefaultPhoneNumber,
                        principalTable: "PhoneNumber",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Agent_PhoneNumber_Cell",
                        column: x => x.DefaultCellNumber,
                        principalTable: "PhoneNumber",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AgentsInAgency",
                columns: table => new
                {
                    AgentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AgencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    AttorneyInFact = table.Column<bool>(type: "bit", nullable: false),
                    PortalUser = table.Column<bool>(type: "bit", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgentsInAgency", x => new { x.AgentId, x.AgencyId });
                    table.ForeignKey(
                        name: "FK_AgentsInAgency_AgencyId",
                        column: x => x.AgencyId,
                        principalTable: "LegalEntity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AgentsInAgency_AgentId",
                        column: x => x.AgentId,
                        principalTable: "LegalEntity",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Insurer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    DefaultRateGroup = table.Column<string>(type: "varchar(4)", unicode: false, maxLength: 4, nullable: true),
                    CurrencyCountry = table.Column<string>(type: "char(2)", unicode: false, fixedLength: true, maxLength: 2, nullable: false, defaultValueSql: "('US')"),
                    LENUM_PeopleSoft = table.Column<string>(type: "varchar(2)", unicode: false, maxLength: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Insurer", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_Insurer_Country",
                        column: x => x.CurrencyCountry,
                        principalTable: "CountryDM",
                        principalColumn: "Code");
                    table.ForeignKey(
                        name: "FK_Insurer_LegalEntity",
                        column: x => x.Id,
                        principalTable: "LegalEntity",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LawEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    MartindaleHubbellRating = table.Column<string>(type: "varchar(6)", unicode: false, maxLength: 6, nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LawFirm", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_LawFirm_LegalEntity",
                        column: x => x.Id,
                        principalTable: "LegalEntity",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LegalEntityPhone",
                columns: table => new
                {
                    LegalEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PhoneNumberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false, defaultValueSql: "('Main')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegalEntityPhone", x => new { x.LegalEntityId, x.PhoneNumberId })
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_LegalEntityPhone_LegalEntity",
                        column: x => x.LegalEntityId,
                        principalTable: "LegalEntity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LegalEntityPhone_PhoneNumber",
                        column: x => x.PhoneNumberId,
                        principalTable: "PhoneNumber",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LegalEntityPhone_PhoneType",
                        column: x => x.Type,
                        principalTable: "PhoneTypeDM",
                        principalColumn: "Type");
                });

            migrationBuilder.CreateTable(
                name: "Obligee",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    ObligeeNum = table.Column<string>(type: "varchar(7)", unicode: false, maxLength: 7, nullable: false),
                    Type = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    EditedBy = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    PrintStatusLetter = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Obligee", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_Obligee_LegalEntity",
                        column: x => x.Id,
                        principalTable: "LegalEntity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Obligee_ObligeeTypeDM",
                        column: x => x.Type,
                        principalTable: "ObligeeTypeDM",
                        principalColumn: "Type");
                });

            migrationBuilder.CreateTable(
                name: "PersonalFinancialHeader",
                columns: table => new
                {
                    AccountNum = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: false),
                    StatementDate = table.Column<DateTime>(type: "date", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Type = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    Basis = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    TaxBasis = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    Quality = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    Scaling = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    StatementFor = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalFinancialHeader", x => new { x.AccountNum, x.StatementDate })
                        .Annotation("SqlServer:Clustered", false);
                    table.UniqueConstraint("AK_PersonalFinancialHeader_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonalFinancialHeader_LegalEntity",
                        column: x => x.PersonId,
                        principalTable: "LegalEntity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PersonalFinancialHeader_PersonalFinancialStatementTypeDM",
                        column: x => x.Type,
                        principalTable: "PersonalFinancialStatementTypeDM",
                        principalColumn: "Type");
                    table.ForeignKey(
                        name: "FK_PersonalFinancialHeader_ScalingDM",
                        column: x => x.Scaling,
                        principalTable: "ScalingDM",
                        principalColumn: "Scaling");
                    table.ForeignKey(
                        name: "FK_PersonalFinancialHeader_StatementBasisDM",
                        column: x => x.Basis,
                        principalTable: "StatementBasisDM",
                        principalColumn: "Basis");
                    table.ForeignKey(
                        name: "FK_PersonalFinancialHeader_StatementQualityDM",
                        column: x => x.Quality,
                        principalTable: "StatementQualityDM",
                        principalColumn: "Quality");
                    table.ForeignKey(
                        name: "FK_PersonalFinancialHeader_TaxBasisDM",
                        column: x => x.TaxBasis,
                        principalTable: "TaxBasisDM",
                        principalColumn: "TaxBasis");
                });

            migrationBuilder.CreateTable(
                name: "Branch",
                columns: table => new
                {
                    BranchKey = table.Column<string>(type: "char(3)", unicode: false, fixedLength: true, maxLength: 3, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Name = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    BranchOfficeAddressId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PhoneId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HomeOffice = table.Column<bool>(type: "bit", nullable: false),
                    EmailReceiver = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    BCCReceiver = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    GeneralLedgerCode = table.Column<string>(type: "char(6)", unicode: false, fixedLength: true, maxLength: 6, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Region = table.Column<string>(type: "char(2)", unicode: false, fixedLength: true, maxLength: 2, nullable: false),
                    HeadCount = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branch", x => x.BranchKey)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_Branch_Address",
                        column: x => x.BranchOfficeAddressId,
                        principalTable: "Address",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Branch_PhoneNumber",
                        column: x => x.PhoneId,
                        principalTable: "PhoneNumber",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Branch_RegionDM",
                        column: x => x.Region,
                        principalTable: "RegionDM",
                        principalColumn: "Region");
                });

            migrationBuilder.CreateTable(
                name: "LegalEntityAddress",
                columns: table => new
                {
                    LegalEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddressId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false, defaultValueSql: "('Main')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegalEntityAddress", x => new { x.LegalEntityId, x.AddressId })
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_LegalEntityAddress_AddressType",
                        column: x => x.Type,
                        principalTable: "AddressTypeDM",
                        principalColumn: "Type");
                    table.ForeignKey(
                        name: "FK_LegalEntityAddress_LegalEntity",
                        column: x => x.LegalEntityId,
                        principalTable: "LegalEntity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LegalEntityAddresse_Address",
                        column: x => x.AddressId,
                        principalTable: "Address",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AgencyErrorAndOmission",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    AgencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Carrier = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Expiration = table.Column<DateTime>(type: "date", nullable: false),
                    CoverageLimit = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgencyErrorAndOmission", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_AgencyErrorAndOmmision_Agency",
                        column: x => x.AgencyId,
                        principalTable: "Agency",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AgencyInventory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    AgencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Sent = table.Column<DateTime>(type: "datetime", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    DocumentType = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    AddresseeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Approver = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgencyInventory", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_AgencyInventory_Agency",
                        column: x => x.AgencyId,
                        principalTable: "Agency",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AgencyInventory_InventoryDocumentDM",
                        column: x => x.DocumentType,
                        principalTable: "InventoryDocumentDM",
                        principalColumn: "DocumentType");
                    table.ForeignKey(
                        name: "FK_AgencyInventory_LegalEntity",
                        column: x => x.AddresseeId,
                        principalTable: "LegalEntity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AgencyInventory_UserProfile",
                        column: x => x.Approver,
                        principalTable: "UserProfile",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "VoidedBond",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    BondNumber = table.Column<string>(type: "varchar(12)", unicode: false, maxLength: 12, nullable: false),
                    AgencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VoidedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoidedBond", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_VoidedBond_Agency",
                        column: x => x.AgencyId,
                        principalTable: "Agency",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_VoidedBond_UserProfile",
                        column: x => x.VoidedBy,
                        principalTable: "UserProfile",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AgencyLicense",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    AgentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    State = table.Column<string>(type: "char(2)", unicode: false, fixedLength: true, maxLength: 2, nullable: false),
                    LicenseNumber = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    IsResident = table.Column<bool>(type: "bit", nullable: false),
                    InsurerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Expiration = table.Column<DateTime>(type: "date", nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    Appointment = table.Column<DateTime>(type: "date", nullable: true),
                    Termination = table.Column<DateTime>(type: "date", nullable: true),
                    AppointingState = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgencyLicense", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_AgencyLicense_Insurer",
                        column: x => x.InsurerId,
                        principalTable: "Insurer",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AgencyLicense_LegalEntity",
                        column: x => x.Id,
                        principalTable: "LegalEntity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AgencyLicense_LegalEntity_AgentId",
                        column: x => x.AgentId,
                        principalTable: "LegalEntity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AgencyLicense_State",
                        column: x => x.State,
                        principalTable: "State",
                        principalColumn: "Code");
                });

            migrationBuilder.CreateTable(
                name: "InsurerState",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    InsurerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    State = table.Column<string>(type: "char(2)", unicode: false, fixedLength: true, maxLength: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsurerState", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_InsurerState_Insurer",
                        column: x => x.InsurerId,
                        principalTable: "Insurer",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InsurerState_State",
                        column: x => x.State,
                        principalTable: "State",
                        principalColumn: "Code");
                });

            migrationBuilder.CreateTable(
                name: "PowerOfAttorney",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    InsurerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AgencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Limit = table.Column<int>(type: "int", nullable: false),
                    ReferenceNumber = table.Column<int>(type: "nvarchar(100)", nullable: false),
                    FirstIssued = table.Column<DateTime>(type: "date", nullable: true),
                    CurrentIssued = table.Column<DateTime>(type: "date", nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PowerOfAttorney", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_PowerOfAttorney_Insurer",
                        column: x => x.InsurerId,
                        principalTable: "Insurer",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PowerOfAttorney_PowerOfAttorneyStatusDM",
                        column: x => x.Status,
                        principalTable: "PowerOfAttorneyStatusDM",
                        principalColumn: "Status");
                });

            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    AccountNum = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    OpenClaim = table.Column<bool>(type: "bit", nullable: false),
                    YearOpened = table.Column<string>(type: "varchar(4)", unicode: false, maxLength: 4, nullable: true),
                    Branch = table.Column<string>(type: "char(3)", unicode: false, fixedLength: true, maxLength: 3, nullable: false),
                    AgencyNumber = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: true),
                    DunBradstreetRate = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    DunBradstreetDate = table.Column<DateTime>(type: "date", nullable: true),
                    DunBradstreetSIC = table.Column<string>(type: "varchar(4)", unicode: false, maxLength: 4, nullable: true),
                    Bank = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    BankReferenceName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    BankAverageBalance = table.Column<int>(type: "int", nullable: true),
                    BankPhoneId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BankLastContacted = table.Column<DateTime>(type: "date", nullable: true),
                    BankLOC = table.Column<int>(type: "int", nullable: true),
                    BankLOCExpires = table.Column<DateTime>(type: "date", nullable: true),
                    BankLOCUsed = table.Column<int>(type: "int", nullable: true),
                    BankLOCHigh = table.Column<int>(type: "int", nullable: true),
                    BankLOCHighDate = table.Column<DateTime>(type: "date", nullable: true),
                    BankLOCSecurity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PublicPercent = table.Column<double>(type: "float", nullable: true),
                    PrivatePercent = table.Column<double>(type: "float", nullable: true),
                    SubcontractPercent = table.Column<double>(type: "float", nullable: true),
                    SubcontractProtection = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    GeographicSpread = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PaydexIntelliscore = table.Column<int>(type: "int", nullable: true),
                    PriorSuretyCompany = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CurrentManagementYear = table.Column<string>(type: "varchar(4)", unicode: false, maxLength: 4, nullable: true),
                    IndemnityFull = table.Column<bool>(type: "bit", nullable: false),
                    IndemnityCorp = table.Column<bool>(type: "bit", nullable: false),
                    IndemnityPerson = table.Column<bool>(type: "bit", nullable: false),
                    BerkleyAffiliate = table.Column<bool>(type: "bit", nullable: false),
                    ContinuityKeyManagementLifeInsurance = table.Column<bool>(type: "bit", nullable: false),
                    ContinuityManagementIncentives = table.Column<bool>(type: "bit", nullable: false),
                    ContinuityFundedBuySell = table.Column<bool>(type: "bit", nullable: false),
                    ContinuityActiveMultipleOwners = table.Column<bool>(type: "bit", nullable: false),
                    FiscalYearEnd = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true),
                    TaxBasis = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    AgentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BusinessType = table.Column<string>(type: "varchar(24)", unicode: false, maxLength: 24, nullable: true),
                    Division = table.Column<string>(type: "char(4)", unicode: false, fixedLength: true, maxLength: 4, nullable: false),
                    RateModifier = table.Column<double>(type: "float", nullable: false),
                    LawFirmId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AttorneyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CPAFirmId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CPAContactId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Underwriter = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WatchStatus = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: false),
                    NAICS = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    HomeOfficeReviewBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HomeOfficeReviewed = table.Column<DateTime>(type: "datetime", nullable: true),
                    BranchReviewBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BranchReviewed = table.Column<DateTime>(type: "datetime", nullable: true),
                    HomeOfficeLock = table.Column<bool>(type: "bit", nullable: false),
                    BusinessTypeClass = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    FundsControl = table.Column<bool>(type: "bit", nullable: false),
                    PollutionLiabilityCarrier = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PollutionLiabilityExpires = table.Column<DateTime>(type: "date", nullable: true),
                    AffiliateCompany = table.Column<bool>(type: "bit", nullable: false),
                    CreditReport = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    EstimatingSystem = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    EstimatingSignoff = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    AccountingSystem = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    POCInterims = table.Column<bool>(type: "bit", nullable: false),
                    InterimWIPs = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.AccountNum);
                    table.ForeignKey(
                        name: "FK_Account_Agency",
                        column: x => x.AgencyNumber,
                        principalTable: "Agency",
                        principalColumn: "AgencyNumber");
                    table.ForeignKey(
                        name: "FK_Account_Branch",
                        column: x => x.Branch,
                        principalTable: "Branch",
                        principalColumn: "BranchKey");
                    table.ForeignKey(
                        name: "FK_Account_BusinessTypeClassCodeDM",
                        column: x => x.BusinessTypeClass,
                        principalTable: "BusinessTypeClassCodeDM",
                        principalColumn: "BusinessType");
                    table.ForeignKey(
                        name: "FK_Account_BusinessTypeDM",
                        column: x => x.BusinessType,
                        principalTable: "BusinessTypeDM",
                        principalColumn: "BusinessType");
                    table.ForeignKey(
                        name: "FK_Account_DivisionDM",
                        column: x => x.Division,
                        principalTable: "DivisionDM",
                        principalColumn: "DivisionCode");
                    table.ForeignKey(
                        name: "FK_Account_LawEntity_Attorney",
                        column: x => x.AttorneyId,
                        principalTable: "LawEntity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Account_LawEntity_LawFirm",
                        column: x => x.LawFirmId,
                        principalTable: "LawEntity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Account_LegalEntity",
                        column: x => x.Id,
                        principalTable: "LegalEntity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Account_LegalEntity_Agent",
                        column: x => x.AgentId,
                        principalTable: "LegalEntity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Account_LegalEntity_CPAContact",
                        column: x => x.CPAContactId,
                        principalTable: "LegalEntity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Account_LegalEntity_CPAFirm",
                        column: x => x.CPAFirmId,
                        principalTable: "LegalEntity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Account_PhoneNumber",
                        column: x => x.BankPhoneId,
                        principalTable: "PhoneNumber",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Account_TaxBasisDM",
                        column: x => x.TaxBasis,
                        principalTable: "TaxBasisDM",
                        principalColumn: "TaxBasis");
                    table.ForeignKey(
                        name: "FK_Account_UserProfile",
                        column: x => x.Underwriter,
                        principalTable: "UserProfile",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Account_UserProfile_BranchReviewBy",
                        column: x => x.BranchReviewBy,
                        principalTable: "UserProfile",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Account_UserProfile_HomeOfficeReviewBy",
                        column: x => x.HomeOfficeReviewBy,
                        principalTable: "UserProfile",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Account_WatchStatusDM",
                        column: x => x.WatchStatus,
                        principalTable: "WatchStatusDM",
                        principalColumn: "WatchStatus");
                });

            migrationBuilder.CreateTable(
                name: "OnlineBondSystem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    AgencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SystemName = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    InsurerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PowerOfAttorneyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnlineBondSystem", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_OnlineBondSystem_Agency",
                        column: x => x.AgencyId,
                        principalTable: "Agency",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OnlineBondSystem_AgentSystemDM",
                        column: x => x.SystemName,
                        principalTable: "AgentSystemDM",
                        principalColumn: "SystemName");
                    table.ForeignKey(
                        name: "FK_OnlineBondSystem_Insurer",
                        column: x => x.InsurerId,
                        principalTable: "Insurer",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OnlineBondSystem_PowerOfAttorney",
                        column: x => x.PowerOfAttorneyId,
                        principalTable: "PowerOfAttorney",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PowerOfAttorneyDocumentStatus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    POAId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Requested = table.Column<DateTime>(type: "date", nullable: true),
                    Received = table.Column<DateTime>(type: "date", nullable: true),
                    Name = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PowerOfAttorneyDocumentStatus", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_PowerOfAttorneyDocumentStatus_PowerOfAttorney",
                        column: x => x.POAId,
                        principalTable: "PowerOfAttorney",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PowerOfAttorneyDocumentStatus_PowerOfAttorneyDocumentNameDM",
                        column: x => x.Name,
                        principalTable: "PowerOfAttorneyDocumentNameDM",
                        principalColumn: "Name");
                });

            migrationBuilder.CreateTable(
                name: "AccountProgram",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    AccountNum = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: false),
                    Effective = table.Column<DateTime>(type: "datetime", nullable: false),
                    Expiration = table.Column<DateTime>(type: "datetime", nullable: false),
                    Single = table.Column<int>(type: "int", nullable: false),
                    Aggregate = table.Column<int>(type: "int", nullable: false),
                    Comments = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    StatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    ApprovedBy = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    ApprovedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    HomeOfficeApproved = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountProgram", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_AccountProgram_Account",
                        column: x => x.AccountNum,
                        principalTable: "Account",
                        principalColumn: "AccountNum");
                });

            migrationBuilder.CreateTable(
                name: "AdditionalRelatedParty",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    AccountNum = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: false),
                    Indemnity = table.Column<bool>(type: "bit", nullable: false),
                    FinancialStatements = table.Column<bool>(type: "bit", nullable: false),
                    Guarantor = table.Column<bool>(type: "bit", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdditionalRelatedParty", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_AdditionalRelatedParty_Account",
                        column: x => x.AccountNum,
                        principalTable: "Account",
                        principalColumn: "AccountNum");
                    table.ForeignKey(
                        name: "FK_AdditionalRelatedParty_LegalEntity",
                        column: x => x.Id,
                        principalTable: "LegalEntity",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AgencyCompetition",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    AccountNum = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: false),
                    AgencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnnualBusiness = table.Column<int>(type: "int", nullable: false),
                    AnnualCommission = table.Column<int>(type: "int", nullable: true),
                    EnteredBy = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgencyCompetition", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_AgencyCompetition_Account",
                        column: x => x.AccountNum,
                        principalTable: "Account",
                        principalColumn: "AccountNum");
                    table.ForeignKey(
                        name: "FK_AgencyCompetition_Agency",
                        column: x => x.AgencyId,
                        principalTable: "Agency",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AgencyCompetition_UserProfile",
                        column: x => x.EnteredBy,
                        principalTable: "UserProfile",
                        principalColumn: "Initials");
                });

            migrationBuilder.CreateTable(
                name: "Bond",
                columns: table => new
                {
                    BondNumber = table.Column<string>(type: "varchar(12)", unicode: false, maxLength: 12, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    CurrentBondMod = table.Column<int>(type: "int", nullable: false, defaultValueSql: "((1))"),
                    AccountNum = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: false),
                    BondTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Effective = table.Column<DateTime>(type: "datetime", nullable: false),
                    Expiration = table.Column<DateTime>(type: "datetime", nullable: false),
                    Status = table.Column<string>(type: "varchar(14)", unicode: false, maxLength: 14, nullable: false),
                    Underwriter = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    AgentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RenewalProvision = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: true),
                    Cancellable = table.Column<bool>(type: "bit", nullable: true),
                    InsurerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AgencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    State = table.Column<string>(type: "char(2)", unicode: false, fixedLength: true, maxLength: 2, nullable: false),
                    RiskDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TerminationType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SFAAClassCode = table.Column<int>(type: "int", nullable: true),
                    SICCode = table.Column<string>(type: "varchar(4)", unicode: false, maxLength: 4, nullable: true),
                    AdditionalObligees = table.Column<bool>(type: "bit", nullable: false),
                    CurrentTreatyYear = table.Column<int>(type: "int", nullable: true),
                    Municipality = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: true),
                    BondClass = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Risk = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    ShortRiskDescription = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    JointVenture = table.Column<bool>(type: "bit", nullable: false),
                    ObligeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ResponsiblePartyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RateGroup = table.Column<string>(type: "varchar(4)", unicode: false, maxLength: 4, nullable: false),
                    RateMultiplier = table.Column<double>(type: "float", nullable: true),
                    CeritifiedMailNumber = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    NonrenewalLetterMailed = table.Column<DateTime>(type: "datetime", nullable: true),
                    HasCollateral = table.Column<bool>(type: "bit", nullable: false),
                    SubjectToRunoff = table.Column<bool>(type: "bit", nullable: false),
                    Federal = table.Column<bool>(type: "bit", nullable: true),
                    HomeOfficeApproved = table.Column<DateTime>(type: "datetime", nullable: true),
                    HomeOfficeApprovedBy = table.Column<string>(type: "varchar(4)", unicode: false, maxLength: 4, nullable: true),
                    HomeOfficeAction = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    LineOfAuthorityExpiration = table.Column<DateTime>(type: "datetime", nullable: true),
                    LineOfAuthoritySingle = table.Column<int>(type: "int", nullable: true),
                    LineOfAuthorityAggregate = table.Column<int>(type: "int", nullable: true),
                    LineOfAuthorityException = table.Column<bool>(type: "bit", nullable: true),
                    LineOfAuthorityExceptionDescription = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CurrentBondLiability = table.Column<int>(type: "int", nullable: false),
                    CurrentRunoff = table.Column<int>(type: "int", nullable: false),
                    EstimatedBondLiability = table.Column<int>(type: "int", nullable: true, computedColumnSql: "([CurrentBondLiability]-[CurrentRunoff])", stored: false),
                    Claim = table.Column<bool>(type: "bit", nullable: false),
                    AttorneyInFactId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDirectBill = table.Column<bool>(type: "bit", nullable: true),
                    DirectBillAddressId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BondPostalCode = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    DaysToCancel = table.Column<int>(type: "int", nullable: true),
                    ExternalRemarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SFAABondType = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bond", x => x.BondNumber);
                    table.ForeignKey(
                        name: "FK_Bond_Account",
                        column: x => x.AccountNum,
                        principalTable: "Account",
                        principalColumn: "AccountNum");
                    table.ForeignKey(
                        name: "FK_Bond_Address",
                        column: x => x.DirectBillAddressId,
                        principalTable: "Address",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Bond_BondTypeDM",
                        column: x => x.BondTypeId,
                        principalTable: "BondTypeDM",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Bond_Insurer",
                        column: x => x.InsurerId,
                        principalTable: "Insurer",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Bond_LegalEntity",
                        column: x => x.AgentId,
                        principalTable: "LegalEntity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Bond_LegalEntity_Agency",
                        column: x => x.AgencyId,
                        principalTable: "LegalEntity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Bond_LegalEntity_AttorneyInFact",
                        column: x => x.AttorneyInFactId,
                        principalTable: "LawEntity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Bond_LegalEntity_Obligee",
                        column: x => x.ObligeeId,
                        principalTable: "LegalEntity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Bond_LegalEntity_ResponsibleParty",
                        column: x => x.ResponsiblePartyId,
                        principalTable: "LegalEntity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Bond_SFAABondTypeDM",
                        column: x => x.SFAABondType,
                        principalTable: "SFAABondTypeDM",
                        principalColumn: "SFAABondType");
                    table.ForeignKey(
                        name: "FK_Bond_SIC",
                        column: x => x.SICCode,
                        principalTable: "SIC",
                        principalColumn: "Code");
                    table.ForeignKey(
                        name: "FK_Bond_State",
                        column: x => x.State,
                        principalTable: "State",
                        principalColumn: "Code");
                    table.ForeignKey(
                        name: "FK_Bond_UserPRofile",
                        column: x => x.Underwriter,
                        principalTable: "UserProfile",
                        principalColumn: "Initials");
                });

            migrationBuilder.CreateTable(
                name: "BondHold",
                columns: table => new
                {
                    BondNumber = table.Column<string>(type: "varchar(12)", unicode: false, maxLength: 12, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    AccountNum = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: false),
                    BondTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Risk = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    Branch = table.Column<string>(type: "char(3)", unicode: false, fixedLength: true, maxLength: 3, nullable: true),
                    Underwriter = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    AgencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AgentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ObligeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ResponsiblePartyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    InsurerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SICCode = table.Column<string>(type: "varchar(4)", unicode: false, maxLength: 4, nullable: true),
                    State = table.Column<string>(type: "char(2)", unicode: false, fixedLength: true, maxLength: 2, nullable: true),
                    Municipality = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: true),
                    Effective = table.Column<DateTime>(type: "datetime", nullable: true),
                    Expiration = table.Column<DateTime>(type: "datetime", nullable: true),
                    BillDate = table.Column<DateTime>(type: "date", nullable: true),
                    SFAACode = table.Column<int>(type: "int", nullable: true),
                    SFAAClassCode = table.Column<int>(type: "int", nullable: true),
                    StatusLetterNext = table.Column<DateTime>(type: "date", nullable: true),
                    ContractAmount = table.Column<int>(type: "int", nullable: true),
                    BondAmount = table.Column<int>(type: "int", nullable: true),
                    PayBondAmout = table.Column<int>(type: "int", nullable: true),
                    Premium = table.Column<int>(type: "int", nullable: true),
                    MunicipalTax = table.Column<double>(type: "float", nullable: true),
                    CommissionRate = table.Column<double>(type: "float", nullable: true),
                    RiskDescription = table.Column<double>(type: "float", nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExternalRemarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rate = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: true),
                    RateStructure = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    RateClass = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    TerminationType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cancellable = table.Column<bool>(type: "bit", nullable: false),
                    RenewalProvision = table.Column<bool>(type: "bit", nullable: true),
                    CurrentTreatyYear = table.Column<int>(type: "int", nullable: true),
                    AccountingDate = table.Column<DateTime>(type: "date", nullable: true),
                    BidNumber = table.Column<string>(type: "varchar(9)", unicode: false, maxLength: 9, nullable: true),
                    HomeOfficeApproved = table.Column<DateTime>(type: "datetime", nullable: true),
                    HomeOfficeApprovedBy = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    HomeOfficeAction = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    IsDirectBill = table.Column<bool>(type: "bit", nullable: true),
                    CommissionAmount = table.Column<double>(type: "float", nullable: true),
                    BondPostalCode = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    DaysToCancel = table.Column<int>(type: "int", nullable: true),
                    SurchargeApply = table.Column<bool>(type: "bit", nullable: false),
                    Surcharge = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BondHold", x => x.BondNumber)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_BondHold_Account",
                        column: x => x.AccountNum,
                        principalTable: "Account",
                        principalColumn: "AccountNum");
                    table.ForeignKey(
                        name: "FK_BondHold_BondTypeDM",
                        column: x => x.BondTypeId,
                        principalTable: "BondTypeDM",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BondHold_Insurer",
                        column: x => x.InsurerId,
                        principalTable: "Insurer",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BondHold_LegalEntity_Agency",
                        column: x => x.AgencyId,
                        principalTable: "LegalEntity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BondHold_LegalEntity_Agent",
                        column: x => x.AgentId,
                        principalTable: "LegalEntity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BondHold_LegalEntity_Obligee",
                        column: x => x.ObligeeId,
                        principalTable: "LegalEntity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BondHold_LegalEntity_ResponsibleParty",
                        column: x => x.ResponsiblePartyId,
                        principalTable: "LegalEntity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BondHold_SFAA",
                        column: x => x.SFAACode,
                        principalTable: "SFAA",
                        principalColumn: "Code");
                    table.ForeignKey(
                        name: "FK_BondHold_SIC",
                        column: x => x.SICCode,
                        principalTable: "SIC",
                        principalColumn: "Code");
                    table.ForeignKey(
                        name: "FK_BondHold_State",
                        column: x => x.State,
                        principalTable: "State",
                        principalColumn: "Code");
                    table.ForeignKey(
                        name: "FK_BondHold_UserProfile",
                        column: x => x.Underwriter,
                        principalTable: "UserProfile",
                        principalColumn: "Initials");
                    table.ForeignKey(
                        name: "FK_BondHold_UserProfile_HomeOfficeApprovedBy",
                        column: x => x.HomeOfficeApprovedBy,
                        principalTable: "UserProfile",
                        principalColumn: "Initials");
                });

            migrationBuilder.CreateTable(
                name: "CashFlowStatement",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    AccountNum = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: false),
                    StatementDate = table.Column<DateTime>(type: "date", nullable: false),
                    Type = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Basis = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    Scaling = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    TaxBasis = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    Quality = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    NetIncome = table.Column<int>(type: "int", nullable: true),
                    DepreciationAmoritization = table.Column<int>(type: "int", nullable: true),
                    AccountsReceivable = table.Column<int>(type: "int", nullable: true),
                    AccountsReceivableRetention = table.Column<int>(type: "int", nullable: true),
                    B2CEE = table.Column<int>(type: "int", nullable: true),
                    AllOtherCashFlow = table.Column<int>(type: "int", nullable: true),
                    TotalCashFlowsOperations = table.Column<int>(type: "int", nullable: true, computedColumnSql: "((((isnull([NetIncome],(0))+isnull([DepreciationAmoritization],(0)))+isnull([AccountsReceivable],(0)))+isnull([AccountsReceivableRetention],(0)))+isnull([AllOtherCashFlow],(0)))", stored: false),
                    NetFixedAssetsAcquired = table.Column<int>(type: "int", nullable: true),
                    AllOtherInvestments = table.Column<int>(type: "int", nullable: true),
                    TotalCashInvestments = table.Column<int>(type: "int", nullable: true, computedColumnSql: "(isnull([NetFixedAssetsAcquired],(0))+isnull([AllOtherInvestments],(0)))", stored: false),
                    Distributions = table.Column<int>(type: "int", nullable: true),
                    LineOfCredit = table.Column<int>(type: "int", nullable: true),
                    TermDebt = table.Column<int>(type: "int", nullable: true),
                    StockholderNotes = table.Column<int>(type: "int", nullable: true),
                    AllOtherFinancing = table.Column<int>(type: "int", nullable: true),
                    TotalCashFinancing = table.Column<int>(type: "int", nullable: true, computedColumnSql: "((((isnull([Distributions],(0))+isnull([LineOfCredit],(0)))+isnull([TermDebt],(0)))+isnull([StockholderNotes],(0)))+isnull([AllOtherFinancing],(0)))", stored: false),
                    NetChangeInCash = table.Column<int>(type: "int", nullable: true, computedColumnSql: "(((((((((((isnull([NetIncome],(0))+isnull([DepreciationAmoritization],(0)))+isnull([AccountsReceivable],(0)))+isnull([AccountsReceivableRetention],(0)))+isnull([AllOtherCashFlow],(0)))+isnull([NetFixedAssetsAcquired],(0)))+isnull([AllOtherInvestments],(0)))+isnull([Distributions],(0)))+isnull([TermDebt],(0)))+isnull([LineOfCredit],(0)))+isnull([StockholderNotes],(0)))+isnull([AllOtherFinancing],(0)))", stored: false),
                    NetFixedAssetsAcquiredDebt = table.Column<int>(type: "int", nullable: true, computedColumnSql: "(isnull([TermDebt],(0))-isnull([NetFixedAssetsAcquired],(0)))", stored: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashFlowStatement", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_CAshFlowStatement_StatementBasisDM",
                        column: x => x.Basis,
                        principalTable: "StatementBasisDM",
                        principalColumn: "Basis");
                    table.ForeignKey(
                        name: "FK_CashFlowStatement_Account",
                        column: x => x.AccountNum,
                        principalTable: "Account",
                        principalColumn: "AccountNum");
                    table.ForeignKey(
                        name: "FK_CashFlowStatement_ScalingDM",
                        column: x => x.Scaling,
                        principalTable: "ScalingDM",
                        principalColumn: "Scaling");
                    table.ForeignKey(
                        name: "FK_CashFlowStatement_StatementQualityDM",
                        column: x => x.Quality,
                        principalTable: "StatementQualityDM",
                        principalColumn: "Quality");
                    table.ForeignKey(
                        name: "FK_CashFlowStatement_StatementTypeDM",
                        column: x => x.Type,
                        principalTable: "StatementTypeDM",
                        principalColumn: "Type");
                    table.ForeignKey(
                        name: "FK_CashFlowStatement_TaxBasisDM",
                        column: x => x.TaxBasis,
                        principalTable: "TaxBasisDM",
                        principalColumn: "TaxBasis");
                });

            migrationBuilder.CreateTable(
                name: "Indemnitor",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    AccountNum = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: false),
                    AgreementDate = table.Column<DateTime>(type: "date", nullable: false),
                    AgreementType = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    AgreementForm = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    NetLiquidAssets = table.Column<int>(type: "int", nullable: true),
                    NetWorth = table.Column<int>(type: "int", nullable: true),
                    IndemnityAmount = table.Column<int>(type: "int", nullable: true),
                    SpouseIndemnitor = table.Column<bool>(type: "bit", nullable: false),
                    Encrypt_SpouseTaxId = table.Column<string>(type: "nvarchar(22)", maxLength: 22, nullable: true),
                    Signatory = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Indemnitor", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_Indemnitor_Account",
                        column: x => x.AccountNum,
                        principalTable: "Account",
                        principalColumn: "AccountNum");
                    table.ForeignKey(
                        name: "FK_Indemnitor_LegalEntity",
                        column: x => x.Id,
                        principalTable: "LegalEntity",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "KeyPersonnel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    AccountNum = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: false),
                    EncryptYearOfBirth = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    Position = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: true),
                    Responsibility = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true),
                    YearJoinedCompany = table.Column<string>(type: "char(4)", unicode: false, fixedLength: true, maxLength: 4, nullable: true),
                    YearEnteredField = table.Column<string>(type: "char(4)", unicode: false, fixedLength: true, maxLength: 4, nullable: true),
                    SpouseName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    PersonalIndemnification = table.Column<bool>(type: "bit", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Profession = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KeyPersonnel", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_KeyPersonnel_Account",
                        column: x => x.AccountNum,
                        principalTable: "Account",
                        principalColumn: "AccountNum");
                    table.ForeignKey(
                        name: "FK_KeyPersonnel_LegalEntity",
                        column: x => x.Id,
                        principalTable: "LegalEntity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_KeyPersonnel_ResponsibilityDM",
                        column: x => x.Responsibility,
                        principalTable: "ResponsibilityDM",
                        principalColumn: "Responsibility");
                });

            migrationBuilder.CreateTable(
                name: "PersonalFinancialStatement",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    AccountNum = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: false),
                    StatementDate = table.Column<DateTime>(type: "date", nullable: false),
                    AccountType = table.Column<string>(type: "char(2)", unicode: false, fixedLength: true, maxLength: 2, nullable: false),
                    Sequence = table.Column<int>(type: "int", nullable: false),
                    AccountName = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    Stated = table.Column<int>(type: "int", nullable: true),
                    Adjustment = table.Column<int>(type: "int", nullable: true),
                    AsAllowed = table.Column<int>(type: "int", nullable: true, computedColumnSql: "([Stated]+[Adjustment])", stored: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HeaderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalFinancialStatement", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_PersonalFinancialStatement_Account",
                        column: x => x.AccountNum,
                        principalTable: "Account",
                        principalColumn: "AccountNum");
                    table.ForeignKey(
                        name: "FK_PersonalFinancialStatement_PersonalFinancialHeader",
                        column: x => x.HeaderId,
                        principalTable: "PersonalFinancialHeader",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Ratio",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    AccountNum = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: false),
                    StatementDate = table.Column<DateTime>(type: "date", nullable: false),
                    StatementType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    StatementBasis = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    StatementTaxBasis = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    StatementQuality = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    WorkingCapitalStated = table.Column<int>(type: "int", nullable: true),
                    TotalAssetsStated = table.Column<int>(type: "int", nullable: true),
                    RetainedEarningsStated = table.Column<int>(type: "int", nullable: true),
                    EarningsBeforeTaxesStated = table.Column<int>(type: "int", nullable: true),
                    NetWorthStated = table.Column<int>(type: "int", nullable: true),
                    TotalLiabilitiesStated = table.Column<int>(type: "int", nullable: true),
                    TotalEquityStated = table.Column<int>(type: "int", nullable: true),
                    TotalRevenueStated = table.Column<int>(type: "int", nullable: true),
                    TotalCostOfGoodsSoldStated = table.Column<int>(type: "int", nullable: true),
                    CurrentAssetsStated = table.Column<int>(type: "int", nullable: true),
                    CurrentLiabilitiesStated = table.Column<int>(type: "int", nullable: true),
                    NetIncomeStated = table.Column<int>(type: "int", nullable: true),
                    InventoryStated = table.Column<int>(type: "int", nullable: true),
                    FixedAssetsStated = table.Column<int>(type: "int", nullable: true),
                    AccountsReceivableStated = table.Column<int>(type: "int", nullable: true),
                    AccountsPayableStated = table.Column<int>(type: "int", nullable: true),
                    SubordinatedNotesStated = table.Column<int>(type: "int", nullable: true),
                    RetainageReceivedStated = table.Column<int>(type: "int", nullable: true),
                    RetainagePayableStated = table.Column<int>(type: "int", nullable: true),
                    CashPlusEarningsGreaterThanBillingsStated = table.Column<int>(type: "int", nullable: true),
                    BillingsGreaterThanCostsPlusEarningsStated = table.Column<int>(type: "int", nullable: true),
                    PrepaidExpensesStated = table.Column<int>(type: "int", nullable: true),
                    CashInventoryStated = table.Column<int>(type: "int", nullable: true),
                    OtherCurrentAssetsStated = table.Column<int>(type: "int", nullable: true),
                    BankDebtStated = table.Column<int>(type: "int", nullable: true),
                    GAExpensesStated = table.Column<int>(type: "int", nullable: true),
                    OperatingProfitStated = table.Column<int>(type: "int", nullable: true),
                    IncomeAfterTaxesStated = table.Column<int>(type: "int", nullable: true),
                    CashStated = table.Column<int>(type: "int", nullable: true),
                    NotesStated = table.Column<int>(type: "int", nullable: true),
                    QScoreStated = table.Column<int>(type: "int", nullable: true),
                    QProfitStated = table.Column<int>(type: "int", nullable: true),
                    QNetworthStated = table.Column<int>(type: "int", nullable: true),
                    QLeverageStated = table.Column<int>(type: "int", nullable: true),
                    QWorkingCapitalStated = table.Column<int>(type: "int", nullable: true),
                    QCRatioStated = table.Column<int>(type: "int", nullable: true),
                    WorkingCapitalAllowed = table.Column<int>(type: "int", nullable: true),
                    TotalAssetsAllowed = table.Column<int>(type: "int", nullable: true),
                    RetainedEarningsAllowed = table.Column<int>(type: "int", nullable: true),
                    EarningsBeforeTaxesAllowed = table.Column<int>(type: "int", nullable: true),
                    NetWorthAllowed = table.Column<int>(type: "int", nullable: true),
                    TotalLiabilitiesAllowed = table.Column<int>(type: "int", nullable: true),
                    TotalEquityAllowed = table.Column<int>(type: "int", nullable: true),
                    TotalRevenueAllowed = table.Column<int>(type: "int", nullable: true),
                    TotalCostOfGoodsSoldAllowed = table.Column<int>(type: "int", nullable: true),
                    CurrentAssetsAllowed = table.Column<int>(type: "int", nullable: true),
                    CurrentLiabilitiesAllowed = table.Column<int>(type: "int", nullable: true),
                    NetIncomeAllowed = table.Column<int>(type: "int", nullable: true),
                    InventoryAllowed = table.Column<int>(type: "int", nullable: true),
                    FixedAssetsAllowed = table.Column<int>(type: "int", nullable: true),
                    AccountsReceivableAllowed = table.Column<int>(type: "int", nullable: true),
                    AccountsPayableAllowed = table.Column<int>(type: "int", nullable: true),
                    SubordinatedNotesAllowed = table.Column<int>(type: "int", nullable: true),
                    RetainageReceivedAllowed = table.Column<int>(type: "int", nullable: true),
                    RetainagePayableAllowed = table.Column<int>(type: "int", nullable: true),
                    CashPlusEarningsGreaterThanBillingsAllowed = table.Column<int>(type: "int", nullable: true),
                    BillingsGreaterThanCostsPlusEarningsAllowed = table.Column<int>(type: "int", nullable: true),
                    PrepaidExpensesAllowed = table.Column<int>(type: "int", nullable: true),
                    CashInventoryAllowed = table.Column<int>(type: "int", nullable: true),
                    OtherCurrentAssetsAllowed = table.Column<int>(type: "int", nullable: true),
                    BankDebtAllowed = table.Column<int>(type: "int", nullable: true),
                    GAExpensesAllowed = table.Column<int>(type: "int", nullable: true),
                    OperatingProfitAllowed = table.Column<int>(type: "int", nullable: true),
                    IncomeAfterTaxesAllowed = table.Column<int>(type: "int", nullable: true),
                    CashAllowed = table.Column<int>(type: "int", nullable: true),
                    NotesAllowed = table.Column<int>(type: "int", nullable: true),
                    QScoreAllowed = table.Column<int>(type: "int", nullable: true),
                    QProfitAllowed = table.Column<int>(type: "int", nullable: true),
                    QNetworthAllowed = table.Column<int>(type: "int", nullable: true),
                    QLeverageAllowed = table.Column<int>(type: "int", nullable: true),
                    QWorkingCapitalAllowed = table.Column<int>(type: "int", nullable: true),
                    QCRatioAllowed = table.Column<int>(type: "int", nullable: true),
                    Scaling = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    SingleLOA = table.Column<int>(type: "int", nullable: true),
                    AggregateLOA = table.Column<int>(type: "int", nullable: true),
                    LargestJob = table.Column<int>(type: "int", nullable: true),
                    LargestBacklog = table.Column<int>(type: "int", nullable: true),
                    CashFromOperations = table.Column<int>(type: "int", nullable: true),
                    CashFromInvestments = table.Column<int>(type: "int", nullable: true),
                    CashFromFinancing = table.Column<int>(type: "int", nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NetChangeCash = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ratio", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.UniqueConstraint("AK_Ratio_AccountNum_StatementDate", x => new { x.AccountNum, x.StatementDate });
                    table.ForeignKey(
                        name: "FK_Ratio_Account",
                        column: x => x.AccountNum,
                        principalTable: "Account",
                        principalColumn: "AccountNum");
                });

            migrationBuilder.CreateTable(
                name: "UnderwriterRecommendation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    AccountNum = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: false),
                    PostedBy = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnderwriterRecommendation", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_UnderwriterRecommendation_Account",
                        column: x => x.AccountNum,
                        principalTable: "Account",
                        principalColumn: "AccountNum");
                });

            migrationBuilder.CreateTable(
                name: "BidRequest",
                columns: table => new
                {
                    BidNumber = table.Column<string>(type: "varchar(9)", unicode: false, maxLength: 9, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    AccountNum = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: false),
                    Requested = table.Column<DateTime>(type: "date", nullable: false),
                    BidDate = table.Column<DateTime>(type: "date", nullable: false),
                    ObligeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EstimatedStart = table.Column<DateTime>(type: "date", nullable: true),
                    EstimatedFinish = table.Column<DateTime>(type: "date", nullable: true),
                    ContractNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    ProjectName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstimatedContractPrice = table.Column<int>(type: "int", nullable: true),
                    WithdrawalPenalty = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    PerformanceBondAmount = table.Column<int>(type: "int", nullable: true),
                    PaymentBondAmount = table.Column<int>(type: "int", nullable: true),
                    MaintenanceTerm = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Penalties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentFrequency = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Retainage = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true),
                    SpecialHazards = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GeographicConcerns = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Conditions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WIPWorkOnHand = table.Column<long>(type: "bigint", nullable: true),
                    WIPDate = table.Column<DateTime>(type: "date", nullable: true),
                    NewContractsLowBids = table.Column<int>(type: "int", nullable: true),
                    EstimatedBacklogRunoff = table.Column<int>(type: "int", nullable: true),
                    TotalWorkOnHand = table.Column<long>(type: "bigint", nullable: true),
                    LineOfCreditSingle = table.Column<int>(type: "int", nullable: true),
                    LineOfCreditAggregate = table.Column<int>(type: "int", nullable: true),
                    LineOfCreditExpiration = table.Column<DateTime>(type: "date", nullable: true),
                    RequestedBy = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    ApprovedBy = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    BondNumber = table.Column<string>(type: "varchar(12)", unicode: false, maxLength: 12, nullable: true),
                    LaborPercentage = table.Column<int>(type: "int", nullable: true),
                    MaterialPercentage = table.Column<int>(type: "int", nullable: true),
                    SubcontractedPercentage = table.Column<int>(type: "int", nullable: true),
                    GeneralConditionsPercentage = table.Column<int>(type: "int", nullable: true),
                    EstimateGrossProfitPercentage = table.Column<int>(type: "int", nullable: true),
                    EquipmentPercentage = table.Column<int>(type: "int", nullable: true),
                    BidResult = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true),
                    HomeOfficeApproved = table.Column<DateTime>(type: "datetime", nullable: true),
                    HomeOfficeApprover = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    HomeOfficeEmailSent = table.Column<DateTime>(type: "datetime", nullable: true),
                    HomeOfficeAction = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    Underwriter = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    RequestType = table.Column<string>(type: "varchar(4)", unicode: false, maxLength: 4, nullable: false),
                    OutstandingBids = table.Column<int>(type: "int", nullable: true),
                    Adjustments = table.Column<int>(type: "int", nullable: true),
                    ActualBidAmount = table.Column<int>(type: "int", nullable: true),
                    ApprovedBidAmount = table.Column<int>(type: "int", nullable: true),
                    CCTo = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    Status = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    HomeOfficeConditions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BidPercent = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BidRequest", x => x.BidNumber);
                    table.ForeignKey(
                        name: "FK_BidRequest_Account",
                        column: x => x.AccountNum,
                        principalTable: "Account",
                        principalColumn: "AccountNum");
                    table.ForeignKey(
                        name: "FK_BidRequest_BidPercentDM",
                        column: x => x.BidPercent,
                        principalTable: "BidPercentDM",
                        principalColumn: "BidPercent");
                    table.ForeignKey(
                        name: "FK_BidRequest_BidResultDM",
                        column: x => x.BidResult,
                        principalTable: "BidResultDM",
                        principalColumn: "Description");
                    table.ForeignKey(
                        name: "FK_BidRequest_BidRetainageDM",
                        column: x => x.Retainage,
                        principalTable: "BidRetainageDM",
                        principalColumn: "Retainage");
                    table.ForeignKey(
                        name: "FK_BidRequest_BidStatusDM",
                        column: x => x.Status,
                        principalTable: "BidStatusDM",
                        principalColumn: "Status");
                    table.ForeignKey(
                        name: "FK_BidRequest_Bond",
                        column: x => x.BondNumber,
                        principalTable: "Bond",
                        principalColumn: "BondNumber");
                    table.ForeignKey(
                        name: "FK_BidRequest_Obligee",
                        column: x => x.ObligeeId,
                        principalTable: "Obligee",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BidRequest_UserProfile",
                        column: x => x.Underwriter,
                        principalTable: "UserProfile",
                        principalColumn: "Initials");
                    table.ForeignKey(
                        name: "FK_BidRequest_UserProfile_CCTo",
                        column: x => x.CCTo,
                        principalTable: "UserProfile",
                        principalColumn: "Initials");
                });

            migrationBuilder.CreateTable(
                name: "BidRequestCommercial",
                columns: table => new
                {
                    BidNumber = table.Column<string>(type: "varchar(9)", unicode: false, maxLength: 9, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    AccountNum = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: false),
                    Requested = table.Column<DateTime>(type: "date", nullable: false),
                    LegacyBidNumber = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    EstimatedStart = table.Column<DateTime>(type: "date", nullable: true),
                    EstimatedFinish = table.Column<DateTime>(type: "date", nullable: true),
                    ObligeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Amount = table.Column<int>(type: "int", nullable: true),
                    RecordedBy = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    Recorded = table.Column<DateTime>(type: "date", nullable: true),
                    RecordedMessageSent = table.Column<DateTime>(type: "datetime", nullable: true),
                    HomeOfficeAction = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    HomeOfficeApproved = table.Column<DateTime>(type: "datetime", nullable: true),
                    HomeOfficeApprovedBy = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    HomeOfficeConditions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BondNumber = table.Column<string>(type: "varchar(12)", unicode: false, maxLength: 12, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Underwriter = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    SFAAClassCode = table.Column<int>(type: "int", nullable: true),
                    CCTo = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    LineOfCreditSingle = table.Column<int>(type: "int", nullable: true),
                    LineOfCreditAggregate = table.Column<int>(type: "int", nullable: true),
                    LineOfCreditExpiration = table.Column<DateTime>(type: "date", nullable: true),
                    Exposure = table.Column<int>(type: "int", nullable: true),
                    Adjustments = table.Column<int>(type: "int", nullable: true),
                    TotalExposure = table.Column<int>(type: "int", nullable: true),
                    ApprovedBidAmount = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BidRequestCommercial", x => x.BidNumber)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_BidRequestCommercial_Account",
                        column: x => x.AccountNum,
                        principalTable: "Account",
                        principalColumn: "AccountNum");
                    table.ForeignKey(
                        name: "FK_BidRequestCommercial_BidStatusDM",
                        column: x => x.Status,
                        principalTable: "BidStatusDM",
                        principalColumn: "Status");
                    table.ForeignKey(
                        name: "FK_BidRequestCommercial_Bond",
                        column: x => x.BondNumber,
                        principalTable: "Bond",
                        principalColumn: "BondNumber");
                    table.ForeignKey(
                        name: "FK_BidRequestCommercial_LegalEntity",
                        column: x => x.ObligeeId,
                        principalTable: "LegalEntity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BidRequestCommercial_SFAA",
                        column: x => x.SFAAClassCode,
                        principalTable: "SFAA",
                        principalColumn: "Code");
                    table.ForeignKey(
                        name: "FK_BidRequestCommercial_UserProfile_CCTo",
                        column: x => x.CCTo,
                        principalTable: "UserProfile",
                        principalColumn: "Initials");
                    table.ForeignKey(
                        name: "FK_BidRequestCommercial_UserProfile_HomeOfficeApprovedBy",
                        column: x => x.HomeOfficeApprovedBy,
                        principalTable: "UserProfile",
                        principalColumn: "Initials");
                    table.ForeignKey(
                        name: "FK_BidRequestCommercial_UserProfile_RecordedBy",
                        column: x => x.RecordedBy,
                        principalTable: "UserProfile",
                        principalColumn: "Initials");
                    table.ForeignKey(
                        name: "FK_BidRequestCommercial_UserProfile_Underwriter",
                        column: x => x.Underwriter,
                        principalTable: "UserProfile",
                        principalColumn: "Initials");
                });

            migrationBuilder.CreateTable(
                name: "BondStatusLetter",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    BondNumber = table.Column<string>(type: "varchar(12)", unicode: false, maxLength: 12, nullable: false),
                    Sent = table.Column<DateTime>(type: "date", nullable: false),
                    ReceivedBack = table.Column<DateTime>(type: "date", nullable: true),
                    PercentComplete = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BondStatusLetter", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_BondStatusLetter_Bond",
                        column: x => x.BondNumber,
                        principalTable: "Bond",
                        principalColumn: "BondNumber");
                });

            migrationBuilder.CreateTable(
                name: "BondTransaction",
                columns: table => new
                {
                    BondNumber = table.Column<string>(type: "varchar(12)", unicode: false, maxLength: 12, nullable: false),
                    GroupNumber = table.Column<int>(type: "int", nullable: false, defaultValueSql: "((1))"),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false),
                    BondMod = table.Column<int>(type: "int", nullable: false, defaultValueSql: "((1))"),
                    AccountNum = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Branch = table.Column<string>(type: "char(3)", unicode: false, fixedLength: true, maxLength: 3, nullable: false),
                    MidtermDescription = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PaymentBondAmount = table.Column<int>(type: "int", nullable: true),
                    Premium = table.Column<int>(type: "int", nullable: false),
                    BondAmount = table.Column<int>(type: "int", nullable: false),
                    ContractAmount = table.Column<int>(type: "int", nullable: true),
                    Effective = table.Column<DateTime>(type: "datetime", nullable: false),
                    Expiration = table.Column<DateTime>(type: "datetime", nullable: false),
                    CoverageDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    BillDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    CommissionRate = table.Column<double>(type: "float", nullable: false),
                    CommissionAmount = table.Column<double>(type: "float", nullable: false),
                    AdminFee = table.Column<double>(type: "float", nullable: true),
                    Surcharge = table.Column<double>(type: "float", nullable: false),
                    MunicipalTax = table.Column<double>(type: "float", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountingDate = table.Column<DateTime>(type: "date", nullable: false),
                    PaidToDate = table.Column<double>(type: "float", nullable: false),
                    AgencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NetDue = table.Column<double>(type: "float", nullable: false, computedColumnSql: "(((([Premium]-[CommissionAmount])+[Surcharge])+[MunicipalTax])+isnull([AdminFee],(0)))", stored: false),
                    Earned = table.Column<double>(type: "float", nullable: false),
                    Unearned = table.Column<double>(type: "float", nullable: false),
                    SfaaCode = table.Column<int>(type: "int", nullable: false),
                    DirectBill = table.Column<bool>(type: "bit", nullable: false),
                    Region = table.Column<string>(type: "char(2)", unicode: false, fixedLength: true, maxLength: 2, nullable: false),
                    AccountClass = table.Column<string>(type: "varchar(4)", unicode: false, maxLength: 4, nullable: false),
                    SurchargeApply = table.Column<bool>(type: "bit", nullable: false),
                    AgencyChange = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TransactionPurpose = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    TransactionGroup = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Rate = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: true),
                    RateStructure = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    RateClass = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    UnderwriterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BondTransaction", x => new { x.BondNumber, x.GroupNumber });
                    table.UniqueConstraint("AK_BondTransaction_BondNumber_BondMod_GroupNumber", x => new { x.BondNumber, x.BondMod, x.GroupNumber });
                    table.ForeignKey(
                        name: "FK_BondTransaction_Account",
                        column: x => x.AccountNum,
                        principalTable: "Account",
                        principalColumn: "AccountNum");
                    table.ForeignKey(
                        name: "FK_BondTransaction_AccountClassDM",
                        column: x => x.AccountClass,
                        principalTable: "AccountClassDM",
                        principalColumn: "AccountClass");
                    table.ForeignKey(
                        name: "FK_BondTransaction_Bond",
                        column: x => x.BondNumber,
                        principalTable: "Bond",
                        principalColumn: "BondNumber");
                    table.ForeignKey(
                        name: "FK_BondTransaction_LegalEntity",
                        column: x => x.AgencyId,
                        principalTable: "LegalEntity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BondTransaction_Region",
                        column: x => x.Region,
                        principalTable: "RegionDM",
                        principalColumn: "Region");
                    table.ForeignKey(
                        name: "FK_BondTransaction_SFAA",
                        column: x => x.SfaaCode,
                        principalTable: "SFAA",
                        principalColumn: "Code");
                    table.ForeignKey(
                        name: "FK_BondTransaction_UserProfile",
                        column: x => x.UnderwriterId,
                        principalTable: "UserProfile",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Collateral",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    AccountNum = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: true),
                    Expiration = table.Column<DateTime>(type: "date", nullable: true),
                    AutoRenew = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BondNumber = table.Column<string>(type: "varchar(12)", unicode: false, maxLength: 12, nullable: true),
                    BondSpecific = table.Column<bool>(type: "bit", nullable: true, computedColumnSql: "(CONVERT([bit],case when isnull([BondNumber],'')='' then (0) else (1) end))", stored: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Collateral", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_Collateral_Bond",
                        column: x => x.BondNumber,
                        principalTable: "Bond",
                        principalColumn: "BondNumber");
                });

            migrationBuilder.CreateTable(
                name: "CoPrincipal",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    BondNumber = table.Column<string>(type: "varchar(12)", unicode: false, maxLength: 12, nullable: false),
                    AccountNum = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: false),
                    Profession = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    YearsInProfession = table.Column<int>(type: "int", nullable: true),
                    LiabilityAmount = table.Column<double>(type: "float", nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoPrincipal", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_CoPrincipal_Account",
                        column: x => x.AccountNum,
                        principalTable: "Account",
                        principalColumn: "AccountNum");
                    table.ForeignKey(
                        name: "FK_CoPrincipal_Bond",
                        column: x => x.BondNumber,
                        principalTable: "Bond",
                        principalColumn: "BondNumber");
                });

            migrationBuilder.CreateTable(
                name: "EmailHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    From = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FromName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    To = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ToName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CC = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    AccountNum = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: false),
                    BondNumber = table.Column<string>(type: "varchar(12)", unicode: false, maxLength: 12, nullable: false),
                    Sent = table.Column<DateTime>(type: "datetime", nullable: false),
                    Action = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    IsContract = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailHistory", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_EmailHistory_Account",
                        column: x => x.AccountNum,
                        principalTable: "Account",
                        principalColumn: "AccountNum");
                    table.ForeignKey(
                        name: "FK_EmailHistory_Bond",
                        column: x => x.BondNumber,
                        principalTable: "Bond",
                        principalColumn: "BondNumber");
                    table.ForeignKey(
                        name: "FK_EmailHistory_EmailActionDM",
                        column: x => x.Action,
                        principalTable: "EmailActionDM",
                        principalColumn: "Action");
                });

            migrationBuilder.CreateTable(
                name: "OpenClaim",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    BondNumber = table.Column<string>(type: "varchar(12)", unicode: false, maxLength: 12, nullable: false),
                    ClaimNumber = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    AdjusterName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Reserve = table.Column<decimal>(type: "decimal(21,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenClaim", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_OpenClaim_Bond",
                        column: x => x.BondNumber,
                        principalTable: "Bond",
                        principalColumn: "BondNumber");
                });

            migrationBuilder.CreateTable(
                name: "WorkInProgressJob",
                columns: table => new
                {
                    AccountNum = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: false),
                    WIPDate = table.Column<DateTime>(type: "date", nullable: false),
                    JobNumber = table.Column<string>(type: "varchar(25)", unicode: false, maxLength: 25, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Description = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    BondNumber = table.Column<string>(type: "varchar(12)", unicode: false, maxLength: 12, nullable: true),
                    ContractPrice = table.Column<long>(type: "bigint", nullable: false),
                    EstimatedCost = table.Column<long>(type: "bigint", nullable: false),
                    EstimatedGrossProfit = table.Column<long>(type: "bigint", nullable: false),
                    PercentComplete = table.Column<double>(type: "float", nullable: false),
                    EarnedRevenue = table.Column<long>(type: "bigint", nullable: false),
                    CostToDate = table.Column<long>(type: "bigint", nullable: false),
                    GrossProfit = table.Column<long>(type: "bigint", nullable: false),
                    GrossProfitPercent = table.Column<decimal>(type: "numeric(38,15)", nullable: true, computedColumnSql: "(case when [ContractPrice]=(0) OR [EstimatedCost]=(0) then (0) else ((100.0)*[EstimatedGrossProfit])/[ContractPrice] end)", stored: false),
                    ProgressBillings = table.Column<long>(type: "bigint", nullable: true),
                    UnderBillings = table.Column<long>(type: "bigint", nullable: false),
                    OverBillings = table.Column<long>(type: "bigint", nullable: false),
                    RevenueRemaining = table.Column<long>(type: "bigint", nullable: false),
                    CostToComplete = table.Column<long>(type: "bigint", nullable: false),
                    TotalEstimatedGrossProfit = table.Column<long>(type: "bigint", nullable: false),
                    JobStatus = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: false, computedColumnSql: "(case when [PercentComplete]>=(100) then 'Complete' else 'Open' end)", stored: false),
                    EnteredContractPrice = table.Column<long>(type: "bigint", nullable: true),
                    EnteredEstimatedCost = table.Column<long>(type: "bigint", nullable: true),
                    EnteredCostToDate = table.Column<long>(type: "bigint", nullable: true),
                    EnteredProgressBillings = table.Column<long>(type: "bigint", nullable: true),
                    OtherJobsContractPrice = table.Column<long>(type: "bigint", nullable: true),
                    OtherJobsEstimatedCost = table.Column<long>(type: "bigint", nullable: true),
                    OtherJobsCostToDate = table.Column<long>(type: "bigint", nullable: true),
                    OtherJobsProgressBillings = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkInProgressJob", x => new { x.AccountNum, x.WIPDate, x.JobNumber })
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_WorkInProgressJob_Account",
                        column: x => x.AccountNum,
                        principalTable: "Account",
                        principalColumn: "AccountNum");
                    table.ForeignKey(
                        name: "FK_WorkInProgressJob_Bond",
                        column: x => x.BondNumber,
                        principalTable: "Bond",
                        principalColumn: "BondNumber");
                    table.ForeignKey(
                        name: "FK_WorkInProgressJob_WorkInProgressSummary",
                        columns: x => new { x.AccountNum, x.WIPDate },
                        principalTable: "WorkInProgressSummary",
                        principalColumns: new[] { "AccountNum", "WIPDate" });
                });

            migrationBuilder.CreateTable(
                name: "PersonalFinancialSubaccount",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    ParentAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Sequence = table.Column<int>(type: "int", nullable: false),
                    Stated = table.Column<int>(type: "int", nullable: true),
                    Adjustment = table.Column<int>(type: "int", nullable: true),
                    AsAllowed = table.Column<int>(type: "int", nullable: true, computedColumnSql: "([Stated]+[Adjustment])", stored: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalFinancialSubaccount", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_PersonalFinancialSubaccount_PersonalFinancialStatement",
                        column: x => x.ParentAccountId,
                        principalTable: "PersonalFinancialStatement",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BalanceSheet",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    AccountNum = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: false),
                    StatementDate = table.Column<DateTime>(type: "date", nullable: false),
                    AccountType = table.Column<string>(type: "char(2)", unicode: false, fixedLength: true, maxLength: 2, nullable: false),
                    Sequence = table.Column<int>(type: "int", nullable: false),
                    AccountName = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    Stated = table.Column<int>(type: "int", nullable: true),
                    Adjustment = table.Column<int>(type: "int", nullable: true),
                    AsAllowed = table.Column<int>(type: "int", nullable: true, computedColumnSql: "([Stated]+[Adjustment])", stored: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BalanceSheet", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_BalanceSheet_Account",
                        column: x => x.AccountNum,
                        principalTable: "Account",
                        principalColumn: "AccountNum");
                    table.ForeignKey(
                        name: "FK_BalanceSheet_Ratio",
                        columns: x => new { x.AccountNum, x.StatementDate },
                        principalTable: "Ratio",
                        principalColumns: new[] { "AccountNum", "StatementDate" });
                });

            migrationBuilder.CreateTable(
                name: "BidSubcontractor",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    BidNumber = table.Column<string>(type: "varchar(9)", unicode: false, maxLength: 9, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Trade = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Amount = table.Column<int>(type: "int", nullable: true),
                    Bonded = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BidSubcontractor", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_BidSubcontractor_BidRequest",
                        column: x => x.BidNumber,
                        principalTable: "BidRequest",
                        principalColumn: "BidNumber");
                });

            migrationBuilder.CreateTable(
                name: "OtherBid",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    BidNumber = table.Column<string>(type: "varchar(9)", unicode: false, maxLength: 9, nullable: false),
                    Bidder = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OtherBid", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_OtherBid_BidRequest",
                        column: x => x.BidNumber,
                        principalTable: "BidRequest",
                        principalColumn: "BidNumber");
                });

            migrationBuilder.CreateTable(
                name: "BondTransactionPurpose",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    BondNumber = table.Column<string>(type: "varchar(12)", unicode: false, maxLength: 12, nullable: false),
                    TransactionGroup = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TransactionPurpose = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    BondMod = table.Column<int>(type: "int", nullable: false),
                    GroupNumber = table.Column<int>(type: "int", nullable: false),
                    OldValue = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    NewValue = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BondTransactionPurpose", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_BondTransactionPurpose_Bond",
                        column: x => x.BondNumber,
                        principalTable: "Bond",
                        principalColumn: "BondNumber");
                    table.ForeignKey(
                        name: "FK_BondTransactionPurpose_BondTransaction",
                        columns: x => new { x.BondNumber, x.BondMod, x.GroupNumber },
                        principalTable: "BondTransaction",
                        principalColumns: new[] { "BondNumber", "BondMod", "GroupNumber" });
                });

            migrationBuilder.CreateTable(
                name: "Subaccount",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    ParentAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Sequence = table.Column<int>(type: "int", nullable: false),
                    Stated = table.Column<int>(type: "int", nullable: true),
                    Adjustment = table.Column<int>(type: "int", nullable: true),
                    AsAllowed = table.Column<int>(type: "int", nullable: true, computedColumnSql: "([Stated]+[Adjustment])", stored: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subaccount", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_Subaccount_BalanceSheet",
                        column: x => x.ParentAccountId,
                        principalTable: "BalanceSheet",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_AgencyNumber",
                table: "Account",
                column: "AgencyNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Account_AgentId",
                table: "Account",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_Account_AttorneyId",
                table: "Account",
                column: "AttorneyId");

            migrationBuilder.CreateIndex(
                name: "IX_Account_BankPhoneId",
                table: "Account",
                column: "BankPhoneId");

            migrationBuilder.CreateIndex(
                name: "IX_Account_Branch",
                table: "Account",
                column: "Branch");

            migrationBuilder.CreateIndex(
                name: "IX_Account_BranchReviewBy",
                table: "Account",
                column: "BranchReviewBy");

            migrationBuilder.CreateIndex(
                name: "IX_Account_BusinessType",
                table: "Account",
                column: "BusinessType");

            migrationBuilder.CreateIndex(
                name: "IX_Account_BusinessTypeClass",
                table: "Account",
                column: "BusinessTypeClass");

            migrationBuilder.CreateIndex(
                name: "IX_Account_CPAContactId",
                table: "Account",
                column: "CPAContactId");

            migrationBuilder.CreateIndex(
                name: "IX_Account_CPAFirmId",
                table: "Account",
                column: "CPAFirmId");

            migrationBuilder.CreateIndex(
                name: "IX_Account_Division",
                table: "Account",
                column: "Division");

            migrationBuilder.CreateIndex(
                name: "IX_Account_HomeOfficeReviewBy",
                table: "Account",
                column: "HomeOfficeReviewBy");

            migrationBuilder.CreateIndex(
                name: "IX_Account_LawFirmId",
                table: "Account",
                column: "LawFirmId");

            migrationBuilder.CreateIndex(
                name: "IX_Account_TaxBasis",
                table: "Account",
                column: "TaxBasis");

            migrationBuilder.CreateIndex(
                name: "IX_Account_Underwriter",
                table: "Account",
                column: "Underwriter");

            migrationBuilder.CreateIndex(
                name: "IX_Account_WatchStatus",
                table: "Account",
                column: "WatchStatus");

            migrationBuilder.CreateIndex(
                name: "UQ_Account_AccountNum",
                table: "Account",
                column: "AccountNum",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_Account_Id",
                table: "Account",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_AccountClassDM_Id",
                table: "AccountClassDM",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccountProgram_AccountNum",
                table: "AccountProgram",
                column: "AccountNum");

            migrationBuilder.CreateIndex(
                name: "UQ_AccountRate_Id",
                table: "AccountRate",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccountRateAttachment_AccountRateId",
                table: "AccountRateAttachment",
                column: "AccountRateId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountReference_Type",
                table: "AccountReference",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "UQ_AccountStatusDM_Id",
                table: "AccountStatusDM",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccountStatusLog_AccountStatus",
                table: "AccountStatusLog",
                column: "AccountStatus");

            migrationBuilder.CreateIndex(
                name: "IX_AccountStatusLog_Created_AccountNum_AccountStatus",
                table: "AccountStatusLog",
                columns: new[] { "Created", "AccountNum" },
                descending: new[] { true, false });

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalRelatedParty_AccountNum",
                table: "AdditionalRelatedParty",
                column: "AccountNum");

            migrationBuilder.CreateIndex(
                name: "IX_Address_StateCode",
                table: "Address",
                column: "StateCode");

            migrationBuilder.CreateIndex(
                name: "UQ_AddressType_Id",
                table: "AddressTypeDM",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_AddressType_Order",
                table: "AddressTypeDM",
                column: "Order",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Agency_BillingContactId",
                table: "Agency",
                column: "BillingContactId");

            migrationBuilder.CreateIndex(
                name: "IX_Agency_Status",
                table: "Agency",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "UQ_Agency_AgencyNumber",
                table: "Agency",
                column: "AgencyNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_Agency_Id",
                table: "Agency",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AgencyCompetition_AccountNum",
                table: "AgencyCompetition",
                column: "AccountNum");

            migrationBuilder.CreateIndex(
                name: "IX_AgencyCompetition_AgencyId",
                table: "AgencyCompetition",
                column: "AgencyId");

            migrationBuilder.CreateIndex(
                name: "IX_AgencyCompetition_EnteredBy",
                table: "AgencyCompetition",
                column: "EnteredBy");

            migrationBuilder.CreateIndex(
                name: "IX_AgencyErrorAndOmission_AgencyId",
                table: "AgencyErrorAndOmission",
                column: "AgencyId");

            migrationBuilder.CreateIndex(
                name: "IX_AgencyInventory_AddresseeId",
                table: "AgencyInventory",
                column: "AddresseeId");

            migrationBuilder.CreateIndex(
                name: "IX_AgencyInventory_AgencyId",
                table: "AgencyInventory",
                column: "AgencyId");

            migrationBuilder.CreateIndex(
                name: "IX_AgencyInventory_Approver",
                table: "AgencyInventory",
                column: "Approver");

            migrationBuilder.CreateIndex(
                name: "IX_AgencyInventory_DocumentType",
                table: "AgencyInventory",
                column: "DocumentType");

            migrationBuilder.CreateIndex(
                name: "IX_AgencyLicense_AgentId",
                table: "AgencyLicense",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_AgencyLicense_InsurerId",
                table: "AgencyLicense",
                column: "InsurerId");

            migrationBuilder.CreateIndex(
                name: "IX_AgencyLicense_State",
                table: "AgencyLicense",
                column: "State");

            migrationBuilder.CreateIndex(
                name: "UQ_AgencyStatusDM_Id",
                table: "AgencyStatusDM",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AgencyStatusLog_ChangedBy",
                table: "AgencyStatusLog",
                column: "ChangedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Agent_DefaultCellNumber",
                table: "Agent",
                column: "DefaultCellNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Agent_DefaultPhoneNumber",
                table: "Agent",
                column: "DefaultPhoneNumber");

            migrationBuilder.CreateIndex(
                name: "IX_AgentsInAgency_AgencyId",
                table: "AgentsInAgency",
                column: "AgencyId");

            migrationBuilder.CreateIndex(
                name: "UQ_AgentSystemDM_Id",
                table: "AgentSystemDM",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_AgreementTypeDM_Id",
                table: "AgreementTypeDM",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BalanceSheet_AccountNum_StatementDate_AccountType_Sequence",
                table: "BalanceSheet",
                columns: new[] { "AccountNum", "StatementDate", "AccountType", "Sequence" })
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateIndex(
                name: "UQ_BidPercentDM_Id",
                table: "BidPercentDM",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BidRequest_AccountNum",
                table: "BidRequest",
                column: "AccountNum");

            migrationBuilder.CreateIndex(
                name: "IX_BidRequest_BidPercent",
                table: "BidRequest",
                column: "BidPercent");

            migrationBuilder.CreateIndex(
                name: "IX_BidRequest_BidResult",
                table: "BidRequest",
                column: "BidResult");

            migrationBuilder.CreateIndex(
                name: "IX_BidRequest_BondNumber",
                table: "BidRequest",
                column: "BondNumber");

            migrationBuilder.CreateIndex(
                name: "IX_BidRequest_CCTo",
                table: "BidRequest",
                column: "CCTo");

            migrationBuilder.CreateIndex(
                name: "IX_BidRequest_ObligeeId",
                table: "BidRequest",
                column: "ObligeeId");

            migrationBuilder.CreateIndex(
                name: "IX_BidRequest_Retainage",
                table: "BidRequest",
                column: "Retainage");

            migrationBuilder.CreateIndex(
                name: "IX_BidRequest_Status",
                table: "BidRequest",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_BidRequest_Underwriter",
                table: "BidRequest",
                column: "Underwriter");

            migrationBuilder.CreateIndex(
                name: "UQ_BidRequest_Id",
                table: "BidRequest",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BidRequestCommercial_AccountNum",
                table: "BidRequestCommercial",
                column: "AccountNum");

            migrationBuilder.CreateIndex(
                name: "IX_BidRequestCommercial_BondNumber",
                table: "BidRequestCommercial",
                column: "BondNumber");

            migrationBuilder.CreateIndex(
                name: "IX_BidRequestCommercial_CCTo",
                table: "BidRequestCommercial",
                column: "CCTo");

            migrationBuilder.CreateIndex(
                name: "IX_BidRequestCommercial_HomeOfficeApprovedBy",
                table: "BidRequestCommercial",
                column: "HomeOfficeApprovedBy");

            migrationBuilder.CreateIndex(
                name: "IX_BidRequestCommercial_ObligeeId",
                table: "BidRequestCommercial",
                column: "ObligeeId");

            migrationBuilder.CreateIndex(
                name: "IX_BidRequestCommercial_RecordedBy",
                table: "BidRequestCommercial",
                column: "RecordedBy");

            migrationBuilder.CreateIndex(
                name: "IX_BidRequestCommercial_SFAAClassCode",
                table: "BidRequestCommercial",
                column: "SFAAClassCode");

            migrationBuilder.CreateIndex(
                name: "IX_BidRequestCommercial_Status",
                table: "BidRequestCommercial",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_BidRequestCommercial_Underwriter",
                table: "BidRequestCommercial",
                column: "Underwriter");

            migrationBuilder.CreateIndex(
                name: "UQ_BidRequestCommercial_Id",
                table: "BidRequestCommercial",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_BidResult_Id",
                table: "BidResultDM",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_BidRetainageDM_Id",
                table: "BidRetainageDM",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_BidStatus_Id",
                table: "BidStatusDM",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BidSubcontractor_BidNumber",
                table: "BidSubcontractor",
                column: "BidNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Bond_AccountNum",
                table: "Bond",
                column: "AccountNum");

            migrationBuilder.CreateIndex(
                name: "IX_Bond_AgencyId",
                table: "Bond",
                column: "AgencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Bond_AgentId",
                table: "Bond",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_Bond_AttorneyInFactId",
                table: "Bond",
                column: "AttorneyInFactId");

            migrationBuilder.CreateIndex(
                name: "IX_Bond_BondTypeId",
                table: "Bond",
                column: "BondTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Bond_DirectBillAddressId",
                table: "Bond",
                column: "DirectBillAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Bond_InsurerId",
                table: "Bond",
                column: "InsurerId");

            migrationBuilder.CreateIndex(
                name: "IX_Bond_ObligeeId",
                table: "Bond",
                column: "ObligeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Bond_ResponsiblePartyId",
                table: "Bond",
                column: "ResponsiblePartyId");

            migrationBuilder.CreateIndex(
                name: "IX_Bond_SFAABondType",
                table: "Bond",
                column: "SFAABondType");

            migrationBuilder.CreateIndex(
                name: "IX_Bond_SICCode",
                table: "Bond",
                column: "SICCode");

            migrationBuilder.CreateIndex(
                name: "IX_Bond_State",
                table: "Bond",
                column: "State");

            migrationBuilder.CreateIndex(
                name: "IX_Bond_Underwriter",
                table: "Bond",
                column: "Underwriter");

            migrationBuilder.CreateIndex(
                name: "UQ_Bond_Id",
                table: "Bond",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BondBlock_IssuedBy",
                table: "BondBlock",
                column: "IssuedBy");

            migrationBuilder.CreateIndex(
                name: "IX_BondHold_AccountNum",
                table: "BondHold",
                column: "AccountNum");

            migrationBuilder.CreateIndex(
                name: "IX_BondHold_AgencyId",
                table: "BondHold",
                column: "AgencyId");

            migrationBuilder.CreateIndex(
                name: "IX_BondHold_AgentId",
                table: "BondHold",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_BondHold_BondTypeId",
                table: "BondHold",
                column: "BondTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BondHold_HomeOfficeApprovedBy",
                table: "BondHold",
                column: "HomeOfficeApprovedBy");

            migrationBuilder.CreateIndex(
                name: "IX_BondHold_InsurerId",
                table: "BondHold",
                column: "InsurerId");

            migrationBuilder.CreateIndex(
                name: "IX_BondHold_ObligeeId",
                table: "BondHold",
                column: "ObligeeId");

            migrationBuilder.CreateIndex(
                name: "IX_BondHold_ResponsiblePartyId",
                table: "BondHold",
                column: "ResponsiblePartyId");

            migrationBuilder.CreateIndex(
                name: "IX_BondHold_SFAACode",
                table: "BondHold",
                column: "SFAACode");

            migrationBuilder.CreateIndex(
                name: "IX_BondHold_SICCode",
                table: "BondHold",
                column: "SICCode");

            migrationBuilder.CreateIndex(
                name: "IX_BondHold_State",
                table: "BondHold",
                column: "State");

            migrationBuilder.CreateIndex(
                name: "IX_BondHold_Underwriter",
                table: "BondHold",
                column: "Underwriter");

            migrationBuilder.CreateIndex(
                name: "UQ_BondHold_Id",
                table: "BondHold",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_BondModTransaction_Id",
                table: "BondModTransaction",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BondStatusLetter_BondNumber",
                table: "BondStatusLetter",
                column: "BondNumber");

            migrationBuilder.CreateIndex(
                name: "IX_BondTransaction_AccountClass",
                table: "BondTransaction",
                column: "AccountClass");

            migrationBuilder.CreateIndex(
                name: "IX_BondTransaction_AccountNum",
                table: "BondTransaction",
                column: "AccountNum");

            migrationBuilder.CreateIndex(
                name: "IX_BondTransaction_AgencyId",
                table: "BondTransaction",
                column: "AgencyId");

            migrationBuilder.CreateIndex(
                name: "IX_BondTransaction_BondNumber_BondMod_GroupNumber",
                table: "BondTransaction",
                columns: new[] { "BondNumber", "BondMod", "GroupNumber" });

            migrationBuilder.CreateIndex(
                name: "IX_BondTransaction_Region",
                table: "BondTransaction",
                column: "Region");

            migrationBuilder.CreateIndex(
                name: "IX_BondTransaction_SfaaCode",
                table: "BondTransaction",
                column: "SfaaCode");

            migrationBuilder.CreateIndex(
                name: "IX_BondTransaction_UnderwriterId",
                table: "BondTransaction",
                column: "UnderwriterId");

            migrationBuilder.CreateIndex(
                name: "UQ_BondTransaaction_Id",
                table: "BondTransaction",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_BondTransaction_BondNumber_BondMod_GroupNumber",
                table: "BondTransaction",
                columns: new[] { "BondNumber", "BondMod", "GroupNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BondTransactionPurpose_BondNumber_BondMod_GroupNumber",
                table: "BondTransactionPurpose",
                columns: new[] { "BondNumber", "BondMod", "GroupNumber" });

            migrationBuilder.CreateIndex(
                name: "UQ_BondType_Id",
                table: "BondTypeDM",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Branch_BranchOfficeAddressId",
                table: "Branch",
                column: "BranchOfficeAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Branch_PhoneId",
                table: "Branch",
                column: "PhoneId");

            migrationBuilder.CreateIndex(
                name: "IX_Branch_Region",
                table: "Branch",
                column: "Region");

            migrationBuilder.CreateIndex(
                name: "UQ_Branch_Id",
                table: "Branch",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_BusinessTypeClassCodeDM_Id",
                table: "BusinessTypeClassCodeDM",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_BusinessTypeDM_Id",
                table: "BusinessTypeDM",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CashFlowStatement_AccountNum",
                table: "CashFlowStatement",
                column: "AccountNum");

            migrationBuilder.CreateIndex(
                name: "IX_CashFlowStatement_Basis",
                table: "CashFlowStatement",
                column: "Basis");

            migrationBuilder.CreateIndex(
                name: "IX_CashFlowStatement_Quality",
                table: "CashFlowStatement",
                column: "Quality");

            migrationBuilder.CreateIndex(
                name: "IX_CashFlowStatement_Scaling",
                table: "CashFlowStatement",
                column: "Scaling");

            migrationBuilder.CreateIndex(
                name: "IX_CashFlowStatement_TaxBasis",
                table: "CashFlowStatement",
                column: "TaxBasis");

            migrationBuilder.CreateIndex(
                name: "IX_CashFlowStatement_Type",
                table: "CashFlowStatement",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_Collateral_BondNumber",
                table: "Collateral",
                column: "BondNumber");

            migrationBuilder.CreateIndex(
                name: "UQ_CollateralTypeDM_Id",
                table: "CollateralTypeDM",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_CommercialBondTypeDM_Id",
                table: "CommercialBondTypeDM",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommercialRate_CommercialBondType",
                table: "CommercialRate",
                column: "CommercialBondType");

            migrationBuilder.CreateIndex(
                name: "IX_CommercialRate_RateGroup",
                table: "CommercialRate",
                column: "RateGroup");

            migrationBuilder.CreateIndex(
                name: "IX_CommercialRate_RiskType",
                table: "CommercialRate",
                column: "RiskType");

            migrationBuilder.CreateIndex(
                name: "UQ_CommercialRegionDM_Id",
                table: "CommercialRegionDM",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContractRate_RateGroup",
                table: "ContractRate",
                column: "RateGroup");

            migrationBuilder.CreateIndex(
                name: "IX_CoPrincipal_AccountNum",
                table: "CoPrincipal",
                column: "AccountNum");

            migrationBuilder.CreateIndex(
                name: "IX_CoPrincipal_BondNumber",
                table: "CoPrincipal",
                column: "BondNumber");

            migrationBuilder.CreateIndex(
                name: "UQ_CreditReportDM_Id",
                table: "CreditReportDM",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DefaultGeneralLedgerAccount_AccountClass",
                table: "DefaultGeneralLedgerAccount",
                column: "AccountClass");

            migrationBuilder.CreateIndex(
                name: "UQ_DivisionDM_Id",
                table: "DivisionDM",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_DocumentDataMissingAction_Id",
                table: "DocumentDataMissingAction",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_DocumentDefinition_Name",
                table: "DocumentDefinition",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DocumentDefinitionRule_RuleId",
                table: "DocumentDefinitionRule",
                column: "RuleId");

            migrationBuilder.CreateIndex(
                name: "UQ_DocumentDefinitionRule_DefinitionId_RuleId",
                table: "DocumentDefinitionRule",
                columns: new[] { "DefinitionId", "RuleId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DocumentRuleReplacementMap_IsMissingId",
                table: "DocumentRuleReplacementMap",
                column: "IsMissingId");

            migrationBuilder.CreateIndex(
                name: "UQ_DocumentRuleReplacementMap_Token",
                table: "DocumentRuleReplacementMap",
                columns: new[] { "RuleId", "Token" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_EmailActionDM_Id",
                table: "EmailActionDM",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmailHistory_AccountNum",
                table: "EmailHistory",
                column: "AccountNum");

            migrationBuilder.CreateIndex(
                name: "IX_EmailHistory_Action",
                table: "EmailHistory",
                column: "Action");

            migrationBuilder.CreateIndex(
                name: "IX_EmailHistory_BondNumber",
                table: "EmailHistory",
                column: "BondNumber");

            migrationBuilder.CreateIndex(
                name: "UQ_FinancialAccountType_Id",
                table: "FinancialAccountTypeDM",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_FinancialRatios_Id",
                table: "FinancialRatio",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_ImagingCategory_Id",
                table: "ImagingCategory",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ImagingCategoryTabDivision_Category",
                table: "ImagingCategoryTabDivision",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_ImagingCategoryTabDivision_DivisionCode",
                table: "ImagingCategoryTabDivision",
                column: "DivisionCode");

            migrationBuilder.CreateIndex(
                name: "IX_ImagingCategoryTabDivision_TabName",
                table: "ImagingCategoryTabDivision",
                column: "TabName");

            migrationBuilder.CreateIndex(
                name: "UQ_ImagingTab_Id",
                table: "ImagingTab",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_ImagingType_Id",
                table: "ImagingType",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_Indemnitor_AccountNum_AgreementDate_AgreementType_Id_Signatory_Title",
                table: "Indemnitor",
                columns: new[] { "AccountNum", "AgreementDate", "AgreementType", "Id", "Signatory", "Title" },
                unique: true,
                filter: "[AgreementType] IS NOT NULL AND [Signatory] IS NOT NULL AND [Title] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Insurer_CurrencyCountry",
                table: "Insurer",
                column: "CurrencyCountry");

            migrationBuilder.CreateIndex(
                name: "IX_InsurerState_InsurerId",
                table: "InsurerState",
                column: "InsurerId");

            migrationBuilder.CreateIndex(
                name: "IX_InsurerState_State",
                table: "InsurerState",
                column: "State");

            migrationBuilder.CreateIndex(
                name: "UQ_InventoryDocumentDM_Id",
                table: "InventoryDocumentDM",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_KeyPersonnel_AccountNum",
                table: "KeyPersonnel",
                column: "AccountNum");

            migrationBuilder.CreateIndex(
                name: "IX_KeyPersonnel_Responsibility",
                table: "KeyPersonnel",
                column: "Responsibility");

            migrationBuilder.CreateIndex(
                name: "IX_LegalEntity_EntityType",
                table: "LegalEntity",
                column: "EntityType");

            migrationBuilder.CreateIndex(
                name: "IX_LegalEntity_Parent",
                table: "LegalEntity",
                column: "Parent");

            migrationBuilder.CreateIndex(
                name: "IX_LegalEntityAddress_Type",
                table: "LegalEntityAddress",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "UQ_LegalEntityAddress_AddressId",
                table: "LegalEntityAddress",
                column: "AddressId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LegalEntityPhone_Type",
                table: "LegalEntityPhone",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "UQ_LegalEntityPhone_PhoneNumberId",
                table: "LegalEntityPhone",
                column: "PhoneNumberId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_LicenseStatusDM_Id",
                table: "LicenseStatusDM",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LineOAuthorityLog_ApprovedBy",
                table: "LineOAuthorityLog",
                column: "ApprovedBy");

            migrationBuilder.CreateIndex(
                name: "IX_LineOAuthorityLog_CreatedBy",
                table: "LineOAuthorityLog",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_LineOAuthorityLog_Division",
                table: "LineOAuthorityLog",
                column: "Division");

            migrationBuilder.CreateIndex(
                name: "IX_LineOAuthorityLog_Status",
                table: "LineOAuthorityLog",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "UQ_LineOFAuthorityLog_Id",
                table: "LineOAuthorityLog",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_LineOfAuthorityStatusDM_Id",
                table: "LineOfAuthorityStatusDM",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_LineOfBusinessDM_Id",
                table: "LineOfBusinessDM",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_NAICSCode_Id",
                table: "NAICSCode",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NotebookEntry_CreatedBy",
                schema: "Beta",
                table: "NotebookEntry",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_NotebookEntry_NotebookId",
                schema: "Beta",
                table: "NotebookEntry",
                column: "NotebookId");

            migrationBuilder.CreateIndex(
                name: "IX_NotebookEntry_Type",
                schema: "Beta",
                table: "NotebookEntry",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "UQ_NotebookEntryTypeDM_Id",
                schema: "Beta",
                table: "NotebookEntryTypeDM",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_NoteTypeDM_Id",
                table: "NoteTypeDM",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Obligee_Type",
                table: "Obligee",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "UQ_ObligeeTypeDM_Id",
                table: "ObligeeTypeDM",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OnlineBondSystem_AgencyId",
                table: "OnlineBondSystem",
                column: "AgencyId");

            migrationBuilder.CreateIndex(
                name: "IX_OnlineBondSystem_InsurerId",
                table: "OnlineBondSystem",
                column: "InsurerId");

            migrationBuilder.CreateIndex(
                name: "IX_OnlineBondSystem_PowerOfAttorneyId",
                table: "OnlineBondSystem",
                column: "PowerOfAttorneyId");

            migrationBuilder.CreateIndex(
                name: "IX_OnlineBondSystem_SystemName",
                table: "OnlineBondSystem",
                column: "SystemName");

            migrationBuilder.CreateIndex(
                name: "IX_OpenClaim_BondNumber",
                table: "OpenClaim",
                column: "BondNumber");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationTitle_Type",
                table: "OrganizationTitle",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "UQ_OrganizationTypeDM_Id",
                table: "OrganizationTypeDM",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OtherBid_BidNumber",
                table: "OtherBid",
                column: "BidNumber");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalFinancialHeader_Basis",
                table: "PersonalFinancialHeader",
                column: "Basis");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalFinancialHeader_PersonId",
                table: "PersonalFinancialHeader",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalFinancialHeader_Quality",
                table: "PersonalFinancialHeader",
                column: "Quality");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalFinancialHeader_Scaling",
                table: "PersonalFinancialHeader",
                column: "Scaling");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalFinancialHeader_TaxBasis",
                table: "PersonalFinancialHeader",
                column: "TaxBasis");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalFinancialHeader_Type",
                table: "PersonalFinancialHeader",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "UQ_PersonalFinancialHeader_Id",
                table: "PersonalFinancialHeader",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonalFinancialStatement_AccountNum_StatementDate_AccountType_Sequence",
                table: "PersonalFinancialStatement",
                columns: new[] { "AccountNum", "StatementDate", "AccountType", "Sequence" })
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonalFinancialStatement_HeaderId",
                table: "PersonalFinancialStatement",
                column: "HeaderId");

            migrationBuilder.CreateIndex(
                name: "UQ_PersonalFinancialStatementTypeDM_Id",
                table: "PersonalFinancialStatementTypeDM",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonalFinancialSubaccount_ParentAccountId",
                table: "PersonalFinancialSubaccount",
                column: "ParentAccountId");

            migrationBuilder.CreateIndex(
                name: "UQ_PhoneType_Id",
                table: "PhoneTypeDM",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_PhoneType_Order",
                table: "PhoneTypeDM",
                column: "Order",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PowerOfAttorney_InsurerId",
                table: "PowerOfAttorney",
                column: "InsurerId");

            migrationBuilder.CreateIndex(
                name: "IX_PowerOfAttorney_Status",
                table: "PowerOfAttorney",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "UQ_PowerOfAttorneyDocumentTypeDM_Id",
                table: "PowerOfAttorneyDocumentNameDM",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PowerOfAttorneyDocumentStatus_Name",
                table: "PowerOfAttorneyDocumentStatus",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_PowerOfAttorneyDocumentStatus_POAId",
                table: "PowerOfAttorneyDocumentStatus",
                column: "POAId");

            migrationBuilder.CreateIndex(
                name: "UQ_PowerOfAttourneyStatusDM_Id",
                table: "PowerOfAttorneyStatusDM",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProfitCenter_BudgetDefault",
                table: "ProfitCenter",
                column: "BudgetDefault");

            migrationBuilder.CreateIndex(
                name: "IX_ProfitCenter_CommercialRegion",
                table: "ProfitCenter",
                column: "CommercialRegion");

            migrationBuilder.CreateIndex(
                name: "IX_ProfitCenter_DivisionCode",
                table: "ProfitCenter",
                column: "DivisionCode");

            migrationBuilder.CreateIndex(
                name: "IX_ProfitCenter_LineOfBusiness",
                table: "ProfitCenter",
                column: "LineOfBusiness");

            migrationBuilder.CreateIndex(
                name: "IX_ProfitCenter_Underwriter",
                table: "ProfitCenter",
                column: "Underwriter");

            migrationBuilder.CreateIndex(
                name: "UQ_RateGroup_Id",
                table: "RateGroup",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_RateStructureDM_Id",
                table: "RateStructureDM",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ratio_AccountNum_StatementDate",
                table: "Ratio",
                columns: new[] { "AccountNum", "StatementDate" });

            migrationBuilder.CreateIndex(
                name: "UQ_Ratio_AccountNum_StatementDate",
                table: "Ratio",
                columns: new[] { "AccountNum", "StatementDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_ReferenceTypeDM_Id",
                table: "ReferenceTypeDM",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_ResponsibilityDM_Id",
                table: "ResponsibilityDM",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ResponsibleParty_Type",
                table: "ResponsibleParty",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "UQ_ResponsiblePartyTypeDM_Id",
                table: "ResponsiblePartyTypeDM",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_RiskTypeDM_Id",
                table: "RiskTypeDM",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_ScalingDM_Id",
                table: "ScalingDM",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_SFAABondType_Id",
                table: "SFAABondTypeDM",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SICRatio_Code",
                table: "SICRatio",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_SICRatio_Type",
                table: "SICRatio",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "UQ_SICRatioTypeDM_Id",
                table: "SICRatioTypeDM",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_State_CountryCode",
                table: "State",
                column: "CountryCode");

            migrationBuilder.CreateIndex(
                name: "UQ_StatementBasisDM_Id",
                table: "StatementBasisDM",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_StatementQualityDM_Id",
                table: "StatementQualityDM",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_StatementTypeDM_Id",
                table: "StatementTypeDM",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subaccount_ParentAccountId",
                table: "Subaccount",
                column: "ParentAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Surcharge_CreatedBy",
                table: "Surcharge",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Surcharge_State",
                table: "Surcharge",
                column: "State");

            migrationBuilder.CreateIndex(
                name: "IX_Surcharge_Type",
                table: "Surcharge",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "UQ_SurchargeTypeDM_Id",
                table: "SurchargeTypeDM",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_SystemNameDM_Id",
                table: "SystemNameDM",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_TaxBasisDM_Id",
                table: "TaxBasisDM",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UnderwriterRecommendation_AccountNum",
                table: "UnderwriterRecommendation",
                column: "AccountNum");

            migrationBuilder.CreateIndex(
                name: "UQ_UserLayoutColumn_Id",
                schema: "Beta",
                table: "UserLayoutColumn",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_UserLayoutWidget_Id",
                schema: "Beta",
                table: "UserLayoutWidget",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserLineOfAuthority_CreatedBy",
                table: "UserLineOfAuthority",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserLineOfAuthority_ModifiedBy",
                table: "UserLineOfAuthority",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserLineOfAuthority_UserId",
                table: "UserLineOfAuthority",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "UQ_UserMenu_Id",
                schema: "Beta",
                table: "UserMenu",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_UserPreference_Id",
                schema: "Beta",
                table: "UserPreference",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserProfile_Initials",
                table: "UserProfile",
                column: "Initials");

            migrationBuilder.CreateIndex(
                name: "UQ_UserProfile_Initials",
                table: "UserProfile",
                column: "Initials",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VoidedBond_AgencyId",
                table: "VoidedBond",
                column: "AgencyId");

            migrationBuilder.CreateIndex(
                name: "IX_VoidedBond_VoidedBy",
                table: "VoidedBond",
                column: "VoidedBy");

            migrationBuilder.CreateIndex(
                name: "UQ_WatchStatusDM_Id",
                table: "WatchStatusDM",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkInProgressJob_AccountNum_WipDate_JobNumber_BondNumber",
                table: "WorkInProgressJob",
                columns: new[] { "AccountNum", "WIPDate", "JobNumber", "BondNumber" })
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkInProgressJob_BondNumber",
                table: "WorkInProgressJob",
                column: "BondNumber");

            migrationBuilder.CreateIndex(
                name: "UQ_WorkInProgressJob_Id",
                table: "WorkInProgressJob",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_WorkInProgressSummary_Id",
                table: "WorkInProgressSummary",
                column: "Id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountProgram");

            migrationBuilder.DropTable(
                name: "AccountProgramEmailNotificationGroups");

            migrationBuilder.DropTable(
                name: "AccountProgramStatusDM");

            migrationBuilder.DropTable(
                name: "AccountProgramStatusHistory");

            migrationBuilder.DropTable(
                name: "AccountProgramUserAuthority");

            migrationBuilder.DropTable(
                name: "AccountRateAttachment");

            migrationBuilder.DropTable(
                name: "AccountReference");

            migrationBuilder.DropTable(
                name: "AccountStatusLog");

            migrationBuilder.DropTable(
                name: "AdditionalObligee");

            migrationBuilder.DropTable(
                name: "AdditionalRelatedParty");

            migrationBuilder.DropTable(
                name: "AgencyCompetition");

            migrationBuilder.DropTable(
                name: "AgencyErrorAndOmission");

            migrationBuilder.DropTable(
                name: "AgencyInventory");

            migrationBuilder.DropTable(
                name: "AgencyLicense");

            migrationBuilder.DropTable(
                name: "AgencyStatusLog");

            migrationBuilder.DropTable(
                name: "Agent");

            migrationBuilder.DropTable(
                name: "AgentsInAgency");

            migrationBuilder.DropTable(
                name: "AgreementTypeDM");

            migrationBuilder.DropTable(
                name: "AppUser");

            migrationBuilder.DropTable(
                name: "BidRequestCommercial");

            migrationBuilder.DropTable(
                name: "BidSubcontractor");

            migrationBuilder.DropTable(
                name: "BondBlock");

            migrationBuilder.DropTable(
                name: "BondHold");

            migrationBuilder.DropTable(
                name: "BondModTransaction");

            migrationBuilder.DropTable(
                name: "BondStatusLetter");

            migrationBuilder.DropTable(
                name: "BondTransactionPurpose");

            migrationBuilder.DropTable(
                name: "BookRatio");

            migrationBuilder.DropTable(
                name: "CashFlowStatement");

            migrationBuilder.DropTable(
                name: "CoInsurer");

            migrationBuilder.DropTable(
                name: "Collateral");

            migrationBuilder.DropTable(
                name: "CollateralTypeDM");

            migrationBuilder.DropTable(
                name: "CommercialRate");

            migrationBuilder.DropTable(
                name: "Competition");

            migrationBuilder.DropTable(
                name: "ContractRate");

            migrationBuilder.DropTable(
                name: "CoPrincipal");

            migrationBuilder.DropTable(
                name: "CreditReportDM");

            migrationBuilder.DropTable(
                name: "DefaultGeneralLedgerAccount");

            migrationBuilder.DropTable(
                name: "DocumentDefinitionRule");

            migrationBuilder.DropTable(
                name: "DocumentRuleReplacementMap");

            migrationBuilder.DropTable(
                name: "EmailHistory");

            migrationBuilder.DropTable(
                name: "FinancialAccountTypeDM");

            migrationBuilder.DropTable(
                name: "FinancialRatio");

            migrationBuilder.DropTable(
                name: "HomeOfficeEmailTeam");

            migrationBuilder.DropTable(
                name: "ImagingCategoryTabDivision");

            migrationBuilder.DropTable(
                name: "ImagingType");

            migrationBuilder.DropTable(
                name: "Indemnitor");

            migrationBuilder.DropTable(
                name: "InsurerState");

            migrationBuilder.DropTable(
                name: "KeyPersonnel");

            migrationBuilder.DropTable(
                name: "LegalEntityAddress");

            migrationBuilder.DropTable(
                name: "LegalEntityPhone");

            migrationBuilder.DropTable(
                name: "LicenseStatusDM");

            migrationBuilder.DropTable(
                name: "LineOAuthorityLog");

            migrationBuilder.DropTable(
                name: "NAICSCode");

            migrationBuilder.DropTable(
                name: "NotebookEntry",
                schema: "Beta");

            migrationBuilder.DropTable(
                name: "NoteTypeDM");

            migrationBuilder.DropTable(
                name: "OnlineBondSystem");

            migrationBuilder.DropTable(
                name: "OpenClaim");

            migrationBuilder.DropTable(
                name: "OrganizationTitle");

            migrationBuilder.DropTable(
                name: "OtherBid");

            migrationBuilder.DropTable(
                name: "PermissionRole");

            migrationBuilder.DropTable(
                name: "PersonalFinancialSubaccount");

            migrationBuilder.DropTable(
                name: "PowerOfAttorneyDocumentStatus");

            migrationBuilder.DropTable(
                name: "ProducerInvoiceEmail");

            migrationBuilder.DropTable(
                name: "ProfitCenter");

            migrationBuilder.DropTable(
                name: "RateStructureDM");

            migrationBuilder.DropTable(
                name: "RenewalRequest");

            migrationBuilder.DropTable(
                name: "RenewalRequestSource");

            migrationBuilder.DropTable(
                name: "ResponsibleParty");

            migrationBuilder.DropTable(
                name: "SICRatio");

            migrationBuilder.DropTable(
                name: "Subaccount");

            migrationBuilder.DropTable(
                name: "Surcharge");

            migrationBuilder.DropTable(
                name: "SystemNameDM");

            migrationBuilder.DropTable(
                name: "TicketTasks");

            migrationBuilder.DropTable(
                name: "UnderwriterRecommendation");

            migrationBuilder.DropTable(
                name: "UserLayoutWidget",
                schema: "Beta");

            migrationBuilder.DropTable(
                name: "UserLineOfAuthority");

            migrationBuilder.DropTable(
                name: "UserMenu",
                schema: "Beta");

            migrationBuilder.DropTable(
                name: "UserPreference",
                schema: "Beta");

            migrationBuilder.DropTable(
                name: "VoidedBond");

            migrationBuilder.DropTable(
                name: "WorkInProgressJob");

            migrationBuilder.DropTable(
                name: "AccountRate");

            migrationBuilder.DropTable(
                name: "ReferenceTypeDM");

            migrationBuilder.DropTable(
                name: "AccountStatusDM");

            migrationBuilder.DropTable(
                name: "InventoryDocumentDM");

            migrationBuilder.DropTable(
                name: "BondTransaction");

            migrationBuilder.DropTable(
                name: "StatementTypeDM");

            migrationBuilder.DropTable(
                name: "CommercialBondTypeDM");

            migrationBuilder.DropTable(
                name: "RiskTypeDM");

            migrationBuilder.DropTable(
                name: "RateGroup");

            migrationBuilder.DropTable(
                name: "DocumentDefinition");

            migrationBuilder.DropTable(
                name: "DocumentDataMissingAction");

            migrationBuilder.DropTable(
                name: "DocumentRule");

            migrationBuilder.DropTable(
                name: "EmailActionDM");

            migrationBuilder.DropTable(
                name: "ImagingCategory");

            migrationBuilder.DropTable(
                name: "ImagingTab");

            migrationBuilder.DropTable(
                name: "ResponsibilityDM");

            migrationBuilder.DropTable(
                name: "AddressTypeDM");

            migrationBuilder.DropTable(
                name: "PhoneTypeDM");

            migrationBuilder.DropTable(
                name: "LineOfAuthorityStatusDM");

            migrationBuilder.DropTable(
                name: "Notebook",
                schema: "Beta");

            migrationBuilder.DropTable(
                name: "NotebookEntryTypeDM",
                schema: "Beta");

            migrationBuilder.DropTable(
                name: "AgentSystemDM");

            migrationBuilder.DropTable(
                name: "OrganizationTypeDM");

            migrationBuilder.DropTable(
                name: "BidRequest");

            migrationBuilder.DropTable(
                name: "PersonalFinancialStatement");

            migrationBuilder.DropTable(
                name: "PowerOfAttorney");

            migrationBuilder.DropTable(
                name: "PowerOfAttorneyDocumentNameDM");

            migrationBuilder.DropTable(
                name: "CommercialRegionDM");

            migrationBuilder.DropTable(
                name: "LineOfBusinessDM");

            migrationBuilder.DropTable(
                name: "ResponsiblePartyTypeDM");

            migrationBuilder.DropTable(
                name: "SICRatioTypeDM");

            migrationBuilder.DropTable(
                name: "BalanceSheet");

            migrationBuilder.DropTable(
                name: "SurchargeTypeDM");

            migrationBuilder.DropTable(
                name: "UserLayoutColumn",
                schema: "Beta");

            migrationBuilder.DropTable(
                name: "WorkInProgressSummary");

            migrationBuilder.DropTable(
                name: "AccountClassDM");

            migrationBuilder.DropTable(
                name: "SFAA");

            migrationBuilder.DropTable(
                name: "BidPercentDM");

            migrationBuilder.DropTable(
                name: "BidResultDM");

            migrationBuilder.DropTable(
                name: "BidRetainageDM");

            migrationBuilder.DropTable(
                name: "BidStatusDM");

            migrationBuilder.DropTable(
                name: "Bond");

            migrationBuilder.DropTable(
                name: "Obligee");

            migrationBuilder.DropTable(
                name: "PersonalFinancialHeader");

            migrationBuilder.DropTable(
                name: "PowerOfAttorneyStatusDM");

            migrationBuilder.DropTable(
                name: "Ratio");

            migrationBuilder.DropTable(
                name: "BondTypeDM");

            migrationBuilder.DropTable(
                name: "Insurer");

            migrationBuilder.DropTable(
                name: "SFAABondTypeDM");

            migrationBuilder.DropTable(
                name: "SIC");

            migrationBuilder.DropTable(
                name: "ObligeeTypeDM");

            migrationBuilder.DropTable(
                name: "PersonalFinancialStatementTypeDM");

            migrationBuilder.DropTable(
                name: "ScalingDM");

            migrationBuilder.DropTable(
                name: "StatementBasisDM");

            migrationBuilder.DropTable(
                name: "StatementQualityDM");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "Agency");

            migrationBuilder.DropTable(
                name: "Branch");

            migrationBuilder.DropTable(
                name: "BusinessTypeClassCodeDM");

            migrationBuilder.DropTable(
                name: "BusinessTypeDM");

            migrationBuilder.DropTable(
                name: "DivisionDM");

            migrationBuilder.DropTable(
                name: "LawEntity");

            migrationBuilder.DropTable(
                name: "TaxBasisDM");

            migrationBuilder.DropTable(
                name: "UserProfile");

            migrationBuilder.DropTable(
                name: "WatchStatusDM");

            migrationBuilder.DropTable(
                name: "AgencyStatusDM");

            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "PhoneNumber");

            migrationBuilder.DropTable(
                name: "RegionDM");

            migrationBuilder.DropTable(
                name: "LegalEntity");

            migrationBuilder.DropTable(
                name: "State");

            migrationBuilder.DropTable(
                name: "LegalEntityTypeDM");

            migrationBuilder.DropTable(
                name: "CountryDM");
        }
    }
}
