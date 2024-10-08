namespace MyTrades.WebApi;

public record UserDto(Guid Id, string UserName, string Email, bool EmailVerified);
