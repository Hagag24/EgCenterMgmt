using System;
using System.ComponentModel.DataAnnotations;
using EgCenterMgmt.Shared.ModelsAuth;
using EgCenterMgmt.Shared.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace EgCenterMgmt.Shared.Models
{
    public class AttendanceCancellation : IMustHaveUser
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]
        public int GroupId { get; set; }

        [Required]
        public int BookingId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public DateTime CancellationDate { get; set; }
        public string? Reason { get; set; }
        public virtual Student Student { get; set; } = null!;
        public virtual Group Group { get; set; } = null!;
        public virtual Booking Booking { get; set; } = null!;
        public virtual ApplicationUser User { get; set; } = null!;
    }
}
