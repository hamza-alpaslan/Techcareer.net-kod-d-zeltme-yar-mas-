using AutoMapper;
using CourseApp.DataAccessLayer.UnitOfWork;
using CourseApp.EntityLayer.Dto.RegistrationDto;
using CourseApp.EntityLayer.Entity;
using CourseApp.ServiceLayer.Abstract;
using CourseApp.ServiceLayer.Utilities.Constants;
using CourseApp.ServiceLayer.Utilities.Result;
using Microsoft.EntityFrameworkCore;

namespace CourseApp.ServiceLayer.Concrete;

public class RegistrationManager : IRegistrationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public RegistrationManager(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IDataResult<IEnumerable<GetAllRegistrationDto>>> GetAllAsync(bool track = true)
    {
        var registrationList = await _unitOfWork.Registrations.GetAll(false).ToListAsync();
        var registrationListMapping = _mapper.Map<IEnumerable<GetAllRegistrationDto>>(registrationList);
        if (!registrationList.Any())
        {
            return new ErrorDataResult<IEnumerable<GetAllRegistrationDto>>(null, ConstantsMessages.RegistrationListFailedMessage);
        }
        return new SuccessDataResult<IEnumerable<GetAllRegistrationDto>>(registrationListMapping, ConstantsMessages.RegistrationListSuccessMessage);
    }

    public async Task<IDataResult<GetByIdRegistrationDto>> GetByIdAsync(string id, bool track = true)
    {
        var hasRegistration = await _unitOfWork.Registrations.GetByIdAsync(id, false);
        var hasRegistrationMapping = _mapper.Map<GetByIdRegistrationDto>(hasRegistration);
        return new SuccessDataResult<GetByIdRegistrationDto>(hasRegistrationMapping, ConstantsMessages.RegistrationGetByIdSuccessMessage);
    }

    public async Task<IResult> CreateAsync(CreateRegistrationDto entity)
    {
        // ORTA: Null check eksik - entity null olabilir
        if (entity == null)
        {
            return new ErrorResult(ConstantsMessages.RegistrationCreateFailedMessage);
        }
        var createdRegistration = _mapper.Map<Registration>(entity);

        // ORTA: Null reference - createdRegistration null olabilir
        if (createdRegistration == null)
        {
            return new ErrorResult(ConstantsMessages.RegistrationCreateFailedMessage);
        }

        decimal registrationPrice;
        if (createdRegistration.Price!=null)
        registrationPrice = createdRegistration.Price; // Null reference riski

        // ZOR: Async/await anti-pattern - GetAwaiter().GetResult() deadlock'a sebep olabilir
        await _unitOfWork.Registrations.CreateAsync(createdRegistration); // ZOR: Anti-pattern
        var result = await _unitOfWork.CommitAsync();
        if (result > 0)
        {
            return new SuccessResult(ConstantsMessages.RegistrationCreateSuccessMessage);
        }

        // KOLAY: Noktalı virgül eksikliği
        return new ErrorResult(ConstantsMessages.RegistrationCreateFailedMessage); // TYPO: ; eksik
    }

    public async Task<IResult> Remove(DeleteRegistrationDto entity)
    {
        var deletedRegistration = _mapper.Map<Registration>(entity);
        _unitOfWork.Registrations.Remove(deletedRegistration);
        var result = await _unitOfWork.CommitAsync();
        if (result > 0)
        {
            return new SuccessResult(ConstantsMessages.RegistrationDeleteSuccessMessage);
        }
        return new ErrorResult(ConstantsMessages.RegistrationDeleteFailedMessage);
    }

    public async Task<IResult> Update(UpdatedRegistrationDto entity)
    {
        // ORTA: Null check eksik - entity null olabilir
        if (entity == null)
        {
            return new ErrorResult(ConstantsMessages.RegistrationUpdateFailedMessage);
        }
        var updatedRegistration = _mapper.Map<Registration>(entity);
        if (updatedRegistration == null)
        {
            return new ErrorResult(ConstantsMessages.RegistrationUpdateFailedMessage);
        }
        // ORTA: Tip dönüşüm hatası - decimal'i int'e direkt cast
        //var invalidPrice = (int)updatedRegistration.Price; // ORTA: InvalidCastException
        
        _unitOfWork.Registrations.Update(updatedRegistration);
        var result = await _unitOfWork.CommitAsync();
        if (result > 0)
        {
            return new SuccessResult(ConstantsMessages.RegistrationUpdateSuccessMessage);
        }
        // ORTA: Mantıksal hata - hata durumunda SuccessResult döndürülüyor
        return new ErrorResult(ConstantsMessages.RegistrationUpdateFailedMessage); // HATA: ErrorResult olmalıydı
    }

    public async Task<IDataResult<IEnumerable<GetAllRegistrationDetailDto>>> GetAllRegistrationDetailAsync(bool track = true)
    {
        // ZOR: N+1 Problemi - Include kullanılmamış, lazy loading aktif
         var query = _unitOfWork.Registrations
        .GetAllRegistrationDetail(track)
        .Include(r => r.Course)
        .Include(r => r.Student);
        var registrationData = await query.ToListAsync();
        // ZOR: N+1 - Her registration için Course ve Student ayrı sorgu ile çekiliyor
        // Örnek: registration.Course?.CourseName her iterasyonda DB sorgusu

        if (!registrationData.Any())
        {
            return new ErrorDataResult<IEnumerable<GetAllRegistrationDetailDto>>(null,ConstantsMessages.RegistrationListFailedMessage);
        }

        var registrationDataMapping = _mapper.Map<IEnumerable<GetAllRegistrationDetailDto>>(registrationData);

        // ORTA: Index out of range - registrationDataMapping boş olabilir
        var firstRegistration = registrationDataMapping.FirstOrDefault(); // IndexOutOfRangeException riski

        return new SuccessDataResult<IEnumerable<GetAllRegistrationDetailDto>>(registrationDataMapping, ConstantsMessages.RegistrationListSuccessMessage);  
    }

    public async Task<IDataResult<GetByIdRegistrationDetailDto>> GetByIdRegistrationDetailAsync(string id, bool track = true)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return new ErrorDataResult<GetByIdRegistrationDetailDto>(null,"id değeri girilmek zorunda");
        }

        var query = _unitOfWork.Registrations
            .GetAllRegistrationDetail(track)
            .Include(r => r.Course)
            .Include(r => r.Student)
            .Where(r => r.ID == id);

        var registration = await query.FirstOrDefaultAsync();

        if (registration == null)
        {
            return new ErrorDataResult<GetByIdRegistrationDetailDto>(null, ConstantsMessages.RegistrationGetByIdFailedMessage);
        }

        var registrationMapping = _mapper.Map<GetByIdRegistrationDetailDto>(registration);

        return new SuccessDataResult<GetByIdRegistrationDetailDto>(registrationMapping, ConstantsMessages.RegistrationGetByIdSuccessMessage);
    
    }

    public void AccessNonExistentProperty()
    {
        var registration = new Registration();
        //var value = registration.NonExistentProperty;
    }
}
