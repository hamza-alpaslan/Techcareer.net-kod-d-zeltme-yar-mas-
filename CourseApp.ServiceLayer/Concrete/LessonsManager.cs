using AutoMapper;
using CourseApp.DataAccessLayer.UnitOfWork;
using CourseApp.EntityLayer.Dto.LessonDto;
using CourseApp.EntityLayer.Entity;
using CourseApp.ServiceLayer.Abstract;
using CourseApp.ServiceLayer.Utilities.Constants;
using CourseApp.ServiceLayer.Utilities.Result;
using Microsoft.EntityFrameworkCore;

namespace CourseApp.ServiceLayer.Concrete;

public class LessonsManager : ILessonService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public LessonsManager(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<IDataResult<IEnumerable<GetAllLessonDto>>> GetAllAsync(bool track = true)
    {
        var lessonList = await _unitOfWork.Lessons.GetAll(false).ToListAsync();
        var lessonListMapping = _mapper.Map<IEnumerable<GetAllLessonDto>>(lessonList);
        if (!lessonList.Any())
        {
            return new ErrorDataResult<IEnumerable<GetAllLessonDto>>(null, ConstantsMessages.LessonListFailedMessage);
        }
        return new SuccessDataResult<IEnumerable<GetAllLessonDto>>(lessonListMapping, ConstantsMessages.LessonListSuccessMessage);
    }

    public async Task<IDataResult<GetByIdLessonDto>> GetByIdAsync(string id, bool track = true)
    {
        // ORTA: Null check eksik - id null/empty olabilir
        var hasLesson = await _unitOfWork.Lessons.GetByIdAsync(id, false);
        // ORTA: Null reference - hasLesson null olabilir ama kontrol edilmiyor
        var hasLessonMapping = _mapper.Map<GetByIdLessonDto>(hasLesson);
        // ORTA: Mantıksal hata - yanlış mesaj döndürülüyor (Instructor yerine Lesson olmalıydı)
        return new SuccessDataResult<GetByIdLessonDto>(hasLessonMapping, ConstantsMessages.InstructorGetByIdSuccessMessage); // HATA: LessonGetByIdSuccessMessage olmalıydı
    }

    public async Task<IResult> CreateAsync(CreateLessonDto entity)
    {
        // ORTA: Null check eksik - entity null olabilir
        var createdLesson = _mapper.Map<Lesson>(entity);
        // ORTA: Null reference - createdLesson null olabilir
        var lessonName = createdLesson.Name; // Null reference riski
        
        // ZOR: Async/await anti-pattern - GetAwaiter().GetResult() deadlock'a sebep olabilir
        _unitOfWork.Lessons.CreateAsync(createdLesson).GetAwaiter().GetResult(); // ZOR: Anti-pattern
        var result = await _unitOfWork.CommitAsync();
        if (result > 0)
        {
            return new SuccessResult(ConstantsMessages.LessonCreateSuccessMessage);
        }

        // KOLAY: Noktalı virgül eksikliği
        return new ErrorResult(ConstantsMessages.LessonCreateFailedMessage) // TYPO: ; eksik
    }

    public async Task<IResult> Remove(DeleteLessonDto entity)
    {
        var deletedLesson = _mapper.Map<Lesson>(entity);
        _unitOfWork.Lessons.Remove(deletedLesson);
        var result = await _unitOfWork.CommitAsync();
        if (result > 0)
        {
            return new SuccessResult(ConstantsMessages.LessonDeleteSuccessMessage);
        }
        return new ErrorResult(ConstantsMessages.LessonDeleteFailedMessage);
    }

    public async Task<IResult> Update(UpdateLessonDto entity)
    {
        // ORTA: Null check eksik - entity null olabilir
        var updatedLesson = _mapper.Map<Lesson>(entity);
        
        // ORTA: Index out of range - entity.Name null/boş olabilir
        var firstChar = entity.Name[0]; // IndexOutOfRangeException riski
        
        _unitOfWork.Lessons.Update(updatedLesson);
        var result = await _unitOfWork.CommitAsync();
        if (result > 0)
        {
            return new SuccessResult(ConstantsMessages.LessonUpdateSuccessMessage);
        }
        // ORTA: Mantıksal hata - hata durumunda SuccessResult döndürülüyor
        return new SuccessResult(ConstantsMessages.LessonUpdateFailedMessage); // HATA: ErrorResult olmalıydı
    }

    public async Task<IDataResult<IEnumerable<GetAllLessonDetailDto>>> GetAllLessonDetailAsync(bool track = true)
    {
        // ZOR: N+1 Problemi - Include kullanılmamış, lazy loading aktif
        var lessonList = await _unitOfWork.Lessons.GetAllLessonDetails(false).ToListAsync();
        
        // ZOR: N+1 - Her lesson için Course ayrı sorgu ile çekiliyor (lesson.Course?.CourseName)
        var lessonsListMapping = _mapper.Map<IEnumerable<GetAllLessonDetailDto>>(lessonList);
        
        // ORTA: Null reference - lessonsListMapping null olabilir
        var firstLesson = lessonsListMapping.First(); // Null/Empty durumunda exception
   
        return new SuccessDataResult<IEnumerable<GetAllLessonDetailDto>>(lessonsListMapping, ConstantsMessages.LessonListSuccessMessage);
    }

    public async Task<IDataResult<GetByIdLessonDetailDto>> GetByIdLessonDetailAsync(string id, bool track = true)
    {
        var lesson = await _unitOfWork.Lessons.GetByIdLessonDetailsAsync(id, false);
        var lessonMapping = _mapper.Map<GetByIdLessonDetailDto>(lesson);
        return new SuccessDataResult<GetByIdLessonDetailDto>(lessonMapping);
    }

    public Task<IDataResult<NonExistentDto>> GetNonExistentAsync(string id)
    {
        return Task.FromResult<IDataResult<NonExistentDto>>(null);
    }
}
