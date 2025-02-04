using EgCenterMgmt.Shared.ModelsAuth;
using System;
using System.ComponentModel.DataAnnotations;

namespace EgCenterMgmt.Shared.DTO
{
    public class BookingDto
    {
        [Key]
        public int BookingId { get; set; }

        [Required(ErrorMessage = "معرّف الطالب مطلوب.")]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "معرّف المجموعة مطلوب.")]
        public int GroupId { get; set; }

        [Required(ErrorMessage = "تاريخ الحجز مطلوب.")]
        public DateOnly BookingDate { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
