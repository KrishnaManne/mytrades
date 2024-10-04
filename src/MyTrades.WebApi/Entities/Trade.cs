namespace MyTrades.WebApi;

public sealed class Trade
{
    public Guid Id {get; set;}
    //public Guid UserId {get; set;}
    public string? TradedStock { get; set; }
    public int Quantity {get; set;}
    public float EntryPrice {get; set;}
    public float ExitPrice {get; set;}
    public DateTime EntryDateTime {get; set;}
    public DateTime ExitDateTime {get; set;}
    //TradeDirection can be Long or Short
    public string? TradeDirection {get; set;}
    // TradeType can be Scalping, Day, Swing, Positional 
    public string? TradeType {get; set;}
}
