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

    public async Task<CapitalDto> AddCapitalAsync(CapitalDto capital)
    {
        var createdEntity = await capitalRepository.AddAsync(DtoToEntity(capital));
        await unitOfWork.SaveAsync();
        return EntityToDto(createdEntity);
    }

    public async Task<CapitalDto?> GetCapitalAsync(Guid id)
    {
        logger.LogInformation("Test log");
        var capital = await capitalRepository
                    .SingleOrDefaultAsync(x=> x.Id == id);
        if (capital is null) return null;
        return EntityToDto(capital);
    }

    public async Task UpdateCapitalAsync(Guid id, CapitalDto capital)
    {   
        await capitalRepository.UpdateAsync(id, DtoToEntity(capital));
        await unitOfWork.SaveAsync();
    }

    public async Task DeleteCapitalAsync(Guid id)
    {
        await capitalRepository.DeleteAsync(id);
        await unitOfWork.SaveAsync();
    }    

    private static CapitalDto EntityToDto(Capital capital)
    {
        return new CapitalDto(
            capital.Id, 
            capital.Amount);
    }

    private static Capital DtoToEntity(CapitalDto capitalDto)
    {
        return new Capital
        {
            Id = capitalDto.Id,
            Amount = capitalDto.Amount
        };
    }

}

public interface ICapitalPersistenceService
{
    Task<CapitalDto> AddCapitalAsync(CapitalDto capitalDto);
    Task<CapitalDto?> GetCapitalAsync(Guid id);
    Task UpdateCapitalAsync(Guid id, CapitalDto capitalDto);
    Task DeleteCapitalAsync(Guid id);
}