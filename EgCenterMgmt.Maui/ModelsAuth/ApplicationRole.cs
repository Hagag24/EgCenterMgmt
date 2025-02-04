using EgCenterMgmt.Shared.Models;
using Microsoft.AspNetCore.Identity;

namespace EgCenterMgmt.Shared.ModelsAuth
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        // إضافة منشئ جديد لتقبل اسم الدور
        public ApplicationRole(string roleName) : base(roleName)
        {
        }

        public ApplicationRole() : base()
        {
        }
        public bool isManage { get; set; }
    }
}
