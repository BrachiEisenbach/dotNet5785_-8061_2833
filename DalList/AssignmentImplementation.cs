
namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

/// <summary>
/// A class for implementing the assignment's interface.
/// </summary>
public class AssignmentImplementation : IAssignment
{
    /// <summary>
    /// A function to add a newly created assignment to the assignment list.
    /// </summary>
    /// <param name="item">Gets a assignment created in the main program.</param>
    public void Create(Assignment item)
    {
        int id = Config.S_NextAssignmentId;
        Assignment newAssignment = item with { Id = id };
        DataSource.Assignments.Add(newAssignment);
    }

    /// <summary>
    /// A function for deleting a assignment by ID.
    /// </summary>
    /// <param name="id">Gets a assignment ID.</param>
    /// <exception cref="Exception">Throws an error in case of empty input.</exception>
    public void Delete(int id)
    {
        if (Read(id) == null)
        {
            throw new Exception($" Call with this ID={id} not exists ");

        }

        DataSource.Assignments.RemoveAll(i => i.Id == id);
    }

    /// <summary>
    /// A function to delete the entire list of data present in assignment type memory.
    /// </summary>
    public void DeleteAll()
    {
        DataSource.Assignments.Clear();
    }

    /// <summary>
    /// A function to find and return a assignment by ID.
    /// </summary>
    /// <param name="id">assignment ID.</param>
    /// <returns>Returns the requested assignment.</returns>
    public Assignment? Read(int id)
    {
        return DataSource.Assignments.Find(item => item.Id == id);

    }

    /// <summary>
    /// A function that returns a list of all assignment type data present in memory.
    /// </summary>
    /// <returns>Returns a list of data of the requested type.</returns>
    public List<Assignment> ReadAll()
    {
        List<Assignment?> c_Assignments = DataSource.Assignments;
        return new List<Assignment?>(c_Assignments);
    }

    /// <summary>
    /// A function to update an existing assignment by ID.
    /// </summary>
    /// <param name="item">An existing assignment.</param>
    /// <exception cref="Exception">Returns an error in case of empty input.</exception>
    public void Update(Assignment item)
    {
        if (Read(item.Id) == null)
        {
            throw new Exception($" Call with ID={item.Id} not exists");

        }
        DataSource.Assignments.RemoveAll(i => i.Id == item.Id);
        DataSource.Assignments.Add(item);
    }
}
