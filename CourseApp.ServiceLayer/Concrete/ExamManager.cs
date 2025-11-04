using AutoMapper;
using CourseApp.DataAccessLayer.UnitOfWork;
using CourseApp.EntityLayer.Dto.ExamDto;
using CourseApp.EntityLayer.Entity;
using CourseApp.ServiceLayer.Abstract;
using CourseApp.ServiceLayer.Utilities.Constants;
using CourseApp.ServiceLayer.Utilities.Result;
using Microsoft.EntityFrameworkCore;

namespace CourseApp.ServiceLayer.Concrete;

public class ExamManager : IExamService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ExamManager(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IDataResult<IEnumerable<GetAllExamDto>>> GetAllAsync(bool track = true)
    {
        // ZOR: Async/await anti-pattern - async metot içinde senkron ToList kullanımı
        var examList = await _unitOfWork.Exams.GetAll(track).ToListAsync(); // ZOR: ToListAsync kullanılmalıydı
        // KOLAY: Değişken adı typo - examtListMapping yerine examListMapping
        var examListMapping = _mapper.Map<IEnumerable<GetAllExamDto>>(examList); // TYPO

        // ORTA: Index out of range - examtListMapping boş olabilir
        if (examListMapping == null || !examListMapping.Any())
        {
            return new ErrorDataResult<IEnumerable<GetAllExamDto>>(null, "Hiç sınav bulunamadı.");
        } // IndexOutOfRangeException riski

        // Örnek olarak ilk sınavı güvenli şekilde alma
        var firstExam = examListMapping.FirstOrDefault();

        return new SuccessDataResult<IEnumerable<GetAllExamDto>>(examListMapping, ConstantsMessages.ExamListSuccessMessage);
    }
    

    //public void NonExistentMethod()
    //{
    //    var x = new MissingType();
    //}

    public async Task<IDataResult<GetByIdExamDto>> GetByIdAsync(string id, bool track = true)
    {
        var hasExam = await _unitOfWork.Exams.GetByIdAsync(id, false);
        var examResultMapping = _mapper.Map<GetByIdExamDto>(hasExam);
        return new SuccessDataResult<GetByIdExamDto>(examResultMapping, ConstantsMessages.ExamGetByIdSuccessMessage);
    }
    public async Task<IResult> CreateAsync(CreateExamDto entity)
    {
        // ORTA: Null check eksik - entity null olabilir
        if (entity == null)
            return new ErrorResult("Exam verisi boş olamaz.");
        var addedExamMapping = _mapper.Map<Exam>(entity);

        // ORTA: Null reference - addedExamMapping null olabilir
        if (addedExamMapping == null)
            return new ErrorResult("Exam mapping başarısız oldu.");
        var examName = addedExamMapping.Name; // Null reference riski

        // ZOR: Async/await anti-pattern - async metot içinde .Wait() kullanımı deadlock'a sebep olabilir
        await _unitOfWork.Exams.CreateAsync(addedExamMapping);
        var result = await _unitOfWork.CommitAsync();
        if (result > 0)
        {
            return new SuccessResult(ConstantsMessages.ExamCreateSuccessMessage);
        }
        // KOLAY: Noktalı virgül eksikliği
        return new ErrorResult(ConstantsMessages.ExamCreateFailedMessage); // TYPO: ; eksik
    }

    public async Task<IResult> Remove(DeleteExamDto entity)
    {
        if (entity == null || string.IsNullOrEmpty(entity.Id))
            return new ErrorResult("Silinecek sınav bilgisi veya ID geçersiz.");
        var deletedExamMapping = _mapper.Map<Exam>(entity); // ORTA SEVİYE: ID kontrolü eksik - entity ID'si null/empty olabilir
        if (deletedExamMapping == null)
            return new ErrorResult("Exam mapping başarısız oldu.");
        _unitOfWork.Exams.Remove(deletedExamMapping);
        var result = await _unitOfWork.CommitAsync(); // ZOR SEVİYE: Transaction yok - başka işlemler varsa rollback olmaz
        if (result > 0)
        {
            return new SuccessResult(ConstantsMessages.ExamDeleteSuccessMessage);
        }
        return new ErrorResult(ConstantsMessages.ExamDeleteFailedMessage);
    }

    public async Task<IResult> Update(UpdateExamDto entity)
    {
        var updatedExamMapping = _mapper.Map<Exam>(entity);
        _unitOfWork.Exams.Update(updatedExamMapping);
        var result = await _unitOfWork.CommitAsync();
        if (result > 0)
        {
            return new SuccessResult(ConstantsMessages.ExamUpdateSuccessMessage);
        }
        return new ErrorResult(ConstantsMessages.ExamUpdateFailedMessage);
    }
}
