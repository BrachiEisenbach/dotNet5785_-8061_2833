using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;

namespace PL
{

    public class TypeOfCallCollection : IEnumerable
    {
        static readonly IEnumerable<BO.TYPEOFCALL> s_enums =
    (Enum.GetValues(typeof(BO.TYPEOFCALL)) as IEnumerable<BO.TYPEOFCALL>)!;

        public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
        public BO.TYPEOFCALL volunteerTypeOfCall { get; set; } = BO.TYPEOFCALL.NONE;



    //    static readonly IEnumerable<BO.TYPEOFDISTANCE> d_enums =
    //(Enum.GetValues(typeof(BO.TYPEOFDISTANCE)) as IEnumerable<BO.TYPEOFDISTANCE>)!;

    //    public IEnumerator GetEnumerator() => d_enums.GetEnumerator();
    //    public BO.TYPEOFDISTANCE volunteerTypeOfDistance { get; set; } = BO.TYPEOFDISTANCE.NONE;




    }
    


}
