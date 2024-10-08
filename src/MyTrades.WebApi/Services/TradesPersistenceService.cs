using Microsoft.EntityFrameworkCore;

namespace MyTrades.WebApi;

public class TradesPersistenceService : ITradesPersistenceService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IGenericRepository<Trade> tradeRepository;
    public TradesPersistenceService(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
        this.tradeRepository = this.unitOfWork.Repository<Trade>();
    }

    public async Task<TradeDto> AddTradeAsync(TradeDto trade)
    {
        var createdTrade = await tradeRepository.AddAsync(DtoToEntity(trade));
        await unitOfWork.SaveAsync();
        return EntityToDto(createdTrade);
    }

    public async Task<PagedEntityDto<TradeDto>> GetTradesAsync(
        int pageSize, 
        int pageNumber)
    {
        pageNumber = pageNumber == 0 ? 1 : Math.Abs(pageNumber);

        double totalInDecimal = tradeRepository.Entity.Count()/pageSize;
        var total = (int)Math.Ceiling(totalInDecimal);

        var pagedEntities = await tradeRepository.Entity
        .OrderByDescending(x => x.EntryDateTime)
        .Skip(pageSize * (pageNumber - 1))
        .Take(pageSize)
        .Select(x => EntityToDto(x))
        .ToListAsync();

        return new PagedEntityDto<TradeDto>(pageNumber, pageSize, total, pagedEntities);
    }

    public async Task UpdateTradeAsync(Guid id, TradeDto tradeDto)
    {
        var trade = DtoToEntity(tradeDto);
        trade.Id = id;
        await tradeRepository.UpdateAsync(id, trade);
        await unitOfWork.SaveAsync();
    }

    public async Task DeleteTradeAsync(Guid id)
    {
        await tradeRepository.DeleteAsync(id);
        await unitOfWork.SaveAsync();
    }    

    public async Task<TradeDto> GetTradeByIdAsync(Guid id)
    {
        var trade = await tradeRepository.GetByIdAsync(id);
        return EntityToDto(trade);
    }

    private static TradeDto EntityToDto(Trade trade)
    {
        return new TradeDto(
            trade.Id, 
            trade.TradedStock, 
            trade.Quantity, 
            trade.EntryPrice, 
            trade.ExitPrice, 
            trade.EntryDateTime, 
            trade.ExitDateTime, 
            trade.TradeDirection, 
            trade.TradeType);
    }

    private static Trade DtoToEntity(TradeDto tradeDto)
    {
        return new Trade
        {
            Id = tradeDto.Id,
            TradedStock = tradeDto.TradedStock,
            Quantity = tradeDto.Quantity,
            EntryPrice = tradeDto.EntryPrice,
            ExitPrice = tradeDto.ExitPrice,
            EntryDateTime = tradeDto.EntryDateTime,
            ExitDateTime = tradeDto.ExitDateTime,
            TradeDirection = tradeDto.TradeDirection,
            TradeType = tradeDto.TradeType
        };
    }
}

public interface ITradesPersistenceService
{
    Task<TradeDto> AddTradeAsync(TradeDto trade);
    Task<PagedEntityDto<TradeDto>> GetTradesAsync(int pageSize, int pageNumber);
    Task UpdateTradeAsync(Guid id, TradeDto trade);
    Task DeleteTradeAsync(Guid id);
    Task<TradeDto> GetTradeByIdAsync(Guid id);
}