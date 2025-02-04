
namespace EgCenterMgmt.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ExpensesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/Expenses
        [HttpGet]
        public async Task<IActionResult> GetAllExpenses()
        {
            try
            {
                var expenses = await _unitOfWork.VExpenses.GetAllAsync();
                return Ok(expenses);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving expenses: {ex.Message}");
            }
        }

        // GET: api/Expenses/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetExpense(int id)
        {
            try
            {
                var expense = await _unitOfWork.Expenses.GetByIdAsync(id);

                if (expense == null)
                {
                    return NotFound($"Expense with ID {id} not found");
                }

                var expenseDto = _mapper.Map<ExpenseDto>(expense);
                return Ok(expenseDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving expense: {ex.Message}");
            }
        }

        // POST: api/Expenses
        [HttpPost]
        public async Task<IActionResult> CreateExpense([FromBody] ExpenseDto expenseDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var expense = _mapper.Map<Expense>(expenseDto);
                _unitOfWork.Expenses.Add(expense);
                await _unitOfWork.Complete();

                return CreatedAtAction(nameof(GetExpense), new { id = expense.ExpenseId }, expenseDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating expense: {ex.Message}");
            }
        }

        // PUT: api/Expenses/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExpense(int id, [FromBody] ExpenseDto expenseDto)
        {
            if (id != expenseDto.ExpenseId)
            {
                return BadRequest("Expense ID mismatch");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var existingExpense = await _unitOfWork.Expenses.GetByIdAsync(id);

                if (existingExpense == null)
                {
                    return NotFound($"Expense with ID {id} not found");
                }

                var expense = _mapper.Map<Expense>(expenseDto);
                _unitOfWork.Expenses.Update(expense);
                await _unitOfWork.Complete();

                return Ok(expenseDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating expense: {ex.Message}");
            }
        }

        // DELETE: api/Expenses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            try
            {
                var expense = await _unitOfWork.Expenses.GetByIdAsync(id);

                if (expense == null)
                {
                    return NotFound($"Expense with ID {id} not found");
                }

                _unitOfWork.Expenses.Delete(expense);
                await _unitOfWork.Complete();

                return Ok($"Expense with ID {id} deleted");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting expense: {ex.Message}");
            }
        }
    }
}
