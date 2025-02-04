using EgCenterMgmt.Shared.Contracts;
using System;
using System.Collections.Generic;

namespace EgCenterMgmt.Shared.Models;

public class Vpayment 
{
    public int PaymentId { get; set; }

    public string? StudentName { get; set; }

    public string? Code { get; set; }

    public string? GroupName { get; set; }

    public DateOnly? PaymentDate { get; set; }

    public decimal? AmountRequired { get; set; }

    public decimal? Amount { get; set; }

    public decimal? Balance { get; set; }

    public string? PaymentType { get; set; }

    public int? GroupId { get; set; }

    public int? StudentId { get; set; }


}
