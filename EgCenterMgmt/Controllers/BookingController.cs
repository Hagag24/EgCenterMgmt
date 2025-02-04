
namespace EgCenterMgmt.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]

    public class BookingController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAttendanceServices _attendanceServices;

        public BookingController(IUnitOfWork unitOfWork, IMapper mapper, IAttendanceServices attendanceServices)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _attendanceServices = attendanceServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetBookings()
        {
            var Bookings = await _unitOfWork.Bookings.GetAllAsync();
            var BookingDtos = _mapper.Map<IEnumerable<BookingDto>>(Bookings);
            return Ok(BookingDtos);
        }

        [HttpGet]
        public async Task<IActionResult> GetBooking(int id)
        {
            var Booking = await _unitOfWork.Bookings.GetByIdAsync(id);

            if (Booking == null)
            {
                return NotFound("لا يوجد حجز ");
            }

            var BookingDto = _mapper.Map<BookingDto>(Booking);
            return Ok(BookingDto);
        }

        [HttpPost]
        public async Task<IActionResult> PostBooking(RegesterBookingDto BookingDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var existingBooking = await _unitOfWork.Bookings.FindAsync(b => b.StudentId == BookingDto.StudentId && b.GroupId == BookingDto.GroupId && b.BookingDate == BookingDto.BookingDate);

             if (existingBooking != null)
            {
                return Conflict("يوجد حجز مسبق بنفس التفاصيل.");
            }
            // الحصول على تفاصيل المجموعة الجديدة
            var newGroup = await _unitOfWork.Groups.FindAsync(g => g.GroupId == BookingDto.GroupId);
            if (newGroup == null)
            {
                return NotFound("لم يتم العثور على المجموعة المطلوبة.");
            }

            // الحصول على تفاصيل المجموعة الجديدة
            var CenterRates = await _unitOfWork.CenterRates.FindAsync(g => g.Rate == newGroup.Pricepermonth);
            if (CenterRates == null)
            {
                return NotFound("لم يتم العثور على نسبة السنتر.");
            }

            var GroupSchedules = await _unitOfWork.GroupSchedules.FindAllAsync(g => g.GroupId == BookingDto.GroupId);
            if (GroupSchedules == null)
            {
                return NotFound("لم يتم العثور على مواعيد لهذه المجموعة المطلوبة قم باذافة جداول  مواعيد للمجموعة.");
            }
            // الحصول على عدد الطلاب الحاليين في المجموعة
            var currentStudentCount = await _unitOfWork.Bookings
                .CountAsync(b => b.GroupId == BookingDto.GroupId);

            // التحقق مما إذا كانت المجموعة قد وصلت إلى الحد الأقصى من الطلاب
            if (currentStudentCount >= newGroup.MaxLenth)
            {
                return Conflict("المجموعة وصلت إلى الحد الأقصى من عدد الطلاب.");
            }

            var existingBookingWithSameTeacherOrSubjectOrGroup = await _unitOfWork.Bookings
                .FindAsync(b => b.StudentId == BookingDto.StudentId &&
                                (b.Group!.TeacherId == newGroup.TeacherId || b.Group.SubjectId == newGroup.SubjectId || b.GroupId == newGroup.GroupId));

            if (existingBookingWithSameTeacherOrSubjectOrGroup != null)
            {
                return Conflict("لا يمكن للطالب الحجز مع نفس المعلم أو المادة أو المجموعة.");
            }
            try
            {
                var Booking = _mapper.Map<Booking>(BookingDto);
                _unitOfWork.Bookings.Add(Booking);
                await _unitOfWork.Complete();
                BookingDto.BookingId = Booking.BookingId;
                try
                {
                    var regesterBookingDto = await _attendanceServices.AddBookingAndAttances(BookingDto);
                    if(regesterBookingDto != null && regesterBookingDto.BookingId != 0)
                    {
                        regesterBookingDto = _mapper.Map<RegesterBookingDto>(Booking);
                        return CreatedAtAction(nameof(GetBooking), new { id = Booking.BookingId }, regesterBookingDto);
                    }
                    else
                    {
                        _unitOfWork.Bookings.Delete(Booking);
                        await _unitOfWork.Complete();
                        return NotFound("حدث خطأ أثناء إضافة الحضور. تم حذف الحجز. الخطأ: ");
                    }
                }
                catch (Exception ex)
                {
                    CreatedAtAction(nameof(GetBooking), BookingDto.BookingId);
                    _unitOfWork.Bookings.Delete(Booking);
                    await _unitOfWork.Complete();
                    return BadRequest("حدث خطأ أثناء إضافة الحضور. تم حذف الحجز. الخطأ: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }


        // PUT: api/Booking/5
        [HttpPut]
        public async Task<IActionResult> PutBooking(int id, RegesterBookingDto BookingDto)
        {
            if (id != BookingDto.BookingId)
            {
                return BadRequest();
            }

            var booking = await _unitOfWork.Bookings.FindAsync(b => b.BookingId == id);
            if (booking == null)
            {
                return NotFound("لم يتم العثور على الحجز المطلوب.");
            }
            var GroupSchedules = await _unitOfWork.GroupSchedules.FindAllAsync(g => g.GroupId == BookingDto.GroupId);
            if (GroupSchedules == null)
            {
                return NotFound("لم يتم العثور على مواعيد لهذه المجموعة المطلوبة قم باذافة جداول  مواعيد للمجموعة.");
            }
            var newGroup = await _unitOfWork.Groups.FindAsync(g => g.GroupId == booking.GroupId);           // الحصول على تفاصيل المجموعة الجديدة
            var CenterRates = await _unitOfWork.CenterRates.FindAsync(g => g.Rate == newGroup.Pricepermonth);
            if (CenterRates == null)
            {
                return NotFound("لم يتم العثور على نسبة السنتر.");
            }

            var attendances = await _unitOfWork.Attendances
            .FindAllAsync(a => a.BookingId == id && a.GroupId == booking.GroupId && a.StudentId == booking.StudentId
                && a.AttendanceDate >= DateOnly.FromDateTime(DateTime.Now.AddDays(-30))
                && a.AttendanceDate <= DateOnly.FromDateTime(DateTime.Now.AddDays(30)));
            var group = await _unitOfWork.Groups.FindAsync(b => b.GroupId == booking.GroupId);
            int num = Convert.ToInt32(group.NumberDayeSattendees);
            var lastNineAttendances = attendances
                .OrderByDescending(a => a.AttendanceDate)
                .Take(num)
                .ToList();

            var paymentIdsToUpdate = new HashSet<int>();

            foreach (var attendance in lastNineAttendances)
            {
                if (attendance.PaymentId.HasValue)
                {
                    paymentIdsToUpdate.Add(attendance.PaymentId.Value);
                }
            }

            if (paymentIdsToUpdate.Any())
            {
                var paymentsToUpdate = await _unitOfWork.Payments
                    .FindAllAsync(p => paymentIdsToUpdate.Contains(p.PaymentId));
                int count = 0;
                foreach (var payment in paymentsToUpdate)
                {
                    payment.PaymentType = BookingDto.PaymentType;

                    if (BookingDto.PaymentType == "حصة")
                    {
                        payment.AmountRequired = _unitOfWork.Groups.FindAsync(g => g.GroupId == BookingDto.GroupId).Result.ServingPrice;
                    }
                    else if (BookingDto.PaymentType == "شهري")
                    {
                        if (count == 0)
                        {
                            payment.AmountRequired = _unitOfWork.Groups.FindAsync(g => g.GroupId == BookingDto.GroupId).Result.Pricepermonth;
                            count++;
                        }
                        else
                        {
                            payment.AmountRequired = null;
                        }
                    }
                    else if (BookingDto.PaymentType == "معفي من المعلم")
                    {
                        if(count == 0)
                        {
                            var requir = await _unitOfWork.CenterRates.FindAsync(a => a.Rate == group.Pricepermonth);
                            payment.AmountRequired = requir.RateMonthly;
                            count++;
                        }
                        else
                        {
                            payment.AmountRequired = null;
                        }
                    }
                    else if (BookingDto.PaymentType == "معفي")
                    {
                        payment.AmountRequired = null;
                    }
                    _unitOfWork.Payments.Update(payment);
                }
            }
            _mapper.Map(BookingDto, booking);
            _unitOfWork.Bookings.Update(booking);
            await _unitOfWork.Complete();
            int i = 0;
            foreach (var attendance in lastNineAttendances)
            {
                attendance.StudentId = Convert.ToInt32(BookingDto.StudentId);
                attendance.GroupId = Convert.ToInt32(BookingDto.GroupId);
                attendance.BookingId = BookingDto.BookingId;
                attendance.PaymentType = BookingDto.PaymentType;
                if (BookingDto.PaymentType == "حصة")
                {
                    attendance.AmountRequired = _unitOfWork.Groups.FindAsync(g => g.GroupId == BookingDto.GroupId).Result.ServingPrice;
                }
                else if (BookingDto.PaymentType == "شهري")
                {
                    if (i == 0)
                    {
                        attendance.AmountRequired = _unitOfWork.Groups.FindAsync(g => g.GroupId == BookingDto.GroupId).Result.Pricepermonth;
                        i++;
                    }
                    else
                    {
                        attendance.AmountRequired = null;
                    }
                }
                else if (BookingDto.PaymentType == "معفي من المعلم")
                {
                    if (i == 0)
                    {
                        var rat = _unitOfWork.CenterRates.FindAsync(g => g.Rate == group.Pricepermonth);
                        attendance.AmountRequired = rat.Result.RateMonthly; i++;
                    }
                    else
                    {
                        attendance.AmountRequired = null;
                    }
                }
                else if (BookingDto.PaymentType == "معفي")
                {
                    attendance.AmountRequired = null;
                }
                _unitOfWork.Attendances.Update(attendance);
            }
            try
            {
                await _unitOfWork.Complete();
            }
            catch (Exception)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var booking = await _unitOfWork.Bookings.GetByIdAsync(id);

            if (booking == null)
            {
                return NotFound();
            }

            _unitOfWork.Bookings.Delete(booking);

            var attendanceRecords = await _unitOfWork.Attendances
                .FindAllAsync(a => a.BookingId == id);

            if (attendanceRecords != null && attendanceRecords.Any())
            {
                var paymentIds = attendanceRecords.Select(a => a.PaymentId).ToList();
                var paymentRecords = await _unitOfWork.Payments
                    .FindAllAsync(p => paymentIds.Contains(p.PaymentId));

                if (paymentRecords != null && paymentRecords.Any())
                {
                    _unitOfWork.Payments.DeleteRange(paymentRecords);
                }

                // حذف سجلات الحضور
                _unitOfWork.Attendances.DeleteRange(attendanceRecords);
            }

            await _unitOfWork.Complete();

            return Ok(booking);
        }

        [HttpGet]
        public async Task<IActionResult> VBookings()
        {
            var vBookings = await _unitOfWork.Vbookings.GetAllAsync();
            return Ok(vBookings);
        }

        [HttpGet]
        public async Task<IActionResult> VBookingsgroup(int groupid)
        {
            var vBookings = await _unitOfWork.Vbookings
                .GetAllAsync(v => v.GroupId == groupid , null);
            return Ok(vBookings);
        }
    }

}