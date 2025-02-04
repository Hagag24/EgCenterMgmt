using EgCenterMgmt.Shared.ModelsAuth;
using System;
using System.ComponentModel.DataAnnotations;

namespace EgCenterMgmt.Shared.Models
{
    public class ExpenseDto
    {
        public int ExpenseId { get; set; }

        [Required(ErrorMessage = "الوصف مطلوب.")]
        public string? Description { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "يجب أن يكون المبلغ أكبر من 0.")]
        public  decimal Amount { get; set; }

        [Required(ErrorMessage = "تاريخ المصروف مطلوب.")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "الفئة مطلوبة.")]
        public string? Category { get; set; }

        [Required(ErrorMessage = "اسم المستلم مطلوب.")]
        public string? Payee { get; set; }

    }
}
