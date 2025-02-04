using EgCenterMgmt.Shared.ModelsAuth;
using System;
using System.ComponentModel.DataAnnotations;

namespace EgCenterMgmt.Shared.DTO
{
    public class BranchDto
    {
        [Key]
        public int BranchId { get; set; }

        [Required(ErrorMessage = "اسم الفرع مطلوب.")]
        public string? BranchName { get; set; } // إلغاء required

        [Required(ErrorMessage = "موقع الفرع مطلوب.")]
        public string? BranchLocation { get; set; } // إلغاء required
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
