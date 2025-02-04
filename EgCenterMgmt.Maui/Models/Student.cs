using EgCenterMgmt.Shared.ModelsAuth;
using EgCenterMgmt.Shared.Contracts;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EgCenterMgmt.Shared.Models
{
    public class Student : IMustHaveUser
    {
        [Key]
        public int StudentId { get; set; }

        public string? StudentName { get; set; }

        public string? Code { get; set; }

        public bool IsMaile { get; set; }

        public bool IStatus { get; set; }

        public string? StudentPhone { get; set; }

        public string? Country { get; set; }

        public string? StudentWhatsApp { get; set; }

        public string? FatherWhatsApp { get; set; }

        public string? StudentEmail { get; set; }

        public int? GradeId { get; set; }

        public Guid UserId { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public virtual Grade? Grade { get; set; }

        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

        public virtual ICollection<AttendanceCancellation> AttendanceCancellations { get; set; } = new List<AttendanceCancellation>();
        public virtual ApplicationUser? User { get; set; }
    }
}
