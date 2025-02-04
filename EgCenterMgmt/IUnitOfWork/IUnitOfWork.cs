
namespace EgCenterMgmt.IRepository.IUnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Branch> Branches { get; }
        IRepository<Grade> Grades { get; }
        IRepository<Group> Groups { get; }
        IRepository<Expense> Expenses { get; }
        IRepository<VExpenses> VExpenses { get; }
        IRepository<GroupSchedule> GroupSchedules { get; }
        IRepository<AttendanceCancellation> AttendanceCancellations { get; }
        IRepository<Student> Students { get; }
        IRepository<Attendance> Attendances { get; }
        IRepository<Booking> Bookings { get; }
        IRepository<CenterRate> CenterRates { get; }
        IRepository<Payment> Payments { get; }
        IRepository<Subject> Subjects { get; }
        IRepository<Teacher> Teachers { get; }
        IRepository<ApplicationUser> Users { get; }
        IRepository<Vbooking> Vbookings { get; }
        IRepository<Vbranch> Vbranchs { get; }
        IRepository<Vgroup> Vgroups { get; }
        IRepository<VgroupSchedule> VgroupSchedules { get; }
        IRepository<Vpayment> Vpayments { get; }
        IRepository<Vstudent> Vstudents { get; }
        IRepository<Vsubject> Vsubjects { get; }
        IRepository<Vteacher> Vtechers { get; }
        IRepository<Vattendance> Vattendances { get; }
        IRepository<VPaymentAnalysis> VPaymentAnalysis { get; }
        IRepository<VAttendanceCancellation> VAttendanceCancellation { get; }

        Task<int> Complete();
    }
}
