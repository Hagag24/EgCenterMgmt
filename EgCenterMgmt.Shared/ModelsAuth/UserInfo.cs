using EgCenterMgmt.Shared.Contracts;
using System.Text.Json.Serialization;

namespace EgCenterMgmt.Shared.ModelsAuth;

public class UserInfo : IMustHaveTenant
{
    public Guid UserId { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string NatinalId { get; set; } = string.Empty;
    [JsonPropertyName("role")]
    public IList<string> Roles { get; set; } = new List<string>();
    public string TenantId { get; set; } = null!;
    public string TenantName { get; set; } = null!;
    public Dictionary<string, string> Claims { get; set; } = [];
}