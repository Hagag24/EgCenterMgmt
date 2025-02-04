using System;
using EgCenterMgmt.Shared.ModelsAuth;
using EgCenterMgmt.Shared.Contracts;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EgCenterMgmt.Shared.Models
{
    public class GroupSchedule : IMustHaveUser
    {
        [Key]
        public int ScheduleId { get; set; }

        public int? GroupId { get; set; }

        public string? DayOfWeek { get; set; }

        public TimeOnly? StartTime { get; set; }

        public TimeOnly? EndTime { get; set; }

        public Guid UserId { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public virtual Group? Group { get; set; }

        public virtual ApplicationUser? User { get; set; }
    }
}
