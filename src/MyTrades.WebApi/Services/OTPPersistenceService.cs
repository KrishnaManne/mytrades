using Microsoft.EntityFrameworkCore;

namespace MyTrades.WebApi;

public interface IOTPPersistenceService
{
    Task<OtpVerification> AddOTPVerfication(OtpVerification otpVerification);
    Task UpdateOTPVerification(Guid id, OtpVerification otpVerification);
    Task<List<OtpVerification>> GetUnverifiedOtps(Guid userId);
}

public class OTPPersistenceService : IOTPPersistenceService
{
    private readonly ILogger<OTPPersistenceService> logger;
    private readonly IUnitOfWork unitOfWork;
    private readonly IGenericRepository<OtpVerification> otpRepository;
    public OTPPersistenceService(IUnitOfWork unitOfWork, ILogger<OTPPersistenceService> logger)
    {
        this.logger = logger;
        this.unitOfWork = unitOfWork;
        this.otpRepository = this.unitOfWork.Repository<OtpVerification>();
    }
    public async Task<OtpVerification> AddOTPVerfication(OtpVerification otpVerification)
    {
        var otpRecord = await otpRepository.AddAsync(otpVerification);
        await unitOfWork.SaveAsync();
        return otpRecord;
    }

    public async Task<List<OtpVerification>> GetUnverifiedOtps(Guid userId)
    {
        return await otpRepository
                        .Entity
                        .Where(x => x.IsVerified == false)
                        .OrderByDescending(x => x.CreatedAt)
                        .ToListAsync();

    }

    public async Task UpdateOTPVerification(Guid id, OtpVerification otpVerification)
    {   
        await otpRepository.UpdateAsync(id, otpVerification);
        await unitOfWork.SaveAsync();
    }
}