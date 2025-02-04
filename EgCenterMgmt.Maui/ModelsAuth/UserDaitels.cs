using System;
using System.Linq;

namespace EgCenterMgmt.Shared.ModelsAuth
{
    public static class UserDaitels
    {
        public static Guid UserId { get; set; }
        //public static string? Name { get; set; }
        //public static string? Email { get; set; }
        //public static string TenantId { get; set; } = null!;
        public static IList<string> Roles { get; set; } = new List<string>();
    }
}
