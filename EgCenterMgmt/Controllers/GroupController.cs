

namespace EgCenterMgmt.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]

    public class GroupController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GroupController(IUnitOfWork unitOfWork, IMapper mapper,IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> GetGroups()
        {
            var groups = await _unitOfWork.Groups.GetAllAsync();
            var groupDtos = _mapper.Map<IEnumerable<GroupDto>>(groups);
            return Ok(groupDtos);
        }


        [HttpGet]
        public async Task<IActionResult> GetGroup(int id)
        {
            var group = await _unitOfWork.Groups.GetByIdAsync(id);

            if (group == null)
            {
                return NotFound();
            }

            var groupDto = _mapper.Map<GroupDto>(group);
            return Ok(groupDto);
        }

        [HttpPost]
        public async Task<IActionResult> PostGroup(GroupDto groupDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // تحقق من وجود مجموعة مسبقة بنفس التفاصيل (مثلاً الاسم والفرع)
            var existingGroup = await _unitOfWork.Groups
                .FindAsync(g => g.GroupName == groupDto.GroupName && g.BranchId == groupDto.BranchId);

            if (existingGroup != null)
            {
                return Conflict("يوجد مجموعة مسبقة بنفس الاسم في هذا الفرع.");
            }
            var group = _mapper.Map<Group>(groupDto);
            _unitOfWork.Groups.Add(group);
            await _unitOfWork.Complete();

            groupDto = _mapper.Map<GroupDto>(group);
            return CreatedAtAction(nameof(GetGroup), new { id = group.GroupId }, groupDto);
        }

        [HttpPut]
        public async Task<IActionResult> PutGroup(int id, GroupDto groupDto)
        {
            if (id != groupDto.GroupId)
            {
                return BadRequest();
            }
            var group = _mapper.Map<Group>(groupDto);
            _unitOfWork.Groups.Update(group);

            try
            {
                await _unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                return BadRequest(error:ex.Message);
            }

            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteGroup(int id)
        {
            var group = await _unitOfWork.Groups.GetByIdAsync(id);

            if (group == null)
            {
                return NotFound();
            }

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
            await _unitOfWork.Complete();

            return Ok(group);
        }


        [HttpGet]
        public async Task<IActionResult> VGroups()
        {
            var vgroups = await _unitOfWork.Vgroups.GetAllAsync();
            return Ok(vgroups);
        }

        [HttpGet]
        public async Task<IActionResult> VgroupScheduleS()
        {
            var vgroupSchedules = await _unitOfWork.VgroupSchedules.GetAllAsync();
            return Ok(vgroupSchedules);
        }
    }
}
