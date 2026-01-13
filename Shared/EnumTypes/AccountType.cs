namespace James.Shared.EnumTypes;

public static class AccountType
{
    public const string Commercial = "Commercial";
    public const string Contract = "Contract";

    public static List<string> GetList()
    {
        return [Commercial, Contract];
    }
}