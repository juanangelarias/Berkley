namespace James.Shared.Data
{
    /// <summary>
    /// Constants used by multiple assemblies
    /// </summary>
    public static class JamesConstants
    {
        public const string LOG_IN_PATH = "/account/Login";
        public const string LOG_OUT_PATH = "/account/Logout";
        public const int Default_Max_Retries = 5;
    }

    /// <summary>
    /// A set of constants to use as categories for log messages to keep them consistent
    /// </summary>
    public static class StandardLoggingCategories
    {
        public const string Imaging = "Imaging";
        public const string UserInterface = "UI";
        public const string Account = "Account";
        public const string Agency = "Agency";
        public const string Obligee = "Obligee";
        public const string Bond = "Bond";
        public const string DataAccess = "Data Access";
        public const string BrowserFeatures = "Browser Features";//For issues with browser features like LocalStorage, etc.
    }
}
