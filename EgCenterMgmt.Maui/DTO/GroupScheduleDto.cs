using EgCenterMgmt.Shared.ModelsAuth;
using System;
using System.ComponentModel.DataAnnotations;

namespace EgCenterMgmt.Shared.DTO
{
    public class GroupScheduleDto
    {
        [Key]
        public int ScheduleId { get; set; }

        [Required(ErrorMessage = "معرف المجموعة مطلوب.")]
        public int? GroupId { get; set; }

        [Required(ErrorMessage = "يوم الأسبوع مطلوب.")]
        [StringLength(50, ErrorMessage = "يوم الأسبوع لا يمكن أن يكون أطول من 50 حرف.")]
        public string? DayOfWeek { get; set; }

        [Required(ErrorMessage = "وقت البدء مطلوب.")]
        public TimeOnly? StartTime { get; set; } = TimeOnly.FromDateTime(DateTime.Now);

        [Required(ErrorMessage = "وقت الانتهاء مطلوب.")]
        public TimeOnly? EndTime { get; set; } = TimeOnly.FromDateTime(DateTime.Now);
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
