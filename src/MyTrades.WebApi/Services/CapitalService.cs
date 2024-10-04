using Microsoft.EntityFrameworkCore;

namespace MyTrades.WebApi;

public class CapitalPersistenceService : ICapitalPersistenceService
{
    private readonly ILogger<CapitalPersistenceService> logger;
    private readonly IUnitOfWork unitOfWork;
    private readonly IGenericRepository<Capital> capitalRepository;
    public CapitalPersistenceService(IUnitOfWork unitOfWork, ILogger<CapitalPersistenceService> logger)
    {
        this.logger = logger;
        this.unitOfWork = unitOfWork;
        this.capitalRepository = this.unitOfWork.Repository<Capital>();
    }

    public async Task AddCapitalAsync(Capital capital)
    {
        await capitalRepository.AddAsync(capital);
        await unitOfWork.SaveAsync();
    }

    public async Task<Capital?> GetCapitalAsync()
    {
        logger.LogInformation("Test log");
        return await capitalRepository
                    .SingleOrDefaultAsync(x=> true);
    }

    public async Task<bool> UpdateCapitalAsync(Guid id, Capital capital)
    {   
        capitalRepository.Update(capital);
        await unitOfWork.SaveAsync();
        return true;
    }

    public async Task<bool> DeleteCapitalAsync(Guid id)
    {
        await capitalRepository.DeleteAsync(id);
        await unitOfWork.SaveAsync();
        return true;
    }    

}

public interface ICapitalPersistenceService
{
    Task AddCapitalAsync(Capital capital);
    Task<Capital?> GetCapitalAsync();
    Task<bool> UpdateCapitalAsync(Guid id, Capital trade);
    Task<bool> DeleteCapitalAsync(Guid id);
}