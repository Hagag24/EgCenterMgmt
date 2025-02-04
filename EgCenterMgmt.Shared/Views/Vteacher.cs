using EgCenterMgmt.Shared.Contracts;
using System;
using System.Collections.Generic;

namespace EgCenterMgmt.Shared.Models;

public class Vteacher 
{
    public int TeacherId { get; set; }

    public string? TeacherName { get; set; }

    public string? GroupName { get; set; }

    public string? SubjectName { get; set; }

    public string? GradeName { get; set; }

    public string? BranchName { get; set; }

    public string? BranchLocation { get; set; }

    public int? SubjectId { get; set; }

    public int? GroupId { get; set; }


}
