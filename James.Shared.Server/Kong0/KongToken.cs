namespace James.Shared.Kong0;

public class KongToken
{
    public string AccessToken { get; set; } = string.Empty;
    public string Scope { get; set; } = string.Empty;
    public int ExpiresIn { get; set; }
    public string TokenType { get; set; } = string.Empty;
    public DateTime? Received { get; set; }
    public bool IsExpired => null == Received || Received?.AddSeconds(ExpiresIn) < DateTime.Now;
}