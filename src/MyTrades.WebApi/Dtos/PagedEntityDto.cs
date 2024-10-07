namespace MyTrades.WebApi;

public record PagedEntityDto<T>(int pageNumber, int pageSize, int totalPages, List<T> data) where T : class;