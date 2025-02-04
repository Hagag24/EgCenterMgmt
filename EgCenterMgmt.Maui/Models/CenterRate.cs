using EgCenterMgmt.Shared.ModelsAuth;
using EgCenterMgmt.Shared.Contracts;
using System;
using System.ComponentModel.DataAnnotations;

namespace EgCenterMgmt.Shared.Models
{
    public class CenterRate
    {
        [Key]
        public int Id { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "يجب أن تكون قيمة النسبه للسنتر في الحصة عددًا غير سالب.")]
        public decimal? RateSession { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "يجب أن تكون قيمة النسبه للسنتر في الشهر عددًا غير سالب.")]
        public decimal? RateMonthly { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "يجب أن تكون قيمة الشهرية عددًا غير سالب.")]
        public decimal? Rate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
