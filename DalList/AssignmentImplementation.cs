
namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

public class AssignmentImplementation : IAssignment
{
    public void Create(Assignment item)
    {
        int id = Config.S_NextAssignmentId;
        Assignment newAssignment = item with { Id = id };
        DataSource.Assignments.Add(newAssignment);
    }

    public void Delete(int id)
    {
        if (Read(id) == null)
        {
            throw new Exception($" Call with this ID={id} not exists ");

        }

        DataSource.Assignments.RemoveAll(i => i.Id == id);
    }

    public void DeleteAll()
    {
        DataSource.Assignments.Clear();
    }

    public Assignment? Read(int id)
    {
        return DataSource.Assignments.Find(item => item.Id == id);

    }

    public List<Assignment> ReadAll()
    {
        List<Assignment?> c_Assignments = DataSource.Assignments;
        return new List<Assignment?>(c_Assignments);
    }

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
