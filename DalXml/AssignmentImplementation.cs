namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

internal class AssignmentImplementation : IAssignment
{
    static Assignment getAssignment(XElement a)
    {
        

        return new DO.Assignment()
        {
            Id = a.ToIntNullable("Id") ?? throw new FormatException("can't convert id"),
            CallId = a.ToIntNullable("CallId") ?? throw new FormatException("can't convert id"),
            VolunteerId = a.ToIntNullable("VolunteerId") ?? throw new FormatException("can't convert id"),

            EntryTimeForTreatment = DateTime.TryParse(a.Element("EntryTimeForTreatment")?.Value, out var entryTime)
                ? entryTime
                : throw new FormatException("can't convert EntryTimeForTreatment"),

            EndTimeOfTreatment = DateTime.TryParse(a.Element("EndTimeOfTreatment")?.Value, out var endTime)
                ? endTime
                : null,

            TypeOfTreatment = a.ToEnumNullable<TYPEOFTREATMENT>("TypeOfTreatment") ?? TYPEOFTREATMENT.TREATE
        };
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Create(Assignment item)
    {
        int id = Config.NextAssignmentId;
        Assignment newAssignment = item with { Id = id };
        XElement assignmentRootElem = XMLTools.LoadListFromXMLElement(Config.s_assignments_xml);
        assignmentRootElem.Add(createAssignmentElement(newAssignment));
        XMLTools.SaveListToXMLElement(assignmentRootElem, Config.s_assignments_xml);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Delete(int id)
    {
        XElement assignmentRootElem = XMLTools.LoadListFromXMLElement(Config.s_assignments_xml);

        (assignmentRootElem.Elements().FirstOrDefault(ass => (int?)ass.Element("Id") == id)
         ?? throw new DO.DalDoesNotExistException("Assignment", $"Assignment with ID={id} does Not exist"))
            .Remove();

        XMLTools.SaveListToXMLElement(assignmentRootElem, Config.s_assignments_xml);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void DeleteAll()
    {
        XElement assignmentRootElem = XMLTools.LoadListFromXMLElement(Config.s_assignments_xml);

        assignmentRootElem.Elements().Remove();

        XMLTools.SaveListToXMLElement(assignmentRootElem, Config.s_assignments_xml);
    }


    [MethodImpl(MethodImplOptions.Synchronized)]
    public Assignment? Read(int id)
    {
        XElement? assignmentElem =
            XMLTools.LoadListFromXMLElement(Config.s_assignments_xml).Elements().FirstOrDefault(st => (int?)st.Element("Id") == id);
        return assignmentElem is null ? null : getAssignment(assignmentElem);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public Assignment? Read(Func<Assignment, bool> filter)
    {
        return XMLTools.LoadListFromXMLElement(Config.s_assignments_xml).Elements().Select(a => getAssignment(a)).FirstOrDefault(filter);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<Assignment> ReadAll(Func<Assignment, bool>? filter = null)
    {
        var allAssignments = XMLTools.LoadListFromXMLElement(Config.s_assignments_xml).Elements().Select(a => getAssignment(a));
        return filter == null ? allAssignments : allAssignments.Where(filter);
    }


    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Update(Assignment item)
    {
        XElement assignmentRootElem = XMLTools.LoadListFromXMLElement(Config.s_assignments_xml);

        (assignmentRootElem.Elements().FirstOrDefault(ass => (int?)ass.Element("Id") == item.Id)
         ?? throw new DO.DalDoesNotExistException("Assignment", $"Assignment with ID={item.Id} does Not exist"))
            .Remove();

        assignmentRootElem.Add(createAssignmentElement(item));  // תוקן כאן - בלי עטיפה כפולה

        XMLTools.SaveListToXMLElement(assignmentRootElem, Config.s_assignments_xml);
    }

    // פונקציה שממירה אובייקט assignment ל-XElement
    private XElement createAssignmentElement(Assignment item)
    {
        return new XElement("Assignment",
            new XElement("Id", item.Id),
            new XElement("CallId", item.CallId),
            new XElement("VolunteerId", item.VolunteerId),
            new XElement("EntryTimeForTreatment", item.EntryTimeForTreatment),
            new XElement("EndTimeOfTreatment", item.EndTimeOfTreatment?.ToString("o")),
            new XElement("TypeOfTreatment", item.TypeOfTreatment.ToString())
        );
    }
}
