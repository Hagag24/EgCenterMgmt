
namespace EgCenterMgmt.Repository.Data;

public partial class EgCenterMgmtContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    public string? TenantId { get; set; }
    public string? UserId { get; set; }

    private readonly ITenantService _tenantService;
    public EgCenterMgmtContext(DbContextOptions options, ITenantService tenantService) : base(options)
    {
        _tenantService = tenantService;
        TenantId = _tenantService.GetCurrentTenant()?.TId;
        UserId = _tenantService.GetCurrentUserId();
        this.Database.SetCommandTimeout(60);
    }

    public DbSet<ApplicationUser> ApplicationUser { get; set; }
    public DbSet<Branch> Branches { get; set; }
    public DbSet<Grade> Grades { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    ///
    public DbSet<Group> Groups { get; set; }
    /////
    public DbSet<Student> Students { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    ///
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Expense> Expenses { get; set; }
    public DbSet<GroupSchedule> GroupSchedules { get; set; }
    ///// 
    public DbSet<Attendance> Attendances { get; set; }
    public DbSet<AttendanceCancellation> AttendanceCancellations { get; set; }
    /////
    public DbSet<CenterRate> CenterRates { get; set; }


    public DbSet<Vattendance> Vattendances { get; set; }
    public DbSet<Vbooking> Vbookings { get; set; }
    public DbSet<Vbranch> Vbranchs { get; set; }
    public DbSet<VExpenses> VExpenses { get; set; }
    public DbSet<Vgroup> Vgroups { get; set; }
    public DbSet<VgroupSchedule> VgroupSchedules { get; set; }
    public DbSet<Vpayment> Vpayments { get; set; }
    public DbSet<Vstudent> Vstudents { get; set; }
    public DbSet<Vsubject> Vsubjects { get; set; }
    public DbSet<Vteacher> Vteachers { get; set; }
    public DbSet<VPaymentAnalysis> VPaymentAnalysis { get; set; }
    public DbSet<VAttendanceCancellation> VAttendanceCancellation { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.Entity<Branch>().HasQueryFilter(e => e.TenantId == TenantId);
        //modelBuilder.Entity<Grade>().HasQueryFilter(e => e.TenantId == TenantId);
        //modelBuilder.Entity<Subject>().HasQueryFilter(e => e.TenantId == TenantId);
        //modelBuilder.Entity<Teacher>().HasQueryFilter(e => e.TenantId == TenantId);
        //modelBuilder.Entity<Group>().HasQueryFilter(e => e.TenantId == TenantId);
        //modelBuilder.Entity<Student>().HasQueryFilter(e => e.TenantId == TenantId);
        //modelBuilder.Entity<Booking>().HasQueryFilter(e => e.TenantId == TenantId);
        //modelBuilder.Entity<Payment>().HasQueryFilter(e => e.TenantId == TenantId);
        //modelBuilder.Entity<Attendance>().HasQueryFilter(e => e.TenantId == TenantId);
        //modelBuilder.Entity<CenterRate>().HasQueryFilter(e => e.TenantId == TenantId);
        //modelBuilder.Entity<Expense>().HasQueryFilter(e => e.TenantId == TenantId);
        //modelBuilder.Entity<GroupSchedule>().HasQueryFilter(e => e.TenantId == TenantId);
        //modelBuilder.Entity<AttendanceCancellation>().HasQueryFilter(e => e.TenantId == TenantId);
        //modelBuilder.Entity<Vattendance>().HasQueryFilter(e => e.TenantId == TenantId);
        //modelBuilder.Entity<Vbooking>().HasQueryFilter(e => e.TenantId == TenantId);
        //modelBuilder.Entity<Vbranch>().HasQueryFilter(e => e.TenantId == TenantId);
        //modelBuilder.Entity<VExpenses>().HasQueryFilter(e => e.TenantId == TenantId);
        //modelBuilder.Entity<Vgroup>().HasQueryFilter(e => e.TenantId == TenantId);
        //modelBuilder.Entity<VgroupSchedule>().HasQueryFilter(e => e.TenantId == TenantId);
        //modelBuilder.Entity<Vpayment>().HasQueryFilter(e => e.TenantId == TenantId);
        //modelBuilder.Entity<Vstudent>().HasQueryFilter(e => e.TenantId == TenantId);
        //modelBuilder.Entity<Vsubject>().HasQueryFilter(e => e.TenantId == TenantId);
        //modelBuilder.Entity<Vteacher>().HasQueryFilter(e => e.TenantId == TenantId);
        //modelBuilder.Entity<VPaymentAnalysis>().HasQueryFilter(e => e.TenantId == TenantId);
        //modelBuilder.Entity<VAttendanceCancellation>().HasQueryFilter(e => e.TenantId == TenantId);



        modelBuilder.HasDefaultSchema("dbo");
        modelBuilder.Entity<VPaymentAnalysis>().HasNoKey().ToView("VPaymentAnalysis");
        modelBuilder.Entity<Vattendance>().HasNoKey().ToView("VAttendance");
        modelBuilder.Entity<VExpenses>().HasNoKey().ToView("VExpenses");
        modelBuilder.Entity<Vbooking>().HasNoKey().ToView("VBookings");
        modelBuilder.Entity<Vbranch>().HasNoKey().ToView("VBranchs");
        modelBuilder.Entity<Vgroup>().HasNoKey().ToView("VGroups");
        modelBuilder.Entity<VgroupSchedule>().HasNoKey().ToView("VGroupSchedules");
        modelBuilder.Entity<Vpayment>().HasNoKey().ToView("VPayments");
        modelBuilder.Entity<Vstudent>().HasNoKey().ToView("VStudents");
        modelBuilder.Entity<Vsubject>().HasNoKey().ToView("VSubject");
        modelBuilder.Entity<Vteacher>().HasNoKey().ToView("VTeacher");
        modelBuilder.Entity<VAttendanceCancellation>().HasNoKey().ToView("VAttendanceCancellation");

        base.OnModelCreating(modelBuilder);
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var tenantConnectionString = _tenantService.GetConnectionString();

        if (!string.IsNullOrWhiteSpace(tenantConnectionString))
        {
            var dbProvider = _tenantService.GetDatabaseProvider();

            if (dbProvider?.ToLower() == "mssql")
            {
                optionsBuilder.UseSqlServer(tenantConnectionString);
            }
        }
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<IMustHaveTenant>().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
        {
            if (entry.Entity.TenantId == null) { entry.Entity.TenantId = TenantId!; }

        }
        foreach (var entry in ChangeTracker.Entries<IMustHaveUser>().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
        {
            if (entry.Entity.UserId == Guid.Empty && !string.IsNullOrEmpty(UserId)) { entry.Entity.UserId = Guid.Parse(UserId); }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}



