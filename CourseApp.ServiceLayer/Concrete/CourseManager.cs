using CourseApp.DataAccessLayer.UnitOfWork;
using CourseApp.EntityLayer.Dto.CourseDto;
using CourseApp.EntityLayer.Entity;
using CourseApp.ServiceLayer.Abstract;
using CourseApp.ServiceLayer.Utilities.Constants;
using CourseApp.ServiceLayer.Utilities.Result;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq;

namespace CourseApp.ServiceLayer.Concrete;

public class CourseManager : ICourseService
{
    private readonly IUnitOfWork _unitOfWork;

    public CourseManager(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IDataResult<IEnumerable<GetAllCourseDto>>> GetAllAsync(bool track = true)
    {
        // ZOR: N+1 Problemi - Her course için Instructor ayrı sorgu ile çekiliyor
        var courseList = await _unitOfWork.Courses
    .GetAll(track)
    .Include(c => c.Instructor)
    .ToListAsync();

        // ZOR: N+1 - Include/ThenInclude kullanılmamış, lazy loading aktif
        var result = courseList
            .Where(course => course != null)
            .Select(course => new GetAllCourseDto
            {
                Id = course.ID,
                CourseName = course.CourseName,
                CreatedDate = course.CreatedDate,
                StartDate = course.StartDate,
                EndDate = course.EndDate,
                IsActive = course.IsActive,
                InstructorID = course.InstructorID,
            })
            .ToList();

        // ORTA: Index out of range - result boş olabilir
        GetAllCourseDto firstCourse;
        if (result != null && result.Any())
        {
            firstCourse = result[0];// IndexOutOfRangeException riski
        }
         

        return new SuccessDataResult<IEnumerable<GetAllCourseDto>>(result, ConstantsMessages.CourseListSuccessMessage);
    }

    public async Task<IDataResult<GetByIdCourseDto>> GetByIdAsync(string id, bool track = true)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return new ErrorDataResult<GetByIdCourseDto>(
                null,
                "Geçersiz kurs ID'si. Lütfen doğru bir ID giriniz.");
        }
        // ORTA: Null check eksik - id null/empty olabilir
        // ORTA: Null reference exception - hasCourse null olabilir ama kontrol edilmiyor
        var hasCourse = await _unitOfWork.Courses.GetByIdAsync(id, track);
        if (hasCourse == null)
        {
            return new ErrorDataResult<GetByIdCourseDto>(
                null,
                "Belirtilen ID'ye sahip bir kurs bulunamadı.");
        }
        // ORTA: Null reference - hasCourse null ise NullReferenceException
        var course = new GetByIdCourseDto
        {
            Id = hasCourse.ID,
            CourseName = hasCourse.CourseName,
            CreatedDate = hasCourse.CreatedDate,
            StartDate = hasCourse.StartDate,
            EndDate = hasCourse.EndDate,
            IsActive = hasCourse.IsActive,
            InstructorID = hasCourse.InstructorID
        };

