using EgCenterMgmt.Shared.ModelsAuth;

namespace EgCenterMgmt.Maui.Identity;

public interface IAccountManagement
{
    Task<AuthResult> LoginAsync(LoginModel credentials);
    Task LogoutAsync();
}