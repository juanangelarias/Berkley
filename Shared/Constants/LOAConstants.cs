namespace James.Shared.Constants;

/// <summary>
/// Represents the various statuses used in LOA (Letter of Authorization) processes.
/// </summary>
/// <remarks>
/// This class contains constant string values that denote the possible states
/// a Letter of Authorization can have throughout its lifecycle in the system.
/// </remarks>
public static class LOAStatus
{
    /// <summary>
    /// Represents the status "Approved" in the LOA (Letter of Authorization) process.
    /// </summary>
    /// <remarks>
    /// This status indicates that the LOA has been reviewed and granted approval.
    /// It signifies the final confirmation step in the process, allowing subsequent
    /// actions or workflows to proceed as authorized.
    /// </remarks>
    public const string Approved = "APPROVED";

    /// <summary>
    /// Represents the status "Proposed" in the LOA (Letter of Authorization) process.
    /// </summary>
    /// <remarks>
    /// This status signifies that a proposal has been initiated in the system but is not yet finalized or approved.
    /// It serves as an early stage in the lifecycle of the authorization process, awaiting further action or decision.
    /// </remarks>
    public const string Proposed = "PROPOSED";

    /// <summary>
    /// Represents the status "Declined" in the LOA (Letter of Authorization) process.
    /// </summary>
    /// <remarks>
    /// This status signifies that the LOA request has been reviewed and rejected.
    /// It indicates the conclusion of the process without approval.
    /// </remarks>
    public const string Declined = "DECLINED";

    /// <summary>
    /// Represents the "Pending" status in the LOA (Letter of Authorization) process.
    /// </summary>
    /// <remarks>
    /// This status signifies that the LOA process is awaiting further action or decision.
    /// It indicates an intermediary state prior to resolution.
    /// </remarks>
    public const string Pending = "PENDING";
    // NOTE: Max length for LineOfAuthorityStatusDM.Status is 12 characters.
    // "APPROVAL REQUESTED" is 18 characters and should only be used for AccountProgramStatusDM.
    /// <summary>
    /// Represents the status "Approval Requested" in the LOA (Letter of Authorization) process.
    /// </summary>
    /// <remarks>
    /// This status is used to indicate that an approval request is pending
    /// within the system. It plays a critical role in transitioning the process
    /// to an approved or declined state based on the outcome of the request.
    /// </remarks>
    public const string ApprovalRequested = "APPROVAL REQUESTED";

    /// <summary>
    /// Represents the shortened designation for the "Approval Requested" status in the LOA process.
    /// </summary>
    public const string ApprovalRequestedShort = "APP. REQ.";
}

/// <summary>
/// Defines the different renewal types applicable to a Letter of Authorization (LOA).
/// </summary>
/// <remarks>
/// This class provides constant string values representing various renewal type categories
/// for managing LOA renewals in the system.
/// </remarks>
public static class LOARenewalType
{
    /// <summary>
    /// Represents the renewal type "Rapid" in the LOA renewal process.
    /// </summary>
    public const string Rapid = "Rapid";

    /// <summary>
    /// Represents the renewal type "Short" in the LOA renewal process.
    /// </summary>
    public const string Short = "Short";

    /// <summary>
    /// Represents the renewal type "Standard" in the LOA renewal process.
    /// </summary>
    public const string Standard = "Standard";
}
