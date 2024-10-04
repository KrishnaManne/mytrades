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

    public async Task AddTradeAsync(Trade trade)
    {
        await tradeRepository.AddAsync(trade);
        await unitOfWork.SaveAsync();
    }

    public async Task<List<Trade>> GetTradesAsync(
        int pageSize = 10, 
        int pageNumber = 1)
    {
        return await tradeRepository.Entity
        .OrderByDescending(x => x.EntryDateTime)
        .Skip(pageSize * ((pageNumber == 0 ? 1 : pageNumber) - 1))
        .Take(pageSize)
        .ToListAsync();
    }

    public async Task<bool> UpdateTradeAsync(Guid id, Trade trade)
    {        
        trade.Id = id;
        tradeRepository.Update(trade);
        await unitOfWork.SaveAsync();
        return true;
    }

    public async Task<bool> DeleteTradeAsync(Guid id)
    {
        await tradeRepository.DeleteAsync(id);
        await unitOfWork.SaveAsync();
        return true;
    }    

}

public interface ITradesPersistenceService
{
    Task AddTradeAsync(Trade trade);
    Task<List<Trade>> GetTradesAsync(int pageSize, int pageNumber);
    Task<bool> UpdateTradeAsync(Guid id, Trade trade);
    Task<bool> DeleteTradeAsync(Guid id);
}