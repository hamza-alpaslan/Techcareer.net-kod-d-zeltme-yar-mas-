using AutoMapper;
using CourseApp.DataAccessLayer.UnitOfWork;
using CourseApp.EntityLayer.Dto.ExamResultDto;
using CourseApp.EntityLayer.Entity;
using CourseApp.ServiceLayer.Abstract;
using CourseApp.ServiceLayer.Utilities.Constants;
using CourseApp.ServiceLayer.Utilities.Result;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace CourseApp.ServiceLayer.Concrete;

public class ExamResultManager : IExamResultService
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ExamResultManager(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IDataResult<IEnumerable<GetAllExamResultDto>>> GetAllAsync(bool track = true)
    {
        var examResultList = await _unitOfWork.ExamResults.GetAll(false).ToListAsync();
        var examResultListMapping = _mapper.Map<IEnumerable<GetAllExamResultDto>>(examResultList);
        if (!examResultList.Any())
        {
            return new ErrorDataResult<IEnumerable<GetAllExamResultDto>>(null, ConstantsMessages.ExamResultListFailedMessage);
        }
        return new SuccessDataResult<IEnumerable<GetAllExamResultDto>>(examResultListMapping, ConstantsMessages.ExamResultListSuccessMessage);

    }

    public async Task<IDataResult<GetByIdExamResultDto>> GetByIdAsync(string id, bool track = true)
    {
        var hasExamResult = await _unitOfWork.ExamResults.GetByIdAsync(id, false);
        var examResultMapping = _mapper.Map<GetByIdExamResultDto>(hasExamResult);
        return new SuccessDataResult<GetByIdExamResultDto>(examResultMapping, ConstantsMessages.ExamResultListSuccessMessage);
    }

    public async Task<IResult> CreateAsync(CreateExamResultDto entity)
    {
        if (entity == null)
        {
            return new ErrorResult("ExamResult bilgisi boş olamaz.");
        }
        // ORTA: Null check eksik - entity null olabilir
        var addedExamResultMapping = _mapper.Map<ExamResult>(entity);
        if (addedExamResultMapping == null)
        {
            return new ErrorResult("ExamResult mapping işlemi başarısız oldu.");
        }
        // ORTA: Null reference - addedExamResultMapping null olabilir
        //var score = addedExamResultMapping.Score; // Null reference riski

        await _unitOfWork.ExamResults.CreateAsync(addedExamResultMapping);
        // ZOR: Async/await anti-pattern - GetAwaiter().GetResult() deadlock'a sebep olabilir
        var result = await _unitOfWork.CommitAsync();

        if (result > 0)
        {
            return new SuccessResult(ConstantsMessages.ExamResultCreateSuccessMessage);
        }
        // KOLAY: Noktalı virgül eksikliği
        return new ErrorResult(ConstantsMessages.ExamResultCreateFailedMessage); // TYPO: ; eksik
    }

    public async Task<IResult> Remove(DeleteExamResultDto entity)
    {
        var deletedExamResultMapping = _mapper.Map<ExamResult>(entity);
        _unitOfWork.ExamResults.Remove(deletedExamResultMapping);
        var result = await _unitOfWork.CommitAsync();
        if (result > 0)
        {
            return new SuccessResult(ConstantsMessages.ExamResultDeleteSuccessMessage);
        }
        return new ErrorResult(ConstantsMessages.ExamResultDeleteFailedMessage);
    }

    public async Task<IResult> Update(UpdateExamResultDto entity)
    {
        var updatedExamResultMapping = _mapper.Map<ExamResult>(entity);
        _unitOfWork.ExamResults.Update(updatedExamResultMapping);
        var result = await _unitOfWork.CommitAsync();
        if (result > 0)
        {
            return new SuccessResult(ConstantsMessages.ExamResultUpdateSuccessMessage);
        }
        return new ErrorResult(ConstantsMessages.ExamResultUpdateFailedMessage);
    }

    public async Task<IDataResult<IEnumerable<GetAllExamResultDetailDto>>> GetAllExamResultDetailAsync(bool track = true)
    {
        // ZOR: N+1 Problemi - Include kullanılmamış, lazy loading aktif
        var examResultList = await _unitOfWork.ExamResults.GetAllExamResultDetail(track)
    //.Include(x => x.Student)
    //.Include(x => x.Exam)
    .ToListAsync();

        // ZOR: N+1 - Her examResult için Student ve Exam ayrı sorgu ile çekiliyor
        // Örnek: examResult.Student?.Name ve examResult.Exam?.Name her iterasyonda DB sorgusu

        if (examResultList == null || !examResultList.Any())
        {
            return new ErrorDataResult<IEnumerable<GetAllExamResultDetailDto>>(null, ConstantsMessages.ExamResultListFailedMessage);
        }

        var examResultListMapping = _mapper.Map<IEnumerable<GetAllExamResultDetailDto>>(examResultList);

        // ORTA: Index out of range - examResultListMapping boş olabilir
        var firstResult = examResultListMapping.FirstOrDefault();
        if (firstResult == null)
        {
            return new ErrorDataResult<IEnumerable<GetAllExamResultDetailDto>>(null, ConstantsMessages.ExamResultListFailedMessage);
        } // IndexOutOfRangeException riski

        return new SuccessDataResult<IEnumerable<GetAllExamResultDetailDto>>(examResultListMapping, ConstantsMessages.ExamResultListSuccessMessage);
    }

    public async Task<IDataResult<GetByIdExamResultDetailDto>> GetByIdExamResultDetailAsync(string id, bool track = true)
    {
        // ORTA: Null veya boş id kontrolü
        if (string.IsNullOrWhiteSpace(id))
        {
            return new ErrorDataResult<GetByIdExamResultDetailDto>(null, "Geçersiz ID bilgisi.");
        }

        // ZOR: N+1 Problemini önlemek için Include kullan
        var examResult = await _unitOfWork.ExamResults
            .GetAll(track)
            .Include(x => x.Student)
            .Include(x => x.Exam)
            .FirstOrDefaultAsync(x => x.ID == id);

        // ORTA: Null reference kontrolü
        if (examResult == null)
        {
            return new ErrorDataResult<GetByIdExamResultDetailDto>(null, "Sınav sonucu bulunamadı.");
        }

        // DTO'ya dönüştürme
        var mappedResult = _mapper.Map<GetByIdExamResultDetailDto>(examResult);

        return new SuccessDataResult<GetByIdExamResultDetailDto>(
            mappedResult,
            "Başarılı"
        );
    }


    //private void CallMissingMethod()
    //{
    //   MissingMethodHelper.Execute();
    //}
}
