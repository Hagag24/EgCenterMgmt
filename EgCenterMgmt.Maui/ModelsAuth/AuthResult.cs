namespace EgCenterMgmt.Shared.ModelsAuth;

public class AuthResult
{
    public bool Succeeded { get; set; }
    public string? Token { get; set; }
    public string? RefreshToken { get; set; }
    public string[] ErrorList { get; set; } = [];
}