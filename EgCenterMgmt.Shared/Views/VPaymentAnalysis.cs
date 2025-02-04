using EgCenterMgmt.Shared.Contracts;
using System;

namespace EgCenterMgmt.Shared.Models
{
    public class VPaymentAnalysis 
    {
        public string? TeacherName { get; set; }
        public string? GroupName { get; set; }
        public string? SubjectName { get; set; }
        public string? GradeName { get; set; }
        public string? Code { get; set; }
        public string? StudentName { get; set; }
        public int? GroupId { get; set; }
        public int? StudentId { get; set; }
        public int? TeacherId { get; set; }
        public decimal? ServingPrice { get; set; }
        public decimal? PricePerMonth { get; set; }
        public int? NumberDayeSattendees { get; set; }
        public int? MaxLenth { get; set; }
        public DateTime? PaymentDate { get; set; }
        public decimal? AmountRequired { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Balance { get; set; }
        public string? PaymentType { get; set; }
        public int? BranchId { get; set; }
        public string? BranchName { get; set; }
        public string? BranchLocation { get; set; }
        public Guid? UserId { get; set; }
    

    }
}
