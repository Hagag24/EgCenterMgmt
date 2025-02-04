
namespace EgCenterMgmt.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class BranchController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BranchController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        //[HttpPost]
        //    public async Task<IActionResult> FilterData([FromBody] BranchFilterDto filter)
        //    {
        //        // Fetch data from the database
        //        var teachers = await _unitOfWork.Teachers.GetAllAsync();
        //        var subjects = await _unitOfWork.Subjects.GetAllAsync();
        //        var students = await _unitOfWork.Students.GetAllAsync();
        //        var payments = await _unitOfWork.Payments.GetAllAsync();
        //        var groupSchedules = await _unitOfWork.GroupSchedules.GetAllAsync();
        //        var grades = await _unitOfWork.Grades.GetAllAsync();
        //        var groups = await _unitOfWork.Groups.GetAllAsync();
        //        var attendances = await _unitOfWork.Attendances.GetAllAsync();
        //        var branches = await _unitOfWork.Branches.GetAllAsync();
        //        var bookings = await _unitOfWork.Bookings.GetAllAsync();

        //        // Apply filters
        //        var filteredStudents = students;
        //        var filteredGroups = groups;
        //        var filteredPayments = payments;
        //        var filteredAttendances = attendances;
        //        var filteredBranches = branches;
        //        var filteredBookings = bookings;

        //        // Filter by student ID or name/code

        //        if (!string.IsNullOrEmpty(filter.StudentName))
        //        {
        //            filteredStudents = filteredStudents
        //                .Where(s => s.StudentName.Contains(filter.StudentName, StringComparison.OrdinalIgnoreCase) ||
        //                            s.Code.Contains(filter.StudentCode, StringComparison.OrdinalIgnoreCase))
        //                .ToList();
        //        }

        //        // Filter by group name
        //        if (!string.IsNullOrEmpty(filter.GroupName))
        //        {
        //            filteredGroups = filteredGroups.Where(g => g.GroupName.Contains(filter.GroupName, StringComparison.OrdinalIgnoreCase)).ToList();
        //            var groupIds = filteredGroups.Select(g => g.GroupId).ToList(); // Get IDs of filtered groups
        //            filteredAttendances = filteredAttendances.Where(a => groupIds.Contains(a.GroupId)).ToList();
        //            filteredBookings = filteredBookings.Where(b => groupIds.Contains(b.GroupId)).ToList();
        //        }

        //        // Filter by start and end date
        //        if (filter.StartDate.HasValue && filter.EndDate.HasValue)
        //        {
        //            var startDateTime = filter.StartDate.Value.ToDateTime(TimeOnly.MinValue);
        //            var endDateTime = filter.EndDate.Value.ToDateTime(TimeOnly.MaxValue);

        //            filteredAttendances = filteredAttendances
        //                .Where(a => a.AttendanceDate >= startDateTime && a.AttendanceDate <= endDateTime)
        //                .ToList();

        //            filteredBookings = filteredBookings
        //                .Where(b => b.BookingDate >= startDateTime && b.BookingDate <= endDateTime)
        //                .ToList();

        //            filteredGroups = filteredGroups
        //                .Where(g => g.CreationDate >= startDateTime && g.CreationDate <= endDateTime)
        //                .ToList();
        //        }

        //        // Filter by teacher name
        //        if (!string.IsNullOrEmpty(filter.TeacherName))
        //        {
        //            var teacherNames = teachers
        //                .Where(t => t.TeacherName.Contains(filter.TeacherName, StringComparison.OrdinalIgnoreCase))
        //                .Select(t => t.TeacherId)
        //                .ToList();

        //            filteredGroups = filteredGroups.Where(g => teacherNames.Contains(g.TeacherId)).ToList();
        //            filteredAttendances = filteredAttendances.Where(a => teacherNames.Contains(a.TeacherId)).ToList();
        //        }

        //        // Filter by branch name
        //        if (!string.IsNullOrEmpty(filter.BranchName))
        //        {
        //            filteredBranches = filteredBranches.Where(b => b.BranchName.Contains(filter.BranchName, StringComparison.OrdinalIgnoreCase)).ToList();
        //            var branchIds = filteredBranches.Select(b => b.BranchId).ToList(); // Get IDs of filtered branches
        //            filteredGroups = filteredGroups.Where(g => branchIds.Contains(g.BranchId)).ToList();
        //            filteredStudents = filteredStudents.Where(s => branchIds.Contains(s.BranchId)).ToList();
        //            filteredAttendances = filteredAttendances.Where(a => branchIds.Contains(a.BranchId)).ToList();
        //            filteredBookings = filteredBookings.Where(b => branchIds.Contains(b.BranchId)).ToList();
        //        }
        //        // Calculate statistics
        //        var statistics = new Dictionary<string, int>
        //{
        //    { "عدد المدرسين", teachers.Count },
        //    { "عدد المواضيع", subjects.Count },
        //    { "عدد الطلاب", filteredStudents.Count },
        //    { "عدد المدفوعات", filteredPayments.Count },
        //    { "عدد المدفوعات بالشهر", filteredPayments.Count(p => p.PaymentType == "شهري") },
        //    { "عدد المدفوعات بالحصة", filteredPayments.Count(p => p.PaymentType == "حصة") },
        //    { "عدد جداول المجموعات", groupSchedules.Count },
        //    { "عدد الصفوف", grades.Count },
        //    { "عدد المجموعات", filteredGroups.Count },
        //    { "عدد الحضور", filteredAttendances.Count(a => a.IsAttendanc == true) },
        //    { "عدد الغياب", filteredAttendances.Count(a => a.IsAttendanc == false) },
        //    { "عدد الفروع", filteredBranches.Count },
        //    { "عدد الحجوزات", filteredBookings.Count }
        //};

        //        return Ok(statistics);
        //    }

        [HttpGet]
        public async Task<IActionResult> VBranches()
        {

            var branches = await _unitOfWork.Vbranchs.GetAllAsync();
            return Ok(branches);
        }

        [HttpGet]
        public async Task<IActionResult> GetBranches()
        {
            var branches = await _unitOfWork.Branches.GetAllAsync();
            var branchDtos = _mapper.Map<IEnumerable<BranchDto>>(branches);
            return Ok(branchDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBranch(int id)
        {
            var branch = await _unitOfWork.Branches.GetByIdAsync(id);

            if (branch == null)
            {
                return NotFound();
            }

            var branchDto = _mapper.Map<BranchDto>(branch);
            return Ok(branchDto);
        }

        [HttpPost]
        public async Task<IActionResult> PostBranch(BranchDto branchDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // تحقق من وجود فرع مسبق بنفس الاسم أو الموقع
            var existingBranch = await _unitOfWork.Branches
                .FindAsync(b => b.BranchName == branchDto.BranchName && b.BranchLocation == branchDto.BranchLocation);

            if (existingBranch != null)
            {
                return Conflict("يوجد فرع مسبق بنفس الاسم أو الموقع.");
            }
            var branch = _mapper.Map<Branch>(branchDto);
            _unitOfWork.Branches.Add(branch);
            await _unitOfWork.Complete();

            branchDto = _mapper.Map<BranchDto>(branch);
            return CreatedAtAction(nameof(GetBranch), new { id = branch.BranchId }, branchDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutBranch(int id, BranchDto branchDto)
        {
            if (id != branchDto.BranchId)
            {
                return BadRequest();
            }
            // تحقق من وجود فرع مسبق بنفس الاسم أو الموقع
            var existingBranch = await _unitOfWork.Branches
                .FindAsync(b => b.BranchName == branchDto.BranchName && b.BranchLocation == branchDto.BranchLocation);

            if (existingBranch != null)
            {
                return Conflict("يوجد فرع مسبق بنفس الاسم أو الموقع.");
            }
            var branch = _mapper.Map<Branch>(branchDto);
            _unitOfWork.Branches.Update(branch);

            try
            {
                await _unitOfWork.Complete();
            }
            catch (Exception)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBranch(int id)
        {
            var branch = await _unitOfWork.Branches.GetByIdAsync(id);

            if (branch == null)
            {
                return NotFound();
            }

            try
            {
                var GroupsBrunch = await _unitOfWork.Groups.FindAllAsync(e => e.BranchId == id);

                if (GroupsBrunch.Any())
                {
                    foreach (var group in GroupsBrunch)
                    {
                        var groupSchedules = await _unitOfWork.GroupSchedules.FindAllAsync(gs => gs.GroupId == group.GroupId);

                        if (groupSchedules.Any())
                        {
                            foreach (var schedule in groupSchedules)
                            {
                                _unitOfWork.GroupSchedules.Delete(schedule);
                            }
                            await _unitOfWork.Complete();
                        }

                        var bookings = await _unitOfWork.Bookings.FindAllAsync(sb => sb.GroupId == group.GroupId);

                        if (bookings.Any())
                        {
                            foreach (var booking in bookings)
                            {
                                _unitOfWork.Bookings.Delete(booking);
                            }
                            await _unitOfWork.Complete();
                        }

                        var payments = await _unitOfWork.Payments.FindAllAsync(sb => sb.GroupId == group.GroupId);

                        if (bookings.Any())
                        {
                            foreach (var payment in payments)
                            {
                                _unitOfWork.Payments.Delete(payment);
                            }
                            await _unitOfWork.Complete();
                        }
                        var Attendances = await _unitOfWork.Attendances.FindAllAsync(sb => sb.GroupId == group.GroupId);

                        if (bookings.Any())
                        {
                            foreach (var Attendance in Attendances)
                            {
                                _unitOfWork.Attendances.Delete(Attendance);
                            }
                            await _unitOfWork.Complete();
                        }
                        _unitOfWork.Groups.Delete(group);
                    }
                    await _unitOfWork.Complete();
                }
            }
            catch (Exception ex) 
            { 
                Console.WriteLine(ex);
            }

            _unitOfWork.Branches.Delete(branch);
            await _unitOfWork.Complete();

            return Ok(branch);
        }

    }
}
