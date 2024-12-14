
namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

/// <summary>
/// A class for implementing the call's interface.
/// </summary>

internal class CallImplementation : ICall
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
            throw new DalDoesNotExistException("Call", $" Call with this ID={id} not exists ");

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
    public Call? Read(Func<Call, bool> filter) =>
    DataSource.Calls.FirstOrDefault(filter);

    public Call? Read(int id)
    {
        return DataSource.Calls.Find(item => item.Id == id);

    }

    /// <summary>
    /// A function that returns a list of all call type data present in memory.
    /// </summary>
    /// <returns>Returns a list of data of the requested type.</returns>

    public IEnumerable<Call> ReadAll(Func<Call, bool>? filter = null)
           => filter == null
                ? DataSource.Calls.Select(item => item)
                : DataSource.Calls.Where(filter);
    /// <summary>
    /// A function to update an existing call by ID.
    /// </summary>
    /// <param name="item">An existing call.</param>
    /// <exception cref="Exception">Returns an error in case of empty input.</exception>

    public void Update(Call item)
    {
        if (Read(item.Id) == null)
        {
            throw new DalDoesNotExistException("Call", $" Call with ID={item.Id} not exists");

        }
        DataSource.Calls.RemoveAll(i => i.Id == item.Id);
        DataSource.Calls.Add(item);
    }
}
