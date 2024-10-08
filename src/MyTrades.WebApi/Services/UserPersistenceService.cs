namespace MyTrades.WebApi;

public class UserPersistenceService : IUserPersistenceService
{
    private readonly ILogger<UserPersistenceService> logger;
    private readonly IUnitOfWork unitOfWork;
    private readonly IGenericRepository<User> userRepository;
    public UserPersistenceService(IUnitOfWork unitOfWork, ILogger<UserPersistenceService> logger)
    {
        this.logger = logger;
        this.unitOfWork = unitOfWork;
        this.userRepository = this.unitOfWork.Repository<User>();
    }

    public async Task<UserDto> AddUserAsync(UserDto user)
    {
        var duplicateEntry = await userRepository
                    .SingleOrDefaultAsync(x=> x.Email.ToLower() == user.Email.ToLowerInvariant());
        if(duplicateEntry is not null)
            throw new DuplicateEntryException("User with the email already exists.");

        var createdEntity = await userRepository.AddAsync(DtoToEntity(user));
        await unitOfWork.SaveAsync();
        return EntityToDto(createdEntity);
    }

    public async Task<UserDto?> GetUserAsync(Guid id)
    {
        logger.LogInformation("Test log");
        var user = await userRepository
                    .SingleOrDefaultAsync(x=> x.Id == id);
        if (user is null) return null;
        return EntityToDto(user);
    }

    public async Task UpdateUserAsync(Guid id, UserDto user)
    {   
        await userRepository.UpdateAsync(id, DtoToEntity(user));
        await unitOfWork.SaveAsync();
    }

    public async Task UpdateUserAsync(Guid id, User user)
    {   
        await userRepository.UpdateAsync(id, user);
        await unitOfWork.SaveAsync();
    }

    public async Task DeleteUserAsync(Guid id)
    {
        await userRepository.DeleteAsync(id);
        await unitOfWork.SaveAsync();
    }    

    private static UserDto EntityToDto(User user)
    {
        return new UserDto(
            user.Id, 
            user.UserName!,
            user.Email!,
            user.EmailVerified);
    }

    private static User DtoToEntity(UserDto userDto)
    {
        return new User
        {
            Id = userDto.Id,
            Email = userDto.Email,
            UserName = userDto.UserName,
            EmailVerified = userDto.EmailVerified
        };
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        var user = await userRepository
                    .SingleOrDefaultAsync(x=> x.Email.ToLower() == email.ToLowerInvariant());
        if (user is null) return null;
        return user;
    }
}

public interface IUserPersistenceService
{
    Task<UserDto?> GetUserAsync(Guid id);
    Task<UserDto> AddUserAsync(UserDto userDto);
    Task UpdateUserAsync(Guid id, UserDto userDto);
    Task UpdateUserAsync(Guid id, User userDto);
    Task DeleteUserAsync(Guid id);
    Task<User?> GetUserByEmailAsync(string email);
}