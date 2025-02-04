using System;
using System.Collections.Generic;
using EgCenterMgmt.Shared.ModelsAuth;
using EgCenterMgmt.Shared.Contracts;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EgCenterMgmt.Shared.Models
{
    public class Teacher : IMustHaveUser
    {
        [Key]
        public int TeacherId { get; set; }

        public string? TeacherName { get; set; }

        public string? TeacherPhone { get; set; }

        public string? TeacherWhatsApp { get; set; }

        public string? TeacherEmail { get; set; }

        public string? TeacherSpecialization { get; set; }

        public Guid UserId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public virtual ICollection<Group> Groups { get; set; } = new List<Group>();
        public virtual ApplicationUser? User { get; set; }
    }
}
