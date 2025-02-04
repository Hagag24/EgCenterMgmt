
namespace EgCenterMgmt.Services
{
    public class AttendanceServices : IAttendanceServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPaymentServices _paymentServices;
        public AttendanceServices(IUnitOfWork unitOfWork, IMapper mapper, IPaymentServices paymentServices)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _paymentServices = paymentServices;
        }
        public async Task<RegesterBookingDto> AddBookingAndAttances(RegesterBookingDto RegesterBookingDto)
        {
            try
            {
                var group = await _unitOfWork.Groups.FindAsync(b => b.GroupId == RegesterBookingDto.GroupId);
                var Schadules = await _unitOfWork.GroupSchedules.FindAllAsync(b => b.GroupId == RegesterBookingDto.GroupId);

                List<GroupSchedule> Schedules = _mapper.Map<List<GroupSchedule>>(Schadules);

                List<DateTime> attendanceDates = new List<DateTime>();

                int totalAttendanceDays = group.NumberDayeSattendees!.Value;
                DateTime startDate = group.StartDate.ToDateTime(TimeOnly.MinValue);
                DateTime now = DateTime.Now;

                if (startDate <= now)
                {
                    // تقليل التاريخ حتى يتطابق اليوم (Day)
                    while (startDate.Day != now.Day)
                    {
                        now = now.AddDays(-1);
                    }
                    startDate = now;

                }


                List<DateTime> missedDays = new List<DateTime>();
                foreach (var schedule in Schedules)
                {
                    DateTime currentDate = startDate;
                    while (currentDate <= DateTime.Now)
                    {
                        if (currentDate.DayOfWeek == DayOfWeekHelper.TranslateToDayOfWeek(schedule.DayOfWeek!) &&
                            !attendanceDates.Contains(currentDate))
                        {
                            if (currentDate.Date < DateTime.Now.Date ||
                                (currentDate.Date == DateTime.Now.Date && TimeOnly.FromDateTime(DateTime.Now) > schedule.EndTime))
                            {
                                missedDays.Add(currentDate);
                            }
                        }
                        currentDate = currentDate.AddDays(1);
                    }
                }



                // حساب عدد الأيام الفائتة وتحديد الخصم
                int daysMissed = missedDays.Count;
                decimal discountAmount = daysMissed * group.ServingPrice!.Value;
                decimal totalAmountDue = group.Pricepermonth!.Value - discountAmount;

                // إعداد قائمة الحضور لتشمل الأيام الفائتة
                attendanceDates.AddRange(missedDays);

                // إضافة الأيام المتبقية بعد الأيام الفائتة
                int remainingAttendanceDays = totalAttendanceDays - daysMissed;
                DateTime nextAttendanceDate = DateTime.Now;
                Schedules = Schedules.OrderBy(schedule =>
                {
                    DateTime nextOccurrence = nextAttendanceDate; // بداية من التاريخ الحالي
                    while (nextOccurrence.DayOfWeek != DayOfWeekHelper.TranslateToDayOfWeek(schedule.DayOfWeek!))
                    {
                        nextOccurrence = nextOccurrence.AddDays(1);
                    }
                    return (nextOccurrence - DateTime.Now).Duration(); // المسافة الزمنية بين التاريخ الحالي وتاريخ الحصة
                }).ToList();
                while (remainingAttendanceDays > 0)
                {
                    foreach (var schedule in Schedules)
                    {
                        // اجعل التاريخ يبدأ من اليوم الحالي ثم يضيف الأيام بناءً على الجدول
                        while (nextAttendanceDate.DayOfWeek != DayOfWeekHelper.TranslateToDayOfWeek(schedule.DayOfWeek!))
                        {
                            nextAttendanceDate = nextAttendanceDate.AddDays(1);
                        }

                        if (!attendanceDates.Contains(nextAttendanceDate))
                        {
                            attendanceDates.Add(nextAttendanceDate);
                            remainingAttendanceDays--;
                        }

                        if (remainingAttendanceDays <= 0)
                            break;

                        nextAttendanceDate = nextAttendanceDate.AddDays(1);
                    }
                }


                if (attendanceDates.Count > 0 && attendanceDates.Count == missedDays.Count)
                {
                    attendanceDates = CalculateFutureAttendanceDates(Schedules, totalAttendanceDays);
                    daysMissed = 0;
                }
                attendanceDates.Sort();

                int count = 0;
                for (int i = 0; i < attendanceDates.Count; i++)
                {
                    var Attendance = new Attendance
                    {
                        AttendanceStatus = false,
                        AttendanceDate = DateOnly.FromDateTime(attendanceDates[i].Date),
                        DayOfWeek = DayOfWeekHelper.dayOfWeekTranslations[attendanceDates[i].DayOfWeek],
                        PaymentId = null,
                        IsAttendanc = false,
                        PaymentType = RegesterBookingDto.PaymentType,
                        StudentId = Convert.ToInt32(RegesterBookingDto?.StudentId),
                        GroupId = Convert.ToInt32(RegesterBookingDto?.GroupId),
                        BookingId = RegesterBookingDto!.BookingId,
                    };
                    if (daysMissed == 0)
                    {
                        if (Attendance.PaymentType == "حصة")
                        {
                            Attendance.AmountRequired = group.ServingPrice;
                        }
                        else if (Attendance.PaymentType == "شهري" && count == 0)
                        {
                            Attendance.AmountRequired = totalAmountDue;
                            count++;
                        }
                        else if (RegesterBookingDto.PaymentType == "معفي")
                        {
                            Attendance.AmountRequired = null;
                        }
                        else if (RegesterBookingDto.PaymentType == "معفي من المعلم" && count == 0)
                        {
                            var requir = await _unitOfWork.CenterRates.FindAsync(a => a.Rate == group.Pricepermonth);
                            Attendance.AmountRequired = requir.RateMonthly;
                            count++;
                        }

                    }
                    else
                    {
                        Attendance.AmountRequired = null;
                        Attendance.IsAttendanc = true;
                        daysMissed -= 1;
                    }
                    _unitOfWork.Attendances.Add(Attendance);

                }

                await _unitOfWork.Complete();
                return RegesterBookingDto;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return new RegesterBookingDto { BookingId = 0 };
            }
        }
        public async Task<bool> UpdateBookingAndAttances(GroupSchedule existingGroupSchedule)
        {
            try
            {
                var group = await _unitOfWork.Groups.FindAsync(b => b.GroupId == existingGroupSchedule.GroupId);
                var Schadules = await _unitOfWork.GroupSchedules.FindAllAsync(b => b.GroupId == existingGroupSchedule.GroupId);

                List<GroupSchedule> Schedules = _mapper.Map<List<GroupSchedule>>(Schadules);

                List<DateTime> attendanceDates = new List<DateTime>();

                int totalAttendanceDays = group.NumberDayeSattendees!.Value;

                DateTime startDate = group.StartDate.ToDateTime(TimeOnly.MinValue);
                DateTime now = DateTime.Now;

                if (startDate <= now)
                {
                    while (startDate.Day != now.Day)
                    {
                        now = now.AddDays(-1);
                    }
                    startDate = now;
                }

                List<DateTime> missedDays = new List<DateTime>();
                foreach (var schedule in Schedules)
                {
                    DateTime currentDate = startDate;
                    while (currentDate <= DateTime.Now)
                    {
                        if (currentDate.DayOfWeek == DayOfWeekHelper.TranslateToDayOfWeek(schedule.DayOfWeek!) &&
                            !attendanceDates.Contains(currentDate))
                        {
                            if (currentDate.Date < DateTime.Now.Date ||
                                (currentDate.Date == DateTime.Now.Date && TimeOnly.FromDateTime(DateTime.Now) > schedule.EndTime))
                            {
                                missedDays.Add(currentDate);
                            }
                        }
                        currentDate = currentDate.AddDays(1);
                    }
                }

                // حساب عدد الأيام الفائتة وتحديد الخصم
                int daysMissed = missedDays.Count;
                decimal discountAmount = daysMissed * group.ServingPrice!.Value;
                decimal totalAmountDue = group.Pricepermonth!.Value - discountAmount;

                // إعداد قائمة الحضور لتشمل الأيام الفائتة
                attendanceDates.AddRange(missedDays);

                // إضافة الأيام المتبقية بعد الأيام الفائتة
                int remainingAttendanceDays = totalAttendanceDays - daysMissed;
                DateTime nextAttendanceDate = DateTime.Now;
                while (remainingAttendanceDays > 0)
                {
                    foreach (var schedule in Schedules)
                    {
                        // اجعل التاريخ يبدأ من اليوم الحالي ثم يضيف الأيام بناءً على الجدول
                        while (nextAttendanceDate.DayOfWeek != DayOfWeekHelper.TranslateToDayOfWeek(schedule.DayOfWeek!))
                        {
                            nextAttendanceDate = nextAttendanceDate.AddDays(1);
                        }

                        if (!attendanceDates.Contains(nextAttendanceDate))
                        {
                            attendanceDates.Add(nextAttendanceDate);
                            remainingAttendanceDays--;
                        }

                        if (remainingAttendanceDays <= 0)
                            break;

                        nextAttendanceDate = nextAttendanceDate.AddDays(1);
                    }
                }

                if (attendanceDates.Count > 0 && attendanceDates.Count == missedDays.Count)
                {
                    attendanceDates = CalculateFutureAttendanceDates(Schedules, totalAttendanceDays);
                    daysMissed = 0;
                }
                attendanceDates.Sort();

                var attendanceRecords = await _unitOfWork.Attendances.FindAllAsync(a => a.GroupId == existingGroupSchedule.GroupId && a.IsAttendanc == false);

                if (attendanceRecords == null || !attendanceRecords.Any())
                {
                    throw new InvalidOperationException("لم يتم العثور على سجلات الحضور للمجموعة المحددة.");
                }

                var attends = _unitOfWork.Bookings.GetAllAsync(s => s.GroupId == group.GroupId, null).Result;
                foreach (var item in attends)
                {
                    int len = 0;
                    foreach (var attedance in attendanceRecords.Where(a => a.StudentId == item.StudentId))
                    {
                        attedance.AttendanceDate = DateOnly.FromDateTime(attendanceDates[len].Date);
                        attedance.DayOfWeek = DayOfWeekHelper.dayOfWeekTranslations[attendanceDates[len].DayOfWeek];
                        _unitOfWork.Attendances.Update(attedance);
                        len++;
                    }

                }

                await _unitOfWork.Complete();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error : {ex}");
                return false;
            }
        }

        private List<DateTime> CalculateFutureAttendanceDates(List<GroupSchedule> schedules, int totalAttendanceDays)
        {
            List<DateTime> attendanceDates = new List<DateTime>();
            DateTime nextAttendanceDate = DateTime.Now;

            // البحث عن أول تاريخ صالح في المستقبل وفقًا لجدول الحصص:
            bool validDateFound = false;
            while (!validDateFound)
            {
                foreach (var schedule in schedules)
                {
                    // إذا كان يوم التاريخ الحالي يتوافق مع اليوم المحدد في الجدول
                    if (nextAttendanceDate.DayOfWeek == DayOfWeekHelper.TranslateToDayOfWeek(schedule.DayOfWeek!))
                    {
                        // نقبل التاريخ إذا كان لاحقاً لتاريخ اليوم أو إذا كان اليوم الحالي وما زال وقت الحصة لم ينتهِ
                        if (nextAttendanceDate.Date > DateTime.Now.Date ||
                           (nextAttendanceDate.Date == DateTime.Now.Date && TimeOnly.FromDateTime(DateTime.Now) <= schedule.EndTime))
                        {
                            validDateFound = true;
                            break;
                        }
                    }
                }
                if (!validDateFound)
                {
                    nextAttendanceDate = nextAttendanceDate.AddDays(1);
                }
            }

            // توليد العدد المطلوب من أيام الحضور بدءًا من nextAttendanceDate:
            int remainingAttendanceDays = totalAttendanceDays;
            while (remainingAttendanceDays > 0)
            {
                // ترتيب الجداول بحيث يتم حساب أقرب تاريخ مطابق من nextAttendanceDate لكل جدول
                foreach (var schedule in schedules.OrderBy(s =>
                {
                    DateTime nextOccurrence = nextAttendanceDate;
                    while (nextOccurrence.DayOfWeek != DayOfWeekHelper.TranslateToDayOfWeek(s.DayOfWeek!))
                    {
                        nextOccurrence = nextOccurrence.AddDays(1);
                    }
                    return (nextOccurrence - DateTime.Now).Duration();
                }))
                {
                    // تعديل nextAttendanceDate ليطابق اليوم المحدد في الجدول
                    while (nextAttendanceDate.DayOfWeek != DayOfWeekHelper.TranslateToDayOfWeek(schedule.DayOfWeek!))
                    {
                        nextAttendanceDate = nextAttendanceDate.AddDays(1);
                    }
                    if (!attendanceDates.Contains(nextAttendanceDate))
                    {
                        attendanceDates.Add(nextAttendanceDate);
                        remainingAttendanceDays--;
                    }
                    if (remainingAttendanceDays <= 0)
                        break;
                    // الانتقال إلى اليوم التالي للبحث عن تاريخ مطابق آخر
                    nextAttendanceDate = nextAttendanceDate.AddDays(1);
                }
            }

            return attendanceDates;
        }
        public async Task<List<Totalattendance>> GenerateTotalAttendanceReportAsync(IEnumerable<Vattendance> records)
        {
            try
            {
                var studentNumbers = records.Select(r => r.StudentId).Distinct().ToList();
                var groupNumbers = records.Select(r => r.GroupId).Distinct().ToList();

                var requestDto = new StudentBalanceRequestDto
                {
                    StudentIds = studentNumbers,
                    GroupIds = groupNumbers
                };

                // استدعاء خدمة الدفع للحصول على أرصدة الطلاب
                var studentBalances = await _paymentServices.GetStudentBalancesAsync(requestDto);

                var total = records
                    .GroupBy(r => new { r.Code, r.StudentName })
                    .Select(g => new Totalattendance
                    {
                        StudentCode = g.Key.Code,
                        StudentName = g.Key.StudentName,
                        TotalAttendance = g.Count(a => a.IsAttendanc == true && a.AttendanceStatus == true),
                        TotalAbsence = g.Count(a => a.IsAttendanc == true && a.AttendanceStatus == false),
                        AttendanceDates = string.Join("> ", g.Where(a => a.IsAttendanc == true && a.AttendanceStatus == true).Select(a => a.DayOfWeek)),
                        AbsenceDates = string.Join("> ", g.Where(a => a.IsAttendanc == true && a.AttendanceStatus == false).Select(a => a.DayOfWeek)),
                        GroupName = string.Join("> ", g.Select(a => a.GroupName).Distinct()),
                        TeacherName = string.Join("> ", g.Select(a => a.TeacherName).Distinct()),

                        // إضافة معلومات الرصيد
                        TotalAmount = studentBalances.FirstOrDefault(sb => sb.Code == g.Key.Code)?.TotalPaid ?? 0,
                        TotalBalance = studentBalances.FirstOrDefault(sb => sb.Code == g.Key.Code)?.RemainingBalance ?? 0,
                        IStatus = g.All(a => a.IStatus), // إذا كان جميع الطلاب ملتزمين
                        FatherWhatsApp = g.Select(a => a.FatherWhatsApp).FirstOrDefault(), // الحصول على رقم هاتف والد الطالب
                        StudentPhone = g.Select(a => a.StudentPhone).FirstOrDefault() // الحصول على رقم هاتف الطالب
                    }).ToList();
                return total;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());

                return new List<Totalattendance>();
            }
        }
    }
}
