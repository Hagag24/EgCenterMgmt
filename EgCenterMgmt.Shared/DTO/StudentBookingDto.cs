using EgCenterMgmt.Shared.ModelsAuth;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EgCenterMgmt.Shared.DTO
{
    public class StudentBookingDto
    {
        // بيانات الطالب
        [Key]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "اسم الطالب مطلوب.")]
        public string? StudentName { get; set; }
        public string? Code { get; set; }

        [Required(ErrorMessage = "الإسماعيل مطلوب.")]
        public bool IsMaile { get; set; }

        [Required(ErrorMessage = "الحالة مطلوبة.")]
        public bool IStatus { get; set; }

        [Phone(ErrorMessage = "رقم الهاتف غير صالح.")]
        public string? StudentPhone { get; set; }

        [Required(ErrorMessage = "البلد مطلوبة.")]
        public string? Country { get; set; }

        [Phone(ErrorMessage = "رقم الواتساب غير صالح.")]
        public string? StudentWhatsApp { get; set; }

        [Phone(ErrorMessage = "رقم الواتساب الخاص بالأب غير صالح.")]
        public string? FatherWhatsApp { get; set; }

        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صالح.")]
        public string? StudentEmail { get; set; }

        [Required(ErrorMessage = "معرف الصف مطلوب.")]
        public int? GradeId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // بيانات الحجز
        public int BookingId { get; set; }

        [Required(ErrorMessage = "معرف المجموعة مطلوب.")]
        public int? GroupId { get; set; }

        [Required(ErrorMessage = "تاريخ الحجز مطلوب.")]
        public DateOnly? BookingDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        [Required(ErrorMessage = "نوع الدفع مطلوب.")]
        public string? PaymentType { get; set; } 
    }
}
