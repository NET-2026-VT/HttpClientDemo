using AutoMapper;
using HttpClientDemo.Models.Dtos;
using HttpClientDemo.Models.Entities;

namespace HttpClientDemo.Data;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Student, StudentDto>();
             //.ForMember(dest => dest.City,
             // opt => opt.MapFrom(src => src.Address.City));

        CreateMap<Student, CreateStudentDto>().ReverseMap();

        CreateMap<Student, UpdateStudentDto>().ReverseMap();

        CreateMap<Enrollment, CourseDto>();
        // .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Course.Title));

        CreateMap<Student, StudentDetailsDto>();
            //.ForMember(dest => dest.Courses,
            //opt => opt.MapFrom(src => src.Enrollments));
    }
}
