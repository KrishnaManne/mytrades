namespace MyTrades.WebApi;

public record ErrorDto(string errorCode, string errorMessage, string detailedMessage);
