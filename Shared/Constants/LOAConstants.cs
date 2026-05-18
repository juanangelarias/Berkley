namespace James.Shared.Constants;

public static class LOAStatus
{
    public const string Approved = "APPROVED";
    public const string Proposed = "PROPOSED";
    public const string Declined = "DECLINED";
    public const string Pending = "PENDING";
    // NOTE: Max length for LineOfAuthorityStatusDM.Status is 12 characters.
    // "APPROVAL REQUESTED" is 18 characters and should only be used for AccountProgramStatusDM.
    public const string ApprovalRequested = "APPROVAL REQUESTED";
    public const string ApprovalRequestedShort = "APP. REQ.";
}

public static class LOARenewalType
{
    public const string Rapid = "Rapid";
    public const string Short = "Short";
    public const string Standard = "Standard";
}
