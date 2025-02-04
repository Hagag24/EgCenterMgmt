
namespace EgCenterMgmt.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]

    public class GradeController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GradeController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/Grade
        [HttpGet]
        public async Task<IActionResult> GetGrades()
        {
            var grades = await _unitOfWork.Grades.GetAllAsync();
            var gradeDtos = _mapper.Map<IEnumerable<GradeDto>>(grades);
            return Ok(gradeDtos);
        }

        // GET: api/Grade/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGrade(int id)
        {
            var grade = await _unitOfWork.Grades.GetByIdAsync(id);

            if (grade == null)
            {
                return NotFound();
            }

            var gradeDto = _mapper.Map<GradeDto>(grade);
            return Ok(gradeDto);
        }

        // POST: api/Grade
        [HttpPost]
        public async Task<IActionResult> PostGrade(GradeDto gradeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existinggrade = await _unitOfWork.Grades
                .FindAsync(b => b.GradeName == gradeDto.GradeName);

            if (existinggrade != null)
            {
                return Conflict("يوجد فرع مسبق بنفس الاسم أو الموقع.");
            }
            var grade = _mapper.Map<Grade>(gradeDto);
            _unitOfWork.Grades.Add(grade);
            await _unitOfWork.Complete();

            gradeDto = _mapper.Map<GradeDto>(grade);
            return CreatedAtAction(nameof(GetGrade), new { id = grade.GradeId }, gradeDto);
        }

        // PUT: api/Grade/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGrade(int id, GradeDto gradeDto)
        {
            if (id != gradeDto.GradeId)
            {
                return BadRequest();
            }

            var existinggrade = await _unitOfWork.Grades
            .FindAsync(b => b.GradeName == gradeDto.GradeName);

            if (existinggrade != null)
            {
                return Conflict("يوجد فرع مسبق بنفس الاسم أو الموقع.");
            }
            var grade = _mapper.Map<Grade>(gradeDto);
            _unitOfWork.Grades.Update(grade);

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

        // DELETE: api/Grade/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGrade(int id)
        {
            var grade = await _unitOfWork.Grades.GetByIdAsync(id);

            if (grade == null)
            {
                return NotFound();
            }

            _unitOfWork.Grades.Delete(grade);
            await _unitOfWork.Complete();

            return Ok(grade);
        }
    }
}
