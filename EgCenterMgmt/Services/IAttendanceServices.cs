
namespace EgCenterMgmt.Services
{
    public interface IAttendanceServices
    {
        Task<List<Totalattendance>> GenerateTotalAttendanceReportAsync(IEnumerable<Vattendance> records);
        Task<RegesterBookingDto> AddBookingAndAttances(RegesterBookingDto RegesterBookingDto);
        Task<bool> UpdateBookingAndAttances(GroupSchedule existingGroupSchedule);
    }
}
