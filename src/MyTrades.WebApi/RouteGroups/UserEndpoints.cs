namespace MyTrades.WebApi;

public static class UserManagementEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        const string userRouteGroupPath = "api/users";
        var group = app.MapGroup(userRouteGroupPath)
                        .WithName("User")
                        .WithTags("User");
        
        group.MapGet("/{id:guid}", async (Guid id, IUserPersistenceService userPersistenceService) => 
                {
                    try
                    {
                        var result = await userPersistenceService.GetUserAsync(id);
                        return Results.Ok(result);
                    }
                    catch(ApplicationException appException)
                    {
                        if(appException is EntityNotFoundException)
                            return Results.NotFound();
                        throw;
                    }
                })  
                .Produces<CapitalDto>(StatusCodes.Status200OK)
                .RequireAuthorization()
                .WithName("GetUser");
        
        group.MapPost("/register", async (UserDto userDto, IUserPersistenceService userPersistenceService) =>
                { 
                    try
                    {
                        var user = await userPersistenceService.AddUserAsync(userDto);
                        return Results.Created($"{userRouteGroupPath}/{user.Id}", user);
                    }           
                    catch(ApplicationException appException)
                    {
                        if(appException is DuplicateEntryException)
                            return Results.Conflict(appException.Message);
                        throw;
                    }
                })
                .Accepts<UserDto>(contentType: "application/json")
                .Produces<UserDto>(StatusCodes.Status201Created)
                .WithName("RegisterUser");

        group.MapPost("/signin", async (SignInDto signInDto, ISignInManagementService signInManagementService) =>
                {
                    try{
                        await signInManagementService.SignInUserAsync(signInDto);
                        return Results.Ok();
                    }            
                    catch(ApplicationException appException)
                    {
                        if(appException is EntityNotFoundException)
                            return Results.NotFound(appException.Message);
                        throw;
                    }
                })
                .Accepts<UserDto>(contentType: "application/json")
                .Produces(StatusCodes.Status204NoContent)
                .WithName("SignIn");

        group.MapPost("/verifyotp", async (VerifyOtpDto verifyOtpDto, ISignInManagementService signInManagementService) =>
                {            
                    try
                    {
                        var tokenResponse = await signInManagementService.VerifyOtp(verifyOtpDto);
                        return Results.Ok(tokenResponse);
                    }            
                    catch(ApplicationException appException)
                    {
                        if(appException is EntityNotFoundException)
                            return Results.NotFound(appException.Message);
                        throw;
                    }
                })
                .Accepts<VerifyOtpDto>(contentType: "application/json")
                .Produces<VerifyOtpResponseDto>(StatusCodes.Status200OK)
                .WithName("VerifyOtp");
    }   
    
}