

namespace Dal;
/// <summary>
/// A static class that serves as the data source for the system's objects.
/// </summary>
internal static class DataSource
{
    /// <summary>
    /// A list of volunteers in the system. Each item in the list is an instance of Volunteer or null.
    /// </summary>
    internal static List<DO.Volunteer?> Volunteers { get; } = new();
    /// <summary>
    /// A list of call in the system. Each item in the list is an instance of Call or null.
    /// </summary>
    internal static List<DO.Call?> Calls { get; } = new();
    /// <summary>
    /// 
    /// A list of assignments in the system. Each item in the list is an instance of Assignments or null.
    /// </summary>
    internal static List<DO.Assignment?> Assignments { get; } = new();

}
