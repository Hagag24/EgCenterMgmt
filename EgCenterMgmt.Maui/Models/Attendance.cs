using System;
using System.ComponentModel.DataAnnotations;
using EgCenterMgmt.Shared.ModelsAuth;
using EgCenterMgmt.Shared.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace EgCenterMgmt.Shared.Models
{
    public partial class Attendance : IMustHaveUser
    {
        [Key]
        public int AttendanceId { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]
        public int GroupId { get; set; }

        public int? PaymentId { get; set; }

        [Required]
        public int BookingId { get; set; }

        public decimal? Amount { get; set; }

        public DateOnly? AttendanceDate { get; set; }

        public bool? AttendanceStatus { get; set; }

        public string? DayOfWeek { get; set; }
        public decimal? AmountRequired { get; set; }

        public bool? IsAttendanc { get; set; }

        public string? PaymentType { get; set; }

        [Required]
        public Guid UserId { get; set; }
        public virtual Booking Booking { get; set; } = null!; // Required
        public virtual Group Group { get; set; } = null!; // Required
        public virtual Payment? Payment { get; set; }
        public virtual Student Student { get; set; } = null!; // Required
        public virtual ApplicationUser? User { get; set; }
    }
}
