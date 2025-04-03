
namespace DO;

[Serializable]

public class DalDoesNotExistException(string name, string? massage) : Exception(massage)
{
    public string EntityName { get; set; } = name;
}

[Serializable]
public class DalAlreadyExistException(string name, string? massage) : Exception(massage)
{
    public string EntityName { get; set; } = name;
}

[Serializable]
public class DalXMLFileLoadCreateException( string? massage) : Exception(massage)
{
    //public string EntityName { get; set; } = name;
}


