using EgCenterMgmt.Shared.Contracts;
using System;
using System.Collections.Generic;

namespace EgCenterMgmt.Shared.Models;

public class Vbranch 
{
    public int BranchId { get; set; }

    public string? BranchLocation { get; set; }

    public string? BranchName { get; set; }

    public string? DayOfWeek { get; set; }

    public string? GradeName { get; set; }

    public int? GroupId { get; set; }

    public string? GroupName { get; set; }

    public string? SubjectName { get; set; }

    public int? Expr1 { get; set; }

    public TimeOnly? StartTime { get; set; }

    public TimeOnly? EndTime { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public decimal? ServingPrice { get; set; }

    public decimal? Pricepermonth { get; set; }

    public int? NumberDayeSattendees { get; set; }


}
