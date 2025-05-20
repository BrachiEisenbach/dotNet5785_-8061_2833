using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL
{
    //internal class Enums
    //{   
    //}

        internal class SemestersCollection : IEnumerable
        {
            static readonly IEnumerable<BO.VOLUNTEERFIELDSORT> s_enums =
        (Enum.GetValues(typeof(BO.VOLUNTEERFIELDSORT)) as IEnumerable<BO.VOLUNTEERFIELDSORT>)!;

            public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
        }

}
