namespace MyTrades.WebApi;

public class OtpVerification
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Otp { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsVerified {get; set;}
}