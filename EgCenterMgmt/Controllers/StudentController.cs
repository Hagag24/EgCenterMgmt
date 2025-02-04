
namespace EgCenterMgmt.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]

    public class StudentController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        //private int? payment;

        public StudentController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetStudents()
        {
            var students = await _unitOfWork.Students.GetAllAsync();

            var studentDtos = _mapper.Map<List<StudentDto>>(students);

            return Ok(studentDtos);
        }

        // GET: api/Student/5
        [HttpGet]
        public async Task<IActionResult> GetStudent(int id)
        {
            var student = await _unitOfWork.Students.GetByIdAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            var studentDto = _mapper.Map<StudentDto>(student);
            return Ok(studentDto);
        }

        [HttpPost]
        public async Task<IActionResult> PostStudent(StudentDto studentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // التحقق من وجود المعلم بالبريد الإلكتروني قبل إضافته
            var existingStudents = await _unitOfWork.Students.FindAsync(t => t.StudentName == studentDto.StudentName && t.FatherWhatsApp == studentDto.FatherWhatsApp && t.StudentWhatsApp == studentDto.StudentWhatsApp);
            if (existingStudents != null)
            {
                return Conflict("هذا المعلم موجود بالفعل.");
            }

            var student = _mapper.Map<Student>(studentDto);
            _unitOfWork.Students.Add(student);
            await _unitOfWork.Complete();
            student.Code = GenerateStudentCode.StudentCode(student.StudentId);
            _unitOfWork.Students.Update(student);
            await _unitOfWork.Complete();

            studentDto = _mapper.Map<StudentDto>(student);
            return CreatedAtAction(nameof(GetStudent), new { id = student.StudentId }, studentDto);
        }

        // PUT: api/Student/5
        [HttpPut]
        public async Task<IActionResult> PutStudent(StudentDto studentDto)
        {
            var student = _mapper.Map<Student>(studentDto);
            if(student.Code == null)
                student.Code = GenerateStudentCode.StudentCode(student.StudentId);
            _unitOfWork.Students.Update(student);

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

        // DELETE: api/Student/5
        [HttpDelete]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _unitOfWork.Students.GetByIdAsync(id);

            if (student == null)
            {
                return NotFound();
            }
            var attanc = await _unitOfWork.Attendances.FindAllAsync(sb => sb.StudentId == student.StudentId);

            if (attanc.Any())
            {
                foreach (var att in attanc)
                {
                    _unitOfWork.Attendances.Delete(att);
                }
                await _unitOfWork.Complete();
            }

            var payments = await _unitOfWork.Payments.FindAllAsync(sb => sb.StudentId == student.StudentId);

            if (payments.Any())
            {
                foreach (var payment in payments)
                {
                    _unitOfWork.Payments.Delete(payment);
                }
                await _unitOfWork.Complete();
            }

            var bookings = await _unitOfWork.Bookings.FindAllAsync(sb => sb.StudentId == student.StudentId);

            if (bookings.Any())
            {
                foreach (var booking in bookings)
                {
                    _unitOfWork.Bookings.Delete(booking);
                }
                await _unitOfWork.Complete();
            }

            _unitOfWork.Students.Delete(student);
            await _unitOfWork.Complete();

            return Ok(student);
        }

        // GET: api/Student/VStudents
        [HttpGet]
        public async Task<IActionResult> VStudents()
        {
            try
            {
                var vstudents = await _unitOfWork.Vstudents.GetAllAsync();
                //var sortedVstudents = vstudents.OrderByDescending(v => v.StudentId).ToList();
                return Ok(vstudents);
            }catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> VStudentsGrade(int Gradeid)
        {
            var vstudents = await _unitOfWork.Vstudents.GetAllAsync(g => g.GradeId == Gradeid, null);
            var sortedVstudents = vstudents.OrderBy(v => v.StudentName).ToList();
            return Ok(sortedVstudents);
        }
    }
}
