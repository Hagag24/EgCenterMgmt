
namespace EgCenterMgmt.Services
{
    public class PaymentServices : IPaymentServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public PaymentServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<StudentBalanceDto>> GetStudentBalancesAsync(StudentBalanceRequestDto requestDto)
        {
            List<int> studentsId = requestDto.StudentIds;
            List<int> groupIds = requestDto.GroupIds;

            try
            {
                // جلب جميع الطلاب
                var students = await _unitOfWork.Students.GetAllAsync(s => studentsId.Contains(s.StudentId), null);
                if (students == null || !students.Any())
                {
                    throw new Exception("لم يتم العثور على طلاب.");
                }

                // جلب جميع المجموعات المرتبطة بالحجوزات
                var groups = await _unitOfWork.Groups.GetAllAsync(g => groupIds.Contains(g.GroupId), null);
                if (groups == null || !groups.Any())
                {
                    throw new Exception("لم يتم العثور على مجموعات.");
                }

                // جلب جميع الحضور
                var attendances = await _unitOfWork.Attendances.GetAllAsync(s => studentsId.Contains(s.StudentId) && groupIds.Contains(s.GroupId), null);
                if (attendances == null || !attendances.Any())
                {
                    throw new Exception("لم يتم العثور على سجلات الحضور.");
                }

                // إنشاء قاموس لتخزين المبالغ المطلوبة لكل مجموعة
                var groupRequirements = groups.ToDictionary(g => g.GroupId, g => g.NumberDayeSattendees);

                // حساب الرصيد المتبقي لكل طالب
                var studentBalances = students.Select(student =>
                {
                    var studentAttendances = attendances.Where(p => p.StudentId == student.StudentId).ToList();
                    var totalPaid = studentAttendances.Sum(p => p.Amount) ?? 0;

                    decimal amountRequired = 0;
                    var studentGroup = studentAttendances
                        .Select(a => a.GroupId)
                        .Distinct()
                        .Select(groupId => groups.FirstOrDefault(g => g.GroupId == groupId))
                        .FirstOrDefault();

                    if (studentGroup != null)
                    {
                        var attendanceDays = groupRequirements.GetValueOrDefault(studentGroup.GroupId, 0);

                        var monthlyAttendances = studentAttendances
                            .Where(p => p.PaymentType == "شهري" && p.AttendanceDate.HasValue)
                            .ToList();

                        var totalMonthlyAmountRequired = monthlyAttendances.Sum(p => p.AmountRequired ?? 0);
                        amountRequired += totalMonthlyAmountRequired;

                        var lessonAttendances = studentAttendances
                            .Where(p => p.PaymentType == "حصة" && p.IsAttendanc == true)
                            .ToList();

                        //var oneFalseAttendance = studentAttendances
                        //    .Where(p => p.PaymentType == "حصة" && p.IsAttendanc == false)
                        //    .FirstOrDefault();

                        //if (oneFalseAttendance != null)
                        //    lessonAttendances.Add(oneFalseAttendance);

                        var totalLessonAmountRequired = lessonAttendances.Sum(p => p.AmountRequired ?? 0);
                        amountRequired += totalLessonAmountRequired;

                        var exemptFromTeacher = studentAttendances
                            .Where(p => p.PaymentType == "معفي من المعلم" && p.AttendanceDate.HasValue)
                            .ToList();
                        var totalFreeAmountRequired = exemptFromTeacher.Sum(p => p.AmountRequired ?? 0);
                        amountRequired += totalFreeAmountRequired;
                    }

                    var remainingBalance = amountRequired - totalPaid;

                    return new StudentBalanceDto
                    {
                        StudentId = student.StudentId,
                        StudentName = student.StudentName,
                        Code = student.Code,
                        TotalPaid = totalPaid,
                        AmountRequired = amountRequired,
                        RemainingBalance = remainingBalance,
                        GroupId = studentGroup?.GroupId // تأكد من وجود مجموعة
                    };
                }).ToList();

                return studentBalances;
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return new List<StudentBalanceDto>();
            }
        }
    }

    public interface IPaymentServices
    {
        Task<List<StudentBalanceDto>> GetStudentBalancesAsync(StudentBalanceRequestDto requestDto);
    }
}
