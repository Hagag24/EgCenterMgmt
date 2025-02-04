
namespace EgCenterMgmt.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]

    public class PaymentController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPaymentServices _paymentServices;

        public PaymentController(IUnitOfWork unitOfWork, IMapper mapper, IPaymentServices paymentServices)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _paymentServices = paymentServices;
        }

        [HttpPost]
        public async Task<IActionResult> GetStudentBalances([FromBody] StudentBalanceRequestDto requestDto)
        {
            try
            {
                var studentBalances = await _paymentServices.GetStudentBalancesAsync(requestDto);
                return Ok(studentBalances);
            }
            catch (Exception ex)
            {
                return BadRequest($"فشل في تحميل بيانات الطلاب. الخطأ: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPayments()
        {
            var Payments = await _unitOfWork.Payments.GetAllAsync();
            var PaymentDtos = _mapper.Map<IEnumerable<PaymentDto>>(Payments);
            return Ok(PaymentDtos);
        }

        // GET: api/Payment/VPayment
        [HttpGet]
        public async Task<IActionResult> VPayment()
        {
            var Payments = await _unitOfWork.Vpayments.GetAllAsync();
            return Ok(Payments);
        }
        // GET: api/Payment/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPayment(int id)
        {
            var Payment = await _unitOfWork.Payments.GetByIdAsync(id);

            if (Payment == null)
            {
                return NotFound();
            }

            var PaymentDto = _mapper.Map<PaymentDto>(Payment);
            return Ok(PaymentDto);
        }

        // POST: api/Payment
        [HttpPost]
        public async Task<IActionResult> PostPayment([FromBody] PaymentDto paymentDto)
        {
            if (paymentDto == null)
            {
                return BadRequest("بيانات الدفع فارغة.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var payment = _mapper.Map<Payment>(paymentDto);
                _unitOfWork.Payments.Add(payment);
                await _unitOfWork.Complete();

                paymentDto = _mapper.Map<PaymentDto>(payment);
                return CreatedAtAction(nameof(GetPayment), new { id = payment.PaymentId }, paymentDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        // PUT: api/Payment/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPayment(int id, PaymentDto PaymentDto)
        {
            if (id != PaymentDto.PaymentId)
            {
                return BadRequest();
            }
            var Payment = _mapper.Map<Payment>(PaymentDto);
            _unitOfWork.Payments.Update(Payment);

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

        // DELETE: api/Payment/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            var Payment = await _unitOfWork.Payments.GetByIdAsync(id);

            if (Payment == null)
            {
                return NotFound();
            }

            try
            {
                _unitOfWork.Payments.Delete(Payment);
                await _unitOfWork.Complete();

                return Ok(Payment);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
