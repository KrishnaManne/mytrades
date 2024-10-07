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

    public async Task<Trade> AddTradeAsync(Trade trade)
    {
        var createdTrade = await tradeRepository.AddAsync(trade);
        await unitOfWork.SaveAsync();
        return createdTrade;
    }

    public async Task<PagedEntityDto<Trade>> GetTradesAsync(
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
        .ToListAsync();

        return new PagedEntityDto<Trade>(pageNumber, pageSize, total, pagedEntities);
    }

    public async Task UpdateTradeAsync(Guid id, Trade trade)
    {
        trade.Id = id;
        await tradeRepository.UpdateAsync(id, trade);
        await unitOfWork.SaveAsync();
    }

    public async Task DeleteTradeAsync(Guid id)
    {
        await tradeRepository.DeleteAsync(id);
        await unitOfWork.SaveAsync();
    }    

    public async Task<Trade> GetTradeByIdAsync(Guid id)
    {
        return await tradeRepository.GetByIdAsync(id);
    }

}

public interface ITradesPersistenceService
{
    Task<Trade> AddTradeAsync(Trade trade);
    Task<PagedEntityDto<Trade>> GetTradesAsync(int pageSize, int pageNumber);
    Task UpdateTradeAsync(Guid id, Trade trade);
    Task DeleteTradeAsync(Guid id);
    Task<Trade> GetTradeByIdAsync(Guid id);
}