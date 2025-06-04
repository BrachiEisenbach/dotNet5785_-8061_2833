using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BO;

namespace PL;

public class TypeOfCallCollection :  IEnumerable
{
    static readonly IEnumerable<BO.TYPEOFCALL> s_enums =
        (Enum.GetValues(typeof(BO.TYPEOFCALL)) as IEnumerable<BO.TYPEOFCALL>)!;

    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
    public BO.TYPEOFCALL volunteerTypeOfCall { get; set; } = BO.TYPEOFCALL.NONE;
}

public class RoleCollection :  IEnumerable
{
    static readonly IEnumerable<BO.ROLE> s_enums =
        (Enum.GetValues(typeof(BO.ROLE)) as IEnumerable<BO.ROLE>)!;

    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
   
}

public class TypeOfDistanceCollection : IEnumerable
{
    static readonly IEnumerable<BO.TYPEOFDISTANCE> s_enums =
        (Enum.GetValues(typeof(BO.TYPEOFDISTANCE)) as IEnumerable<BO.TYPEOFDISTANCE>)!;

    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
}
public class StatusCollection : IEnumerable
{
    static readonly IEnumerable<BO.STATUS> s_enums =
        (Enum.GetValues(typeof(BO.STATUS)) as IEnumerable<BO.STATUS>)!;

    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
}


