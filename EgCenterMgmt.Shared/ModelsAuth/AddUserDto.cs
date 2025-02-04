using EgCenterMgmt.Shared.ModelsAuth;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace EgCenterMgmt.Shared.ModelsAuth
{
    public class AddUserDto
    {
        public required string UserId { get; set; }
        [Required(ErrorMessage = "اسم المستخدم مطلوب")]
        [StringLength(50, ErrorMessage = "اسم المستخدم يجب ألا يتجاوز 50 حرفًا")]
        public required string UserName { get; set; }

        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "صيغة البريد الإلكتروني غير صحيحة")]
        public required string Email { get; set; }
        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "كلمة المرور يجب أن تكون بين 6 و 100 حرف")]
        public required string Password { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        [Required(ErrorMessage = "الدور مطلوب")]
        public required List<string> Roles { get; set; }
    }
}
