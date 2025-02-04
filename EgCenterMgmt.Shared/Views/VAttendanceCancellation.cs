using EgCenterMgmt.Shared.Contracts;
using System;

namespace EgCenterMgmt.Shared.Models
{
    public class VAttendanceCancellation 
    {
        public int Id { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string GroupName { get; set; } = string.Empty;
        public DateTime CancellationDate { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;    

    }
}
