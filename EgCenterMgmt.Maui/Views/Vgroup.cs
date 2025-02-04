using SmartLearnMgmt.Shared.Contracts;
using System;
using System.Collections.Generic;

namespace SmartLearnMgmt.Shared.Models;

public class Vgroup 
{
    public int GroupId { get; set; }

    public string? GroupName { get; set; }

    public string? SubjectName { get; set; }

    public string? TeacherName { get; set; }

    public string? GradeName { get; set; }

    public DateOnly? StartDate { get; set; }

    public decimal? ServingPrice { get; set; }

    public decimal? Pricepermonth { get; set; }

    public int? NumberDayeSattendees { get; set; }

    public string? BranchName { get; set; }

    public string? BranchLocation { get; set; }
    public int? MaxLenth { get; set; }

    public int? TeacherId { get; set; }


}
