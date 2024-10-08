namespace MyTrades.WebApi;

public interface ISignInManagementService
{
    Task SignInUserAsync(SignInDto signInDto);
    Task<VerifyOtpResponseDto> VerifyOtp(VerifyOtpDto verifyOtpDto);
}

public class SignInManagementService : ISignInManagementService
{
    private readonly ILogger<SignInManagementService> logger;
    private readonly IUserPersistenceService userPersisitenceService;
    private readonly IEmailService emailService;
    private readonly IOTPPersistenceService otpPersistenceService;
    private readonly IJwtTokenGenerator jwtTokenGenerator;

    public SignInManagementService(ILogger<SignInManagementService> logger,
                                IUserPersistenceService userPersistenceService,
                                IEmailService emailService,
                                IOTPPersistenceService otpPersistenceService,
                                IJwtTokenGenerator jwtTokenGenerator)
    {
        this.logger = logger;
        this.userPersisitenceService = userPersistenceService;
        this.emailService = emailService;
        this.otpPersistenceService = otpPersistenceService;
        this.jwtTokenGenerator = jwtTokenGenerator;
    }
    public async Task SignInUserAsync(SignInDto signInDto)
    {
        var user = await userPersisitenceService.GetUserByEmailAsync(signInDto.Email);
        if(user is null)
            throw new EntityNotFoundException($"User with email id { signInDto.Email } not found.");
        
        var generatedOtp = OtpGenerator.GenerateOtp();
        await emailService.SendEmailAsync(user.Email, "SignUp/SignIn Verification OTP", $"The OTP for SignUp/SignIn is {generatedOtp}");

        var otpVerficationEntity = new OtpVerification
        { 
            Id = Guid.NewGuid(), 
            UserId = user.Id,
            Otp = generatedOtp,
            CreatedAt = DateTime.UtcNow,
            IsVerified = false
        };

        await otpPersistenceService.AddOTPVerfication(otpVerficationEntity);
    }

    public async Task<VerifyOtpResponseDto> VerifyOtp(VerifyOtpDto verifyOtpDto)
    {
        var user = await userPersisitenceService.GetUserByEmailAsync(verifyOtpDto.Email);
        if(user is null)
            throw new EntityNotFoundException($"User with email id { verifyOtpDto.Email } not found.");

        var otpRecords = await otpPersistenceService.GetUnverifiedOtps(user.Id);
        if(otpRecords.Count() == 0)
            throw new EntityNotFoundException($"No OTPs sent for verification for { verifyOtpDto.Email }.");

        var otpRecordMatched = otpRecords.Find(x => x.Otp == verifyOtpDto.Otp);
        if(otpRecordMatched is null)
            throw new EntityNotFoundException($"OTP verification failed for { verifyOtpDto.Email } and otp {verifyOtpDto.Otp}.");
        
        otpRecordMatched.IsVerified = true;
        await otpPersistenceService.UpdateOTPVerification(otpRecordMatched.Id, otpRecordMatched);

        if(!user.EmailVerified)
        {
            user.EmailVerified = true;
            await userPersisitenceService.UpdateUserAsync(user.Id, user);
        }

        var jwtToken = jwtTokenGenerator.GenerateJwtToken(user.Id, user.UserName!, user.Email!, 30);
        return new VerifyOtpResponseDto(jwtToken);
    }
}