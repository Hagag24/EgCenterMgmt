
namespace EgCenterMgmt.Controllers
{
    [ApiController]
    [Route("api/[controller]/[Action]")]
    public class AnalyticsController : ControllerBase
    {
        private HttpContext? _httpContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        public AnalyticsController(IUnitOfWork unitOfWork,UserManager<ApplicationUser> userManager, IHttpContextAccessor contextAccessor)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _httpContext = contextAccessor.HttpContext;
            
        }

        [HttpGet]
        public async Task<IActionResult> VPaymentAnalysisgroup(int groupid, DateTime fromDate, DateTime toDate)
        {
            // تحقق من صحة التواريخ (إذا كانت غير مُدخلة أو قيمتها افتراضية)
            if (fromDate == DateTime.MinValue || toDate == DateTime.MinValue)
            {
                return BadRequest("يجب إدخال التاريخين من والى");
            }

            // تحقق من أن fromDate ليس بعد toDate
            if (fromDate > toDate)
            {
                return BadRequest("تاريخ البداية يجب أن يكون قبل تاريخ النهاية");
            }

            var vattendances = await _unitOfWork.VPaymentAnalysis
                .GetAllAsync(v => v.GroupId == groupid && v.PaymentDate >= fromDate && v.PaymentDate <= toDate, null);
            if (!vattendances.Any())
                return NotFound("لا توجد سجلات لهذه المجموعة");
            return Ok(vattendances);
        }

        [HttpGet]
        public async Task<IActionResult> VPaymentAnalysisstudent(int studentid, DateTime fromDate, DateTime toDate)
        {
            // تحقق من صحة التواريخ (إذا كانت غير مُدخلة أو قيمتها افتراضية)
            if (fromDate == DateTime.MinValue || toDate == DateTime.MinValue)
            {
                return BadRequest("يجب إدخال التاريخين من والى");
            }

            // تحقق من أن fromDate ليس بعد toDate
            if (fromDate > toDate)
            {
                return BadRequest("تاريخ البداية يجب أن يكون قبل تاريخ النهاية");
            }
            var vattendances = await _unitOfWork.VPaymentAnalysis
                .GetAllAsync(v => v.StudentId == studentid && v.PaymentDate >= fromDate && v.PaymentDate <= toDate, null);
            if (!vattendances.Any())
                return NotFound("لا توجد سجلات لهذا الطتالب");
            return Ok(vattendances);
        }

