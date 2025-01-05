
namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;

internal class VolunteerImplementation : IVolunteer
{
    public void Create(Volunteer item)
    {
        List<Volunteer> volunteers = XMLTools.LoadListFromXMLSerializer<Volunteer>(Config.s_volunteers_xml);
        if (Read(item.Id) != null)
        {
            throw new DalDoesNotExistException("Volunteer", "Volunteer with does Not exist");

        }
        volunteers.Add(item);
        XMLTools.SaveListToXMLSerializer(volunteers, Config.s_volunteers_xml);

    }

    public void Delete(int id)
    {
        List<Volunteer> volunteers = XMLTools.LoadListFromXMLSerializer<Volunteer>(Config.s_volunteers_xml);
        if (volunteers.RemoveAll(it => it.Id == id) == 0)
            throw new DalDoesNotExistException("Volunteer", $"Volunteer with ID={id} does Not exist");
        XMLTools.SaveListToXMLSerializer(volunteers, Config.s_volunteers_xml);

    }
    public void DeleteAll()
    {
        XMLTools.SaveListToXMLSerializer(new List<Volunteer>(), Config.s_volunteers_xml);
    }

    public Volunteer? Read(int id)
    {
        List<Volunteer> volunteers = XMLTools.LoadListFromXMLSerializer<Volunteer>(Config.s_volunteers_xml);

        return volunteers.Find(it => it.Id == id);
    }

    public Volunteer? Read(Func<Volunteer, bool> filter)
    {
        List<Volunteer> volunteers = XMLTools.LoadListFromXMLSerializer<Volunteer>(Config.s_volunteers_xml);

        return volunteers.FirstOrDefault(filter)
               ?? throw new DalDoesNotExistException("Volunteer", "No call matches the given filter");
    }
    public IEnumerable<Volunteer> ReadAll(Func<Volunteer, bool>? filter = null)
    {
        List<Volunteer> volunteers = XMLTools.LoadListFromXMLSerializer<Volunteer>(Config.s_volunteers_xml);

        if (filter == null)
            return volunteers;
        return volunteers.Where(filter);
    }

    public void Update(Volunteer item)
    {
        List<Volunteer> volunteers = XMLTools.LoadListFromXMLSerializer<Volunteer>(Config.s_volunteers_xml);
        if (volunteers.RemoveAll(it => it.Id == item.Id) == 0)
            throw new DalDoesNotExistException("Volunteer", $"Volunteer with ID={item.Id} does Not exist");
        volunteers.Add(item);
        XMLTools.SaveListToXMLSerializer(volunteers, Config.s_volunteers_xml);
    }
}
