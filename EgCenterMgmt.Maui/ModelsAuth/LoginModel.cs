using EgCenterMgmt.Shared.Contracts;
using System.ComponentModel.DataAnnotations;

namespace EgCenterMgmt.Shared.ModelsAuth;

public class LoginModel
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    [Required]
    public string TenantId { get; set; } = null!;

}