using EgCenterMgmt.Shared.ModelsAuth;
using System;
using System.ComponentModel.DataAnnotations;

namespace EgCenterMgmt.Shared.DTO
{
    public class AttendanceCancellationDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "معرّف الطالب مطلوب.")]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "معرّف المجموعة مطلوب.")]
        public int GroupId { get; set; }

        [Required(ErrorMessage = "معرّف الحجز مطلوب.")]
        public int BookingId { get; set; }

        [Required(ErrorMessage = "تاريخ الرجوع مطلوب.")]
        public DateTime CancellationDate { get; set; }

        [StringLength(500, ErrorMessage = "سبب الرجوع لا يمكن أن يتجاوز 500 حرف.")]
        public string? Reason { get; set; } // سبب الرجوع
    }
}
