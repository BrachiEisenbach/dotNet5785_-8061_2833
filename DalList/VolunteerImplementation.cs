
namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

public class VolunteerImplementation : IVolunteer
{
    public void Create(Volunteer item)

    {
        if (Read(item.Id) != null)
        {
            throw new Exception($" Volunteer with ID={item.Id} already exists");

        }
        DataSource.Volunteers.Add(item);
    }

    public void Delete(int id)
    {
        if (Read(id) == null)
        {
            throw new Exception($" Volunteer with ID={id} does'nt exists");

        }
        DataSource.Volunteers.RemoveAll(i => i.Id == id);
    }

    public void DeleteAll()
    {
        DataSource.Volunteers.Clear();
    }

    public Volunteer? Read(int id)
    {
        return DataSource.Volunteers.Find(item => item.Id == id);

        //throw new NotImplementedException();
    }

    public List<Volunteer> ReadAll()
    {
        List<Volunteer?> v_Volunteers = DataSource.Volunteers;
        return new List<Volunteer?>(v_Volunteers);
    }

    public void Update(Volunteer item)
    {
        if (Read(item.Id) == null)
        {
            throw new Exception($" Volunteer with ID={item.Id} does'nt exists");

        }
        DataSource.Volunteers.RemoveAll(i => i.Id == item.Id);
        DataSource.Volunteers.Add(item);
    }
}
