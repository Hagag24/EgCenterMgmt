using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EgCenterMgmt.Shared.ModelsAuth
{
    public class EditUserDto
    {
        [Required(ErrorMessage = "كود المستخدم مطلوب")]
        public required string UserId { get; set; }
        [Required(ErrorMessage = "اسم المستخدم مطلوب")]
        [StringLength(75, ErrorMessage = "اسم المستخدم يجب ألا يتجاوز 50 حرفًا")]
        public required string UserName { get; set; }

        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "صيغة البريد الإلكتروني غير صحيحة")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "الرقم القومي مطلوب")]
        [StringLength(14, ErrorMessage = "الرقم القومي يجب أن يكون 14 رقم")]
        public string? NatinalId { get; set; }

        [StringLength(250, ErrorMessage = "العنوان يجب ألا يتجاوز 250 حرفًا")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [Phone(ErrorMessage = "صيغة رقم الهاتف غير صحيحة")]
        public string? PhoneNumber { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.Now;
    }
}
