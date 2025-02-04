using EgCenterMgmt.Shared.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EgCenterMgmt.Shared.Models
{
    public class VExpenses 
    {
        public int ExpenseId { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Category { get; set; } = string.Empty;
        public string Payee { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
    

    }
}
