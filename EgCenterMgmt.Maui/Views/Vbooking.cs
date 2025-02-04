using SmartLearnMgmt.Shared.Contracts;
using System;
using System.Collections.Generic;

namespace SmartLearnMgmt.Shared.Models;

public class Vbooking 
{
    public int BookingId { get; set; }

    public string? StudentName { get; set; }

    public string? GroupName { get; set; }

    public DateOnly? BookingDate { get; set; }

    public string? BranchName { get; set; }

    public string? BranchLocation { get; set; }

    public string? Code { get; set; }
    public bool IsMaile { get; set; }

    public int? StudentId { get; set; }

    public int? GroupId { get; set; }

    public DateOnly? StartDate { get; set; }

    public decimal? ServingPrice { get; set; }

    public decimal? Pricepermonth { get; set; }

    public int? NumberDayeSattendees { get; set; }


}
