using EgCenterMgmt.Shared.Contracts;
using System;

namespace EgCenterMgmt.Shared.Models;

public class Vstudent 
{
    public int StudentId { get; set; }

    public string? StudentName { get; set; }

    public string? Code { get; set; }
    public bool IsMaile { get; set; }

    public string? Country { get; set; }

    public string? StudentEmail { get; set; }

    public string? StudentPhone { get; set; }

    public string? StudentWhatsApp { get; set; }

    public string? FatherWhatsApp { get; set; }

    public string? GradeName { get; set; }
    public int GradeId { get; set; }
    public bool IStatus { get; set; }


}
