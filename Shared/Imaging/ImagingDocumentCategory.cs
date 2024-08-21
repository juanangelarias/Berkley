namespace James.Shared.Imaging
{
    /// <summary>
    /// Source of truth for <c>document</c> categories
    /// </summary>
    public enum ImagingDocumentCategory
    {
        Invalid = 0,
        [DocumentCategory("BSGAccounting")]
        Account = 1,
        [DocumentCategory("BSGUnderwriting")]
        Bond = 2,
        [DocumentCategory("BSGAgency")]
        Agency = 3,
        [DocumentCategory("BSGUnderwriting")]
        CommBid = 4,
        [DocumentCategory("BSGUnderwriting")]
        ContBid = 5,
        [DocumentCategory("BSGBilling")]
        Billing = 6
    }
}
