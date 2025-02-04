using EgCenterMgmt.Shared.ModelsAuth;
using Microsoft.VisualBasic;
using System;
using System.ComponentModel.DataAnnotations;

namespace EgCenterMgmt.Shared.DTO
{
    public class GroupDto
    {
        [Key]
        public int GroupId { get; set; }

        [Required(ErrorMessage = "اسم المجموعة مطلوب.")]
        public string? GroupName { get; set; }

        [Required(ErrorMessage = "معرّف الفرع مطلوب.")]
        public int BranchId { get; set; }

        [Required(ErrorMessage = "معرّف المادة مطلوب.")]
        public int SubjectId { get; set; }

        [Required(ErrorMessage = "معرّف المعلم مطلوب.")]
        public int TeacherId { get; set; }

        [Required(ErrorMessage = "معرّف الصف مطلوب.")]
        public int GradeId { get; set; }

        [Required(ErrorMessage = "تاريخ البدء مطلوب.")]
        public DateOnly StartDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public decimal ServingPrice { get; set; }
        public decimal Pricepermonth { get; set; }

        [Required(ErrorMessage = "عدد الأيام المطلوبة مطلوب.")]
        [Range(1, int.MaxValue, ErrorMessage = "يجب أن يكون عدد الأيام أكبر من 0.")]
        public int NumberDayeSattendees { get; set; }

        [Required(ErrorMessage = "الحد الأقصى مطلوب.")]
        [Range(1, int.MaxValue, ErrorMessage = "يجب أن يكون الحد الأقصى أكبر من 0.")]
        public int MaxLenth { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
