
namespace EgCenterMgmt.Seed
{
    public static class SeedViews
    {
        public static async Task AddOrUpdateViews(EgCenterMgmtContext dbcontext)
        {
            try
            {
                await dbcontext.Database.ExecuteSqlRawAsync(@"
                    -- تحقق وإعادة إنشاء VAttendance
                    IF OBJECT_ID('dbo.VAttendance', 'V') IS NOT NULL
                        DROP VIEW [dbo].[VAttendance];
                ");

                await dbcontext.Database.ExecuteSqlRawAsync(@"
                    CREATE VIEW [dbo].[VAttendance] AS
                    SELECT a.AttendanceId, a.AttendanceDate, a.AttendanceStatus, s.StudentId, s.StudentName, s.Code, g.GroupId, g.GroupName, t.TeacherId, t.TeacherName, b.BookingId, b.BookingDate, 
                           p.PaymentId, p.PaymentDate, p.Balance, a.DayOfWeek, dbo.Branches.BranchName, dbo.Branches.BranchLocation, a.Amount, a.IsAttendanc, a.PaymentType, a.AmountRequired, s.IStatus, 
                           s.StudentPhone, s.StudentWhatsApp, s.FatherWhatsApp
                    FROM dbo.Attendances AS a
                    INNER JOIN dbo.Bookings AS b ON a.BookingId = b.BookingId
                    INNER JOIN dbo.Groups AS g ON a.GroupId = g.GroupId
                    INNER JOIN dbo.Students AS s ON a.StudentId = s.StudentId
                    LEFT OUTER JOIN dbo.Payments AS p ON a.PaymentId = p.PaymentId
                    INNER JOIN dbo.Teachers AS t ON g.TeacherId = t.TeacherId
                    INNER JOIN dbo.Branches ON g.BranchId = dbo.Branches.BranchId;
                ");

                await dbcontext.Database.ExecuteSqlRawAsync(@"
                    -- تحقق وإعادة إنشاء VBookings
                    IF OBJECT_ID('dbo.VBookings', 'V') IS NOT NULL
                        DROP VIEW [dbo].[VBookings];
                ");

                await dbcontext.Database.ExecuteSqlRawAsync(@"
                    CREATE VIEW [dbo].[VBookings] AS
                    SELECT dbo.Bookings.BookingId, dbo.Students.StudentName, dbo.Groups.GroupName, dbo.Bookings.BookingDate, dbo.Branches.BranchName, dbo.Branches.BranchLocation, dbo.Students.Code, 
                           dbo.Students.StudentId, dbo.Groups.GroupId, dbo.Groups.StartDate, dbo.Groups.ServingPrice, dbo.Groups.Pricepermonth, dbo.Groups.NumberDayeSattendees, dbo.Students.IsMaile
                    FROM dbo.Bookings
                    LEFT OUTER JOIN dbo.Groups ON dbo.Bookings.GroupId = dbo.Groups.GroupId
                    LEFT OUTER JOIN dbo.Branches ON dbo.Groups.BranchId = dbo.Branches.BranchId
                    LEFT OUTER JOIN dbo.Students ON dbo.Bookings.StudentId = dbo.Students.StudentId;
                ");

                await dbcontext.Database.ExecuteSqlRawAsync(@"
                    -- تحقق وإعادة إنشاء VBranchs
                    IF OBJECT_ID('dbo.VBranchs', 'V') IS NOT NULL
                        DROP VIEW [dbo].[VBranchs];
                ");

                await dbcontext.Database.ExecuteSqlRawAsync(@"
                    CREATE VIEW [dbo].[VBranchs] AS
                    SELECT br.BranchID, br.BranchLocation, br.BranchName, gs.DayOfWeek, gr.GradeName, g.GroupID, g.GroupName, su.SubjectName, gs.GroupID AS Expr1, gs.StartTime, gs.EndTime, g.StartDate, 
                           g.ServingPrice, g.Pricepermonth, g.NumberDayeSattendees
                    FROM dbo.Branches AS br
                    CROSS JOIN dbo.GroupSchedules AS gs
                    LEFT OUTER JOIN dbo.Groups AS g ON gs.GroupID = g.GroupID
                    LEFT OUTER JOIN dbo.Grades AS gr ON g.GradeID = gr.GradeID
                    LEFT OUTER JOIN dbo.Subjects AS su ON g.SubjectID = su.SubjectID;
                ");

                await dbcontext.Database.ExecuteSqlRawAsync(@"
                    -- تحقق وإعادة إنشاء VExpenses
                    IF OBJECT_ID('dbo.VExpenses', 'V') IS NOT NULL
                        DROP VIEW [dbo].[VExpenses];
                ");

                await dbcontext.Database.ExecuteSqlRawAsync(@"
                    CREATE VIEW [dbo].[VExpenses] AS
                    SELECT dbo.Expenses.ExpenseId, dbo.Expenses.Description, dbo.Expenses.Amount, dbo.Expenses.Date, dbo.Expenses.Category, dbo.Expenses.Payee, dbo.AspNetUsers.UserName
                    FROM dbo.Expenses
                    INNER JOIN dbo.AspNetUsers ON dbo.Expenses.UserId = dbo.AspNetUsers.Id;
                ");

                await dbcontext.Database.ExecuteSqlRawAsync(@"
                    -- تحقق وإعادة إنشاء VAttendanceCancellation
                    IF OBJECT_ID('dbo.VAttendanceCancellation', 'V') IS NOT NULL
                        DROP VIEW [dbo].[VAttendanceCancellation];
                ");

                await dbcontext.Database.ExecuteSqlRawAsync(@"
                  CREATE VIEW [dbo].[VAttendanceCancellation]
                  AS
                  SELECT dbo.AttendanceCancellations.Id, dbo.Students.StudentName, dbo.Groups.GroupName, dbo.AttendanceCancellations.CancellationDate, dbo.AttendanceCancellations.Reason, dbo.AspNetUsers.UserName
                  FROM     dbo.AttendanceCancellations INNER JOIN
                  dbo.Students ON dbo.AttendanceCancellations.StudentId = dbo.Students.StudentId INNER JOIN
                  dbo.Groups ON dbo.AttendanceCancellations.GroupId = dbo.Groups.GroupId INNER JOIN
                  dbo.AspNetUsers ON dbo.AttendanceCancellations.UserId = dbo.AspNetUsers.Id AND dbo.Students.UserId = dbo.AspNetUsers.Id AND dbo.Students.UserId = dbo.AspNetUsers.Id AND dbo.Groups.UserId = dbo.AspNetUsers.Id AND 
                  dbo.Groups.UserId = dbo.AspNetUsers.Id
                ");

                await dbcontext.Database.ExecuteSqlRawAsync(@"
                    -- تحقق وإعادة إنشاء VGroups
                    IF OBJECT_ID('dbo.VGroups', 'V') IS NOT NULL
                        DROP VIEW [dbo].[VGroups];
                ");

                await dbcontext.Database.ExecuteSqlRawAsync(@"
                    CREATE VIEW [dbo].[VGroups] AS
                    SELECT dbo.Groups.GroupId, dbo.Groups.GroupName, dbo.Subjects.SubjectName, dbo.Teachers.TeacherName, dbo.Grades.GradeName, dbo.Groups.StartDate, dbo.Groups.ServingPrice, 
                           dbo.Groups.Pricepermonth, dbo.Groups.NumberDayeSattendees, dbo.Branches.BranchName, dbo.Branches.BranchLocation, dbo.Teachers.TeacherId, dbo.Groups.MaxLenth
                    FROM dbo.Groups
                    LEFT OUTER JOIN dbo.Teachers ON dbo.Groups.TeacherId = dbo.Teachers.TeacherId
                    LEFT OUTER JOIN dbo.Grades ON dbo.Groups.GradeId = dbo.Grades.GradeId
                    LEFT OUTER JOIN dbo.Branches ON dbo.Groups.BranchId = dbo.Branches.BranchId
                    LEFT OUTER JOIN dbo.Subjects ON dbo.Groups.SubjectId = dbo.Subjects.SubjectId
                    GROUP BY dbo.Groups.GroupId, dbo.Groups.GroupName, dbo.Subjects.SubjectName, dbo.Teachers.TeacherName, dbo.Grades.GradeName, dbo.Groups.StartDate, dbo.Groups.ServingPrice, 
                             dbo.Groups.Pricepermonth, dbo.Groups.NumberDayeSattendees, dbo.Branches.BranchName, dbo.Branches.BranchLocation, dbo.Teachers.TeacherId, dbo.Groups.MaxLenth;
                ");

                await dbcontext.Database.ExecuteSqlRawAsync(@"
                    -- تحقق وإعادة إنشاء VGroupSchedules
                    IF OBJECT_ID('dbo.VGroupSchedules', 'V') IS NOT NULL
                        DROP VIEW [dbo].[VGroupSchedules];
                ");

                await dbcontext.Database.ExecuteSqlRawAsync(@"
                    CREATE VIEW [dbo].[VGroupSchedules] AS
                    SELECT dbo.GroupSchedules.ScheduleID, dbo.Groups.GroupName, dbo.GroupSchedules.DayOfWeek, dbo.GroupSchedules.StartTime, dbo.GroupSchedules.EndTime, dbo.Groups.GroupID
                    FROM dbo.GroupSchedules
                    LEFT OUTER JOIN dbo.Groups ON dbo.GroupSchedules.GroupID = dbo.Groups.GroupID;
                ");

                await dbcontext.Database.ExecuteSqlRawAsync(@"
                    -- تحقق وإعادة إنشاء VPaymentAnalysis
                    IF OBJECT_ID('dbo.VPaymentAnalysis', 'V') IS NOT NULL
                        DROP VIEW [dbo].[VPaymentAnalysis];
                ");

                await dbcontext.Database.ExecuteSqlRawAsync(@"
                    CREATE VIEW [dbo].[VPaymentAnalysis] AS
                    SELECT dbo.Teachers.TeacherName, dbo.Groups.GroupName, dbo.Subjects.SubjectName, dbo.Grades.GradeName, dbo.Students.Code, dbo.Students.StudentName, dbo.Groups.GroupId, 
                           dbo.Students.StudentId, dbo.Teachers.TeacherId, dbo.Groups.ServingPrice, dbo.Groups.Pricepermonth, dbo.Groups.NumberDayeSattendees, dbo.Groups.MaxLenth, dbo.Payments.PaymentDate, 
                           dbo.Payments.AmountRequired, dbo.Payments.Amount, dbo.Payments.Balance, dbo.Payments.PaymentType, dbo.Branches.BranchId, dbo.Branches.BranchName, dbo.Branches.BranchLocation, 
                           dbo.Payments.UserId
                    FROM dbo.Payments
                    LEFT OUTER JOIN dbo.Groups ON dbo.Payments.GroupId = dbo.Groups.GroupId
                    LEFT OUTER JOIN dbo.Teachers ON dbo.Groups.TeacherId = dbo.Teachers.TeacherId
                    LEFT OUTER JOIN dbo.Students ON dbo.Payments.StudentId = dbo.Students.StudentId
                    LEFT OUTER JOIN dbo.Subjects ON dbo.Groups.SubjectId = dbo.Subjects.SubjectId
                    LEFT OUTER JOIN dbo.Grades ON dbo.Groups.GradeId = dbo.Grades.GradeId AND dbo.Students.GradeId = dbo.Grades.GradeId
                    LEFT OUTER JOIN dbo.Branches ON dbo.Groups.BranchId = dbo.Branches.BranchId;
                ");

                await dbcontext.Database.ExecuteSqlRawAsync(@"
                    -- تحقق وإعادة إنشاء VPayments
                    IF OBJECT_ID('dbo.VPayments', 'V') IS NOT NULL
                        DROP VIEW [dbo].[VPayments];
                ");

                await dbcontext.Database.ExecuteSqlRawAsync(@"
                    CREATE VIEW [dbo].[VPayments] AS
                    SELECT dbo.Payments.PaymentID, dbo.Students.StudentName, dbo.Students.Code, dbo.Groups.GroupName, dbo.Payments.PaymentDate, dbo.Payments.AmountRequired, dbo.Payments.Amount, 
                           dbo.Payments.Balance, dbo.Payments.PaymentType, dbo.Groups.GroupID, dbo.Students.StudentID
                    FROM dbo.Payments
                    LEFT OUTER JOIN dbo.Groups ON dbo.Payments.GroupID = dbo.Groups.GroupID
                    LEFT OUTER JOIN dbo.Students ON dbo.Payments.StudentID = dbo.Students.StudentID;
                ");

                await dbcontext.Database.ExecuteSqlRawAsync(@"
                    -- تحقق وإعادة إنشاء VStudents
                    IF OBJECT_ID('dbo.VStudents', 'V') IS NOT NULL
                        DROP VIEW [dbo].[VStudents];
                ");

                await dbcontext.Database.ExecuteSqlRawAsync(@"
                    CREATE VIEW [dbo].[VStudents]
                    AS
                    SELECT s.StudentId, s.StudentName, s.Code, s.Country, s.StudentEmail, s.StudentPhone, s.StudentWhatsApp, s.FatherWhatsApp, s.IStatus, s.GradeId, g.GradeName, s.IsMaile
                    FROM dbo.Students AS s
                    INNER JOIN dbo.Grades AS g ON s.GradeId = g.GradeId
                ");
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message.ToString());
            }
        }

    }
}
