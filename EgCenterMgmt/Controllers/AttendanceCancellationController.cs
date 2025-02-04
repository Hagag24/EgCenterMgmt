

namespace EgCenterMgmt.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class AttendanceCancellationController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AttendanceCancellationController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/AttendanceCancellation
        [HttpGet]
        public async Task<IActionResult> GetAllCancellations()
        {
            try
            {
                var cancellations = await _unitOfWork.VAttendanceCancellation.GetAllAsync();
                return Ok(cancellations);
            }
            catch (Exception ex)
            {
                return BadRequest(error: $"Error retrieving cancellations: {ex.Message}");
            }
        }

        // GET: api/AttendanceCancellation/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCancellation(int id)
        {
            try
            {
                var cancellation = await _unitOfWork.AttendanceCancellations.GetByIdAsync(id);

                if (cancellation == null)
                {
                    return NotFound($"Cancellation with ID {id} not found");
                }

                var cancellationDto = _mapper.Map<AttendanceCancellationDto>(cancellation);
                return Ok(cancellationDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving cancellation: {ex.Message}");
            }
        }

        // POST: api/AttendanceCancellation
        [HttpPost]
        public async Task<IActionResult> CreateCancellation([FromBody] AttendanceCancellationDto cancellationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var cancellation = _mapper.Map<AttendanceCancellation>(cancellationDto);
                _unitOfWork.AttendanceCancellations.Add(cancellation);
                await _unitOfWork.Complete();

                return CreatedAtAction(nameof(GetCancellation), new { id = cancellation.Id }, cancellationDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating cancellation: {ex.Message}");
            }
        }

        // PUT: api/AttendanceCancellation/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCancellation(int id, [FromBody] AttendanceCancellationDto cancellationDto)
        {
            if (id != cancellationDto.Id)
            {
                return BadRequest("Cancellation ID mismatch");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var existingCancellation = await _unitOfWork.AttendanceCancellations.GetByIdAsync(id);

                if (existingCancellation == null)
                {
                    return NotFound($"Cancellation with ID {id} not found");
                }

                var cancellation = _mapper.Map<AttendanceCancellation>(cancellationDto);
                _unitOfWork.AttendanceCancellations.Update(cancellation);
                await _unitOfWork.Complete();

                return Ok(cancellationDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating cancellation: {ex.Message}");
            }
        }

        // DELETE: api/AttendanceCancellation/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCancellation(int id)
        {
            try
            {
                var cancellation = await _unitOfWork.AttendanceCancellations.GetByIdAsync(id);

                if (cancellation == null)
                {
                    return NotFound($"Cancellation with ID {id} not found");
                }

                _unitOfWork.AttendanceCancellations.Delete(cancellation);
                await _unitOfWork.Complete();

                return Ok($"Cancellation with ID {id} deleted");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting cancellation: {ex.Message}");
            }
        }
    }
}
