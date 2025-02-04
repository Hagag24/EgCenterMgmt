using EgCenterMgmt.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EgCenterMgmt.Shared.DTO;
public class AllDataDto
{
    public IEnumerable<Teacher>? Teachers { get; set; }
    public IEnumerable<Subject>? Subjects { get; set; }
    public IEnumerable<Student>? Students { get; set; }
    public IEnumerable<Payment>? Payments { get; set; }
    public IEnumerable<GroupSchedule>? GroupSchedules { get; set; }
    public IEnumerable<Grade>? Grades { get; set; }
    public IEnumerable<Group>? Groups { get; set; }
    public IEnumerable<Attendance>? Attendances { get; set; }
    public IEnumerable<Branch>? Branches { get; set; }
    public IEnumerable<Booking>? Bookings { get; set; }
}
