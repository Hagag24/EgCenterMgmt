using EgCenterMgmt.Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EgCenterMgmt.Shared.ModelsAuth;

namespace EgCenterMgmt.Shared.DTO
{
    public class CenterRateDto
    {
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
