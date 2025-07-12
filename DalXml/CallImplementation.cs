
namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

internal class CallImplementation : ICall
{

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Create(Call item)
    {

        int id = Config.NextCallId;
        Call newCall = item with { Id = id };
        List<Call> calls = XMLTools.LoadListFromXMLSerializer<Call>(Config.s_calls_xml);
        calls.Add(newCall);
        XMLTools.SaveListToXMLSerializer(calls, Config.s_calls_xml);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Delete(int id)
    {

        List<Call> calls = XMLTools.LoadListFromXMLSerializer<Call>(Config.s_calls_xml);
        if (calls.RemoveAll(it => it.Id == id) == 0)
            throw new DalDoesNotExistException("call",$"Call with ID={id} does Not exist");
        XMLTools.SaveListToXMLSerializer(calls, Config.s_calls_xml);
    }

    public void DeleteAll()
    {
        XMLTools.SaveListToXMLSerializer(new List<Call>(), Config.s_calls_xml);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public Call Read(int id)
    {
        List<Call> calls = XMLTools.LoadListFromXMLSerializer<Call>(Config.s_calls_xml);

        return calls.Find(it => it.Id == id);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public Call Read(Func<Call, bool> filter)
    {
        List<Call> calls = XMLTools.LoadListFromXMLSerializer<Call>(Config.s_calls_xml);

        return calls.FirstOrDefault(filter)
               ?? throw new DalDoesNotExistException("Call", "No call matches the given filter");
    }

    
    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<Call> ReadAll(Func<Call, bool>? filter = null)
    {
        List<Call> calls = XMLTools.LoadListFromXMLSerializer<Call>(Config.s_calls_xml);

        if (filter == null)
            return calls;
        return calls.Where(filter);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Update(Call item)
    {

        List<Call> calls = XMLTools.LoadListFromXMLSerializer<Call>(Config.s_calls_xml);
        if (calls.RemoveAll(it => it.Id == item.Id) == 0)
            throw new DalDoesNotExistException("Call",$"Call with ID={item.Id} does Not exist");
        calls.Add(item);
        XMLTools.SaveListToXMLSerializer(calls, Config.s_calls_xml);
        
    }
}
