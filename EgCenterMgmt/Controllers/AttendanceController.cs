
namespace EgCenterMgmt.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]

    public class AttendanceController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAttendanceServices _attendanceServices;

        public AttendanceController(IUnitOfWork unitOfWork, IMapper mapper,IAttendanceServices attendanceServices)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _attendanceServices = attendanceServices;
        }

        [HttpGet]
        public async Task<IActionResult> VAttendances()
        {
            var vattendances = await _unitOfWork.Vattendances.GetAllAsync();
            var sortedVAttendances = vattendances
                .OrderBy(a => a.StudentName) // ترتيب أبجدي حسب اسم الطالب
                .ThenBy(a => a.GroupId) // ترتيب إضافي حسب GroupId
                .ThenBy(a => a.IsAttendanc == false) // ترتيب حسب حالة الحضور (الغائب قبل الحاضر)
                .ToList();

            return Ok(sortedVAttendances);
        }

        [HttpGet]
        public async Task<IActionResult> VAttendancesgroup(int groupid)
        {
            try
            {
                int? numday = _unitOfWork.Groups.GetById(groupid)?.NumberDayeSattendees;
                if (numday == null)
                    return BadRequest("Number of days for attendance is not defined.");

                var students = await _unitOfWork.Bookings.CountAsync(s => s.GroupId == groupid);

                var count = students * numday.Value;

                var vattendances = await _unitOfWork.Vattendances
                    .GetAllAsync(v => v.GroupId == groupid, count);

                var filteredRecords = vattendances
                    .GroupBy(v => v.AttendanceDate) // تجميع السجلات حسب تاريخ الحضور
                    .SelectMany(dateGroup => dateGroup
                        .GroupBy(v => v.StudentName) // تجميع السجلات داخل كل تاريخ حسب اسم الطالب
                        .Select(g => g.OrderBy(v => v.AttendanceDate) // ترتيب السجلات حسب تاريخ الحضور (يجب أن تكون مرتبة بالفعل)
                                      .ThenBy(v => v.IsAttendanc == false) // ترتيب حسب حالة الحضور
                                      .FirstOrDefault()) // أخذ أول سجل في المجموعة
                        .OrderBy(v => v!.StudentName) // ترتيب أبجدي حسب اسم الطالب
                        .ToList())
                    .OrderBy(v => v!.AttendanceDate) // ترتيب حسب تاريخ الحضور من الأصغر للأكبر
                    .ToList();

                return Ok(filteredRecords);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        public async Task<IActionResult> VAttendancesstudent(int studentid)
        {
            var booking = await _unitOfWork.Bookings.FindAsync(s => s.StudentId == studentid);

            if (booking == null)
                return NotFound("Booking not found for the given student ID.");

            var vattendances = await _unitOfWork.Vattendances
                .GetAllAsync(v => v.StudentId == studentid, null);

            var sortedVAttendances = vattendances
                .OrderByDescending(v => v.AttendanceDate)
                .ToList();

            return Ok(sortedVAttendances);
        }


        [HttpGet]
        public async Task<IActionResult> VAttendancetech(int techid)
        {
            var groups = await _unitOfWork.Groups.GetAllAsync(g => g.TeacherId == techid, null);
            List<int> attendanceCounts = new List<int>();

            foreach (var group in groups)
            {
                int numday = group.NumberDayeSattendees ?? 0;

                var totalBookings = await _unitOfWork.Bookings.CountAsync(b => b.GroupId == group.GroupId);

                int attendanceCount = totalBookings * numday;
                attendanceCounts.Add(attendanceCount);
            }
            int totalAttendance = attendanceCounts.Sum();

            var vattendances = await _unitOfWork.Vattendances
                .GetAllAsync(v => v.TeacherId == techid, totalAttendance);

            var sortedVAttendances = vattendances
                .OrderByDescending(v => v.AttendanceDate)
                .ToList();

            return Ok(sortedVAttendances);
        }


        [HttpGet]
        public async Task<IActionResult> reportAttendancesgroup(int groupid, DateOnly fromDate, DateOnly toDate)
        {
            var vattendances = await _unitOfWork.Vattendances
                .GetAllAsync(v => v.GroupId == groupid && v.AttendanceDate >= fromDate && v.AttendanceDate <= toDate, null);

            List<Totalattendance> totalAttendanceList = await _attendanceServices.GenerateTotalAttendanceReportAsync(vattendances);
            return Ok(totalAttendanceList);
        }

        [HttpGet]
        public async Task<IActionResult> reportAttendancesstudent(int studentid, DateOnly fromDate, DateOnly toDate)
        {
            // تحويل DateOnly إلى DateTime للتصفية
            var vattendances = await _unitOfWork.Vattendances
                .GetAllAsync(v => v.StudentId == studentid && v.AttendanceDate >= fromDate && v.AttendanceDate <= toDate, null);

            if (!vattendances.Any())
                return NotFound("No attendance records found for the given student ID.");

            List<Totalattendance> totalAttendanceList = await _attendanceServices.GenerateTotalAttendanceReportAsync(vattendances);
            return Ok(totalAttendanceList);
        }

        [HttpGet]
        public async Task<IActionResult> reportAttendancetech(int techid, DateOnly fromDate, DateOnly toDate)
        {

            var vattendances = await _unitOfWork.Vattendances
                .GetAllAsync(v => v.TeacherId == techid && v.AttendanceDate >= fromDate && v.AttendanceDate <= toDate, null);
            List<Totalattendance> totalAttendanceList = await _attendanceServices.GenerateTotalAttendanceReportAsync(vattendances);


            return Ok(totalAttendanceList);
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> PutAttendanceCancellation(int id, AttendanceDto attendanceDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != attendanceDto.AttendanceId)
            {
                return BadRequest();
            }

            var Attendance = _mapper.Map<Attendance>(attendanceDto);
            _unitOfWork.Attendances.Update(Attendance);
            await _unitOfWork.Complete();

            attendanceDto = _mapper.Map<AttendanceDto>(Attendance);
            return Ok(attendanceDto);
        }

        [HttpGet]
        public async Task<IActionResult> FirestAttendancTrue(int GroupId)
        {
            try
            {
                var attendancesUpdate = await _unitOfWork.Attendances.FindAllAsync(a =>
                    a.GroupId == GroupId &&
                    a.AttendanceDate < DateOnly.FromDateTime(DateTime.Now) && a.IsAttendanc == false);

                if (attendancesUpdate != null)
                {
                    foreach (var attendance in attendancesUpdate)
                    {
                        attendance.IsAttendanc = true;
                        _unitOfWork.Attendances.Update(attendance);
                        await _unitOfWork.Complete();
                    }
                }
                return Ok();
            }
            catch (Exception ex) 
            {
                return BadRequest(error: ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> PutAttendance([FromQuery] int id, [FromBody] AttendanceDto attendanceDto)
        {
            if (id != attendanceDto.AttendanceId)
            {
                return BadRequest();
            }

            try
            {
                var existingAttendance = await _unitOfWork.Attendances.GetByIdAsync(id);

                if (existingAttendance == null)
                {
                    return NotFound();
                }



                if (attendanceDto.PaymentType == "شهري")
                {
                    var attendances = await _unitOfWork.Attendances
                        .FindAllAsync(a => a.BookingId == attendanceDto.BookingId && a.GroupId == attendanceDto.GroupId && a.StudentId == attendanceDto.StudentId && a.AttendanceDate > DateOnly.FromDateTime(DateTime.Now));

                    var lastNineAttendances = attendances
                        .OrderByDescending(a => a.AttendanceDate)
                        .Take(9)
                        .ToList();

                    decimal? totalAmount = lastNineAttendances.Sum(a => a.Amount ?? 0) + attendanceDto.Amount;

                    if (totalAmount >= attendanceDto.AmountRequired && attendanceDto.PaymentType == "شهري")
                    {
                        foreach (var attendance in lastNineAttendances)
                        {
                            attendance.PaymentId = attendanceDto.PaymentId;
                            _unitOfWork.Attendances.Update(attendance);
                        }
                    }
                }
                _mapper.Map(attendanceDto, existingAttendance);
                _unitOfWork.Attendances.Update(existingAttendance);
                await _unitOfWork.Complete();
                var Attendance = await _unitOfWork.Attendances.FindAllAsync(e => e.IsAttendanc == false);
                if (Attendance == null)
                {
                    CreatedAtAction(nameof(putAttendanceMonthOrSession), attendanceDto);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return NoContent();
        }



        [HttpPut]
        public async Task<IActionResult> putAttendanceMonthOrSession(AttendanceDto AttendanceDto)
        {
            var booking = await _unitOfWork.Bookings.FindAsync(b=>b.BookingId == AttendanceDto.BookingId);
            var RegesterBookingDto = _mapper.Map<RegesterBookingDto>(booking);
            RegesterBookingDto.PaymentType = AttendanceDto.PaymentType;
            var regesterBookingDto = await _attendanceServices.AddBookingAndAttances(RegesterBookingDto);

            return Ok(AttendanceDto);

        }

        // DELETE: api/Attendance/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAttendance(int id)
        {
            var Attendance = await _unitOfWork.Attendances.GetByIdAsync(id);

            if (Attendance == null)
            {
                return NotFound();
            }

            _unitOfWork.Attendances.Delete(Attendance);
            await _unitOfWork.Complete();

            return Ok(Attendance);
        }
    }

}
