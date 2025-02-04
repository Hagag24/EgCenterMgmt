using EgCenterMgmt.Shared.ModelsAuth;

namespace EgCenterMgmt.Client.Identity;

public interface IAccountManagement
{
    Task<AuthResult> LoginAsync(LoginModel credentials);
    Task LogoutAsync();
}