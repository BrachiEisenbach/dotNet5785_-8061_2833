
namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

/// <summary>
/// A class for implementing the call's interface.
/// </summary>

public class CallImplementation : ICall
{

    /// <summary>
    /// A function to add a newly created call to the call list.
    /// </summary>
    /// <param name="item">Gets a call created in the main program.</param>

    public void Create(Call item)
    {
        int id = Config.S_NextCallId;
        Call newCall = item with { Id = id };
        DataSource.Calls.Add(newCall);
    }

    /// <summary>
    /// A function for deleting a call by ID.
    /// </summary>
    /// <param name="id">Gets a call ID.</param>
    /// <exception cref="Exception">Throws an error in case of empty input.</exception>

    public void Delete(int id)
    {
        if (Read(id) == null) {
            throw new Exception($" Call with this ID={id} not exists ");

        }

        DataSource.Calls.RemoveAll(i => i.Id == id);
    }

    /// <summary>
    /// A function to delete the entire list of data present in call type memory.
    /// </summary>

    public void DeleteAll()
    {
        DataSource.Calls.Clear();
    }

    /// <summary>
    /// A function to find and return a call by ID.
    /// </summary>
    /// <param name="id">call ID.</param>
    /// <returns>Returns the requested call.</returns>

    public Call? Read(int id)
    {
        return DataSource.Calls.Find(item => item.Id == id);

    }

    /// <summary>
    /// A function that returns a list of all call type data present in memory.
    /// </summary>
    /// <returns>Returns a list of data of the requested type.</returns>

    public List<Call> ReadAll()
    {
        List<Call?> c_Calls = DataSource.Calls;
        return new List<Call?>(c_Calls);
    }

    /// <summary>
    /// A function to update an existing call by ID.
    /// </summary>
    /// <param name="item">An existing call.</param>
    /// <exception cref="Exception">Returns an error in case of empty input.</exception>

    public void Update(Call item)
    {
        if (Read(item.Id) == null)
        {
            throw new Exception($" Call with ID={item.Id} not exists");

        }
        DataSource.Calls.RemoveAll(i => i.Id == item.Id);
        DataSource.Calls.Add(item);
    }
}
