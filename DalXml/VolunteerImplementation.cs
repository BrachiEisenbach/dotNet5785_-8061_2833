namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

internal class VolunteerImplementation : IVolunteer
{

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Create(Volunteer item)
    {
        List<Volunteer> volunteers = XMLTools.LoadListFromXMLSerializer<Volunteer>(Config.s_volunteers_xml);
        if (Read(item.Id) != null)
        {
            throw new DalAlreadyExistException("Volunteer", $"Volunteer with ID={item.Id} already exists");
        }
        volunteers.Add(item);
        XMLTools.SaveListToXMLSerializer(volunteers, Config.s_volunteers_xml);
    }


    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Delete(int id)
    {
        List<Volunteer> volunteers = XMLTools.LoadListFromXMLSerializer<Volunteer>(Config.s_volunteers_xml);
        if (volunteers.RemoveAll(it => it.Id == id) == 0)
            throw new DalDoesNotExistException("Volunteer", $"Volunteer with ID={id} does Not exist");
        XMLTools.SaveListToXMLSerializer(volunteers, Config.s_volunteers_xml);
    }


    [MethodImpl(MethodImplOptions.Synchronized)]
    public void DeleteAll()
    {
        XMLTools.SaveListToXMLSerializer(new List<Volunteer>(), Config.s_volunteers_xml);
    }


    [MethodImpl(MethodImplOptions.Synchronized)]
    public Volunteer? Read(int id)
    {
        List<Volunteer> volunteers = XMLTools.LoadListFromXMLSerializer<Volunteer>(Config.s_volunteers_xml);
        return volunteers.Find(it => it.Id == id);
    }


    [MethodImpl(MethodImplOptions.Synchronized)]
    public Volunteer? Read(Func<Volunteer, bool> filter)
    {
        List<Volunteer> volunteers = XMLTools.LoadListFromXMLSerializer<Volunteer>(Config.s_volunteers_xml);

        var volunteer = volunteers.FirstOrDefault(filter);
        if (volunteer == null)
            throw new DalDoesNotExistException("Volunteer", "No volunteer matches the given filter");

        return volunteer;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<Volunteer> ReadAll(Func<Volunteer, bool>? filter = null)
    {
        List<Volunteer> volunteers = XMLTools.LoadListFromXMLSerializer<Volunteer>(Config.s_volunteers_xml);

        if (filter == null)
            return volunteers;

        return volunteers.Where(filter);
    }


    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Update(Volunteer item)
    {
        List<Volunteer> volunteers = XMLTools.LoadListFromXMLSerializer<Volunteer>(Config.s_volunteers_xml);
        if (volunteers.RemoveAll(it => it.Id == item.Id) == 0)
            throw new DalDoesNotExistException("Volunteer", $"Volunteer with ID={item.Id} does Not exist");
        volunteers.Add(item);
        XMLTools.SaveListToXMLSerializer(volunteers, Config.s_volunteers_xml);
    }
}

    
