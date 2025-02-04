using System;
using System.Collections.Generic;
using EgCenterMgmt.Shared.ModelsAuth;
using EgCenterMgmt.Shared.Contracts;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EgCenterMgmt.Shared.Models
{
    public class Group : IMustHaveUser
    {
        [Key]
        public int GroupId { get; set; }

        public string? GroupName { get; set; }

        public int? BranchId { get; set; }

        public int? SubjectId { get; set; }

        public int? TeacherId { get; set; }

        public int? GradeId { get; set; }

        public DateOnly StartDate { get; set; }

        public decimal? ServingPrice { get; set; }
        public decimal? Pricepermonth { get; set; }

        public int? NumberDayeSattendees { get; set; }
        public int? MaxLenth { get; set; }

        public Guid UserId { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

        public virtual ICollection<GroupSchedule> GroupSchedules { get; set; } = new List<GroupSchedule>();

        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

        public virtual ICollection<AttendanceCancellation> AttendanceCancellations { get; set; } = new List<AttendanceCancellation>();

        public virtual Branch? Branch { get; set; }

        public virtual Grade? Grade { get; set; }

        public virtual Subject? Subject { get; set; }

        public virtual Teacher? Teacher { get; set; }
        public virtual ApplicationUser? User { get; set; }
    }
}
