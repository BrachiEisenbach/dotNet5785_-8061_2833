
namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

public class CallImplementation : ICall
{
    public void Create(Call item)
    {
        int id = Config.S_NextCallId;
        Call newCall = item with { Id = id };
        DataSource.Calls.Add(newCall);
    }

    public void Delete(int id)
    {
        if (Read(id) == null) {
            throw new Exception($" Call with this ID={id} not exists ");

        }

        DataSource.Calls.RemoveAll(i => i.Id == id);
    }

    public void DeleteAll()
    {
        DataSource.Calls.Clear();
    }

    public Call? Read(int id)
    {
        return DataSource.Calls.Find(item => item.Id == id);

    }

    public List<Call> ReadAll()
    {
        List<Call?> c_Calls = DataSource.Calls;
        return new List<Call?>(c_Calls);
    }

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
