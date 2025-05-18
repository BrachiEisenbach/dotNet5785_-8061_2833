
namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

/// <summary>
/// A class for implementing the volunteer's interface.
/// </summary>

internal class VolunteerImplementation : IVolunteer
{
    /// <summary>
    /// A function to add a newly created Volunteer to the Volunteer list.
    /// </summary>
    /// <param name="item">Gets a Volunteer created in the main program.</param>

    public void Create(Volunteer item)

    {
        if (Read(item.Id) != null)
        {
            throw new Exception($" Volunteer with ID={item.Id} already exists");

        }
        DataSource.Volunteers.Add(item);
    }

    /// <summary>
    /// A function for deleting a Volunteer by ID.
    /// </summary>
    /// <param name="id">Gets a Volunteer ID.</param>
    /// <exception cref="Exception">Throws an error in case of empty input.</exception>

    public void Delete(int id)
    {
        if (Read(id) == null)
        {
            throw new DalDoesNotExistException("Volunteer", $" Volunteer with ID={id} does'nt exists");

        }
        DataSource.Volunteers.RemoveAll(i => i.Id == id);
    }

    /// <summary>
    /// A function to delete the entire list of data present in Volunteer type memory.
    /// </summary>

    public void DeleteAll()
    {
        DataSource.Volunteers.Clear();
    }

    /// <summary>
    /// A function to find and return a Volunteer by ID.
    /// </summary>
    /// <param name="id">Volunteer ID.</param>
    /// <returns>Returns the requested assignment.</returns>
    public Volunteer? Read(Func<Volunteer, bool> filter) =>
    DataSource.Volunteers.FirstOrDefault(filter);

    public Volunteer? Read(int id)
    {
        var v=DataSource.Volunteers.Find(i => i.Id == id);
        return v;

    }

    /// <summary>
    /// A function that returns a list of all Volunteer type data present in memory.
    /// </summary>
    /// <returns>Returns a list of data of the requested type.</returns>

    //public List<Volunteer> ReadAll()
    //{
    //    List<Volunteer?> v_Volunteers = DataSource.Volunteers;
    //    return new List<Volunteer?>(v_Volunteers);
    //}

    public IEnumerable<Volunteer> ReadAll(Func<Volunteer, bool>? filter = null)
   => filter == null
        ? DataSource.Volunteers.Select(item => item)
        : DataSource.Volunteers.Where(filter);

    /// <summary>
    /// A function to update an existing Volunteer by ID.
    /// </summary>
    /// <param name="item">An existing Volunteer.</param>
    /// <exception cref="Exception">Returns an error in case of empty input.</exception>

    public void Update(Volunteer item)
    {
        if (Read(item.Id) == null)
        {
            throw new DalDoesNotExistException("Volunteer", $" Volunteer with ID={item.Id} does'nt exists");

        }
        DataSource.Volunteers.RemoveAll(i => i.Id == item.Id);
        DataSource.Volunteers.Add(item);
    }

}
