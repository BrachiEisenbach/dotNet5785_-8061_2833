
namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

internal class AssignmentImplementation : IAssignment
{
    static Assignment getAssignment(XElement s)
    {
        throw new NotImplementedException();

        //return new DO.Assignment()
        //{
        //    Id = s.ToIntNullable("Id") ?? throw new FormatException("can't convert id"),
        //    CallId = s.ToIntNullable("CallId") ?? throw new FormatException("can't convert id"),
        //    VolunteerId = s.ToIntNullable("VolunteerId") ?? throw new FormatException("can't convert id"),
        //    EntryTimeForTreatment = (DateTime)s.Element("EntryTimeForTreatment"),
        //    EndTimeOfTreatment = (DateTime)s.Element("EndTimeOfTreatment"),
        //    TypeOfTreatment = TYPEOFTREATMENT.TREATE// s.ToDateTimeNullable<EndTimeOfTreatment>("TypeOfTreatment") ?? EndTimeOfTreatment.
        //};
    }

    public void Create(Assignment item)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        List<Assignment> Courses = XMLTools.LoadListFromXMLSerializer<Assignment>(Config.s_assignment_xml);
        if (Courses.RemoveAll(it => it.Id == id) == 0)
            throw new DalDoesNotExistException("Assignment", $"Course with ID={id} does Not exist");
        XMLTools.SaveListToXMLSerializer(Courses, Config.s_assignment_xml);
    }

    public void DeleteAll()
    {
        XMLTools.SaveListToXMLSerializer(new List<Assignment>(), Config.s_assignment_xml);
    }

    public Assignment? Read(int id)
    {
        XElement? studentElem =
    XMLTools.LoadListFromXMLElement(Config.s_students_xml).Elements().FirstOrDefault(st => (int?)st.Element("Id") == id);
        return studentElem is null ? null : getStudent(studentElem);
    }

    public Assignment? Read(Func<Assignment, bool> filter)
    {
        return XMLTools.LoadListFromXMLElement(Config.s_students_xml).Elements().Select(s => getStudent(s)).FirstOrDefault(filter);
    }


    public IEnumerable<Assignment> ReadAll(Func<Assignment, bool>? filter = null)
    {
        throw new NotImplementedException();
    }

    public void Update(Assignment item)
    {
        List<Assignment> Courses = XMLTools.LoadListFromXMLSerializer<Assignment>(Config.s_assignment_xml);
        if (Courses.RemoveAll(it => it.Id == item.Id) == 0)
            throw new DalDoesNotExistException("Assignment",$"Course with ID={item.Id} does Not exist");
        Courses.Add(item);
        XMLTools.SaveListToXMLSerializer(Courses, Config.s_assignment_xml);
    }
}
