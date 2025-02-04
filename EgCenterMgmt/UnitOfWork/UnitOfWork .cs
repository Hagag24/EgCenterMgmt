
namespace EgCenterMgmt.Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EgCenterMgmtContext _context;

        public IRepository<Branch> Branches { get; private set; }
        public IRepository<Grade> Grades { get; private set; }
        public IRepository<Group> Groups { get; private set; }
        public IRepository<Expense> Expenses { get; private set; }
        public IRepository<VExpenses> VExpenses { get; private set; }
        public IRepository<GroupSchedule> GroupSchedules { get; private set; }
        public IRepository<AttendanceCancellation> AttendanceCancellations { get; private set; }
        public IRepository<Student> Students { get; private set; }
        public IRepository<Attendance> Attendances { get; private set; }
        public IRepository<Booking> Bookings { get; private set; }
        public IRepository<Payment> Payments { get; private set; }
        public IRepository<Subject> Subjects { get; private set; }
        public IRepository<CenterRate> CenterRates { get; private set; }
        public IRepository<Teacher> Teachers { get; private set; }
        public IRepository<ApplicationUser> Users { get; private set; }
        public IRepository<Vbooking> Vbookings { get; private set; }
        public IRepository<Vbranch> Vbranchs { get; private set; }
        public IRepository<Vgroup> Vgroups { get; private set; }
        public IRepository<VgroupSchedule> VgroupSchedules { get; private set; }
        public IRepository<Vpayment> Vpayments { get; private set; }
        public IRepository<Vstudent> Vstudents { get; private set; }
        public IRepository<Vsubject> Vsubjects { get; private set; }
        public IRepository<Vteacher> Vtechers { get; private set; }
        public IRepository<Vattendance> Vattendances { get; private set; }
        public IRepository<VPaymentAnalysis> VPaymentAnalysis { get; private set; }
        public IRepository<VAttendanceCancellation> VAttendanceCancellation { get; private set; }
        public UnitOfWork(EgCenterMgmtContext context)
        {
            _context = context;

            Branches = new Repository<Branch>(_context);
            Grades = new Repository<Grade>(_context);
            Groups = new Repository<Group>(_context);
            Expenses = new Repository<Expense>(_context);
            VExpenses = new Repository<VExpenses>(_context);
            GroupSchedules = new Repository<GroupSchedule>(_context);
            AttendanceCancellations = new Repository<AttendanceCancellation>(_context);
            Students = new Repository<Student>(_context);
            Attendances = new Repository<Attendance>(_context);
            CenterRates = new Repository<CenterRate>(_context);
            Bookings = new Repository<Booking>(_context);
            Payments = new Repository<Payment>(_context);
            Subjects = new Repository<Subject>(_context);
            Teachers = new Repository<Teacher>(_context);
            Users = new Repository<ApplicationUser>(_context);
            Vbookings = new Repository<Vbooking>(_context);
            Vbranchs = new Repository<Vbranch>(_context);
            Vgroups = new Repository<Vgroup>(_context);
            VgroupSchedules = new Repository<VgroupSchedule>(_context);
            Vpayments = new Repository<Vpayment>(_context);
            Vstudents = new Repository<Vstudent>(_context);
            Vsubjects = new Repository<Vsubject>(_context);
            Vtechers = new Repository<Vteacher>(_context);
            Vattendances = new Repository<Vattendance>(_context);
            VPaymentAnalysis = new Repository<VPaymentAnalysis>(_context);
            VAttendanceCancellation = new Repository<VAttendanceCancellation>(_context);
        }

        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
