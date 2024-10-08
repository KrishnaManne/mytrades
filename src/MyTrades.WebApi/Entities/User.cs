namespace MyTrades.WebApi;

public class User
{
    public Guid Id { get; set; }
    public string? UserName { get; set; }
    public string? Email {get; set;}
    public bool EmailVerified {get; set;}
}