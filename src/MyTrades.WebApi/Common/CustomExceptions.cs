namespace MyTrades.WebApi;
public class ApplicationException : Exception
{
    public ApplicationException(string message) : base(message)
    {
    }
}

public class DuplicateEntryException : ApplicationException
{
    public DuplicateEntryException(string message) : base(message)
    {
    }
}

public class EntityNotFoundException : ApplicationException
{
    public EntityNotFoundException(Guid id, string message) : base(message)
    {
    }

    public EntityNotFoundException(string message) : base(message)
    {
    }
}


