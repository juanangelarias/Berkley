namespace James.Shared.Constants;

public class RegExs
{
    public const string Email = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
    public const string Year = @"^(?=(?:.*\d){4,}).*$";
}