using System;
using System.Collections.Generic;
using James.Shared;
using James.Shared.Model;
using Microsoft.EntityFrameworkCore;

namespace JamesDAL.Server.Model;

public partial class JamesDatabaseContext : DbContext
{
    public JamesDatabaseContext()
    {
    }

    public JamesDatabaseContext(DbContextOptions<JamesDatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<AccountClassDm> AccountClassDms { get; set; }

    public virtual DbSet<AccountProgram> AccountPrograms { get; set; }

    public virtual DbSet<AccountProgramEmailNotificationGroup> AccountProgramEmailNotificationGroups { get; set; }

    public virtual DbSet<AccountProgramStatusDm> AccountProgramStatusDms { get; set; }

    public virtual DbSet<AccountProgramStatusHistory> AccountProgramStatusHistories { get; set; }

    public virtual DbSet<AccountProgramUserAuthority> AccountProgramUserAuthorities { get; set; }

    public virtual DbSet<AccountRate> AccountRates { get; set; }

    public virtual DbSet<AccountRateAttachment> AccountRateAttachments { get; set; }

    public virtual DbSet<AccountReference> AccountReferences { get; set; }

    public virtual DbSet<AccountStatusDm> AccountStatusDms { get; set; }

    public virtual DbSet<AccountStatusLog> AccountStatusLogs { get; set; }

    public virtual DbSet<AdditionalObligee> AdditionalObligees { get; set; }

    public virtual DbSet<AdditionalRelatedParty> AdditionalRelatedParties { get; set; }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<AddressTypeDm> AddressTypeDms { get; set; }

    public virtual DbSet<Agency> Agencies { get; set; }

    public virtual DbSet<AgencyCompetition> AgencyCompetitions { get; set; }

    public virtual DbSet<AgencyErrorAndOmission> AgencyErrorAndOmissions { get; set; }

    public virtual DbSet<AgencyInventory> AgencyInventories { get; set; }

    public virtual DbSet<AgencyLicense> AgencyLicenses { get; set; }

    public virtual DbSet<AgencyStatusDm> AgencyStatusDms { get; set; }

    public virtual DbSet<AgencyStatusLog> AgencyStatusLogs { get; set; }

    public virtual DbSet<Agent> Agents { get; set; }

    public virtual DbSet<AgentSystemDm> AgentSystemDms { get; set; }

    public virtual DbSet<AgentsInAgency> AgentsInAgencies { get; set; }

    public virtual DbSet<AgreementTypeDm> AgreementTypeDms { get; set; }

    public virtual DbSet<AppUser> AppUsers { get; set; }

    public virtual DbSet<BalanceSheet> BalanceSheets { get; set; }

    public virtual DbSet<BidPercentDm> BidPercentDms { get; set; }

    public virtual DbSet<BidRequest> BidRequests { get; set; }

    public virtual DbSet<BidRequestCommercial> BidRequestCommercials { get; set; }

    public virtual DbSet<BidResultDm> BidResultDms { get; set; }

    public virtual DbSet<BidRetainageDm> BidRetainageDms { get; set; }

    public virtual DbSet<BidStatusDm> BidStatusDms { get; set; }

    public virtual DbSet<BidSubcontractor> BidSubcontractors { get; set; }

    public virtual DbSet<Bond> Bonds { get; set; }

    public virtual DbSet<BondBlock> BondBlocks { get; set; }

    public virtual DbSet<BondHold> BondHolds { get; set; }

    public virtual DbSet<BondModTransaction> BondModTransactions { get; set; }

    public virtual DbSet<BondStatusLetter> BondStatusLetters { get; set; }

    public virtual DbSet<BondTransaction> BondTransactions { get; set; }

    public virtual DbSet<BondTransactionPurpose> BondTransactionPurposes { get; set; }

    public virtual DbSet<BondTypeDm> BondTypeDms { get; set; }

    public virtual DbSet<BookRatio> BookRatios { get; set; }

    public virtual DbSet<Branch> Branches { get; set; }

    public virtual DbSet<BusinessTypeClassCodeDm> BusinessTypeClassCodeDms { get; set; }

    public virtual DbSet<BusinessTypeDm> BusinessTypeDms { get; set; }

    public virtual DbSet<CashFlowStatement> CashFlowStatements { get; set; }

    public virtual DbSet<CoInsurer> CoInsurers { get; set; }

    public virtual DbSet<CoPrincipal> CoPrincipals { get; set; }

    public virtual DbSet<Collateral> Collaterals { get; set; }

    public virtual DbSet<CollateralTypeDm> CollateralTypeDms { get; set; }

    public virtual DbSet<CommercialBondTypeDm> CommercialBondTypeDms { get; set; }

    public virtual DbSet<CommercialRate> CommercialRates { get; set; }

    public virtual DbSet<CommercialRegionDm> CommercialRegionDms { get; set; }

    public virtual DbSet<Competition> Competitions { get; set; }

    public virtual DbSet<ContractRate> ContractRates { get; set; }

    public virtual DbSet<CountryDm> CountryDms { get; set; }

    public virtual DbSet<CreditReportDm> CreditReportDms { get; set; }

    public virtual DbSet<DefaultGeneralLedgerAccount> DefaultGeneralLedgerAccounts { get; set; }

    public virtual DbSet<DivisionDm> DivisionDms { get; set; }

    public virtual DbSet<DocumentDataMissingAction> DocumentDataMissingActions { get; set; }

    public virtual DbSet<DocumentDefinition> DocumentDefinitions { get; set; }

    public virtual DbSet<DocumentDefinitionRule> DocumentDefinitionRules { get; set; }

    public virtual DbSet<DocumentRule> DocumentRules { get; set; }

    public virtual DbSet<DocumentRuleReplacementMap> DocumentRuleReplacementMaps { get; set; }

    public virtual DbSet<EmailActionDm> EmailActionDms { get; set; }

    public virtual DbSet<EmailHistory> EmailHistories { get; set; }

    public virtual DbSet<FinancialAccountTypeDm> FinancialAccountTypeDms { get; set; }

    public virtual DbSet<FinancialRatio> FinancialRatios { get; set; }

    public virtual DbSet<HomeOfficeEmailTeam> HomeOfficeEmailTeams { get; set; }

    public virtual DbSet<ImagingCategory> ImagingCategories { get; set; }

    public virtual DbSet<ImagingCategoryTabDivision> ImagingCategoryTabDivisions { get; set; }

    public virtual DbSet<ImagingTab> ImagingTabs { get; set; }

    public virtual DbSet<ImagingType> ImagingTypes { get; set; }

    public virtual DbSet<Indemnitor> Indemnitors { get; set; }

    public virtual DbSet<Insurer> Insurers { get; set; }

    public virtual DbSet<InsurerState> InsurerStates { get; set; }

    public virtual DbSet<InventoryDocumentDm> InventoryDocumentDms { get; set; }

    public virtual DbSet<KeyPersonel> KeyPersonels { get; set; }

    public virtual DbSet<LawEntity> LawEntities { get; set; }

    public virtual DbSet<LegalEntity> LegalEntities { get; set; }

    public virtual DbSet<LegalEntityAddress> LegalEntityAddresses { get; set; }

    public virtual DbSet<LegalEntityPhone> LegalEntityPhones { get; set; }

    public virtual DbSet<LegalEntityTypeDm> LegalEntityTypeDms { get; set; }

    public virtual DbSet<LicenseStatusDm> LicenseStatusDms { get; set; }

    public virtual DbSet<LineOauthorityLog> LineOauthorityLogs { get; set; }

    public virtual DbSet<LineOfAuthorityStatusDm> LineOfAuthorityStatusDms { get; set; }

    public virtual DbSet<LineOfBusinessDm> LineOfBusinessDms { get; set; }

    public virtual DbSet<Naicscode> Naicscodes { get; set; }

    public virtual DbSet<NoteTypeDm> NoteTypeDms { get; set; }

    public virtual DbSet<Notebook> Notebooks { get; set; }

    public virtual DbSet<NotebookEntry> NotebookEntries { get; set; }

    public virtual DbSet<NotebookEntryTypeDm> NotebookEntryTypeDms { get; set; }

    public virtual DbSet<Obligee> Obligees { get; set; }

    public virtual DbSet<ObligeeTypeDm> ObligeeTypeDms { get; set; }

    public virtual DbSet<OnlineBondSystem> OnlineBondSystems { get; set; }

    public virtual DbSet<OpenClaim> OpenClaims { get; set; }

    public virtual DbSet<OrganizationTitle> OrganizationTitles { get; set; }

    public virtual DbSet<OrganizationTypeDm> OrganizationTypeDms { get; set; }

    public virtual DbSet<OtherBid> OtherBids { get; set; }

    public virtual DbSet<PermissionRole> PermissionRoles { get; set; }

    public virtual DbSet<PersonalFinancialHeader> PersonalFinancialHeaders { get; set; }

    public virtual DbSet<PersonalFinancialStatement> PersonalFinancialStatements { get; set; }

    public virtual DbSet<PersonalFinancialStatementTypeDm> PersonalFinancialStatementTypeDms { get; set; }

    public virtual DbSet<PersonalFinancialSubaccount> PersonalFinancialSubaccounts { get; set; }

    public virtual DbSet<PhoneNumber> PhoneNumbers { get; set; }

    public virtual DbSet<PhoneTypeDm> PhoneTypeDms { get; set; }

    public virtual DbSet<PowerOfAttorney> PowerOfAttorneys { get; set; }

    public virtual DbSet<PowerOfAttorneyDocumentNameDm> PowerOfAttorneyDocumentNameDms { get; set; }

    public virtual DbSet<PowerOfAttorneyDocumentStatus> PowerOfAttorneyDocumentStatuses { get; set; }

    public virtual DbSet<PowerOfAttorneyStatusDm> PowerOfAttorneyStatusDms { get; set; }

    public virtual DbSet<ProducerInvoiceEmail> ProducerInvoiceEmails { get; set; }

    public virtual DbSet<ProfitCenter> ProfitCenters { get; set; }

    public virtual DbSet<RateGroup> RateGroups { get; set; }

    public virtual DbSet<RateStructureDm> RateStructureDms { get; set; }

    public virtual DbSet<Ratio> Ratios { get; set; }

    public virtual DbSet<ReferenceTypeDm> ReferenceTypeDms { get; set; }

    public virtual DbSet<RegionDm> RegionDms { get; set; }

    public virtual DbSet<RenewalRequest> RenewalRequests { get; set; }

    public virtual DbSet<RenewalRequestSource> RenewalRequestSources { get; set; }

    public virtual DbSet<ResponsibilityDm> ResponsibilityDms { get; set; }

    public virtual DbSet<ResponsibleParty> ResponsibleParties { get; set; }

    public virtual DbSet<ResponsiblePartyTypeDm> ResponsiblePartyTypeDms { get; set; }

    public virtual DbSet<RiskTypeDm> RiskTypeDms { get; set; }

    public virtual DbSet<ScalingDm> ScalingDms { get; set; }

    public virtual DbSet<Sfaa> Sfaas { get; set; }

    public virtual DbSet<SfaabondTypeDm> SfaabondTypeDms { get; set; }

    public virtual DbSet<Sic> Sics { get; set; }

    public virtual DbSet<Sicratio> Sicratios { get; set; }

    public virtual DbSet<SicratioTypeDm> SicratioTypeDms { get; set; }

    public virtual DbSet<State> States { get; set; }

    public virtual DbSet<StatementBasisDm> StatementBasisDms { get; set; }

    public virtual DbSet<StatementQualityDm> StatementQualityDms { get; set; }

    public virtual DbSet<StatementTypeDm> StatementTypeDms { get; set; }

    public virtual DbSet<Subaccount> Subaccounts { get; set; }

    public virtual DbSet<Surcharge> Surcharges { get; set; }

    public virtual DbSet<SurchargeTypeDm> SurchargeTypeDms { get; set; }

    public virtual DbSet<SystemNameDm> SystemNameDms { get; set; }

    public virtual DbSet<TaxBasisDm> TaxBasisDms { get; set; }

    public virtual DbSet<TicketTask> TicketTasks { get; set; }

    public virtual DbSet<UnderwriterRecommendation> UnderwriterRecommendations { get; set; }

    public virtual DbSet<UserLayoutColumn> UserLayoutColumns { get; set; }

    public virtual DbSet<UserLayoutWidget> UserLayoutWidgets { get; set; }

    public virtual DbSet<UserLineOfAuthority> UserLineOfAuthorities { get; set; }

    public virtual DbSet<UserMenu> UserMenus { get; set; }

    public virtual DbSet<UserPreference> UserPreferences { get; set; }

    public virtual DbSet<UserProfile> UserProfiles { get; set; }

    public virtual DbSet<VAccountRateDatum> VAccountRateData { get; set; }

    public virtual DbSet<VAccountRateEmail> VAccountRateEmails { get; set; }

    public virtual DbSet<VAccountRateParent> VAccountRateParents { get; set; }

    public virtual DbSet<VConfiguration> VConfigurations { get; set; }

    public virtual DbSet<VoidedBond> VoidedBonds { get; set; }

    public virtual DbSet<WatchStatusDm> WatchStatusDms { get; set; }

    public virtual DbSet<WorkInProgressJob> WorkInProgressJobs { get; set; }

    public virtual DbSet<WorkInProgressSummary> WorkInProgressSummaries { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(_connectionString);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        AddFunctionsForDefaultValues(modelBuilder);
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountNum);

            entity.ToTable("Account", tb => tb.HasTrigger("trgAccountModified"));

            entity.HasIndex(e => e.AccountNum, "UQ_Account_AccountNum").IsUnique();

            entity.HasIndex(e => e.Id, "UQ_Account_Id").IsUnique();

            entity.Property(e => e.AccountNum)
                .HasMaxLength(8)
                .IsUnicode(false);
                //.HasDefaultValueSql("([dbo].[NewAccountNum]())");
            entity.Property(e => e.AccountingSystem).HasMaxLength(255);
            entity.Property(e => e.AgencyNumber)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.Bank).HasMaxLength(60);
            entity.Property(e => e.BankLastContacted).HasColumnType("date");
            entity.Property(e => e.BankLoc).HasColumnName("BankLOC");
            entity.Property(e => e.BankLocexpires)
                .HasColumnType("date")
                .HasColumnName("BankLOCExpires");
            entity.Property(e => e.BankLochigh).HasColumnName("BankLOCHigh");
            entity.Property(e => e.BankLochighDate)
                .HasColumnType("date")
                .HasColumnName("BankLOCHighDate");
            entity.Property(e => e.BankLocsecurity)
                .HasMaxLength(50)
                .HasColumnName("BankLOCSecurity");
            entity.Property(e => e.BankLocused).HasColumnName("BankLOCUsed");
            entity.Property(e => e.BankReferenceName).HasMaxLength(30);
            entity.Property(e => e.Branch)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.BranchReviewed).HasColumnType("datetime");
            entity.Property(e => e.BusinessType)
                .HasMaxLength(24)
                .IsUnicode(false);
            entity.Property(e => e.BusinessTypeClass)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CpacontactId).HasColumnName("CPAContactId");
            entity.Property(e => e.CpafirmId).HasColumnName("CPAFirmId");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreditReport)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CurrentManagementYear)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.Division)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.DunBradstreetDate).HasColumnType("date");
            entity.Property(e => e.DunBradstreetRate).HasMaxLength(10);
            entity.Property(e => e.DunBradstreetSic)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("DunBradstreetSIC");
            entity.Property(e => e.EstimatingSignoff).HasMaxLength(255);
            entity.Property(e => e.EstimatingSystem).HasMaxLength(255);
            entity.Property(e => e.FiscalYearEnd)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.GeographicSpread).HasMaxLength(255);
            entity.Property(e => e.HomeOfficeReviewed).HasColumnType("datetime");
            entity.Property(e => e.InterimWips).HasColumnName("InterimWIPs");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Naics)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("NAICS");
            entity.Property(e => e.Pocinterims).HasColumnName("POCInterims");
            entity.Property(e => e.PolutionLiabilityCarrier).HasMaxLength(100);
            entity.Property(e => e.PolutionLiabilityExpires).HasColumnType("date");
            entity.Property(e => e.PriorSuretyCompany).HasMaxLength(50);
            entity.Property(e => e.SubcontractProtection).HasMaxLength(20);
            entity.Property(e => e.TaxBasis)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.WatchStatus)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.YearOpened)
                .HasMaxLength(4)
                .IsUnicode(false);

            entity.HasOne(d => d.AgencyNumberNavigation).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.AgencyNumber)
                .HasConstraintName("FK_Account_Agency");

            entity.HasOne(d => d.Agent).WithMany(p => p.AccountAgents)
                .HasForeignKey(d => d.AgentId)
                .HasConstraintName("FK_Account_LegalEntity_Agent");

            entity.HasOne(d => d.Attorney).WithMany(p => p.AccountAttorneys)
                .HasForeignKey(d => d.AttorneyId)
                .HasConstraintName("FK_Account_LawEntity_Attorney");

            entity.HasOne(d => d.BankPhone).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.BankPhoneId)
                .HasConstraintName("FK_Account_PhoneNumber");

            entity.HasOne(d => d.BranchNavigation).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.Branch)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Account_Branch");

            entity.HasOne(d => d.BranchReviewByNavigation).WithMany(p => p.AccountBranchReviewByNavigations).HasForeignKey(d => d.BranchReviewBy);

            entity.HasOne(d => d.BusinessTypeNavigation).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.BusinessType)
                .HasConstraintName("FK_Account_BusinessTypeDM");

            entity.HasOne(d => d.BusinessTypeClassNavigation).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.BusinessTypeClass)
                .HasConstraintName("FK_Account_BusinessTypeClassCodeDM");

            entity.HasOne(d => d.Cpacontact).WithMany(p => p.AccountCpacontacts)
                .HasForeignKey(d => d.CpacontactId)
                .HasConstraintName("FK_Account_LegalEntity_CPAContact");

            entity.HasOne(d => d.Cpafirm).WithMany(p => p.AccountCpafirms)
                .HasForeignKey(d => d.CpafirmId)
                .HasConstraintName("FK_Account_LegalEntity_CPAFirm");

            entity.HasOne(d => d.DivisionNavigation).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.Division)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Account_DivisionDM");

            entity.HasOne(d => d.HomeOfficeReviewByNavigation).WithMany(p => p.AccountHomeOfficeReviewByNavigations).HasForeignKey(d => d.HomeOfficeReviewBy);

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.AccountIdNavigation)
                .HasForeignKey<Account>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Account_LegalEntity");

            entity.HasOne(d => d.LawFirm).WithMany(p => p.AccountLawFirms)
                .HasForeignKey(d => d.LawFirmId)
                .HasConstraintName("FK_Account_LawEntity_LawFirm");

            entity.HasOne(d => d.TaxBasisNavigation).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.TaxBasis)
                .HasConstraintName("FK_Account_TaxBasisDM");

            entity.HasOne(d => d.UnderwriterNavigation).WithMany(p => p.AccountUnderwriterNavigations)
                .HasForeignKey(d => d.Underwriter)
                .HasConstraintName("FK_Account_UserProfile");

            entity.HasOne(d => d.WatchStatusNavigation).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.WatchStatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Account_WatchStatusDM");
        });

        modelBuilder.Entity<AccountClassDm>(entity =>
        {
            entity.HasKey(e => e.AccountClass);

            entity.ToTable("AccountClassDM", tb => tb.HasTrigger("trgAccountClassDMModified"));

            entity.HasIndex(e => e.Id, "UQ_AccountClassDM_Id").IsUnique();

            entity.Property(e => e.AccountClass)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.Active)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.LineOfAuthorityNotificationGroup).HasMaxLength(255);
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<AccountProgram>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered(false);

            entity.ToTable("AccountProgram", tb => tb.HasTrigger("trgAccountProgramModified"));

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.AccountNum)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.ApprovedBy).IsUnicode(false);
            entity.Property(e => e.ApprovedDate).HasColumnType("datetime");
            entity.Property(e => e.Comments).IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).IsUnicode(false);
            entity.Property(e => e.Effective).HasColumnType("datetime");
            entity.Property(e => e.Expiration).HasColumnType("datetime");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.AccountNumNavigation).WithMany(p => p.AccountPrograms)
                .HasForeignKey(d => d.AccountNum)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AccountProgram_Account");
        });

        modelBuilder.Entity<AccountProgramEmailNotificationGroup>(entity =>
        {
            entity.HasKey(e => e.UserName);

            entity.ToTable(tb => tb.HasTrigger("trgAccountProgramEmailNotificationGroupsModified"));

            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.SendTo).HasMaxLength(510);
        });

        modelBuilder.Entity<AccountProgramStatusDm>(entity =>
        {
            entity.HasKey(e => e.Id)
                .HasName("PK_AccountProgramStatus")
                .IsClustered(false);

            entity.ToTable("AccountProgramStatusDM", tb => tb.HasTrigger("trgAccountProgramStatusDMModified"));

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<AccountProgramStatusHistory>(entity =>
        {
            entity.ToTable("AccountProgramStatusHistory", tb => tb.HasTrigger("trgAccountProgramStatusHistoryModified"));

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.StatusDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<AccountProgramUserAuthority>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered(false);

            entity.ToTable("AccountProgramUserAuthority", tb => tb.HasTrigger("trgAccountProgramUserAuthorityModified"));

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(4);
            entity.Property(e => e.Effective).HasColumnType("datetime");
            entity.Property(e => e.Expiration).HasColumnType("datetime");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(4);
            entity.Property(e => e.User).HasMaxLength(4);
        });

        modelBuilder.Entity<AccountRate>(entity =>
        {
            entity.HasKey(e => e.CoNum);

            entity.ToTable("AccountRate", tb => tb.HasTrigger("trgAccountRateModified"));

            entity.HasIndex(e => e.Id, "UQ_AccountRate_Id").IsUnique();

            entity.Property(e => e.CoNum)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<AccountRateAttachment>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered(false);

            entity.ToTable("AccountRateAttachment", tb => tb.HasTrigger("trgAccountRateAttachmentModified"));

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.AccountRate).WithMany(p => p.AccountRateAttachments)
                .HasPrincipalKey(p => p.Id)
                .HasForeignKey(d => d.AccountRateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AccountRateAttachment_AccountRate");
        });

        modelBuilder.Entity<AccountReference>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered(false);

            entity.ToTable("AccountReference", tb => tb.HasTrigger("trgAccountReferenceModified"));

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Type)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.AccountReference)
                .HasForeignKey<AccountReference>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AccountReference_LegalEntity");

            entity.HasOne(d => d.TypeNavigation).WithMany(p => p.AccountReferences)
                .HasForeignKey(d => d.Type)
                .HasConstraintName("FK_AccountReference_ReferenceTypeDM");
        });

        modelBuilder.Entity<AccountStatusDm>(entity =>
        {
            entity.HasKey(e => e.AccountStatus).IsClustered(false);

            entity.ToTable("AccountStatusDM", tb => tb.HasTrigger("trgAccountStatusDMModified"));

            entity.HasIndex(e => e.Id, "UQ_AccountStatusDM_Id").IsUnique();

            entity.Property(e => e.AccountStatus)
                .HasMaxLength(24)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<AccountStatusLog>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered(false);

            entity.ToTable("AccountStatusLog", tb => tb.HasTrigger("trgAccountStatusLogModified"));

            entity.HasIndex(e => new { e.Created, e.AccountNum }, "IX_AccountStatusLog_Created_AccountNum_AccountStatus").IsDescending(true, false);

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.AccountNum)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.AccountStatus)
                .HasMaxLength(24)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(4)
                .IsUnicode(false);

            entity.HasOne(d => d.AccountStatusNavigation).WithMany(p => p.AccountStatusLogs)
                .HasForeignKey(d => d.AccountStatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AccountStatusLog_AccountStatusDM");
        });

        modelBuilder.Entity<AdditionalObligee>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered(false);

            entity.ToTable("AdditionalObligee", tb => tb.HasTrigger("trgAdditionalObligeeModified"));

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ObligeeNum)
                .HasMaxLength(7)
                .IsUnicode(false);
            //.HasDefaultValueSql("([dbo].[NewObligeeNum]())");
        });

        modelBuilder.Entity<AdditionalRelatedParty>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered(false);

            entity.ToTable("AdditionalRelatedParty", tb => tb.HasTrigger("trgAdditionalRelatedPartyModified"));

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.AccountNum)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.AccountNumNavigation).WithMany(p => p.AdditionalRelatedParties)
                .HasForeignKey(d => d.AccountNum)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AdditionalRelatedParty_Account");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.AdditionalRelatedParty)
                .HasForeignKey<AdditionalRelatedParty>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AdditionalRelatedParty_LegalEntity");
        });

        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered(false);

            entity.ToTable("Address", tb => tb.HasTrigger("trgAddressModified"));

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Address1)
                .HasMaxLength(60)
                .HasDefaultValueSql("('')");
            entity.Property(e => e.Address2).HasMaxLength(60);
            entity.Property(e => e.Address3).HasMaxLength(60);
            entity.Property(e => e.City).HasMaxLength(50);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PostalCode)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.StateCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.StateCodeNavigation).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.StateCode)
                .HasConstraintName("FK_Address_State");
        });

        modelBuilder.Entity<AddressTypeDm>(entity =>
        {
            entity.HasKey(e => e.Type).HasName("PK_AddressType");

            entity.ToTable("AddressTypeDM", tb => tb.HasTrigger("trgAddressTypeDMModified"));

            entity.HasIndex(e => e.Id, "UQ_AddressType_Id").IsUnique();

            entity.HasIndex(e => e.Order, "UQ_AddressType_Order").IsUnique();

            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Agency>(entity =>
        {
            entity.HasKey(e => e.AgencyNumber);

            entity.ToTable("Agency", tb => tb.HasTrigger("trgAgencyModified"));

            entity.HasIndex(e => e.AgencyNumber, "UQ_Agency_AgencyNumber").IsUnique();

            entity.HasIndex(e => e.Id, "UQ_Agency_Id").IsUnique();

            entity.Property(e => e.AgencyNumber)
                .HasMaxLength(8)
                .IsUnicode(false);
                //.HasDefaultValueSql("([dbo].[NewAgencyNum]())");
            entity.Property(e => e.Branch)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Comments).HasMaxLength(255);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ErrorsAndOmmissionsCarrier)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ErrorsAndOmmissionsExpiration).HasColumnType("datetime");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nasbp).HasColumnName("NASBP");
            entity.Property(e => e.NationalProducerNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(24)
                .IsUnicode(false);

            entity.HasOne(d => d.BillingContact).WithMany(p => p.AgencyBillingContacts)
                .HasForeignKey(d => d.BillingContactId)
                .HasConstraintName("FK_Agency_LegalEntity_BillingContact");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.AgencyIdNavigation)
                .HasForeignKey<Agency>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Agency_LegalEntity");

            entity.HasOne(d => d.StatusNavigation).WithMany(p => p.Agencies)
                .HasForeignKey(d => d.Status)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Agency_AgencyStatusDM");
        });

        modelBuilder.Entity<AgencyCompetition>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered(false);

            entity.ToTable("AgencyCompetition", tb => tb.HasTrigger("trgAgencyCompetitionModified"));

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.AccountNum)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EnteredBy).HasMaxLength(4);
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.AccountNumNavigation).WithMany(p => p.AgencyCompetitions)
                .HasForeignKey(d => d.AccountNum)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AgencyCompetition_Account");

            entity.HasOne(d => d.Agency).WithMany(p => p.AgencyCompetitions)
                .HasPrincipalKey(p => p.Id)
                .HasForeignKey(d => d.AgencyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AgencyCompetition_Agency");

            entity.HasOne(d => d.EnteredByNavigation).WithMany(p => p.AgencyCompetitions)
                .HasPrincipalKey(p => p.Initials)
                .HasForeignKey(d => d.EnteredBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AgencyCompetition_UserProfile");
        });

        modelBuilder.Entity<AgencyErrorAndOmission>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered(false);

            entity.ToTable("AgencyErrorAndOmission", tb => tb.HasTrigger("trgAgencyErrorAndOmissionModified"));

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Carrier).HasMaxLength(100);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Expiration).HasColumnType("date");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Agency).WithMany(p => p.AgencyErrorAndOmissions)
                .HasPrincipalKey(p => p.Id)
                .HasForeignKey(d => d.AgencyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AgencyErrorAndOmmision_Agency");
        });

        modelBuilder.Entity<AgencyInventory>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered(false);

            entity.ToTable("AgencyInventory");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DocumentType)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Sent).HasColumnType("datetime");

            entity.HasOne(d => d.Addressee).WithMany(p => p.AgencyInventories)
                .HasForeignKey(d => d.AddresseeId)
                .HasConstraintName("FK_AgencyInventory_LegalEntity");

            entity.HasOne(d => d.Agency).WithMany(p => p.AgencyInventories)
                .HasPrincipalKey(p => p.Id)
                .HasForeignKey(d => d.AgencyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AgencyInventory_Agency");

            entity.HasOne(d => d.ApproverNavigation).WithMany(p => p.AgencyInventories)
                .HasForeignKey(d => d.Approver)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AgencyInventory_UserProfile");

            entity.HasOne(d => d.DocumentTypeNavigation).WithMany(p => p.AgencyInventories)
                .HasForeignKey(d => d.DocumentType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AgencyInventory_InventoryDocumentDM");
        });

        modelBuilder.Entity<AgencyLicense>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered(false);

            entity.ToTable("AgencyLicense", tb => tb.HasTrigger("trgAgencyLicenseModified"));

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Appointment).HasColumnType("date");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Expiration).HasColumnType("date");
            entity.Property(e => e.LicenseNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.State)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Status)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Termination).HasColumnType("date");

            entity.HasOne(d => d.Agent).WithMany(p => p.AgencyLicenseAgents).HasForeignKey(d => d.AgentId);

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.AgencyLicenseIdNavigation)
                .HasForeignKey<AgencyLicense>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AgencyLicense_LegalEntity");

            entity.HasOne(d => d.Insurer).WithMany(p => p.AgencyLicenses)
                .HasForeignKey(d => d.InsurerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AgencyLicense_Insurer");

            entity.HasOne(d => d.StateNavigation).WithMany(p => p.AgencyLicenses)
                .HasForeignKey(d => d.State)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AgencyLicense_State");
        });

        modelBuilder.Entity<AgencyStatusDm>(entity =>
        {
            entity.HasKey(e => e.Status).IsClustered(false);

            entity.ToTable("AgencyStatusDM", tb => tb.HasTrigger("trgAgencyStatusDMModified"));

            entity.HasIndex(e => e.Id, "UQ_AgencyStatusDM_Id").IsUnique();

            entity.Property(e => e.Status)
                .HasMaxLength(24)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<AgencyStatusLog>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered(false);

            entity.ToTable("AgencyStatusLog", tb => tb.HasTrigger("trgAgencyStatusLogModified"));

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.AccountNum)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.ChangedBy).HasMaxLength(4);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Effective).HasColumnType("datetime");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NewStatus)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.OldStatus)
                .HasMaxLength(15)
                .IsUnicode(false);

            entity.HasOne(d => d.ChangedByNavigation).WithMany(p => p.AgencyStatusLogs)
                .HasPrincipalKey(p => p.Initials)
                .HasForeignKey(d => d.ChangedBy)
                .HasConstraintName("FK_AgencyStatusLog_UserProfile");
        });

        modelBuilder.Entity<Agent>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered(false);

            entity.ToTable("Agent", tb => tb.HasTrigger("trgAgentModified"));

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DefaultEmail)
                .HasMaxLength(255)
                .HasColumnName("DefaultEMail");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NationalProducerNumber)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.DefaultCellNumberNavigation).WithMany(p => p.AgentDefaultCellNumberNavigations)
                .HasForeignKey(d => d.DefaultCellNumber)
                .HasConstraintName("FK_Agent_PhoneNumber_Cell");

            entity.HasOne(d => d.DefaultPhoneNumberNavigation).WithMany(p => p.AgentDefaultPhoneNumberNavigations)
                .HasForeignKey(d => d.DefaultPhoneNumber)
                .HasConstraintName("FK_Agent_PhoneNumber");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.Agent)
                .HasForeignKey<Agent>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Agent_LegalEntity");
        });

        modelBuilder.Entity<AgentSystemDm>(entity =>
        {
            entity.HasKey(e => e.SystemName).IsClustered(false);

            entity.ToTable("AgentSystemDM");

            entity.HasIndex(e => e.Id, "UQ_AgentSystemDM_Id").IsUnique();

            entity.Property(e => e.SystemName)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<AgentsInAgency>(entity =>
        {
            entity.HasKey(e => new { e.AgentId, e.AgencyId });

            entity.ToTable("AgentsInAgency", tb => tb.HasTrigger("trgAgentsInAgencyModified"));

            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Agency).WithMany(p => p.AgentsInAgencyAgencies)
                .HasForeignKey(d => d.AgencyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AgentsInAgency_AgencyId");

            entity.HasOne(d => d.Agent).WithMany(p => p.AgentsInAgencyAgents)
                .HasForeignKey(d => d.AgentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AgentsInAgency_AgentId");
        });

        modelBuilder.Entity<AgreementTypeDm>(entity =>
        {
            entity.HasKey(e => e.Type).IsClustered(false);

            entity.ToTable("AgreementTypeDM", tb => tb.HasTrigger("trgAgreementTypeDMModified"));

            entity.HasIndex(e => e.Id, "UQ_AgreementTypeDM_Id").IsUnique();

            entity.Property(e => e.Type)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.HasKey(e => e.ActiveDirectoryAccount);

            entity.ToTable("AppUser", tb => tb.HasTrigger("trgAppUserModified"));

            entity.Property(e => e.ActiveDirectoryAccount).HasMaxLength(128);
            entity.Property(e => e.Active)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(128);
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Initials).HasMaxLength(4);
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(128);
        });

        modelBuilder.Entity<BalanceSheet>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered(false);

            entity.ToTable("BalanceSheet", tb => tb.HasTrigger("trgBalanceSheetModified"));

            entity.HasIndex(e => new { e.AccountNum, e.StatementDate, e.AccountType, e.Sequence }, "IX_BalanceSheet_AccountNum_StatementDate_AccountType_Sequence").IsClustered();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.AccountName).HasMaxLength(45);
            entity.Property(e => e.AccountNum)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.AccountType)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.AsAllowed).HasComputedColumnSql("([Stated]+[Adjustment])", false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.StatementDate).HasColumnType("date");

            entity.HasOne(d => d.AccountNumNavigation).WithMany(p => p.BalanceSheets)
                .HasForeignKey(d => d.AccountNum)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BalanceSheet_Account");

            entity.HasOne(d => d.Ratio).WithMany(p => p.BalanceSheets)
                .HasPrincipalKey(p => new { p.AccountNum, p.StatementDate })
                .HasForeignKey(d => new { d.AccountNum, d.StatementDate })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BalanceSheet_Ratio");
        });

        modelBuilder.Entity<BidPercentDm>(entity =>
        {
            entity.HasKey(e => e.BidPercent).IsClustered(false);

            entity.ToTable("BidPercentDM");

            entity.HasIndex(e => e.Id, "UQ_BidPercentDM_Id").IsUnique();

            entity.Property(e => e.BidPercent)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<BidRequest>(entity =>
        {
            entity.HasKey(e => e.BidNumber);

            entity.ToTable("BidRequest", tb => tb.HasTrigger("trgBidRequestModified"));

            entity.HasIndex(e => e.Id, "UQ_BidRequest_Id").IsUnique();

            entity.Property(e => e.BidNumber)
                .HasMaxLength(9)
                .IsUnicode(false);
                //.HasDefaultValueSql("([dbo].[NewBidRequestNumber]())");
            entity.Property(e => e.AccountNum)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.ApprovedBy).HasMaxLength(40);
            entity.Property(e => e.BidDate).HasColumnType("date");
            entity.Property(e => e.BidPercent)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.BidResult)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.BondNumber)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.Ccto)
                .HasMaxLength(4)
                .HasColumnName("CCTo");
            entity.Property(e => e.ContractNumber).HasMaxLength(30);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EstimatedFinish).HasColumnType("date");
            entity.Property(e => e.EstimatedStart).HasColumnType("date");
            entity.Property(e => e.HomeOfficeAction)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.HomeOfficeApproved).HasColumnType("datetime");
            entity.Property(e => e.HomeOfficeApprover)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.HomeOfficeEmailSent).HasColumnType("datetime");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.LineOfCreditExpiration).HasColumnType("date");
            entity.Property(e => e.MaintenanceTerm).HasMaxLength(30);
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PaymentFrequency).HasMaxLength(50);
            entity.Property(e => e.ProjectName).HasMaxLength(255);
            entity.Property(e => e.RequestType)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.Requested).HasColumnType("date");
            entity.Property(e => e.RequestedBy).HasMaxLength(40);
            entity.Property(e => e.Retainage)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Underwriter).HasMaxLength(4);
            entity.Property(e => e.Wipdate)
                .HasColumnType("date")
                .HasColumnName("WIPDate");
            entity.Property(e => e.WipworkOnHand).HasColumnName("WIPWorkOnHand");
            entity.Property(e => e.WithdrawalPenalty).HasMaxLength(25);

            entity.HasOne(d => d.AccountNumNavigation).WithMany(p => p.BidRequests)
                .HasForeignKey(d => d.AccountNum)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BidRequest_Account");

            entity.HasOne(d => d.BidPercentNavigation).WithMany(p => p.BidRequests)
                .HasForeignKey(d => d.BidPercent)
                .HasConstraintName("FK_BidRequest_BidPercentDM");

            entity.HasOne(d => d.BidResultNavigation).WithMany(p => p.BidRequests)
                .HasForeignKey(d => d.BidResult)
                .HasConstraintName("FK_BidRequest_BidResultDM");

            entity.HasOne(d => d.BondNumberNavigation).WithMany(p => p.BidRequests)
                .HasForeignKey(d => d.BondNumber)
                .HasConstraintName("FK_BidRequest_Bond");

            entity.HasOne(d => d.CctoNavigation).WithMany(p => p.BidRequestCctoNavigations)
                .HasPrincipalKey(p => p.Initials)
                .HasForeignKey(d => d.Ccto);

            entity.HasOne(d => d.Obligee).WithMany(p => p.BidRequests)
                .HasForeignKey(d => d.ObligeeId)
                .HasConstraintName("FK_BidRequest_Obligee");

            entity.HasOne(d => d.RetainageNavigation).WithMany(p => p.BidRequests)
                .HasForeignKey(d => d.Retainage)
                .HasConstraintName("FK_BidRequest_BidRetainageDM");

            entity.HasOne(d => d.StatusNavigation).WithMany(p => p.BidRequests)
                .HasForeignKey(d => d.Status)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BidRequest_BidStatusDM");

            entity.HasOne(d => d.UnderwriterNavigation).WithMany(p => p.BidRequestUnderwriterNavigations)
                .HasPrincipalKey(p => p.Initials)
                .HasForeignKey(d => d.Underwriter)
                .HasConstraintName("FK_BidRequest_UserProfile");
        });

        modelBuilder.Entity<BidRequestCommercial>(entity =>
        {
            entity.HasKey(e => e.BidNumber).IsClustered(false);

            entity.ToTable("BidRequestCommercial", tb => tb.HasTrigger("trgBidRequestCommercialModified"));

            entity.HasIndex(e => e.Id, "UQ_BidRequestCommercial_Id").IsUnique();

            entity.Property(e => e.BidNumber)
                .HasMaxLength(9)
                .IsUnicode(false);
                //.HasDefaultValueSql("([dbo].[NewBidRequestNumber]())");
            entity.Property(e => e.AccountNum)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.BondNumber)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.Ccto)
                .HasMaxLength(4)
                .HasColumnName("CCTo");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EstimatedFinish).HasColumnType("date");
            entity.Property(e => e.EstimatedStart).HasColumnType("date");
            entity.Property(e => e.HomeOfficeAction)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.HomeOfficeApproved).HasColumnType("datetime");
            entity.Property(e => e.HomeOfficeApprovedBy).HasMaxLength(4);
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.LineOfCreditExpiration).HasColumnType("date");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Recorded).HasColumnType("date");
            entity.Property(e => e.RecordedBy).HasMaxLength(4);
            entity.Property(e => e.RecordedMessageSent).HasColumnType("datetime");
            entity.Property(e => e.Requested).HasColumnType("date");
            entity.Property(e => e.SfaaclassCode).HasColumnName("SFAAClassCode");
            entity.Property(e => e.Status)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Underwriter).HasMaxLength(4);

            entity.HasOne(d => d.AccountNumNavigation).WithMany(p => p.BidRequestCommercials)
                .HasForeignKey(d => d.AccountNum)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BidRequestCommercial_Account");

            entity.HasOne(d => d.BondNumberNavigation).WithMany(p => p.BidRequestCommercials)
                .HasForeignKey(d => d.BondNumber)
                .HasConstraintName("FK_BidRequestCommercial_Bond");

            entity.HasOne(d => d.CctoNavigation).WithMany(p => p.BidRequestCommercialCctoNavigations)
                .HasPrincipalKey(p => p.Initials)
                .HasForeignKey(d => d.Ccto);

            entity.HasOne(d => d.HomeOfficeApprovedByNavigation).WithMany(p => p.BidRequestCommercialHomeOfficeApprovedByNavigations)
                .HasPrincipalKey(p => p.Initials)
                .HasForeignKey(d => d.HomeOfficeApprovedBy);

            entity.HasOne(d => d.Obligee).WithMany(p => p.BidRequestCommercials)
                .HasForeignKey(d => d.ObligeeId)
                .HasConstraintName("FK_BidRequestCommercial_LegalEntity");

            entity.HasOne(d => d.RecordedByNavigation).WithMany(p => p.BidRequestCommercialRecordedByNavigations)
                .HasPrincipalKey(p => p.Initials)
                .HasForeignKey(d => d.RecordedBy);

            entity.HasOne(d => d.SfaaclassCodeNavigation).WithMany(p => p.BidRequestCommercials)
                .HasForeignKey(d => d.SfaaclassCode)
                .HasConstraintName("FK_BidRequestCommercial_SFAA");

            entity.HasOne(d => d.StatusNavigation).WithMany(p => p.BidRequestCommercials)
                .HasForeignKey(d => d.Status)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BidRequestCommercial_BidStatusDM");

            entity.HasOne(d => d.UnderwriterNavigation).WithMany(p => p.BidRequestCommercialUnderwriterNavigations)
                .HasPrincipalKey(p => p.Initials)
                .HasForeignKey(d => d.Underwriter);
        });

        modelBuilder.Entity<BidResultDm>(entity =>
        {
            entity.HasKey(e => e.Description)
                .HasName("PK_BidResult")
                .IsClustered(false);

            entity.ToTable("BidResultDM", tb => tb.HasTrigger("trgBidResultDMModified"));

            entity.HasIndex(e => e.Id, "UQ_BidResult_Id").IsUnique();

            entity.Property(e => e.Description)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<BidRetainageDm>(entity =>
        {
            entity.HasKey(e => e.Retainage).IsClustered(false);

            entity.ToTable("BidRetainageDM");

            entity.HasIndex(e => e.Id, "UQ_BidRetainageDM_Id").IsUnique();

            entity.Property(e => e.Retainage)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<BidStatusDm>(entity =>
        {
            entity.HasKey(e => e.Status).HasName("PK_BidStatus");

            entity.ToTable("BidStatusDM", tb => tb.HasTrigger("trgBidStatusDMModified"));

            entity.HasIndex(e => e.Id, "UQ_BidStatus_Id").IsUnique();

            entity.Property(e => e.Status)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<BidSubcontractor>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered(false);

            entity.ToTable("BidSubcontractor", tb => tb.HasTrigger("trgBidSubcontractorModified"));

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.BidNumber)
                .HasMaxLength(9)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Trade).HasMaxLength(15);

            entity.HasOne(d => d.BidNumberNavigation).WithMany(p => p.BidSubcontractors)
                .HasForeignKey(d => d.BidNumber)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BidSubcontractor_BidRequest");
        });

        modelBuilder.Entity<Bond>(entity =>
        {
            entity.HasKey(e => e.BondNumber);

            entity.ToTable("Bond", tb => tb.HasTrigger("trgBondModified"));

            entity.HasIndex(e => e.Id, "UQ_Bond_Id").IsUnique();

            entity.Property(e => e.BondNumber)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.AccountNum)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.BondClass).HasMaxLength(40);
            entity.Property(e => e.BondPostalCode)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.CeritifiedMailNumber).HasMaxLength(60);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CurrentBondMod).HasDefaultValueSql("((1))");
            entity.Property(e => e.Effective).HasColumnType("datetime");
            entity.Property(e => e.EstimatedBondLiability).HasComputedColumnSql("([CurrentBondLiability]-[CurrentRunoff])", false);
            entity.Property(e => e.Expiration).HasColumnType("datetime");
            entity.Property(e => e.HomeOfficeAction)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.HomeOfficeApproved).HasColumnType("datetime");
            entity.Property(e => e.HomeOfficeApprovedBy)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.LineOfAuthorityExceptionDescription).HasMaxLength(100);
            entity.Property(e => e.LineOfAuthorityExpiration).HasColumnType("datetime");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Municipality).HasMaxLength(70);
            entity.Property(e => e.NonrenewalLetterMailed).HasColumnType("datetime");
            entity.Property(e => e.RateGroup)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.RenewalProvision)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Risk).HasMaxLength(80);
            entity.Property(e => e.SfaabondType)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("SFAABondType");
            entity.Property(e => e.SfaaclassCode).HasColumnName("SFAAClassCode");
            entity.Property(e => e.ShortRiskDescription).HasMaxLength(100);
            entity.Property(e => e.Siccode)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("SICCode");
            entity.Property(e => e.State)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Status)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.TerminationType).HasMaxLength(20);
            entity.Property(e => e.Underwriter).HasMaxLength(4);

            entity.HasOne(d => d.AccountNumNavigation).WithMany(p => p.Bonds)
                .HasForeignKey(d => d.AccountNum)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bond_Account");

            entity.HasOne(d => d.Agency).WithMany(p => p.BondAgencies)
                .HasForeignKey(d => d.AgencyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bond_LegalEntity_Agency");

            entity.HasOne(d => d.Agent).WithMany(p => p.BondAgents)
                .HasForeignKey(d => d.AgentId)
                .HasConstraintName("FK_Bond_LegalEntity");

            entity.HasOne(d => d.AttorneyInFact).WithMany(p => p.Bonds)
                .HasForeignKey(d => d.AttorneyInFactId)
                .HasConstraintName("FK_Bond_LegalEntity_AttorneyInFact");

            entity.HasOne(d => d.BondType).WithMany(p => p.Bonds)
                .HasPrincipalKey(p => p.Id)
                .HasForeignKey(d => d.BondTypeId)
                .HasConstraintName("FK_Bond_BondTypeDM");

            entity.HasOne(d => d.DirectBillAddress).WithMany(p => p.Bonds)
                .HasForeignKey(d => d.DirectBillAddressId)
                .HasConstraintName("FK_Bond_Address");

            entity.HasOne(d => d.Insurer).WithMany(p => p.Bonds)
                .HasForeignKey(d => d.InsurerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bond_Insurer");

            entity.HasOne(d => d.Obligee).WithMany(p => p.BondObligees)
                .HasForeignKey(d => d.ObligeeId)
                .HasConstraintName("FK_Bond_LegalEntity_Obligee");

            entity.HasOne(d => d.ResponsibleParty).WithMany(p => p.BondResponsibleParties)
                .HasForeignKey(d => d.ResponsiblePartyId)
                .HasConstraintName("FK_Bond_LegalEntity_ResponsibleParty");

            entity.HasOne(d => d.SfaabondTypeNavigation).WithMany(p => p.Bonds)
                .HasForeignKey(d => d.SfaabondType)
                .HasConstraintName("FK_Bond_SFAABondTypeDM");

            entity.HasOne(d => d.SiccodeNavigation).WithMany(p => p.Bonds)
                .HasForeignKey(d => d.Siccode)
                .HasConstraintName("FK_Bond_SIC");

            entity.HasOne(d => d.StateNavigation).WithMany(p => p.Bonds)
                .HasForeignKey(d => d.State)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bond_State");

            entity.HasOne(d => d.UnderwriterNavigation).WithMany(p => p.Bonds)
                .HasPrincipalKey(p => p.Initials)
                .HasForeignKey(d => d.Underwriter)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bond_UserPRofile");
        });

        modelBuilder.Entity<BondBlock>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered(false);

            entity.ToTable("BondBlock", tb => tb.HasTrigger("trgBondBlockModified"));

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.AgencyRestricted)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IssuedBy).HasMaxLength(4);
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Prefix)
                .HasMaxLength(4)
                .IsUnicode(false);

            entity.HasOne(d => d.IssuedByNavigation).WithMany(p => p.BondBlocks)
                .HasPrincipalKey(p => p.Initials)
                .HasForeignKey(d => d.IssuedBy)
                .HasConstraintName("FK_BondBlock_UserProfile");
        });

        modelBuilder.Entity<BondHold>(entity =>
        {
            entity.HasKey(e => e.BondNumber).IsClustered(false);

            entity.ToTable("BondHold", tb => tb.HasTrigger("trgBondHoldModified"));

            entity.HasIndex(e => e.Id, "UQ_BondHold_Id").IsUnique();

            entity.Property(e => e.BondNumber)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.AccountNum)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.AccountingDate).HasColumnType("date");
            entity.Property(e => e.BidNumber)
                .HasMaxLength(9)
                .IsUnicode(false);
            entity.Property(e => e.BillDate).HasColumnType("date");
            entity.Property(e => e.BondPostalCode)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Branch)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Effective).HasColumnType("datetime");
            entity.Property(e => e.Expiration).HasColumnType("datetime");
            entity.Property(e => e.HomeOfficeAction)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.HomeOfficeApproved).HasColumnType("datetime");
            entity.Property(e => e.HomeOfficeApprovedBy).HasMaxLength(4);
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Municipality).HasMaxLength(70);
            entity.Property(e => e.Rate).HasMaxLength(35);
            entity.Property(e => e.RateClass).HasMaxLength(8);
            entity.Property(e => e.RateStructure).HasMaxLength(15);
            entity.Property(e => e.Risk).HasMaxLength(40);
            entity.Property(e => e.SfaaclassCode).HasColumnName("SFAAClassCode");
            entity.Property(e => e.Sfaacode).HasColumnName("SFAACode");
            entity.Property(e => e.Siccode)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("SICCode");
            entity.Property(e => e.State)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.StatusLetterNext).HasColumnType("date");
            entity.Property(e => e.TerminationType).HasMaxLength(20);
            entity.Property(e => e.Underwriter).HasMaxLength(4);

            entity.HasOne(d => d.AccountNumNavigation).WithMany(p => p.BondHolds)
                .HasForeignKey(d => d.AccountNum)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BondHold_Account");

            entity.HasOne(d => d.Agency).WithMany(p => p.BondHoldAgencies)
                .HasForeignKey(d => d.AgencyId)
                .HasConstraintName("FK_BondHold_LegalEntity_Agency");

            entity.HasOne(d => d.Agent).WithMany(p => p.BondHoldAgents)
                .HasForeignKey(d => d.AgentId)
                .HasConstraintName("FK_BondHold_LegalEntity_Agent");

            entity.HasOne(d => d.BondType).WithMany(p => p.BondHolds)
                .HasPrincipalKey(p => p.Id)
                .HasForeignKey(d => d.BondTypeId)
                .HasConstraintName("FK_BondHold_BondTypeDM");

            entity.HasOne(d => d.HomeOfficeApprovedByNavigation).WithMany(p => p.BondHoldHomeOfficeApprovedByNavigations)
                .HasPrincipalKey(p => p.Initials)
                .HasForeignKey(d => d.HomeOfficeApprovedBy);

            entity.HasOne(d => d.Insurer).WithMany(p => p.BondHolds)
                .HasForeignKey(d => d.InsurerId)
                .HasConstraintName("FK_BondHold_Insurer");

            entity.HasOne(d => d.Obligee).WithMany(p => p.BondHoldObligees)
                .HasForeignKey(d => d.ObligeeId)
                .HasConstraintName("FK_BondHold_LegalEntity_Obligee");

            entity.HasOne(d => d.ResponsibleParty).WithMany(p => p.BondHoldResponsibleParties)
                .HasForeignKey(d => d.ResponsiblePartyId)
                .HasConstraintName("FK_BondHold_LegalEntity_ResponsibleParty");

            entity.HasOne(d => d.SfaacodeNavigation).WithMany(p => p.BondHolds)
                .HasForeignKey(d => d.Sfaacode)
                .HasConstraintName("FK_BondHold_SFAA");

            entity.HasOne(d => d.SiccodeNavigation).WithMany(p => p.BondHolds)
                .HasForeignKey(d => d.Siccode)
                .HasConstraintName("FK_BondHold_SIC");

            entity.HasOne(d => d.StateNavigation).WithMany(p => p.BondHolds)
                .HasForeignKey(d => d.State)
                .HasConstraintName("FK_BondHold_State");

            entity.HasOne(d => d.UnderwriterNavigation).WithMany(p => p.BondHoldUnderwriterNavigations)
                .HasPrincipalKey(p => p.Initials)
                .HasForeignKey(d => d.Underwriter)
                .HasConstraintName("FK_BondHold_UserProfile");
        });

        modelBuilder.Entity<BondModTransaction>(entity =>
        {
            entity.HasKey(e => e.BondNumber).IsClustered(false);

            entity.ToTable("BondModTransaction", tb => tb.HasTrigger("trgBondModTransactionModified"));

            entity.HasIndex(e => e.Id, "UQ_BondModTransaction_Id").IsUnique();

            entity.Property(e => e.BondNumber)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Effective).HasColumnType("date");
            entity.Property(e => e.Expiration).HasColumnType("date");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<BondStatusLetter>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered(false);

            entity.ToTable("BondStatusLetter", tb => tb.HasTrigger("trgBondStatusLetterModified"));

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.BondNumber)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ReceivedBack).HasColumnType("date");
            entity.Property(e => e.Sent).HasColumnType("date");

            entity.HasOne(d => d.BondNumberNavigation).WithMany(p => p.BondStatusLetters)
                .HasForeignKey(d => d.BondNumber)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BondStatusLetter_Bond");
        });

        modelBuilder.Entity<BondTransaction>(entity =>
        {
            entity.HasKey(e => new { e.BondNumber, e.GroupNumber });

            entity.ToTable("BondTransaction", tb => tb.HasTrigger("trgBondTransactionModified"));

            entity.HasIndex(e => new { e.BondNumber, e.BondMod, e.GroupNumber }, "IX_BondTransaction_BondNumber_BondMod_GroupNumber");

            entity.HasIndex(e => e.Id, "UQ_BondTransaaction_Id").IsUnique();

            entity.HasIndex(e => new { e.BondNumber, e.BondMod, e.GroupNumber }, "UQ_BondTransaction_BondNumber_BondMod_GroupNumber").IsUnique();

            entity.Property(e => e.BondNumber)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.GroupNumber).HasDefaultValueSql("((1))");
            entity.Property(e => e.AccountClass)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.AccountNum)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.AccountingDate).HasColumnType("date");
            entity.Property(e => e.BillDate).HasColumnType("datetime");
            entity.Property(e => e.BondMod).HasDefaultValueSql("((1))");
            entity.Property(e => e.Branch)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.CoverageDate).HasColumnType("datetime");
            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.Effective).HasColumnType("datetime");
            entity.Property(e => e.Expiration).HasColumnType("datetime");
            entity.Property(e => e.MidtermDescription).HasMaxLength(200);
            entity.Property(e => e.Modified).HasColumnType("datetime");
            entity.Property(e => e.NetDue).HasComputedColumnSql("(((([Premium]-[CommissionAmount])+[Surcharge])+[MunicipalTax])+isnull([AdminFee],(0)))", false);
            entity.Property(e => e.Rate).HasMaxLength(35);
            entity.Property(e => e.RateClass).HasMaxLength(8);
            entity.Property(e => e.RateStructure).HasMaxLength(12);
            entity.Property(e => e.Region)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.TransactionPurpose)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Type).HasMaxLength(20);

            entity.HasOne(d => d.AccountClassNavigation).WithMany(p => p.BondTransactions)
                .HasForeignKey(d => d.AccountClass)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BondTransaction_AccountClassDM");

            entity.HasOne(d => d.AccountNumNavigation).WithMany(p => p.BondTransactions)
                .HasForeignKey(d => d.AccountNum)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BondTransaction_Account");

            entity.HasOne(d => d.Agency).WithMany(p => p.BondTransactions)
                .HasForeignKey(d => d.AgencyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BondTransaction_LegalEntity");

            entity.HasOne(d => d.BondNumberNavigation).WithMany(p => p.BondTransactions)
                .HasForeignKey(d => d.BondNumber)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BondTransaction_Bond");

            entity.HasOne(d => d.RegionNavigation).WithMany(p => p.BondTransactions)
                .HasForeignKey(d => d.Region)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BondTransaction_Region");

            entity.HasOne(d => d.SfaaCodeNavigation).WithMany(p => p.BondTransactions)
                .HasForeignKey(d => d.SfaaCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BondTransaction_SFAA");

            entity.HasOne(d => d.Underwriter).WithMany(p => p.BondTransactions)
                .HasForeignKey(d => d.UnderwriterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BondTransaction_UserProfile");
        });

        modelBuilder.Entity<BondTransactionPurpose>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered(false);

            entity.ToTable("BondTransactionPurpose", tb => tb.HasTrigger("trgBondTransactionPurposeModified"));

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.BondNumber)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NewValue)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.OldValue)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.TransactionPurpose)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.BondNumberNavigation).WithMany(p => p.BondTransactionPurposes)
                .HasForeignKey(d => d.BondNumber)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BondTransactionPurpose_Bond");

            entity.HasOne(d => d.BondTransaction).WithMany(p => p.BondTransactionPurposes)
                .HasPrincipalKey(p => new { p.BondNumber, p.BondMod, p.GroupNumber })
                .HasForeignKey(d => new { d.BondNumber, d.BondMod, d.GroupNumber })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BondTransactionPurpose_BondTransaction");
        });

        modelBuilder.Entity<BondTypeDm>(entity =>
        {
            entity.HasKey(e => new { e.BondType, e.BondClass }).HasName("PK_BondType");

            entity.ToTable("BondTypeDM", tb => tb.HasTrigger("trgBondTypeDMModified"));

            entity.HasIndex(e => e.Id, "UQ_BondType_Id").IsUnique();

            entity.Property(e => e.BondType)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.BondClass)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Active)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.Class)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(40);
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<BookRatio>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered(false);

            entity.ToTable("BookRatio", tb => tb.HasTrigger("trgBookRatioModified"));

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Branch>(entity =>
        {
            entity.HasKey(e => e.BranchKey).IsClustered(false);

            entity.ToTable("Branch", tb => tb.HasTrigger("trgBranchModified"));

            entity.HasIndex(e => e.Id, "UQ_Branch_Id").IsUnique();

            entity.Property(e => e.BranchKey)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Bccreceiver)
                .HasMaxLength(200)
                .HasColumnName("BCCReceiver");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EmailReceiver).HasMaxLength(200);
            entity.Property(e => e.GeneralLedgerCode)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(60);
            entity.Property(e => e.Region)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.BranchOfficeAddress).WithMany(p => p.Branches)
                .HasForeignKey(d => d.BranchOfficeAddressId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Branch_Address");

            entity.HasOne(d => d.Phone).WithMany(p => p.Branches)
                .HasForeignKey(d => d.PhoneId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Branch_PhoneNumber");

            entity.HasOne(d => d.RegionNavigation).WithMany(p => p.Branches)
                .HasForeignKey(d => d.Region)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Branch_RegionDM");
        });

        modelBuilder.Entity<BusinessTypeClassCodeDm>(entity =>
        {
            entity.HasKey(e => e.BusinessType).IsClustered(false);

            entity.ToTable("BusinessTypeClassCodeDM", tb => tb.HasTrigger("trgBusinessTypeClassCodeDMModified"));

            entity.HasIndex(e => e.Id, "UQ_BusinessTypeClassCodeDM_Id").IsUnique();

            entity.Property(e => e.BusinessType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<BusinessTypeDm>(entity =>
        {
            entity.HasKey(e => e.BusinessType).IsClustered(false);

            entity.ToTable("BusinessTypeDM", tb => tb.HasTrigger("trgBusinessTypeDMModified"));

            entity.HasIndex(e => e.Id, "UQ_BusinessTypeDM_Id").IsUnique();

            entity.Property(e => e.BusinessType)
                .HasMaxLength(24)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<CashFlowStatement>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered(false);

            entity.ToTable("CashFlowStatement", tb => tb.HasTrigger("trgCashFlowStatementModified"));

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.AccountNum)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.B2cee).HasColumnName("B2CEE");
            entity.Property(e => e.Basis)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NetChangeInCash).HasComputedColumnSql("(((((((((((isnull([NetIncome],(0))+isnull([DepreciationAmoritization],(0)))+isnull([AccountsReceivable],(0)))+isnull([AccountsReceivableRetention],(0)))+isnull([AllOtherCashFlow],(0)))+isnull([NetFixedAssetsAcquired],(0)))+isnull([AllOtherInvestments],(0)))+isnull([Distributions],(0)))+isnull([TermDebt],(0)))+isnull([LineOfCredit],(0)))+isnull([StockholderNotes],(0)))+isnull([AllOtherFinancing],(0)))", false);
            entity.Property(e => e.NetFixedAssetsAcquiredDebt).HasComputedColumnSql("(isnull([TermDebt],(0))-isnull([NetFixedAssetsAcquired],(0)))", false);
            entity.Property(e => e.Quality)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Scaling)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.StatementDate).HasColumnType("date");
            entity.Property(e => e.TaxBasis)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.TotalCashFinancing).HasComputedColumnSql("((((isnull([Distributions],(0))+isnull([LineOfCredit],(0)))+isnull([TermDebt],(0)))+isnull([StockholderNotes],(0)))+isnull([AllOtherFinancing],(0)))", false);
            entity.Property(e => e.TotalCashFlowsOperations).HasComputedColumnSql("((((isnull([NetIncome],(0))+isnull([DepreciationAmoritization],(0)))+isnull([AccountsReceivable],(0)))+isnull([AccountsReceivableRetention],(0)))+isnull([AllOtherCashFlow],(0)))", false);
            entity.Property(e => e.TotalCashInvestments).HasComputedColumnSql("(isnull([NetFixedAssetsAcquired],(0))+isnull([AllOtherInvestments],(0)))", false);
            entity.Property(e => e.Type)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.AccountNumNavigation).WithMany(p => p.CashFlowStatements)
                .HasForeignKey(d => d.AccountNum)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CashFlowStatement_Account");

            entity.HasOne(d => d.BasisNavigation).WithMany(p => p.CashFlowStatements)
                .HasForeignKey(d => d.Basis)
                .HasConstraintName("FK_CAshFlowStatement_StatementBasisDM");

            entity.HasOne(d => d.QualityNavigation).WithMany(p => p.CashFlowStatements)
                .HasForeignKey(d => d.Quality)
                .HasConstraintName("FK_CashFlowStatement_StatementQualityDM");

            entity.HasOne(d => d.ScalingNavigation).WithMany(p => p.CashFlowStatements)
                .HasForeignKey(d => d.Scaling)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CashFlowStatement_ScalingDM");

            entity.HasOne(d => d.TaxBasisNavigation).WithMany(p => p.CashFlowStatements)
                .HasForeignKey(d => d.TaxBasis)
                .HasConstraintName("FK_CashFlowStatement_TaxBasisDM");

            entity.HasOne(d => d.TypeNavigation).WithMany(p => p.CashFlowStatements)
                .HasForeignKey(d => d.Type)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CashFlowStatement_StatementTypeDM");
        });

        modelBuilder.Entity<CoInsurer>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered(false);

            entity.ToTable("CoInsurer", tb => tb.HasTrigger("trgCoInsurerModified"));

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.BondNumber)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<CoPrincipal>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered(false);

            entity.ToTable("CoPrincipal", tb => tb.HasTrigger("trgCoPrincipalModified"));

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.AccountNum)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.BondNumber)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Profession).HasMaxLength(24);

            entity.HasOne(d => d.AccountNumNavigation).WithMany(p => p.CoPrincipals)
                .HasForeignKey(d => d.AccountNum)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CoPrincipal_Account");

            entity.HasOne(d => d.BondNumberNavigation).WithMany(p => p.CoPrincipals)
                .HasForeignKey(d => d.BondNumber)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CoPrincipal_Bond");
        });

        modelBuilder.Entity<Collateral>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered(false);

            entity.ToTable("Collateral", tb => tb.HasTrigger("trgCollateralModified"));

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.AccountNum)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.BondNumber)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.BondSpecific).HasComputedColumnSql("(CONVERT([bit],case when isnull([BondNumber],'')='' then (0) else (1) end))", false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Expiration).HasColumnType("date");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Type).HasMaxLength(50);

            entity.HasOne(d => d.BondNumberNavigation).WithMany(p => p.Collaterals)
                .HasForeignKey(d => d.BondNumber)
                .HasConstraintName("FK_Collateral_Bond");
        });

        modelBuilder.Entity<CollateralTypeDm>(entity =>
        {
            entity.HasKey(e => e.Type).IsClustered(false);

            entity.ToTable("CollateralTypeDM", tb => tb.HasTrigger("trgCollateralTypeDMModified"));

            entity.HasIndex(e => e.Id, "UQ_CollateralTypeDM_Id").IsUnique();

            entity.Property(e => e.Type).HasMaxLength(50);
            entity.Property(e => e.Active)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<CommercialBondTypeDm>(entity =>
        {
            entity.HasKey(e => e.CommercialBondType).IsClustered(false);

            entity.ToTable("CommercialBondTypeDM", tb => tb.HasTrigger("trgCommercialBondTypeDMModified"));

            entity.HasIndex(e => e.Id, "UQ_CommercialBondTypeDM_Id").IsUnique();

            entity.Property(e => e.CommercialBondType)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<CommercialRate>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered(false);

            entity.ToTable("CommercialRate", tb => tb.HasTrigger("trgCommercialRateModified"));

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CommercialBondType)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.RateGroup)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.RiskType)
                .HasMaxLength(16)
                .IsUnicode(false);

            entity.HasOne(d => d.CommercialBondTypeNavigation).WithMany(p => p.CommercialRates)
                .HasForeignKey(d => d.CommercialBondType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CommercialRate_CommercialBondTypeDM");

            entity.HasOne(d => d.RateGroupNavigation).WithMany(p => p.CommercialRates)
                .HasForeignKey(d => d.RateGroup)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CommercialRate_RateGroup");

            entity.HasOne(d => d.RiskTypeNavigation).WithMany(p => p.CommercialRates)
                .HasForeignKey(d => d.RiskType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CommercialRate_RiskTypeDM");
        });

        modelBuilder.Entity<CommercialRegionDm>(entity =>
        {
            entity.HasKey(e => e.Region).IsClustered(false);

            entity.ToTable("CommercialRegionDM", tb => tb.HasTrigger("trgCommercialRegionDMModified"));

            entity.HasIndex(e => e.Id, "UQ_CommercialRegionDM_Id").IsUnique();

            entity.Property(e => e.Region)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Competition>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered(false);

            entity.ToTable("Competition", tb => tb.HasTrigger("trgCompetitionModified"));

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<ContractRate>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered(false);

            entity.ToTable("ContractRate", tb => tb.HasTrigger("trgContractRateModified"));

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Class)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.RateGroup)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.RateType)
                .HasMaxLength(12)
                .IsUnicode(false);

            entity.HasOne(d => d.RateGroupNavigation).WithMany(p => p.ContractRates)
                .HasForeignKey(d => d.RateGroup)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ContractRate_RateGroup");
        });

        modelBuilder.Entity<CountryDm>(entity =>
        {
            entity.HasKey(e => e.Code).HasName("PK_Country");

            entity.ToTable("CountryDM", tb => tb.HasTrigger("trgCountryDMModified"));

            entity.Property(e => e.Code)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(35);
        });

        modelBuilder.Entity<CreditReportDm>(entity =>
        {
            entity.HasKey(e => e.CreditReport).IsClustered(false);

            entity.ToTable("CreditReportDM", tb => tb.HasTrigger("trgCreditReportDMModified"));

            entity.HasIndex(e => e.Id, "UQ_CreditReportDM_Id").IsUnique();

            entity.Property(e => e.CreditReport)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<DefaultGeneralLedgerAccount>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered(false);

            entity.ToTable("DefaultGeneralLedgerAccount", tb => tb.HasTrigger("trgDefaultGeneralLedgerAccountModified"));

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.AccountClass)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.AccountName)
                .HasMaxLength(45)
                .IsUnicode(false);
            entity.Property(e => e.AccountType)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.AccountClassNavigation).WithMany(p => p.DefaultGeneralLedgerAccounts)
                .HasForeignKey(d => d.AccountClass)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DefaultGeneralLedgerAccount_AccountClassDM");
        });

        modelBuilder.Entity<DivisionDm>(entity =>
        {
            entity.HasKey(e => e.DivisionCode);

            entity.ToTable("DivisionDM", tb => tb.HasTrigger("trgDivisionDMModified"));

            entity.HasIndex(e => e.Id, "UQ_DivisionDM_Id").IsUnique();

            entity.Property(e => e.DivisionCode)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Active)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Division)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.LoanotificationGroup)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("LOANotificationGroup");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<DocumentDataMissingAction>(entity =>
        {
            entity.HasKey(e => e.Action);

            entity.ToTable("DocumentDataMissingAction", tb => tb.HasTrigger("trgDocumentDataMissingActionModified"));

            entity.HasIndex(e => e.Id, "UQ_DocumentDataMissingAction_Id").IsUnique();

            entity.Property(e => e.Action)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<DocumentDefinition>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered(false);

            entity.ToTable("DocumentDefinition", tb => tb.HasTrigger("trgDocumentDefinitionModified"));

            entity.HasIndex(e => e.Name, "UQ_DocumentDefinition_Name").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.IsActiveGiaform).HasColumnName("IsActiveGIAForm");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<DocumentDefinitionRule>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered(false);

            entity.ToTable("DocumentDefinitionRule", tb => tb.HasTrigger("trgDocumentDefinitionRuleModified"));

            entity.HasIndex(e => new { e.DefinitionId, e.RuleId }, "UQ_DocumentDefinitionRule_DefinitionId_RuleId").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Definition).WithMany(p => p.DocumentDefinitionRules)
                .HasForeignKey(d => d.DefinitionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DocumentDefinitionRule_DocumentDefinition");

            entity.HasOne(d => d.Rule).WithMany(p => p.DocumentDefinitionRules)
                .HasForeignKey(d => d.RuleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DocumentDefinitionRule_DocumentRule");
        });

        modelBuilder.Entity<DocumentRule>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered(false);

            entity.ToTable("DocumentRule", tb => tb.HasTrigger("trgDocumentRuleModified"));

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Condition).IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.ForEach)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.OutputDefinition).IsUnicode(false);
        });

        modelBuilder.Entity<DocumentRuleReplacementMap>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered(false);

            entity.ToTable("DocumentRuleReplacementMap", tb => tb.HasTrigger("trgDocumentRuleReplacementMapModified"));

            entity.HasIndex(e => new { e.RuleId, e.Token }, "UQ_DocumentRuleReplacementMap_Token").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Token)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ValuePath).IsUnicode(false);

            entity.HasOne(d => d.IsMissing).WithMany(p => p.DocumentRuleReplacementMaps)
                .HasPrincipalKey(p => p.Id)
                .HasForeignKey(d => d.IsMissingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DocumentRuleReplacementMap_DocumentDataMissingAction");

            entity.HasOne(d => d.Rule).WithMany(p => p.DocumentRuleReplacementMaps)
                .HasForeignKey(d => d.RuleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DocumentRuleReplacementMap_DocumentRule");
        });

        modelBuilder.Entity<EmailActionDm>(entity =>
        {
            entity.HasKey(e => e.Action).IsClustered(false);

            entity.ToTable("EmailActionDM", tb => tb.HasTrigger("trgEmailActionDMModified"));

            entity.HasIndex(e => e.Id, "UQ_EmailActionDM_Id").IsUnique();

            entity.Property(e => e.Action)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<EmailHistory>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered(false);

            entity.ToTable("EmailHistory", tb => tb.HasTrigger("trgEmailHistoryModified"));

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.AccountNum)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.Action)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.BondNumber)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.Cc)
                .HasMaxLength(120)
                .HasColumnName("CC");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.From).HasMaxLength(256);
            entity.Property(e => e.FromName).HasMaxLength(256);
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Sent).HasColumnType("datetime");
            entity.Property(e => e.To).HasMaxLength(256);
            entity.Property(e => e.ToName).HasMaxLength(256);

            entity.HasOne(d => d.AccountNumNavigation).WithMany(p => p.EmailHistories)
                .HasForeignKey(d => d.AccountNum)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmailHistory_Account");

            entity.HasOne(d => d.ActionNavigation).WithMany(p => p.EmailHistories)
                .HasForeignKey(d => d.Action)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmailHistory_EmailActionDM");

            entity.HasOne(d => d.BondNumberNavigation).WithMany(p => p.EmailHistories)
                .HasForeignKey(d => d.BondNumber)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmailHistory_Bond");
        });

        modelBuilder.Entity<FinancialAccountTypeDm>(entity =>
        {
            entity.HasKey(e => e.AccountType).HasName("PK_FinancialAccountType");

            entity.ToTable("FinancialAccountTypeDM", tb => tb.HasTrigger("trgFinancialAccountTypeDMModified"));

            entity.HasIndex(e => e.Id, "UQ_FinancialAccountType_Id").IsUnique();

            entity.Property(e => e.AccountType)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(40);
        });

        modelBuilder.Entity<FinancialRatio>(entity =>
        {
            entity.HasKey(e => new { e.AccountNum, e.StatementDate })
                .HasName("PK_FinancialRatios")
                .IsClustered(false);

            entity.ToTable("FinancialRatio", tb => tb.HasTrigger("trgFinancialRatioModified"));

            entity.HasIndex(e => e.Id, "UQ_FinancialRatios_Id").IsUnique();

            entity.Property(e => e.AccountNum)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.StatementDate).HasColumnType("date");
            entity.Property(e => e.AppayableDays).HasColumnName("APPayableDays");
            entity.Property(e => e.ArapbalanceAllowed).HasColumnName("ARAPBalanceAllowed");
            entity.Property(e => e.ArapbalanceStated).HasColumnName("ARAPBalanceStated");
            entity.Property(e => e.ArcollectionDatsNoReturn).HasColumnName("ARCollectionDatsNoReturn");
            entity.Property(e => e.ArcollectionDays).HasColumnName("ARCollectionDays");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Gaexpenses2SalesAllowed).HasColumnName("GAExpenses2SalesAllowed");
            entity.Property(e => e.Gaexpenses2SalesStated).HasColumnName("GAExpenses2SalesStated");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<HomeOfficeEmailTeam>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered(false);

            entity.ToTable("HomeOfficeEmailTeam", tb => tb.HasTrigger("trgHomeOfficeEmailTeamModified"));

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Team)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
        });

        modelBuilder.Entity<ImagingCategory>(entity =>
        {
            entity.HasKey(e => e.Category).IsClustered(false);

            entity.ToTable("ImagingCategory", tb => tb.HasTrigger("trgImagingCategoryModified"));

            entity.HasIndex(e => e.Id, "UQ_ImagingCategory_Id").IsUnique();

            entity.Property(e => e.Category)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<ImagingCategoryTabDivision>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered(false);

            entity.ToTable("ImagingCategoryTabDivision", tb => tb.HasTrigger("trgImagingCategoryTabDivisionModified"));

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Category)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DivisionCode)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TabName)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.CategoryNavigation).WithMany(p => p.ImagingCategoryTabDivisions)
                .HasForeignKey(d => d.Category)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ImagingCategoryTabDivision_ImagingCategory");

            entity.HasOne(d => d.DivisionCodeNavigation).WithMany(p => p.ImagingCategoryTabDivisions)
                .HasForeignKey(d => d.DivisionCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ImagingCategoryTabDivision_DivisionDM");

            entity.HasOne(d => d.TabNameNavigation).WithMany(p => p.ImagingCategoryTabDivisions)
                .HasForeignKey(d => d.TabName)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ImagingCategoryTabDivision_ImagingTab");
        });

        modelBuilder.Entity<ImagingTab>(entity =>
        {
            entity.HasKey(e => e.TabName).IsClustered(false);

            entity.ToTable("ImagingTab", tb => tb.HasTrigger("trgImagingTabModified"));

            entity.HasIndex(e => e.Id, "UQ_ImagingTab_Id").IsUnique();

            entity.Property(e => e.TabName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<ImagingType>(entity =>
        {
            entity.HasKey(e => e.Type).IsClustered(false);

            entity.ToTable("ImagingType", tb => tb.HasTrigger("trgImagingTypeModified"));

            entity.HasIndex(e => e.Id, "UQ_ImagingType_Id").IsUnique();

            entity.Property(e => e.Type)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Indemnitor>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered(false);

            entity.ToTable("Indemnitor", tb => tb.HasTrigger("trgIndemnitorModified"));

            entity.HasIndex(e => new { e.AccountNum, e.AgreementDate, e.AgreementType, e.Id, e.Signatory, e.Title }, "UQ_Indemnitor_AccountNum_AgreementDate_AgreementType_Id_Signatory_Title").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.AccountNum)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.AgreementDate).HasColumnType("date");
            entity.Property(e => e.AgreementForm).HasMaxLength(15);
            entity.Property(e => e.AgreementType)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EncryptSpouseTaxId)
                .HasMaxLength(22)
                .HasColumnName("Encrypt_SpouseTaxId");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Signatory).HasMaxLength(128);
            entity.Property(e => e.Title).HasMaxLength(128);

            entity.HasOne(d => d.AccountNumNavigation).WithMany(p => p.Indemnitors)
                .HasForeignKey(d => d.AccountNum)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Indemnitor_Account");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.Indemnitor)
                .HasForeignKey<Indemnitor>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Indemnitor_LegalEntity");
        });

        modelBuilder.Entity<Insurer>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered(false);

            entity.ToTable("Insurer", tb => tb.HasTrigger("trgInsurerModified"));

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CurrencyCountry)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasDefaultValueSql("('US')")
                .IsFixedLength();
            entity.Property(e => e.DefaultRateGroup)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.LenumPeopleSoft)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("LENUM_PeopleSoft");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.CurrencyCountryNavigation).WithMany(p => p.Insurers)
                .HasForeignKey(d => d.CurrencyCountry)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Insurer_Country");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.Insurer)
                .HasForeignKey<Insurer>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Insurer_LegalEntity");
        });

        modelBuilder.Entity<InsurerState>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered(false);

            entity.ToTable("InsurerState");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.State)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.Insurer).WithMany(p => p.InsurerStates)
                .HasForeignKey(d => d.InsurerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InsurerState_Insurer");

            entity.HasOne(d => d.StateNavigation).WithMany(p => p.InsurerStates)
                .HasForeignKey(d => d.State)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InsurerState_State");
        });

        modelBuilder.Entity<InventoryDocumentDm>(entity =>
        {
            entity.HasKey(e => e.DocumentType).IsClustered(false);

            entity.ToTable("InventoryDocumentDM");

            entity.HasIndex(e => e.Id, "UQ_InventoryDocumentDM_Id").IsUnique();

            entity.Property(e => e.DocumentType)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<KeyPersonel>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered(false);

            entity.ToTable("KeyPersonel", tb => tb.HasTrigger("trgKeyPersonelModified"));

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.AccountNum)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EncryptYearOfBirth).HasMaxLength(8);
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Position).HasMaxLength(35);
            entity.Property(e => e.Profession).HasMaxLength(20);
            entity.Property(e => e.Responsibility)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.SpouseName).HasMaxLength(30);
            entity.Property(e => e.YearEnteredField)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.YearJoinedCompany)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.AccountNumNavigation).WithMany(p => p.KeyPersonels)
                .HasForeignKey(d => d.AccountNum)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_KeyPersonel_Account");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.KeyPersonel)
                .HasForeignKey<KeyPersonel>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_KeyPersonel_LegalEntity");

            entity.HasOne(d => d.ResponsibilityNavigation).WithMany(p => p.KeyPersonels)
                .HasForeignKey(d => d.Responsibility)
                .HasConstraintName("FK_KeyPersonel_ResponsibilityDM");
        });

        modelBuilder.Entity<LawEntity>(entity =>
        {
            entity.HasKey(e => e.Id)
                .HasName("PK_LawFirm")
                .IsClustered(false);

            entity.ToTable("LawEntity", tb => tb.HasTrigger("trgLawEntityModified"));

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.MartindaleHubbellRating)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.LawEntity)
                .HasForeignKey<LawEntity>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LawFirm_LegalEntity");
        });

        modelBuilder.Entity<LegalEntity>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered(false);

            entity.ToTable("LegalEntity", tb => tb.HasTrigger("trgLegalEntityModified"));

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EmailAddress).HasMaxLength(255);
            entity.Property(e => e.EntityType).HasMaxLength(255);
            entity.Property(e => e.FamilyName).HasMaxLength(50);
            entity.Property(e => e.FullName).HasMaxLength(255);
            entity.Property(e => e.GivenName).HasMaxLength(50);
            entity.Property(e => e.MiddleInitial)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TaxIdEncrypted)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Website).HasMaxLength(255);

            entity.HasOne(d => d.EntityTypeNavigation).WithMany(p => p.LegalEntities)
                .HasForeignKey(d => d.EntityType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LegalEntity_LegalEntityType");

            entity.HasOne(d => d.ParentNavigation).WithMany(p => p.InverseParentNavigation)
                .HasForeignKey(d => d.Parent)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LegalEntity_LegalEntity");
        });

        modelBuilder.Entity<LegalEntityAddress>(entity =>
        {
            entity.HasKey(e => new { e.LegalEntityId, e.AddressId }).IsClustered(false);

            entity.ToTable("LegalEntityAddress");

            entity.HasIndex(e => e.AddressId, "UQ_LegalEntityAddress_AddressId").IsUnique();

            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("('Main')");

            entity.HasOne(d => d.Address).WithOne(p => p.LegalEntityAddress)
                .HasForeignKey<LegalEntityAddress>(d => d.AddressId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LegalEntityAddresse_Address");

            entity.HasOne(d => d.LegalEntity).WithMany(p => p.LegalEntityAddresses)
                .HasForeignKey(d => d.LegalEntityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LegalEntityAddress_LegalEntity");

            entity.HasOne(d => d.TypeNavigation).WithMany(p => p.LegalEntityAddresses)
                .HasForeignKey(d => d.Type)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LegalEntityAddress_AddressType");
        });

        modelBuilder.Entity<LegalEntityPhone>(entity =>
        {
            entity.HasKey(e => new { e.LegalEntityId, e.PhoneNumberId }).IsClustered(false);

            entity.ToTable("LegalEntityPhone");

            entity.HasIndex(e => e.PhoneNumberId, "UQ_LegalEntityPhone_PhoneNumberId").IsUnique();

            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("('Main')");

            entity.HasOne(d => d.LegalEntity).WithMany(p => p.LegalEntityPhones)
                .HasForeignKey(d => d.LegalEntityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LegalEntityPhone_LegalEntity");

            entity.HasOne(d => d.PhoneNumber).WithOne(p => p.LegalEntityPhone)
                .HasForeignKey<LegalEntityPhone>(d => d.PhoneNumberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LegalEntityPhone_PhoneNumber");

            entity.HasOne(d => d.TypeNavigation).WithMany(p => p.LegalEntityPhones)
                .HasForeignKey(d => d.Type)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LegalEntityPhone_PhoneType");
        });

        modelBuilder.Entity<LegalEntityTypeDm>(entity =>
        {
            entity.HasKey(e => e.EntityType).HasName("PK_LegalEntityType");

            entity.ToTable("LegalEntityTypeDM", tb => tb.HasTrigger("trgLegalEntityTypeDMModified"));

            entity.Property(e => e.EntityType).HasMaxLength(255);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<LicenseStatusDm>(entity =>
        {
            entity.HasKey(e => e.Status).IsClustered(false);

            entity.ToTable("LicenseStatusDM", tb => tb.HasTrigger("trgLicenseStatusDMModified"));

            entity.HasIndex(e => e.Id, "UQ_LicenseStatusDM_Id").IsUnique();

            entity.Property(e => e.Status)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<LineOauthorityLog>(entity =>
        {
            entity.HasKey(e => e.AccountNum)
                .HasName("PK_LineOFAuthorityLog")
                .IsClustered(false);

            entity.ToTable("LineOAuthorityLog", tb => tb.HasTrigger("trgLineOAuthorityLogModified"));

            entity.HasIndex(e => e.Id, "UQ_LineOFAuthorityLog_Id").IsUnique();

            entity.Property(e => e.AccountNum)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.Approved).HasColumnType("datetime");
            entity.Property(e => e.ApprovedBy).HasMaxLength(4);
            entity.Property(e => e.BondType)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(4);
            entity.Property(e => e.Division)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Effective).HasColumnType("datetime");
            entity.Property(e => e.Expiration).HasColumnType("datetime");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Loaaggregate).HasColumnName("LOAAggregate");
            entity.Property(e => e.Loasingle).HasColumnName("LOASingle");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.SequenceNumber).ValueGeneratedOnAdd();
            entity.Property(e => e.Status)
                .HasMaxLength(12)
                .IsUnicode(false);

            entity.HasOne(d => d.ApprovedByNavigation).WithMany(p => p.LineOauthorityLogApprovedByNavigations)
                .HasPrincipalKey(p => p.Initials)
                .HasForeignKey(d => d.ApprovedBy)
                .HasConstraintName("FK_LineOfAuthorityLog_UserProfile");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.LineOauthorityLogCreatedByNavigations)
                .HasPrincipalKey(p => p.Initials)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK_LineOfAuthorityLog_UserProfile_CreatedBy");

            entity.HasOne(d => d.DivisionNavigation).WithMany(p => p.LineOauthorityLogs)
                .HasForeignKey(d => d.Division)
                .HasConstraintName("FK_LineOfAuthorityLog_DivisionDM");

            entity.HasOne(d => d.StatusNavigation).WithMany(p => p.LineOauthorityLogs)
                .HasForeignKey(d => d.Status)
                .HasConstraintName("FK_LineOfAuthorityLog_LineOfAuthorityStatusDM");
        });

        modelBuilder.Entity<LineOfAuthorityStatusDm>(entity =>
        {
            entity.HasKey(e => e.Status).IsClustered(false);

            entity.ToTable("LineOfAuthorityStatusDM", tb => tb.HasTrigger("trgLineOfAuthorityStatusDMModified"));

            entity.HasIndex(e => e.Id, "UQ_LineOfAuthorityStatusDM_Id").IsUnique();

            entity.Property(e => e.Status)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<LineOfBusinessDm>(entity =>
        {
            entity.HasKey(e => e.LineOfBusiness).IsClustered(false);

            entity.ToTable("LineOfBusinessDM", tb => tb.HasTrigger("trgLineOfBusinessDMModified"));

            entity.HasIndex(e => e.Id, "UQ_LineOfBusinessDM_Id").IsUnique();

            entity.Property(e => e.LineOfBusiness)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Naicscode>(entity =>
        {
            entity.HasKey(e => e.Code).IsClustered(false);

            entity.ToTable("NAICSCode", tb => tb.HasTrigger("trgNAICSCodeModified"));

            entity.HasIndex(e => e.Id, "UQ_NAICSCode_Id").IsUnique();

            entity.Property(e => e.Code)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Sector)
                .HasMaxLength(80)
                .IsUnicode(false);
        });

        modelBuilder.Entity<NoteTypeDm>(entity =>
        {
            entity.HasKey(e => e.NoteType).IsClustered(false);

            entity.ToTable("NoteTypeDM");

            entity.HasIndex(e => e.Id, "UQ_NoteTypeDM_Id").IsUnique();

            entity.Property(e => e.NoteType)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Notebook>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered(false);

            entity.ToTable("Notebook", "Beta", tb => tb.HasTrigger("trgNotebookModified"));

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.SourceTable).HasMaxLength(128);
        });

        modelBuilder.Entity<NotebookEntry>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered(false);

            entity.ToTable("NotebookEntry", "Beta", tb => tb.HasTrigger("trgNotebookEntryModified"));

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Category).HasMaxLength(18);
            entity.Property(e => e.Completion).HasColumnType("datetime");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(4);
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ReminderTime).HasColumnType("datetime");
            entity.Property(e => e.Type)
                .HasMaxLength(30)
                .IsUnicode(false);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.NotebookEntries)
                .HasPrincipalKey(p => p.Initials)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NotebookEntry_UserProfile");

            entity.HasOne(d => d.Notebook).WithMany(p => p.NotebookEntries)
                .HasForeignKey(d => d.NotebookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NotebookEntry_Notebook");

            entity.HasOne(d => d.TypeNavigation).WithMany(p => p.NotebookEntries)
                .HasForeignKey(d => d.Type)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NotebookEntry_NotebookEntryTypeDM");
        });

        modelBuilder.Entity<NotebookEntryTypeDm>(entity =>
        {
            entity.HasKey(e => e.Type).IsClustered(false);

            entity.ToTable("NotebookEntryTypeDM", "Beta", tb => tb.HasTrigger("trgNotebookEntryTypeDMModified"));

            entity.HasIndex(e => e.Id, "UQ_NotebookEntryTypeDM_Id").IsUnique();

            entity.Property(e => e.Type)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Obligee>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered(false);

            entity.ToTable("Obligee", tb => tb.HasTrigger("trgObligeeModified"));

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EditedBy).HasMaxLength(4);
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ObligeeNum)
                .HasMaxLength(7)
                .IsUnicode(false);
                //.HasDefaultValueSql("([dbo].[NewObligeeNum]())");
            entity.Property(e => e.Type)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.Obligee)
                .HasForeignKey<Obligee>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Obligee_LegalEntity");

            entity.HasOne(d => d.TypeNavigation).WithMany(p => p.Obligees)
                .HasForeignKey(d => d.Type)
                .HasConstraintName("FK_Obligee_ObligeeTypeDM");
        });

        modelBuilder.Entity<ObligeeTypeDm>(entity =>
        {
            entity.HasKey(e => e.Type).IsClustered(false);

            entity.ToTable("ObligeeTypeDM", tb => tb.HasTrigger("trgObligeeTypeDMModified"));

            entity.HasIndex(e => e.Id, "UQ_ObligeeTypeDM_Id").IsUnique();

            entity.Property(e => e.Type)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<OnlineBondSystem>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered(false);

            entity.ToTable("OnlineBondSystem", tb => tb.HasTrigger("trgOnlineBondSystemModified"));

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.SystemName)
                .HasMaxLength(30)
                .IsUnicode(false);

            entity.HasOne(d => d.Agency).WithMany(p => p.OnlineBondSystems)
                .HasPrincipalKey(p => p.Id)
                .HasForeignKey(d => d.AgencyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OnlineBondSystem_Agency");

            entity.HasOne(d => d.Insurer).WithMany(p => p.OnlineBondSystems)
                .HasForeignKey(d => d.InsurerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OnlineBondSystem_Insurer");

            entity.HasOne(d => d.PowerOfAttorney).WithMany(p => p.OnlineBondSystems)
                .HasForeignKey(d => d.PowerOfAttorneyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OnlineBondSystem_PowerOfAttorney");

            entity.HasOne(d => d.SystemNameNavigation).WithMany(p => p.OnlineBondSystems)
                .HasForeignKey(d => d.SystemName)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OnlineBondSystem_AgentSystemDM");
        });

        modelBuilder.Entity<OpenClaim>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered(false);

            entity.ToTable("OpenClaim", tb => tb.HasTrigger("trgOpenClaimModified"));

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.AdjusterName).HasMaxLength(250);
            entity.Property(e => e.BondNumber)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.ClaimNumber)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Reserve).HasColumnType("decimal(21, 2)");

            entity.HasOne(d => d.BondNumberNavigation).WithMany(p => p.OpenClaims)
                .HasForeignKey(d => d.BondNumber)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OpenClaim_Bond");
        });

        modelBuilder.Entity<OrganizationTitle>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered(false);

            entity.ToTable("OrganizationTitle", tb => tb.HasTrigger("trgOrganizationTitleModified"));

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Type)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.TypeNavigation).WithMany(p => p.OrganizationTitles)
                .HasForeignKey(d => d.Type)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrganizationTitle_OrganizationTypeDM");
        });

        modelBuilder.Entity<OrganizationTypeDm>(entity =>
        {
            entity.HasKey(e => e.Type).IsClustered(false);

            entity.ToTable("OrganizationTypeDM", tb => tb.HasTrigger("trgOrganizationTypeDMModified"));

            entity.HasIndex(e => e.Id, "UQ_OrganizationTypeDM_Id").IsUnique();

            entity.Property(e => e.Type)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<OtherBid>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered(false);

            entity.ToTable("OtherBid", tb => tb.HasTrigger("trgOtherBidModified"));

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.BidNumber)
                .HasMaxLength(9)
                .IsUnicode(false);
            entity.Property(e => e.Bidder).HasMaxLength(50);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.BidNumberNavigation).WithMany(p => p.OtherBids)
                .HasForeignKey(d => d.BidNumber)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OtherBid_BidRequest");
        });

        modelBuilder.Entity<PermissionRole>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered(false);

            entity.ToTable("PermissionRole", tb => tb.HasTrigger("trgPermissionRoleModified"));

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Role)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PersonalFinancialHeader>(entity =>
        {
            entity.HasKey(e => new { e.AccountNum, e.StatementDate }).IsClustered(false);

            entity.ToTable("PersonalFinancialHeader", tb => tb.HasTrigger("trgPersonalFinancialHeaderModified"));

            entity.HasIndex(e => e.Id, "UQ_PersonalFinancialHeader_Id").IsUnique();

            entity.Property(e => e.AccountNum)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.StatementDate).HasColumnType("date");
            entity.Property(e => e.Basis)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Quality)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Scaling)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.StatementFor).HasMaxLength(35);
            entity.Property(e => e.TaxBasis)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Type)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.BasisNavigation).WithMany(p => p.PersonalFinancialHeaders)
                .HasForeignKey(d => d.Basis)
                .HasConstraintName("FK_PersonalFinancialHeader_StatementBasisDM");

            entity.HasOne(d => d.Person).WithMany(p => p.PersonalFinancialHeaders)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PersonalFinancialHeader_LegalEntity");

            entity.HasOne(d => d.QualityNavigation).WithMany(p => p.PersonalFinancialHeaders)
                .HasForeignKey(d => d.Quality)
                .HasConstraintName("FK_PersonalFinancialHeader_StatementQualityDM");

            entity.HasOne(d => d.ScalingNavigation).WithMany(p => p.PersonalFinancialHeaders)
                .HasForeignKey(d => d.Scaling)
                .HasConstraintName("FK_PersonalFinancialHeader_ScalingDM");

            entity.HasOne(d => d.TaxBasisNavigation).WithMany(p => p.PersonalFinancialHeaders)
                .HasForeignKey(d => d.TaxBasis)
                .HasConstraintName("FK_PersonalFinancialHeader_TaxBasisDM");

            entity.HasOne(d => d.TypeNavigation).WithMany(p => p.PersonalFinancialHeaders)
                .HasForeignKey(d => d.Type)
                .HasConstraintName("FK_PersonalFinancialHeader_PersonalFinancialStatementTypeDM");
        });

        modelBuilder.Entity<PersonalFinancialStatement>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered(false);

            entity.ToTable("PersonalFinancialStatement", tb => tb.HasTrigger("trgPersonalFinancialStatementModified"));

            entity.HasIndex(e => new { e.AccountNum, e.StatementDate, e.AccountType, e.Sequence }, "IX_PersonalFinancialStatement_AccountNum_StatementDate_AccountType_Sequence").IsClustered();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.AccountName).HasMaxLength(45);
            entity.Property(e => e.AccountNum)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.AccountType)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.AsAllowed).HasComputedColumnSql("([Stated]+[Adjustment])", false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.StatementDate).HasColumnType("date");

            entity.HasOne(d => d.AccountNumNavigation).WithMany(p => p.PersonalFinancialStatements)
                .HasForeignKey(d => d.AccountNum)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PersonalFinancialStatement_Account");

            entity.HasOne(d => d.Header).WithMany(p => p.PersonalFinancialStatements)
                .HasPrincipalKey(p => p.Id)
                .HasForeignKey(d => d.HeaderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PersonalFinancialStatement_PersonalFinancialHeader");
        });

        modelBuilder.Entity<PersonalFinancialStatementTypeDm>(entity =>
        {
            entity.HasKey(e => e.Type).IsClustered(false);

            entity.ToTable("PersonalFinancialStatementTypeDM", tb => tb.HasTrigger("trgPersonalFinancialStatementTypeDMModified"));

            entity.HasIndex(e => e.Id, "UQ_PersonalFinancialStatementTypeDM_Id").IsUnique();

            entity.Property(e => e.Type)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<PersonalFinancialSubaccount>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered(false);

            entity.ToTable("PersonalFinancialSubaccount", tb => tb.HasTrigger("trgPersonalFinancialSubaccountModified"));

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.AsAllowed).HasComputedColumnSql("([Stated]+[Adjustment])", false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.ParentAccount).WithMany(p => p.PersonalFinancialSubaccounts)
                .HasForeignKey(d => d.ParentAccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PersonalFinancialSubaccount_PersonalFinancialStatement");
        });

        modelBuilder.Entity<PhoneNumber>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered(false);

            entity.ToTable("PhoneNumber", tb => tb.HasTrigger("trgPhoneNumberModified"));

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CountryCode)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasDefaultValueSql("('1')");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Extension)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.MainNumber)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<PhoneTypeDm>(entity =>
        {
            entity.HasKey(e => e.Type).HasName("PK_PhoneType");

            entity.ToTable("PhoneTypeDM", tb => tb.HasTrigger("trgPhoneTypeDMModified"));

            entity.HasIndex(e => e.Id, "UQ_PhoneType_Id").IsUnique();

            entity.HasIndex(e => e.Order, "UQ_PhoneType_Order").IsUnique();

            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<PowerOfAttorney>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered(false);

            entity.ToTable("PowerOfAttorney", tb => tb.HasTrigger("trgPowerOfAttorneyModified"));

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CurrentIssued).HasColumnType("date");
            entity.Property(e => e.FirstIssued).HasColumnType("date");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(15)
                .IsUnicode(false);

            entity.HasOne(d => d.Insurer).WithMany(p => p.PowerOfAttorneys)
                .HasForeignKey(d => d.InsurerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PowerOfAttorney_Insurer");

            entity.HasOne(d => d.StatusNavigation).WithMany(p => p.PowerOfAttorneys)
                .HasForeignKey(d => d.Status)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PowerOfAttorney_PowerOfAttorneyStatusDM");
        });

        modelBuilder.Entity<PowerOfAttorneyDocumentNameDm>(entity =>
        {
            entity.HasKey(e => e.Name)
                .HasName("PK_PowerOfAttorneyDocumentTypeDM")
                .IsClustered(false);

            entity.ToTable("PowerOfAttorneyDocumentNameDM", tb => tb.HasTrigger("trgPowerOfAttorneyDocumentNameDMModified"));

            entity.HasIndex(e => e.Id, "UQ_PowerOfAttorneyDocumentTypeDM_Id").IsUnique();

            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<PowerOfAttorneyDocumentStatus>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered(false);

            entity.ToTable("PowerOfAttorneyDocumentStatus", tb => tb.HasTrigger("trgPowerOfAttorneyDocumentStatusModified"));

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Poaid).HasColumnName("POAId");
            entity.Property(e => e.Received).HasColumnType("date");
            entity.Property(e => e.Requested).HasColumnType("date");

            entity.HasOne(d => d.NameNavigation).WithMany(p => p.PowerOfAttorneyDocumentStatuses)
                .HasForeignKey(d => d.Name)
                .HasConstraintName("FK_PowerOfAttorneyDocumentStatus_PowerOfAttorneyDocumentNameDM");

            entity.HasOne(d => d.Poa).WithMany(p => p.PowerOfAttorneyDocumentStatuses)
                .HasForeignKey(d => d.Poaid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PowerOfAttorneyDocumentStatus_PowerOfAttorney");
        });

        modelBuilder.Entity<PowerOfAttorneyStatusDm>(entity =>
        {
            entity.HasKey(e => e.Status)
                .HasName("PK_PowerOfAttourneyStatusDM")
                .IsClustered(false);

            entity.ToTable("PowerOfAttorneyStatusDM", tb => tb.HasTrigger("trgPowerOfAttorneyStatusDMModified"));

            entity.HasIndex(e => e.Id, "UQ_PowerOfAttourneyStatusDM_Id").IsUnique();

            entity.Property(e => e.Status)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<ProducerInvoiceEmail>(entity =>
        {
            entity.HasKey(e => e.Pnumber);

            entity.ToTable("ProducerInvoiceEmail", tb => tb.HasTrigger("trgProducerInvoiceEmailModified"));

            entity.Property(e => e.Pnumber)
                .ValueGeneratedNever()
                .HasColumnName("PNumber");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<ProfitCenter>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered(false);

            entity.ToTable("ProfitCenter", tb => tb.HasTrigger("trgProfitCenterModified"));

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CommercialRegion)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DivisionCode)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Effective).HasColumnType("date");
            entity.Property(e => e.Expiration).HasColumnType("date");
            entity.Property(e => e.LineOfBusiness)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Location)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.LocationOverview)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ProfitCenter1)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("ProfitCenter");
            entity.Property(e => e.Underwriter).HasMaxLength(4);

            entity.HasOne(d => d.BudgetDefaultNavigation).WithMany(p => p.InverseBudgetDefaultNavigation)
                .HasForeignKey(d => d.BudgetDefault)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProfitCenter_ProfitCenter");

            entity.HasOne(d => d.CommercialRegionNavigation).WithMany(p => p.ProfitCenters)
                .HasForeignKey(d => d.CommercialRegion)
                .HasConstraintName("FK_ProfitCenter_CommercialRegionDM");

            entity.HasOne(d => d.DivisionCodeNavigation).WithMany(p => p.ProfitCenters)
                .HasForeignKey(d => d.DivisionCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProfitCenter_DivisionDM");

            entity.HasOne(d => d.LineOfBusinessNavigation).WithMany(p => p.ProfitCenters)
                .HasForeignKey(d => d.LineOfBusiness)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProfitCenter_LineOfBusinessDM");

            entity.HasOne(d => d.UnderwriterNavigation).WithMany(p => p.ProfitCenters)
                .HasPrincipalKey(p => p.Initials)
                .HasForeignKey(d => d.Underwriter)
                .HasConstraintName("FK_ProfitCenter_UserProfile");
        });

        modelBuilder.Entity<RateGroup>(entity =>
        {
            entity.HasKey(e => e.RateGroup1).IsClustered(false);

            entity.ToTable("RateGroup", tb => tb.HasTrigger("trgRateGroupModified"));

            entity.HasIndex(e => e.Id, "UQ_RateGroup_Id").IsUnique();

            entity.Property(e => e.RateGroup1)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("RateGroup");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(40);
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<RateStructureDm>(entity =>
        {
            entity.HasKey(e => e.RateStructure).IsClustered(false);

            entity.ToTable("RateStructureDM", tb => tb.HasTrigger("trgRateStructureDMModified"));

            entity.HasIndex(e => e.Id, "UQ_RateStructureDM_Id").IsUnique();

            entity.Property(e => e.RateStructure)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Ratio>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered(false);

            entity.ToTable("Ratio", tb => tb.HasTrigger("trgRatioModified"));

            entity.HasIndex(e => new { e.AccountNum, e.StatementDate }, "IX_Ratio_AccountNum_StatementDate");

            entity.HasIndex(e => new { e.AccountNum, e.StatementDate }, "UQ_Ratio_AccountNum_StatementDate").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.AccountNum)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.AggregateLoa).HasColumnName("AggregateLOA");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.GaexpensesAllowed).HasColumnName("GAExpensesAllowed");
            entity.Property(e => e.GaexpensesStated).HasColumnName("GAExpensesStated");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.QcratioAllowed).HasColumnName("QCRatioAllowed");
            entity.Property(e => e.QcratioStated).HasColumnName("QCRatioStated");
            entity.Property(e => e.QleverageAllowed).HasColumnName("QLeverageAllowed");
            entity.Property(e => e.QleverageStated).HasColumnName("QLeverageStated");
            entity.Property(e => e.QnetworthAllowed).HasColumnName("QNetworthAllowed");
            entity.Property(e => e.QnetworthStated).HasColumnName("QNetworthStated");
            entity.Property(e => e.QprofitAllowed).HasColumnName("QProfitAllowed");
            entity.Property(e => e.QprofitStated).HasColumnName("QProfitStated");
            entity.Property(e => e.QscoreAllowed).HasColumnName("QScoreAllowed");
            entity.Property(e => e.QscoreStated).HasColumnName("QScoreStated");
            entity.Property(e => e.QworkingCapitalAllowed).HasColumnName("QWorkingCapitalAllowed");
            entity.Property(e => e.QworkingCapitalStated).HasColumnName("QWorkingCapitalStated");
            entity.Property(e => e.Scaling).HasMaxLength(8);
            entity.Property(e => e.SingleLoa).HasColumnName("SingleLOA");
            entity.Property(e => e.StatementBasis).HasMaxLength(20);
            entity.Property(e => e.StatementDate).HasColumnType("date");
            entity.Property(e => e.StatementQuality).HasMaxLength(20);
            entity.Property(e => e.StatementTaxBasis).HasMaxLength(20);
            entity.Property(e => e.StatementType).HasMaxLength(30);

            entity.HasOne(d => d.AccountNumNavigation).WithMany(p => p.Ratios)
                .HasForeignKey(d => d.AccountNum)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ratio_Account");
        });

        modelBuilder.Entity<ReferenceTypeDm>(entity =>
        {
            entity.HasKey(e => e.Type).IsClustered(false);

            entity.ToTable("ReferenceTypeDM", tb => tb.HasTrigger("trgReferenceTypeDMModified"));

            entity.HasIndex(e => e.Id, "UQ_ReferenceTypeDM_Id").IsUnique();

            entity.Property(e => e.Type)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<RegionDm>(entity =>
        {
            entity.HasKey(e => e.Region).HasName("PK_Region");

            entity.ToTable("RegionDM", tb =>
                {
                    tb.HasTrigger("trgRegionDMModified");
                    tb.HasTrigger("trgRegionModified");
                });

            entity.Property(e => e.Region)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<RenewalRequest>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered(false);

            entity.ToTable("RenewalRequest", tb => tb.HasTrigger("trgRenewalRequestModified"));

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.AccountName).HasMaxLength(150);
            entity.Property(e => e.BasisBondNumber)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.BondChangeReportEmailed).HasColumnType("datetime");
            entity.Property(e => e.BondChangeReportToEmail).HasMaxLength(1048);
            entity.Property(e => e.BondChangeReportUploaded).HasColumnType("datetime");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DataUsed).HasColumnType("xml");
            entity.Property(e => e.EmailSent).HasColumnType("datetime");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.RenewalCompleted).HasColumnType("datetime");
            entity.Property(e => e.RenewalRequestSourceId).HasColumnName("RenewalRequestSourceID");
            entity.Property(e => e.RenewalStarted).HasColumnType("datetime");
            entity.Property(e => e.RequestedBy).HasMaxLength(128);
            entity.Property(e => e.SendEmailTo).HasMaxLength(256);
            entity.Property(e => e.UnderWriterNotified).HasColumnType("datetime");
        });

        modelBuilder.Entity<RenewalRequestSource>(entity =>
        {
            entity.HasKey(e => e.Source);

            entity.ToTable("RenewalRequestSource", tb => tb.HasTrigger("trgRenewalRequestSourceModified"));

            entity.Property(e => e.Source).HasMaxLength(128);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<ResponsibilityDm>(entity =>
        {
            entity.HasKey(e => e.Responsibility).IsClustered(false);

            entity.ToTable("ResponsibilityDM");

            entity.HasIndex(e => e.Id, "UQ_ResponsibilityDM_Id").IsUnique();

            entity.Property(e => e.Responsibility)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<ResponsibleParty>(entity =>
        {
            entity.HasKey(e => e.Id)
                .HasName("PK_DesignatedParty")
                .IsClustered(false);

            entity.ToTable("ResponsibleParty", tb => tb.HasTrigger("trgResponsiblePartyModified"));

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Type)
                .HasMaxLength(30)
                .IsUnicode(false);

            entity.HasOne(d => d.TypeNavigation).WithMany(p => p.ResponsibleParties)
                .HasForeignKey(d => d.Type)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ResponsibleParty_ResponsiblePartyTypeDM");
        });

        modelBuilder.Entity<ResponsiblePartyTypeDm>(entity =>
        {
            entity.HasKey(e => e.Type).IsClustered(false);

            entity.ToTable("ResponsiblePartyTypeDM");

            entity.HasIndex(e => e.Id, "UQ_ResponsiblePartyTypeDM_Id").IsUnique();

            entity.Property(e => e.Type)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<RiskTypeDm>(entity =>
        {
            entity.HasKey(e => e.RiskType).IsClustered(false);

            entity.ToTable("RiskTypeDM", tb => tb.HasTrigger("trgRiskTypeDMModified"));

            entity.HasIndex(e => e.Id, "UQ_RiskTypeDM_Id").IsUnique();

            entity.Property(e => e.RiskType)
                .HasMaxLength(16)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<ScalingDm>(entity =>
        {
            entity.HasKey(e => e.Scaling).IsClustered(false);

            entity.ToTable("ScalingDM", tb => tb.HasTrigger("trgScalingDMModified"));

            entity.HasIndex(e => e.Id, "UQ_ScalingDM_Id").IsUnique();

            entity.Property(e => e.Scaling)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Sfaa>(entity =>
        {
            entity.HasKey(e => e.Code)
                .HasName("PK_SAA")
                .IsClustered(false);

            entity.ToTable("SFAA", tb => tb.HasTrigger("trgSFAAModified"));

            entity.Property(e => e.Code).ValueGeneratedNever();
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.General).HasMaxLength(50);
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NmlsclassCode).HasColumnName("NMLSClassCode");
            entity.Property(e => e.RateClass).HasMaxLength(6);
            entity.Property(e => e.RiskType)
                .HasMaxLength(14)
                .IsUnicode(false)
                .IsFixedLength();
        });

        modelBuilder.Entity<SfaabondTypeDm>(entity =>
        {
            entity.HasKey(e => e.SfaabondType)
                .HasName("PK_SFAABondType")
                .IsClustered(false);

            entity.ToTable("SFAABondTypeDM");

            entity.HasIndex(e => e.Id, "UQ_SFAABondType_Id").IsUnique();

            entity.Property(e => e.SfaabondType)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("SFAABondType");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Sic>(entity =>
        {
            entity.HasKey(e => e.Code);

            entity.ToTable("SIC", tb => tb.HasTrigger("trgSICModified"));

            entity.Property(e => e.Code)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.General).HasMaxLength(40);
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.RiskLevel)
                .HasMaxLength(8)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Sicratio>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered(false);

            entity.ToTable("SICRatio", tb => tb.HasTrigger("trgSICRatioModified"));

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Code)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(30);
            entity.Property(e => e.Type)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.CodeNavigation).WithMany(p => p.Sicratios)
                .HasForeignKey(d => d.Code)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SICRatio_SIC");

            entity.HasOne(d => d.TypeNavigation).WithMany(p => p.Sicratios)
                .HasForeignKey(d => d.Type)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SICRatio_SICRatioTypeDM");
        });

        modelBuilder.Entity<SicratioTypeDm>(entity =>
        {
            entity.HasKey(e => e.Type).IsClustered(false);

            entity.ToTable("SICRatioTypeDM", tb => tb.HasTrigger("trgSICRatioTypeDMModified"));

            entity.HasIndex(e => e.Id, "UQ_SICRatioTypeDM_Id").IsUnique();

            entity.Property(e => e.Type)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<State>(entity =>
        {
            entity.HasKey(e => e.Code);

            entity.ToTable("State", tb => tb.HasTrigger("trgStateModified"));

            entity.Property(e => e.Code)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.CountryCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Saacode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("SAACode");
            entity.Property(e => e.WrbstateCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("WRBStateCode");

            entity.HasOne(d => d.CountryCodeNavigation).WithMany(p => p.States)
                .HasForeignKey(d => d.CountryCode)
                .HasConstraintName("FK_State_Country");
        });

        modelBuilder.Entity<StatementBasisDm>(entity =>
        {
            entity.HasKey(e => e.Basis).IsClustered(false);

            entity.ToTable("StatementBasisDM", tb => tb.HasTrigger("trgStatementBasisDMModified"));

            entity.HasIndex(e => e.Id, "UQ_StatementBasisDM_Id").IsUnique();

            entity.Property(e => e.Basis)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<StatementQualityDm>(entity =>
        {
            entity.HasKey(e => e.Quality).IsClustered(false);

            entity.ToTable("StatementQualityDM", tb => tb.HasTrigger("trgStatementQualityDMModified"));

            entity.HasIndex(e => e.Id, "UQ_StatementQualityDM_Id").IsUnique();

            entity.Property(e => e.Quality)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<StatementTypeDm>(entity =>
        {
            entity.HasKey(e => e.Type).IsClustered(false);

            entity.ToTable("StatementTypeDM", tb => tb.HasTrigger("trgStatementTypeDMModified"));

            entity.HasIndex(e => e.Id, "UQ_StatementTypeDM_Id").IsUnique();

            entity.Property(e => e.Type)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Subaccount>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered(false);

            entity.ToTable("Subaccount", tb => tb.HasTrigger("trgSubaccountModified"));

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.AsAllowed).HasComputedColumnSql("([Stated]+[Adjustment])", false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.ParentAccount).WithMany(p => p.Subaccounts)
                .HasForeignKey(d => d.ParentAccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Subaccount_BalanceSheet");
        });

        modelBuilder.Entity<Surcharge>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered(false);

            entity.ToTable("Surcharge", tb => tb.HasTrigger("trgSurchargeModified"));

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(4);
            entity.Property(e => e.Effective).HasColumnType("date");
            entity.Property(e => e.Expiration).HasColumnType("date");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.State)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Type)
                .HasMaxLength(14)
                .IsUnicode(false);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Surcharges)
                .HasPrincipalKey(p => p.Initials)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Surcharge_UserProfile");

            entity.HasOne(d => d.StateNavigation).WithMany(p => p.Surcharges)
                .HasForeignKey(d => d.State)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Surcharge_State");

            entity.HasOne(d => d.TypeNavigation).WithMany(p => p.Surcharges)
                .HasForeignKey(d => d.Type)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Surcharge_SurchargeTypeDM");
        });

        modelBuilder.Entity<SurchargeTypeDm>(entity =>
        {
            entity.HasKey(e => e.Type).IsClustered(false);

            entity.ToTable("SurchargeTypeDM", tb => tb.HasTrigger("trgSurchargeTypeDMModified"));

            entity.HasIndex(e => e.Id, "UQ_SurchargeTypeDM_Id").IsUnique();

            entity.Property(e => e.Type)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<SystemNameDm>(entity =>
        {
            entity.HasKey(e => e.SystemName).IsClustered(false);

            entity.ToTable("SystemNameDM", tb => tb.HasTrigger("trgSystemNameDMModified"));

            entity.HasIndex(e => e.Id, "UQ_SystemNameDM_Id").IsUnique();

            entity.Property(e => e.SystemName)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<TaxBasisDm>(entity =>
        {
            entity.HasKey(e => e.TaxBasis).IsClustered(false);

            entity.ToTable("TaxBasisDM", tb => tb.HasTrigger("trgTaxBasisDMModified"));

            entity.HasIndex(e => e.Id, "UQ_TaxBasisDM_Id").IsUnique();

            entity.Property(e => e.TaxBasis)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<TicketTask>(entity =>
        {
            entity.HasKey(e => e.TicketId);

            entity.ToTable(tb => tb.HasTrigger("trgTicketTasksModified"));

            entity.Property(e => e.TicketId).ValueGeneratedNever();
            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Modified).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.RefTicket).HasColumnName("refTicket");
            entity.Property(e => e.TicketDate).HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<UnderwriterRecommendation>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered(false);

            entity.ToTable("UnderwriterRecommendation", tb => tb.HasTrigger("trgUnderwriterRecommendationModified"));

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.AccountNum)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PostedBy).HasMaxLength(4);

            entity.HasOne(d => d.AccountNumNavigation).WithMany(p => p.UnderwriterRecommendations)
                .HasForeignKey(d => d.AccountNum)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UnderwriterRecommendation_Account");
        });

        modelBuilder.Entity<UserLayoutColumn>(entity =>
        {
            entity.HasKey(e => new { e.Username, e.ColumnOrder }).IsClustered(false);

            entity.ToTable("UserLayoutColumn", "Beta");

            entity.HasIndex(e => e.Id, "UQ_UserLayoutColumn_Id").IsUnique();

            entity.Property(e => e.Username).HasMaxLength(128);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<UserLayoutWidget>(entity =>
        {
            entity.HasKey(e => new { e.Username, e.ColumnOrder, e.WidgetOrder }).IsClustered(false);

            entity.ToTable("UserLayoutWidget", "Beta");

            entity.HasIndex(e => e.Id, "UQ_UserLayoutWidget_Id").IsUnique();

            entity.Property(e => e.Username).HasMaxLength(128);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.UserLayoutColumn).WithMany(p => p.UserLayoutWidgets)
                .HasForeignKey(d => new { d.Username, d.ColumnOrder })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserLayoutWidget_UserLayoutColumn");
        });

        modelBuilder.Entity<UserLineOfAuthority>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered(false);

            entity.ToTable("UserLineOfAuthority");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.BondType)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DivisionCode)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Expiration).HasColumnType("date");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.UserLineOfAuthorityCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.UserLineOfAuthorityModifiedByNavigations)
                .HasForeignKey(d => d.ModifiedBy)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.User).WithMany(p => p.UserLineOfAuthorityUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserLineOfAuthority_UserProfile");
        });

        modelBuilder.Entity<UserMenu>(entity =>
        {
            entity.HasKey(e => new { e.Username, e.MenuName, e.MenuOrder }).IsClustered(false);

            entity.ToTable("UserMenu", "Beta");

            entity.HasIndex(e => e.Id, "UQ_UserMenu_Id").IsUnique();

            entity.Property(e => e.Username).HasMaxLength(128);
            entity.Property(e => e.MenuName)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Icon)
                .HasMaxLength(35)
                .IsUnicode(false);
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Text)
                .HasMaxLength(35)
                .IsUnicode(false);
            entity.Property(e => e.UrlPattern)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UserPreference>(entity =>
        {
            entity.HasKey(e => new { e.Username, e.Key }).IsClustered(false);

            entity.ToTable("UserPreference", "Beta");

            entity.HasIndex(e => e.Id, "UQ_UserPreference_Id").IsUnique();

            entity.Property(e => e.Username).HasMaxLength(128);
            entity.Property(e => e.Key)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<UserProfile>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered(false);

            entity.ToTable("UserProfile", tb => tb.HasTrigger("trgUserProfileModified"));

            entity.HasIndex(e => e.Initials, "IX_UserProfile_Initials");

            entity.HasIndex(e => e.Initials, "UQ_UserProfile_Initials").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FullName).HasMaxLength(46);
            entity.Property(e => e.Initials).HasMaxLength(4);
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(30);
            entity.Property(e => e.Username).HasMaxLength(20);
        });

        modelBuilder.Entity<VAccountRateDatum>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vAccountRateData");
        });

        modelBuilder.Entity<VAccountRateEmail>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vAccountRateEmail");

            entity.Property(e => e.Invoiceemail).HasColumnName("invoiceemail");
        });

        modelBuilder.Entity<VAccountRateParent>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vAccountRateParents");
        });

        modelBuilder.Entity<VConfiguration>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vConfiguration");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.Key)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Value)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VoidedBond>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered(false);

            entity.ToTable("VoidedBond", tb => tb.HasTrigger("trgVoidedBondModified"));

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.BondNumber)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Agency).WithMany(p => p.VoidedBonds)
                .HasPrincipalKey(p => p.Id)
                .HasForeignKey(d => d.AgencyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VoidedBond_Agency");

            entity.HasOne(d => d.VoidedByNavigation).WithMany(p => p.VoidedBonds)
                .HasForeignKey(d => d.VoidedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VoidedBond_UserProfile");
        });

        modelBuilder.Entity<WatchStatusDm>(entity =>
        {
            entity.HasKey(e => e.WatchStatus).IsClustered(false);

            entity.ToTable("WatchStatusDM", tb => tb.HasTrigger("trgWatchStatusDMModified"));

            entity.HasIndex(e => e.Id, "UQ_WatchStatusDM_Id").IsUnique();

            entity.Property(e => e.WatchStatus)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<WorkInProgressJob>(entity =>
        {
            entity.HasKey(e => new { e.AccountNum, e.Wipdate, e.JobNumber }).IsClustered(false);

            entity.ToTable("WorkInProgressJob", tb => tb.HasTrigger("trgWorkInProgressJobModified"));

            entity.HasIndex(e => new { e.AccountNum, e.Wipdate, e.JobNumber, e.BondNumber }, "IX_WorkInProgressJob_AccountNum_WipDate_JobNumber_BondNumber").IsClustered();

            entity.HasIndex(e => e.Id, "UQ_WorkInProgressJob_Id").IsUnique();

            entity.Property(e => e.AccountNum)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.Wipdate)
                .HasColumnType("date")
                .HasColumnName("WIPDate");
            entity.Property(e => e.JobNumber)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.BondNumber)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(30);
            entity.Property(e => e.GrossProfitPercent)
                .HasComputedColumnSql("(case when [ContractPrice]=(0) OR [EstimatedCost]=(0) then (0) else ((100.0)*[EstimatedGrossProfit])/[ContractPrice] end)", false)
                .HasColumnType("numeric(38, 15)");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.JobStatus)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasComputedColumnSql("(case when [PercentComplete]>=(100) then 'Complete' else 'Open' end)", false);
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.AccountNumNavigation).WithMany(p => p.WorkInProgressJobs)
                .HasForeignKey(d => d.AccountNum)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WorkInProgressJob_Account");

            entity.HasOne(d => d.BondNumberNavigation).WithMany(p => p.WorkInProgressJobs)
                .HasForeignKey(d => d.BondNumber)
                .HasConstraintName("FK_WorkInProgressJob_Bond");

            entity.HasOne(d => d.WorkInProgressSummary).WithMany(p => p.WorkInProgressJobs)
                .HasForeignKey(d => new { d.AccountNum, d.Wipdate })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WorkInProgressJob_WorkInProgressSummary");
        });

        modelBuilder.Entity<WorkInProgressSummary>(entity =>
        {
            entity.HasKey(e => new { e.AccountNum, e.Wipdate }).IsClustered(false);

            entity.ToTable("WorkInProgressSummary", tb => tb.HasTrigger("trgWorkInProgressSummaryModified"));

            entity.HasIndex(e => e.Id, "UQ_WorkInProgressSummary_Id").IsUnique();

            entity.Property(e => e.AccountNum)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.Wipdate)
                .HasColumnType("date")
                .HasColumnName("WIPDate");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.GrossProfitPercent)
                .HasComputedColumnSql("(case when [ContractPrice]=(0) OR [EstimatedCost]=(0) then (0) else ((100.0)*[EstimatedGrossProfit])/[ContractPrice] end)", false)
                .HasColumnType("numeric(38, 15)");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);

        if (this._seedSnapshot)
            SeedSnapshotData(modelBuilder);
        if (this._seedTestData)
            SeedTestData(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
