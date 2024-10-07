namespace MyTrades.WebApi;

public static class CapitalEndpoints
{
    public static void MapCapitalEndpoints(this WebApplication app)
    {
        const string CapitalApiGroupPath = "api/capital";
        var group = app.MapGroup(CapitalApiGroupPath)
                        .WithName("Capital")
                        .WithTags("Capital");
        
        group.MapGet("/{id:guid}", async (Guid id, ICapitalPersistenceService capitalPersistenceServiceService) => 
        {
            try
            {
                var result = await capitalPersistenceServiceService.GetCapitalAsync(id);
                return Results.Ok(result);
            }
            catch(ApplicationException appException)
            {
                if(appException is EntityNotFoundException)
                    return Results.NotFound();
                throw;
            }
        })  
        .WithName("GetCapital");

        group.MapPost("/", async (Capital capital, ICapitalPersistenceService capitalPersistenceServiceService) =>
        {            
            var createdCapital = await capitalPersistenceServiceService.AddCapitalAsync(capital);
            return Results.Created($"{CapitalApiGroupPath}/{createdCapital.Id}", createdCapital);
        })
        .WithName("PostCapital");

        group.MapPut("/{id:guid}", async (Guid id, Capital capital, ICapitalPersistenceService capitalPersistenceServiceService) =>
        {
            try
            {
                await capitalPersistenceServiceService.UpdateCapitalAsync(id, capital);
                return Results.NoContent();
            }
            catch(ApplicationException appException)
            {
                if(appException is EntityNotFoundException)
                    return Results.NotFound();
                throw;
            }
        })
        .WithName("UpdateCapital");

        group.MapDelete("/{id:guid}", async (Guid id, ICapitalPersistenceService capitalPersistenceServiceService) =>
        {
            try
            {
                await capitalPersistenceServiceService.DeleteCapitalAsync(id);
                return Results.NoContent();
            }
            catch(ApplicationException appException)
            {
                if(appException is EntityNotFoundException)
                    return Results.NotFound();
                throw;
            }
        })
        .WithName("DeleteCapital");
    }
}