using EgCenterMgmt.Shared.ModelsAuth;
using System;
using System.ComponentModel.DataAnnotations;

namespace EgCenterMgmt.Shared.DTO
{
    public class GradeDto
    {
        [Key]
        public int GradeId { get; set; }

        [Required(ErrorMessage = "اسم الصف مطلوب.")]
        public string? GradeName { get; set; } // إلغاء required
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
