
namespace BO;

//[Serializable]

//public class DalDoesNotExistException(string name, string? massage) : Exception(massage)
//{
//    public string EntityName { get; set; } = name;
//}

[Serializable]

public class BlException : Exception
{
    public BlException(string message) : base(message) { }
    public BlException(string message, Exception innerException) : base(message, innerException) { }
}

[Serializable]

public class BlDoesNotExistException : Exception
{
    public BlDoesNotExistException(string? message) : base(message) { }
    public BlDoesNotExistException(string message, Exception innerException)
                : base(message, innerException) { }
}

[Serializable]

public class BlArgumentException : Exception
{
    public BlArgumentException(string? message) : base(message) { }
    public BlArgumentException(string message, Exception innerException)
                : base(message, innerException) { }
}

[Serializable]

public class BlInvalidOperationException : Exception
{
    public BlInvalidOperationException(string? message) : base(message) { }
    public BlInvalidOperationException(string message, Exception innerException)
                : base(message, innerException) { }
}

[Serializable]

public class BlUnauthorizedException : Exception
{
    public BlUnauthorizedException(string? message) : base(message) { }
    public BlUnauthorizedException(string message, Exception innerException)
                : base(message, innerException) { }
}

[Serializable]

public class BlVolunteerInProgressException : Exception
{
    public BlVolunteerInProgressException(string? message) : base(message) { }
    public BlVolunteerInProgressException(string message, Exception innerException)
                : base(message, innerException) { }
}


[Serializable]

public class BlAlreadyExistException : Exception
{
    public BlAlreadyExistException(string? message) : base(message) { }
    public BlAlreadyExistException(string message, Exception innerException)
                : base(message, innerException) { }
}

public class BLTemporaryNotAvailableException : Exception
{
    public BLTemporaryNotAvailableException(string? message) : base(message) { }
    public BLTemporaryNotAvailableException(string message, Exception innerException)
                : base(message, innerException) { }
}
