using EgCenterMgmt.Shared.Contracts;
using EgCenterMgmt.Shared.Models;
using Microsoft.AspNetCore.Identity;

namespace EgCenterMgmt.Shared.ModelsAuth
{
    public class ApplicationUser : IdentityUser<Guid>, IMustHaveTenant
    {
        public string? NatinalId { get; set; }
        public string? Address { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool isOwner { get; set; }
        public bool isManage { get; set; }
        public string? AllowedBranchIds { get; set; }
        public string TenantId { get; set; } = null!;

    }
}
