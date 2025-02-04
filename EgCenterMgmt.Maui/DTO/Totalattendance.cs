using System;
using System.Linq;

namespace EgCenterMgmt.Shared.DTO
{
    public class Totalattendance
    {
        // الخصائص الحالية
        public string? StudentCode { get; set; }
        public string? StudentName { get; set; }
        public int TotalAttendance { get; set; } // إجمالي الحضور
        public int TotalAbsence { get; set; } // إجمالي الغياب
        public string? AttendanceDates { get; set; } // أيام الحضور
        public string? AbsenceDates { get; set; } // أيام الغياب
        public string? GroupName { get; set; } // أيام الغياب
        public string? TeacherName { get; set; } // أيام الغياب
        public decimal TotalAmount { get; set; } // المبالغ التي دفعها الطالب
        public decimal TotalBalance { get; set; } // المبالغ المتبقية عليه
        public bool IStatus { get; set; } // حالة التزام الطالب (ملتزم أم لا)
        public string? StudentPhone { get; set; }
        public string? FatherWhatsApp { get; set; }
    }
}
