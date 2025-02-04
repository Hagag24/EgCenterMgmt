using AutoMapper;
using SmartLearnMgmt.Shared.DTO;
using SmartLearnMgmt.Shared.Models;
using SmartLearnMgmt.Shared.ModelsAuth;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SmartLearnMgmt.Maui
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<Branch, BranchDto>().ReverseMap();
            CreateMap<Grade, GradeDto>().ReverseMap();
            CreateMap<Expense, ExpenseDto>().ReverseMap();
            CreateMap<Group, GroupDto>().ReverseMap();
            CreateMap<GroupSchedule, GroupScheduleDto>().ReverseMap();
            CreateMap<Student, StudentDto>().ReverseMap();
            CreateMap<Booking, BookingDto>().ReverseMap();
            CreateMap<Booking, RegesterBookingDto>().ReverseMap();
            CreateMap<Payment, PaymentDto>().ReverseMap();
            CreateMap<Subject, SubjectDto>().ReverseMap();
            CreateMap<Teacher, TeacherDto>().ReverseMap();
            CreateMap<AttendanceDto, Vattendance>().ReverseMap();
            CreateMap<AttendanceDto, Attendance>().ReverseMap();
            CreateMap<CenterRateDto, CenterRate>().ReverseMap();
            CreateMap<AttendanceCancellationDto, AttendanceCancellation>().ReverseMap();
        }
    }
}
