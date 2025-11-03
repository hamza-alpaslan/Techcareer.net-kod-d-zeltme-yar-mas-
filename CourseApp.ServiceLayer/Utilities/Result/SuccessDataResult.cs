using CourseApp.EntityLayer.Dto.CourseDto;
using CourseApp.EntityLayer.Dto.LessonDto;

namespace CourseApp.ServiceLayer.Utilities.Result;

public class SuccessDataResult<T>:DataResult<T>    
{
    private IEnumerable<GetAllCourseDto> courseDetailDtoList;
    private string courseDetailsFetchedSuccessfully;
    private GetByIdLessonDetailDto lessonMapping;

    public SuccessDataResult(GetByIdLessonDetailDto lessonMapping)
        : base((T)(object)lessonMapping, true, default)
    {
    }


    public SuccessDataResult(IEnumerable<EntityLayer.Dto.CourseDto.GetAllCourseDto> courseDetailDtoList, T data):base(data,true,default)
    {
        
    }
    public SuccessDataResult(T data,string message):base(data,true,message)
    {
        
    }

    public SuccessDataResult(IEnumerable<GetAllCourseDto> courseDetailDtoList, string courseDetailsFetchedSuccessfully)
        : base((T)(object)courseDetailDtoList, true, courseDetailsFetchedSuccessfully)
    {
    }

}