        return new SuccessDataResult<GetByIdCourseDto>(
            course,
            ConstantsMessages.CourseGetByIdSuccessMessage);
    }
    public async Task<IResult> CreateAsync(CreateCourseDto entity)
    {
        var instructor = await _unitOfWork.Instructors.GetByIdAsync(entity.InstructorID);
        if (instructor == null)
        {
            return new ErrorDataResult<CreateCourseDto>(
                null,
                "Geçersiz InstructorID. Böyle bir eğitmen bulunamadı.");
        }
        var createdCourse = new Course
        {
            CourseName = entity.CourseName,
            CreatedDate = entity.CreatedDate,
            EndDate = entity.EndDate,
            InstructorID = entity.InstructorID,
            IsActive = entity.IsActive,
            StartDate = entity.StartDate,
        };

        await _unitOfWork.Courses.CreateAsync(createdCourse);

        var result = await _unitOfWork.CommitAsync();

        if (result > 0)
        {
            return new SuccessResult(ConstantsMessages.CourseCreateSuccessMessage);
        }

        return new ErrorResult(ConstantsMessages.CourseCreateFailedMessage);
    }
    public async Task<IResult> Remove(DeleteCourseDto entity)
    {
        var deletedCourse = new Course
        {
            ID = entity.Id,
        };
        var deletecousechech= await _unitOfWork.Courses.GetByIdAsync(entity.Id);
        if (deletecousechech==null)
        {
            return new ErrorResult(ConstantsMessages.CourseDeleteFailedMessage);
        }
        _unitOfWork.Courses.Remove(deletedCourse);
        var result = await _unitOfWork.CommitAsync();
        if (result > 0)
        {
            return new SuccessResult(ConstantsMessages.CourseDeleteSuccessMessage);
        }

        return new ErrorResult(ConstantsMessages.CourseDeleteFailedMessage);
    }

    public async Task<IResult> Update(UpdateCourseDto entity)
    {
        var updatedCourse = await _unitOfWork.Courses.GetByIdAsync(entity.Id);
        if (updatedCourse == null)
        {
            return new ErrorResult(ConstantsMessages.CourseUpdateFailedMessage);
        }

        updatedCourse.CourseName = entity.CourseName;
        updatedCourse.StartDate = entity.StartDate;
        updatedCourse.EndDate = entity.EndDate;
        updatedCourse.InstructorID = entity.InstructorID;
        updatedCourse.IsActive = entity.IsActive;

        _unitOfWork.Courses.Update(updatedCourse);
        var result = await _unitOfWork.CommitAsync();
        if (result > 0)
        {
            return new SuccessResult(ConstantsMessages.CourseUpdateSuccessMessage);
        }
        return new ErrorResult(ConstantsMessages.CourseUpdateFailedMessage);
    }

    public async Task<IDataResult<IEnumerable<GetAllCourseDetailDto>>> GetAllCourseDetail(bool track = true)
    {
        // ZOR: N+1 Problemi - Include kullanılmamış, lazy loading aktif
        var courseListDetailList = await _unitOfWork.Courses
    .GetAllCourseDetail(track)
    .Include(c => c.Instructor)
    .ToListAsync();

        // ZOR: N+1 - Her course için Instructor ayrı sorgu ile çekiliyor (x.Instructor?.Name)
        var courseDetailDtoList = courseListDetailList
            .Where(x => x != null)
            .Select(x => new GetAllCourseDetailDto
            {
                Id = x.ID,
                CourseName = x.CourseName,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                CreatedDate = x.CreatedDate,
                InstructorID = x.InstructorID,
                InstructorName = x.Instructor?.Name ?? "Bilinmiyor",
                IsActive = x.IsActive
            })
            .ToList();

        // ORTA: Null reference - courseDetailDtoList null olabilir
        GetAllCourseDetailDto? firstDetail = courseDetailDtoList.FirstOrDefault(); // Null/Empty durumunda exception

        return new SuccessDataResult<IEnumerable<GetAllCourseDetailDto>>(courseDetailDtoList, ConstantsMessages.CourseDetailsFetchedSuccessfully);
    }

    private IResult CourseNameIsNullOrEmpty(string courseName)
    {
        if(courseName == null || courseName.Length == 0)
        {
            return new ErrorResult("Kurs Adı Boş Olamaz");
        }
        return new SuccessResult();
    }

    private async Task<IResult> CourseNameUniqeCheck(string id,string courseName)
    {
        var courseNameCheck = await _unitOfWork.Courses.GetAll(false).AnyAsync(c => c.CourseName == courseName);
        if(!courseNameCheck)
        {
            return new ErrorResult("Bu kurs adi ile zaten bir kurs var");
        }
        return new SuccessResult();
    }

    private  IResult CourseNameLenghtCehck(string courseName)
    {
        if(courseName == null || courseName.Length < 2 || courseName.Length > 50)
        {
            return new ErrorResult("Kurs Adı Uzunluğu 2 - 50 Karakter Arasında Olmalı");
        }
        return new SuccessResult();
    }

    private IResult IsValidDateFormat(string date)
    {
        DateTime tempDate;
        bool isValid = DateTime.TryParse(date, out tempDate);

        if (!isValid)
        {
            return new ErrorResult("Geçersiz tarih formatı.");
        }
        return new SuccessResult();
    }
    private IResult CheckCourseDates(DateTime startDate, DateTime endDate)
    {
        if (endDate <= startDate)
        {
            return new ErrorResult("Bitiş tarihi, başlangıç tarihinden sonra olmalıdır.");
        }
        return new SuccessResult();
    }
    
    private IResult CheckInstructorNameIsNullOrEmpty(string instructorName)
    {
        if (string.IsNullOrEmpty(instructorName))
        {
            return new ErrorResult("Eğitmen alanı boş olamaz");
        }

        return new SuccessResult();
    }
}
