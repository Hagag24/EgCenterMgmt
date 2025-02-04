using System;
using System.Collections.Generic;
using EgCenterMgmt.Shared.ModelsAuth;
using EgCenterMgmt.Shared.Contracts;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EgCenterMgmt.Shared.Models
{
    public class Payment : IMustHaveUser
    {
        [Key]
        public int PaymentId { get; set; }

        public int? StudentId { get; set; }

        public int? GroupId { get; set; }

        public DateOnly? PaymentDate { get; set; }

        public decimal? AmountRequired { get; set; }

        public decimal? Amount { get; set; }

        public decimal? Balance { get; set; }

        public string? PaymentType { get; set; }

        public Guid UserId { get; set; }
        public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

        public virtual Group? Group { get; set; }

        public virtual Student? Student { get; set; }

        public virtual ApplicationUser? User { get; set; }
    }
}
