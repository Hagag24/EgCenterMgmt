using SmartLearnMgmt.Shared.Contracts;
using System;

namespace SmartLearnMgmt.Shared.Models;

public partial class Vattendance 
{
    public int AttendanceId { get; set; }

    public DateOnly? AttendanceDate { get; set; }

    public bool? AttendanceStatus { get; set; }

    public int StudentId { get; set; }

    public string? StudentName { get; set; }
    public string? StudentPhone { get; set; }
    public string? FatherWhatsApp { get; set; }

    public string? Code { get; set; }

    public int GroupId { get; set; }

    public string? GroupName { get; set; }

    public int TeacherId { get; set; }

    public string? TeacherName { get; set; }

    public int BookingId { get; set; }

    public DateOnly? BookingDate { get; set; }

    public int? PaymentId { get; set; }

    public DateOnly? PaymentDate { get; set; }

    public decimal? AmountRequired { get; set; }

    public decimal? Balance { get; set; }

    public string? DayOfWeek { get; set; }

    public string? BranchName { get; set; }

    public string? BranchLocation { get; set; }

    public decimal? Amount { get; set; }

    public bool? IsAttendanc { get; set; }
    public string? PaymentType { get; set; }
    public bool IStatus { get; set; }



}
