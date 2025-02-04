using EgCenterMgmt.Shared.ModelsAuth;
using System;
using System.ComponentModel.DataAnnotations;

namespace EgCenterMgmt.Shared.DTO
{
    public class TeacherDto
    {
        [Key]
        public int TeacherId { get; set; }

        [Required(ErrorMessage = "اسم المعلم مطلوب.")]
        public string? TeacherName { get; set; }

        public string? TeacherPhone { get; set; }

        public string? TeacherWhatsApp { get; set; }

        public string? TeacherEmail { get; set; }

        [Required(ErrorMessage = "تخصص المعلم مطلوب.")]
        public string? TeacherSpecialization { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
