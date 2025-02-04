using System.ComponentModel.DataAnnotations;

namespace EgCenterMgmt.Shared.ModelsAuth
{
    public class ResetPasswordDtoBase
    {
        [Required(ErrorMessage = "كلمة المرور الجديده مطلوبة")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "كلمة المرور يجب أن تكون بين 6 و 100 حرف")]
        public string? NewPassword { get; set; }
        [Required(ErrorMessage = "كود المستخدم مطلوب")]
        public string UserId { get; set; } = string.Empty;
    }
}