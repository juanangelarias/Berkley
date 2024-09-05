namespace James.Shared.Kong0;

public class KongToken
{
    public string AccessToken { get; set; }
    public string Scope { get; set; }
    public int ExpiresIn { get; set; }
    public string TokenType { get; set; }
    public DateTime? Received { get; set; }
    public bool IsExpired => null == Received || Received?.AddSeconds(ExpiresIn) < DateTime.Now;
}