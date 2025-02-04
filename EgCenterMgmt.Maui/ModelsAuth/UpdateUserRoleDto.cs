using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EgCenterMgmt.Shared.ModelsAuth
{
    public class UpdateUserRoleDto
    {
        [Required(ErrorMessage = "معرف المستخدم مطلوب")]
        public string? UserId { get; set; }

        [Required(ErrorMessage = "الدور الجديد مطلوب")]
        public List<string> Roles { get; set; } = new List<string>();
    }
}
