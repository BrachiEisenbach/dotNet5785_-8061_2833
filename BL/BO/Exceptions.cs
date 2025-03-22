
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
