using EgCenterMgmt.Shared.ModelsAuth;
using System;
using System.ComponentModel.DataAnnotations;

namespace EgCenterMgmt.Shared.DTO
{
    public class PaymentDto
    {
        [Key]
        public int PaymentId { get; set; }

        [Required(ErrorMessage = "معرف الطالب مطلوب.")]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "معرف المجموعة مطلوب.")]
        public int GroupId { get; set; }

        [Required(ErrorMessage = "تاريخ الدفع مطلوب.")]
        public DateOnly PaymentDate { get; set; }
        public decimal? AmountRequired { get; set; }

        [Required(ErrorMessage = "المبلغ مطلوب.")]
        public decimal? Amount { get; set; }
        public decimal? Balance { get; set; }

        [Required(ErrorMessage = "نوع الدفع مطلوب.")]
        public string? PaymentType { get; set; }
    }
}
