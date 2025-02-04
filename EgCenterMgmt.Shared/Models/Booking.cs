using EgCenterMgmt.Shared.ModelsAuth;
using EgCenterMgmt.Shared.Contracts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EgCenterMgmt.Shared.Models
{
    public partial class Booking : IMustHaveUser
    {
        [Key]
        public int BookingId { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]
        public int GroupId { get; set; }

        public DateOnly? BookingDate { get; set; }

        [Required]
        public Guid UserId { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

        public virtual ICollection<AttendanceCancellation> AttendanceCancellations { get; set; } = new List<AttendanceCancellation>();
        public virtual Group? Group { get; set; }
        public virtual Student? Student { get; set; }
        public virtual ApplicationUser? User { get; set; }
    }
}
