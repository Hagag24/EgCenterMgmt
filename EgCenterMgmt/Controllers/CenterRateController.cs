namespace EgCenterMgmt.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class CenterRateController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CenterRateController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetRates()
        {
            var rates = await _unitOfWork.CenterRates.GetAllAsync();
            var ratesDto = _mapper.Map<IEnumerable<CenterRate>>(rates);
            return Ok(ratesDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRate([FromBody] CenterRateDto ratedto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var rate = _mapper.Map<CenterRate>(ratedto);
            _unitOfWork.CenterRates.Add(rate);
            await _unitOfWork.Complete();

            return CreatedAtAction(nameof(GetRates), new { id = rate.Id }, rate);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRate(int id)
        {
            var rate = await _unitOfWork.CenterRates.GetByIdAsync(id);
            if (rate == null)
                return NotFound($"Rate with ID {id} not found");

            _unitOfWork.CenterRates.Delete(rate);
            await _unitOfWork.Complete();

            return Ok($"Rate with ID {id} deleted");
        }
    }
}
