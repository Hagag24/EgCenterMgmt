using EgCenterMgmt.Shared.ModelsAuth;
using System;
using System.ComponentModel.DataAnnotations;

namespace EgCenterMgmt.Shared.DTO
{
    public class AttendanceDto
    {
        [Key]
        public int AttendanceId { get; set; }

        [Required(ErrorMessage = "معرّف الطالب مطلوب.")]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "معرّف المجموعة مطلوب.")]
        public int GroupId { get; set; }

        public int? PaymentId { get; set; }

        [Required(ErrorMessage = "معرّف الحجز مطلوب.")]
        public int BookingId { get; set; }

        public decimal? Amount { get; set; }

        [Required(ErrorMessage = "تاريخ الحضور مطلوب.")]
        public DateOnly AttendanceDate { get; set; }

        [Required(ErrorMessage = "حالة الحضور مطلوبة.")]
        public bool? AttendanceStatus { get; set; }

        public string? DayOfWeek { get; set; }

        public decimal? AmountRequired { get; set; }

        public bool IsAttendanc { get; set; }

        public string? PaymentType { get; set; }
    }
}
