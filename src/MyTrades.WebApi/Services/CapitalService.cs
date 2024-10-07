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

    public async Task<Capital> AddCapitalAsync(Capital capital)
    {
        var createdEntity = await capitalRepository.AddAsync(capital);
        await unitOfWork.SaveAsync();
        return createdEntity;
    }

    public async Task<Capital?> GetCapitalAsync(Guid id)
    {
        logger.LogInformation("Test log");
        return await capitalRepository
                    .SingleOrDefaultAsync(x=> x.Id == id);
    }

    public async Task UpdateCapitalAsync(Guid id, Capital capital)
    {   
        await capitalRepository.UpdateAsync(id, capital);
        await unitOfWork.SaveAsync();
    }

    public async Task DeleteCapitalAsync(Guid id)
    {
        await capitalRepository.DeleteAsync(id);
        await unitOfWork.SaveAsync();
    }    

}

public interface ICapitalPersistenceService
{
    Task<Capital> AddCapitalAsync(Capital capital);
    Task<Capital?> GetCapitalAsync(Guid id);
    Task UpdateCapitalAsync(Guid id, Capital trade);
    Task DeleteCapitalAsync(Guid id);
}