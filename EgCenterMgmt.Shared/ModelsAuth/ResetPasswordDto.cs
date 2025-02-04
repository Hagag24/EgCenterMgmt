using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EgCenterMgmt.Shared.ModelsAuth
{
    public class ResetPasswordDto
    {
        [Required(ErrorMessage = "كود المستخدم مطلوب")]
        public string UserId { get; set; } = string.Empty;
        [Required(ErrorMessage = "كلمة المرور الجديده مطلوبة")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "كلمة المرور يجب أن تكون بين 6 و 100 حرف")]
        public string? NewPassword { get; set; }
    }
}
