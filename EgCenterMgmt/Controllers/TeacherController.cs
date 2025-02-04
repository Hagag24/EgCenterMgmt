
namespace EgCenterMgmt.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]

    public class TeacherController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TeacherController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/Teacher
        [HttpGet]
        public async Task<IActionResult> GetTeachers()
        {
            try
            {
                var teachers = await _unitOfWork.Teachers.GetAllAsync();
                var teacherDtos = _mapper.Map<IEnumerable<TeacherDto>>(teachers);
                return Ok(teacherDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving teachers: {ex.Message}");
            }
        }

        // GET: api/Teacher/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTeacher(int id)
        {
            try
            {
                var teacher = await _unitOfWork.Teachers.GetByIdAsync(id);

                if (teacher == null)
                {
                    return NotFound($"Teacher with ID {id} not found");
                }

                var teacherDto = _mapper.Map<TeacherDto>(teacher);
                return Ok(teacherDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving teacher: {ex.Message}");
            }
        }

        // POST: api/Teacher
        [HttpPost]
        public async Task<IActionResult> CreateTeacher([FromBody] TeacherDto teacherDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // التحقق من وجود المعلم بالبريد الإلكتروني قبل إضافته
            var existingTeacher = await _unitOfWork.Teachers.FindAsync(t => t.TeacherName == teacherDto.TeacherName && t.TeacherPhone == teacherDto.TeacherWhatsApp && t.TeacherPhone == teacherDto.TeacherWhatsApp);
            if (existingTeacher != null)
            {
                return Conflict("هذا المعلم موجود بالفعل.");
            }

            try
            {
                var teacher = _mapper.Map<Teacher>(teacherDto);
                _unitOfWork.Teachers.Add(teacher);
                await _unitOfWork.Complete();

                teacherDto = _mapper.Map<TeacherDto>(teacher);
                return CreatedAtAction(nameof(GetTeacher), new { id = teacher.TeacherId }, teacherDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating teacher: {ex.Message}");
            }
        }

        // PUT: api/Teacher/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTeacher(int id, [FromBody] TeacherDto teacherDto)
        {
            if (id != teacherDto.TeacherId)
            {
                return BadRequest("Teacher ID mismatch");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var teacher = _mapper.Map<Teacher>(teacherDto);
                _unitOfWork.Teachers.Update(teacher);
                await _unitOfWork.Complete();

                return Ok(teacherDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating teacher: {ex.Message}");
            }
        }

        // DELETE: api/Teacher/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeacher(int id)
        {
            try
            {
                var teacher = await _unitOfWork.Teachers.GetByIdAsync(id);

                if (teacher == null)
                {
                    return NotFound($"Teacher with ID {id} not found");
                }

                _unitOfWork.Teachers.Delete(teacher);
                await _unitOfWork.Complete();

                return Ok($"Teacher with ID {id} deleted");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting teacher: {ex.Message}");
            }
        }

        // GET: api/Teacher/VTeachers
        [HttpGet("VTeachers")]
        public async Task<IActionResult> VTeachers()
        {
            var vteachers = await _unitOfWork.Vtechers.GetAllAsync();
            return Ok(vteachers);
        }
    }
}
