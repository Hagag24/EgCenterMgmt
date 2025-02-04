
namespace EgCenterMgmt.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]

    public class SubjectController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SubjectController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/Subject
        [HttpGet]
        public async Task<IActionResult> GetSubjects()
        {
            try
            {
                var subjects = await _unitOfWork.Subjects.GetAllAsync();
                var subjectDtos = _mapper.Map<IEnumerable<SubjectDto>>(subjects);
                return Ok(subjectDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving subjects: {ex.Message}");
            }
        }

        // GET: api/Subject/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSubject(int id)
        {
            try
            {
                var subject = await _unitOfWork.Subjects.GetByIdAsync(id);

                if (subject == null)
                {
                    return NotFound($"Subject with ID {id} not found");
                }

                var subjectDto = _mapper.Map<SubjectDto>(subject);
                return Ok(subjectDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving subject: {ex.Message}");
            }
        }

        // POST: api/Subject
        [HttpPost]
        public async Task<IActionResult> CreateSubject([FromBody] SubjectDto subjectDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
 
            var existingSubject = await _unitOfWork.Subjects.FindAsync(s => s.SubjectName == subjectDto.SubjectName);
            if (existingSubject != null)
            {
                return Conflict($"هذه الماده موجوده بالفعل ");
            }
            try
            {
                var subject = _mapper.Map<Subject>(subjectDto);
                _unitOfWork.Subjects.Add(subject);
                await _unitOfWork.Complete();

                subjectDto = _mapper.Map<SubjectDto>(subject);
                return CreatedAtAction(nameof(GetSubject), new { id = subject.SubjectId }, subjectDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating subject: {ex.Message}");
            }
        }

        // PUT: api/Subject/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSubject(int id, [FromBody] SubjectDto subjectDto)
        {
            if (id != subjectDto.SubjectId)
            {
                return BadRequest("Subject ID mismatch");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingSubject = await _unitOfWork.Subjects.FindAsync(s => s.SubjectName == subjectDto.SubjectName);
            if (existingSubject != null)
            {
                return Conflict($"هذه الماده موجوده بالفعل ");
            }
            try
            {
                var subject = _mapper.Map<Subject>(subjectDto);
                _unitOfWork.Subjects.Update(subject);
                await _unitOfWork.Complete();

                return Ok(subjectDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating subject: {ex.Message}");
            }
        }

        // DELETE: api/Subject/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubject(int id)
        {
            try
            {
                var subject = await _unitOfWork.Subjects.GetByIdAsync(id);

                if (subject == null)
                {
                    return NotFound($"Subject with ID {id} not found");
                }

                _unitOfWork.Subjects.Delete(subject);
                await _unitOfWork.Complete();

                return Ok($"Subject with ID {id} deleted");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting subject: {ex.Message}");
            }
        }
    }
}
