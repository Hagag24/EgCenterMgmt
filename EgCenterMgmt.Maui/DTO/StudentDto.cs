using EgCenterMgmt.Shared.ModelsAuth;
using System;
using System.ComponentModel.DataAnnotations;

namespace EgCenterMgmt.Shared.DTO
{
    public class StudentDto
    {
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

    }
    public class StudentBalanceDto
    {
        public int StudentId { get; set; }
        public int? GroupId { get; set; }
        public string? StudentName { get; set; }
        public string? Code { get; set; }
        public decimal? AmountRequired { get; set; }
        public decimal? TotalPaid { get; set; }
        public decimal? RemainingBalance { get; set; }
    }
    public class StudentBalanceRequestDto
    {
        public List<int> StudentIds { get; set; } = new List<int>();
        public List<int> GroupIds { get; set; } = new List<int>();
    }
}
