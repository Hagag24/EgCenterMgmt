using EgCenterMgmt.Shared.ModelsAuth;
using System;
using System.ComponentModel.DataAnnotations;

namespace EgCenterMgmt.Shared.DTO
{
    public class RegesterBookingDto
    {
        [Key]
        public int BookingId { get; set; }

        [Required(ErrorMessage = "معرف الطالب مطلوب.")]
        public int? StudentId { get; set; }

        [Required(ErrorMessage = "معرف المجموعة مطلوب.")]
        public int? GroupId { get; set; }

        [Required(ErrorMessage = "تاريخ الحجز مطلوب.")]
        public DateOnly? BookingDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "نوع الدفع مطلوب.")]
        public string? PaymentType { get; set; } // يمكنك تعديل نوع البيانات حسب احتياجاتك
    }
}
