using EgCenterMgmt.Shared.ModelsAuth;
using System;
using System.ComponentModel.DataAnnotations;

namespace EgCenterMgmt.Shared.DTO
{
    public class SubjectDto
    {
        [Key]
        public int SubjectId { get; set; }

        [Required(ErrorMessage = "اسم المادة مطلوب.")]
        public string? SubjectName { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
