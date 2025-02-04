using SmartLearnMgmt.Shared.Contracts;
using System;
using System.Collections.Generic;

namespace SmartLearnMgmt.Shared.Models;

public class VgroupSchedule 
{
    public int ScheduleId { get; set; }

    public string? GroupName { get; set; }

    public string? DayOfWeek { get; set; }

    public TimeOnly? StartTime { get; set; }

    public TimeOnly? EndTime { get; set; }

    public int? GroupId { get; set; }


}
