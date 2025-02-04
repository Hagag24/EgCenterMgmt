
namespace EgCenterMgmt.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]

    public class GroupScheduleController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAttendanceServices _attendanceServices;

        public GroupScheduleController(IUnitOfWork unitOfWork, IMapper mapper, IAttendanceServices attendanceServices)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _attendanceServices = attendanceServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetVgroupSchedules()
        {
            var vgroupSchedules = await _unitOfWork.VgroupSchedules.GetAllAsync();
            return Ok(vgroupSchedules);
        }
        [HttpGet]
        public async Task<IActionResult> GetVgroupSchedulesByGroupId(int groupid)
        {
            var vgroupSchedules = await _unitOfWork.VgroupSchedules.GetAllAsync(g=>g.GroupId == groupid,null);
            return Ok(vgroupSchedules);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGroupSchedule(int id)
        {
            var groupSchedule = await _unitOfWork.GroupSchedules.GetByIdAsync(id);

            if (groupSchedule == null)
            {
                return NotFound();
            }

            var groupScheduleDto = _mapper.Map<GroupScheduleDto>(groupSchedule);
            return Ok(groupScheduleDto);
        }

        [HttpPost]
        public async Task<IActionResult> PostGroupSchedule(GroupScheduleDto groupScheduleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // تحقق من وجود جدول مسبق بنفس التفاصيل
            var existingSchedule = await _unitOfWork.GroupSchedules
                .FindAsync(gs => gs.GroupId == groupScheduleDto.GroupId &&
                            gs.DayOfWeek == groupScheduleDto.DayOfWeek &&
                            gs.StartTime == groupScheduleDto.StartTime &&
                            gs.EndTime == groupScheduleDto.EndTime);

            if (existingSchedule != null)
            {
                return Conflict("يوجد جدول مسبق لنفس المجموعة في هذا التاريخ والوقت.");
            }
            var existingSchedulename = await _unitOfWork.GroupSchedules
                .FindAsync(gs => gs.GroupId == groupScheduleDto.GroupId &&
                            gs.DayOfWeek == groupScheduleDto.DayOfWeek);

            if (existingSchedulename != null)
            {
                return Conflict("يوجد جدول مسبق لنفس المجموعة في هذا اليوم.");
            }

            try
            {
                var schagules = _unitOfWork.Vattendances.GetAllAsync(g => g.GroupId == groupScheduleDto.GroupId, null).Result.Count();
                if (schagules > 0)
                {

                    var Schadules = await _unitOfWork.GroupSchedules.FindAllAsync(b => b.GroupId == groupScheduleDto.GroupId);

                    var groupSchedule = _mapper.Map<GroupSchedule>(groupScheduleDto);
                    _unitOfWork.GroupSchedules.Add(groupSchedule);
                    await _unitOfWork.Complete();

                    var update = await _attendanceServices.UpdateBookingAndAttances(groupSchedule);

                    if (update)
                    {
                        return Ok();
                    }
                    else
                    {
                        var SchaduleNew = await _unitOfWork.GroupSchedules.FindAllAsync(b => b.GroupId == groupScheduleDto.GroupId);
                        foreach (var item in SchaduleNew)
                        {
                            var Schaduledto = Schadules.FirstOrDefault(s => s.ScheduleId == item.ScheduleId);
                            var Schadule = _mapper.Map<GroupSchedule>(Schaduledto);
                            _unitOfWork.GroupSchedules.Update(Schadule);
                            await _unitOfWork.Complete();
                        }
                    }

                    groupScheduleDto = _mapper.Map<GroupScheduleDto>(groupSchedule);
                    return CreatedAtAction(nameof(GetGroupSchedule), new { id = groupSchedule.ScheduleId }, groupScheduleDto);
                }
                else
                {
                    var groupSchedule = _mapper.Map<GroupSchedule>(groupScheduleDto);
                    _unitOfWork.GroupSchedules.Add(groupSchedule);
                    await _unitOfWork.Complete();
                    return CreatedAtAction(nameof(GetGroupSchedule), new { id = groupSchedule.ScheduleId }, groupScheduleDto);
                }
            }catch(Exception ex)
            {
                return BadRequest(error: ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutGroupSchedule(int id, GroupScheduleDto groupScheduleDto)
        {
            try
            {
                if (id != groupScheduleDto.ScheduleId)
                {
                    return BadRequest();
                }

                var existingGroupSchedule = await _unitOfWork.GroupSchedules.GetByIdAsync(id);
                if (existingGroupSchedule == null)
                {
                    return NotFound();
                }

                // تحقق من وجود جدول مسبق بنفس التفاصيل
                var existingSchedule = await _unitOfWork.GroupSchedules
                    .FindAsync(gs => gs.GroupId == groupScheduleDto.GroupId &&
                                gs.DayOfWeek == groupScheduleDto.DayOfWeek &&
                                gs.StartTime == groupScheduleDto.StartTime &&
                                gs.EndTime == groupScheduleDto.EndTime);

                if (existingSchedule != null)
                {
                    return Conflict("يوجد جدول مسبق لنفس المجموعة في هذا التاريخ والوقت.");

                }
                var Schadules = await _unitOfWork.GroupSchedules.FindAllAsync(b => b.GroupId == groupScheduleDto.GroupId);

                _mapper.Map(groupScheduleDto, existingGroupSchedule);
                _unitOfWork.GroupSchedules.Update(existingGroupSchedule);
                await _unitOfWork.Complete();

                var update = await _attendanceServices.UpdateBookingAndAttances(existingGroupSchedule);

                if (update)
                {
                    return Ok();
                }
                else
                {
                    var SchaduleNew = await _unitOfWork.GroupSchedules.FindAllAsync(b => b.GroupId == groupScheduleDto.GroupId);
                    foreach (var item in SchaduleNew)
                    {
                        var Schaduledto = Schadules.FirstOrDefault(s => s.ScheduleId == item.ScheduleId);
                        var Schadule = _mapper.Map<GroupSchedule>(Schaduledto);
                        _unitOfWork.GroupSchedules.Update(Schadule);
                        await _unitOfWork.Complete();
                    }
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to update the group schedule");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroupSchedule(int id)
        {
            var groupSchedule = await _unitOfWork.GroupSchedules.GetByIdAsync(id);

            if (groupSchedule == null)
            {
                return NotFound();
            }

            var Schadules = await _unitOfWork.GroupSchedules.FindAllAsync(b => b.GroupId == groupSchedule.GroupId);

            _unitOfWork.GroupSchedules.Delete(groupSchedule);
            await _unitOfWork.Complete();

            var update = await _attendanceServices.UpdateBookingAndAttances(groupSchedule);

            if (update)
            {
                return Ok();
            }
            else
            {
                var SchaduleNew = await _unitOfWork.GroupSchedules.FindAllAsync(b => b.GroupId == groupSchedule.GroupId);
                foreach (var item in SchaduleNew)
                {
                    var Schaduledto = Schadules.FirstOrDefault(s => s.ScheduleId == item.ScheduleId);
                    var Schadule = _mapper.Map<GroupSchedule>(Schaduledto);
                    _unitOfWork.GroupSchedules.Update(Schadule);
                    await _unitOfWork.Complete();
                }
            }


            return Ok(groupSchedule);
        }
    }
}