        [HttpGet]
        public async Task<IActionResult> VPaymentAnalysistech(int techid, DateTime fromDate, DateTime toDate)
        {

            try
            {
                // تحقق من صحة التواريخ (إذا كانت غير مُدخلة أو قيمتها افتراضية)
                if (fromDate == DateTime.MinValue || toDate == DateTime.MinValue)
                {
                    return BadRequest("يجب إدخال التاريخين من والى");
                }

                // تحقق من أن fromDate ليس بعد toDate
                if (fromDate > toDate)
                {
                    return BadRequest("تاريخ البداية يجب أن يكون قبل تاريخ النهاية");
                }

                var vattendances = await _unitOfWork.VPaymentAnalysis
               .GetAllAsync(v => v.TeacherId == techid && v.PaymentDate >= fromDate && v.PaymentDate <= toDate, null);
                if (!vattendances.Any())
                    return NotFound("لا توجد سجلات لهذا المعلم");
                return Ok(vattendances);
            }catch (Exception ex)
            {
                return BadRequest(error: ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAdminStatistics()
        {
            try
            {
                var Roles = User?.FindAll(ClaimTypes.Role).ToList();
                var userid = User?.FindAll(ClaimTypes.NameIdentifier).ToString();



                var teachers = await _unitOfWork.Teachers.GetAllAsync();
                var subjects = await _unitOfWork.Subjects.GetAllAsync();
                var students = await _unitOfWork.Students.GetAllAsync();
                var payments = await _unitOfWork.Payments.GetAllAsync();
                var groupSchedules = await _unitOfWork.GroupSchedules.GetAllAsync();
                var grades = await _unitOfWork.Grades.GetAllAsync();
                var groups = await _unitOfWork.Groups.GetAllAsync();
                var vAttendances = await _unitOfWork.Attendances.GetAllAsync();
                var branches = await _unitOfWork.Branches.GetAllAsync();
                var bookings = await _unitOfWork.Bookings.GetAllAsync();

                var amount = Convert.ToInt32(payments.Sum(e => e.Amount));
                var amountrequir = Convert.ToInt32(payments.Sum(e => e.AmountRequired));
                var statistics = new Dictionary<string, int>
                {
                    { "عدد المدرسين", teachers.Count() },
                    { "عدد المواد", subjects.Count() },
                    { "عدد الطلاب", students.Count() },
                    { "عدد المدفوعات", payments.Count() },
                    { "مبلغ المدفوعات", Convert.ToInt32(payments.Sum(e => e.Amount)) },
                    { "مبلغ الديون الخارجية ", amountrequir - amount },
                    { "عدد المدفوعات بالشهر", payments.Count(s => s.PaymentType == "شهري") },
                    { "عدد المدفوعات بالحصة", payments.Count(s => s.PaymentType == "حصة") },
                    { "عدد جداول المجموعات", groupSchedules.Count() },
                    { "عدد الصفوف", grades.Count() },
                    { "عدد المجموعات", groups.Count() },
                    { "عدد الحضور", vAttendances.Count(s => s.IsAttendanc == true && s.AttendanceStatus == true) },
                    { "عدد الغياب", vAttendances.Count(s => s.IsAttendanc == true && s.AttendanceStatus == false) },
                    { "عدد الفروع", branches.Count() },
                    { "عدد الحجوزات", bookings.Count() }
                };

                return Ok(statistics);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAssistantStatistics([FromQuery] Guid userId)
        {
            try
            {

                var today = DateOnly.FromDateTime(DateTime.Today);
                var startDate = today.AddMonths(-1);


                var teachers = await _unitOfWork.Teachers.GetAllAsync();
                var subjects = await _unitOfWork.Subjects.GetAllAsync();
                var students = await _unitOfWork.Students.GetAllAsync();
                var payments = await _unitOfWork.Payments.GetAllAsync(a => a.PaymentDate >= startDate && a.PaymentDate <= today, null);
                var groupSchedules = await _unitOfWork.GroupSchedules.GetAllAsync();
                var grades = await _unitOfWork.Grades.GetAllAsync();
                var groups = await _unitOfWork.Groups.GetAllAsync();
                var vAttendances = await _unitOfWork.Attendances.GetAllAsync(a => a.AttendanceDate >= startDate && a.AttendanceDate <= today, null);
                var branches = await _unitOfWork.Branches.GetAllAsync();
                var bookings = await _unitOfWork.Bookings.GetAllAsync(a => a.BookingDate >= startDate && a.BookingDate <= today, null);

                var statistics = new Dictionary<string, int>
                {
                    { "عدد المدرسين", teachers.Count(e => e.CreatedDate == DateTime.Today && e.UserId == userId) },
                    { "عدد المواد", subjects.Count(e => e.CreatedDate == DateTime.Today && e.UserId == userId) },
                    { "عدد الطلاب", students.Count(e => e.CreatedDate == DateTime.Today && e.UserId == userId) },
                    { "عدد المدفوعات", payments.Count(e => e.PaymentDate == today && e.UserId == userId) },
                    { "مبلغ المدفوعات", Convert.ToInt32(payments.Where(e => e.PaymentDate == today && e.UserId == userId).Sum(e => e.Amount)) },
                    { "مبلغ الديون الخارجية ", Convert.ToInt32(payments.Where(e => e.PaymentDate == today && e.UserId == userId).Sum(e => e.Balance)) },
                    { "عدد المدفوعات بالشهر", payments.Count(e => e.PaymentType == "شهري" && e.PaymentDate == today && e.UserId == userId) },
                    { "عدد المدفوعات بالحصة", payments.Count(e => e.PaymentType == "حصة" && e.PaymentDate == today && e.UserId == userId) },
                    { "عدد جداول المجموعات", groupSchedules.Count(e => e.CreatedDate == DateTime.Today && e.UserId == userId) },
                    { "عدد الصفوف", grades.Count(e => e.CreatedDate == DateTime.Today && e.UserId == userId) },
                    { "عدد المجموعات", groups.Count(e => e.CreatedDate == DateTime.Today && e.UserId == userId) },
                    { "عدد الحضور", vAttendances.Count(e => e.IsAttendanc == true && e.AttendanceStatus == true && e.AttendanceDate == today && e.UserId == userId) },
                    { "عدد الغياب", vAttendances.Count(e => e.IsAttendanc == true && e.AttendanceStatus == false && e.AttendanceDate == today && e.UserId == userId) },
                    { "عدد الفروع", branches.Count(e => e.CreatedDate == DateTime.Today && e.UserId == userId) },
                    { "عدد الحجوزات", bookings.Count(e => e.CreatedDate == DateTime.Today && e.UserId == userId) }
                };

                return Ok(statistics);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
