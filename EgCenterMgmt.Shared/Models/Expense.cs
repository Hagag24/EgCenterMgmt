using System;
using EgCenterMgmt.Shared.ModelsAuth;
using EgCenterMgmt.Shared.Contracts;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EgCenterMgmt.Shared.Models
{
    public class Expense : IMustHaveUser
    {
        [Key]
        public int ExpenseId { get; set; }

        public string? Description { get; set; }

        public decimal Amount { get; set; }

        public DateTime Date { get; set; }

        public string? Category { get; set; }

        public string? Payee { get; set; }
        public Guid UserId { get; set; }
        public virtual ApplicationUser? User { get; set; }
    }
}
