namespace EgCenterMgmt.Services
{
    public static class DayOfWeekHelper
    {
        // تعريف القاموس كـ public static readonly ليمكن الوصول إليه من أماكن أخرى
        public static readonly Dictionary<DayOfWeek, string> dayOfWeekTranslations = new Dictionary<DayOfWeek, string>
    {
        { DayOfWeek.Sunday, "الأحد" },
        { DayOfWeek.Monday, "الإثنين" },
        { DayOfWeek.Tuesday, "الثلاثاء" },
        { DayOfWeek.Wednesday, "الأربعاء" },
        { DayOfWeek.Thursday, "الخميس" },
        { DayOfWeek.Friday, "الجمعة" },
        { DayOfWeek.Saturday, "السبت" }
    };

        public static DayOfWeek TranslateToDayOfWeek(string dayName)
        {
            switch (dayName)
            {
                case "السبت": return DayOfWeek.Saturday;
                case "الأحد": return DayOfWeek.Sunday;
                case "الإثنين": return DayOfWeek.Monday;
                case "الثلاثاء": return DayOfWeek.Tuesday;
                case "الأربعاء": return DayOfWeek.Wednesday;
                case "الخميس": return DayOfWeek.Thursday;
                case "الجمعة": return DayOfWeek.Friday;
                default: throw new ArgumentException($"لا يمكن تحويل {dayName} إلى DayOfWeek.");
            }
        }
    }


}
