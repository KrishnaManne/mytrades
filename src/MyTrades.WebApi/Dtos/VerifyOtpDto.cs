namespace MyTrades.WebApi;

public record SignInDto(string Email);
public record VerifyOtpDto(string Email, string Otp);

public record VerifyOtpResponseDto(string AccessToken);