namespace MyTrades.WebApi;

public record TradeDto
(
  Guid Id,
  string? TradedStock,
  int Quantity,
  float EntryPrice,
  float ExitPrice,
  DateTime EntryDateTime,
  DateTime ExitDateTime,
  string? TradeDirection,
  string? TradeType
);
