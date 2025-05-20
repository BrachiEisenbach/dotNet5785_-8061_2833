using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;

namespace PL
{

    internal class TypeOfCallCollection : IEnumerable
    {
        static readonly IEnumerable<BO.TYPEOFCALL> s_enums =
    (Enum.GetValues(typeof(BO.TYPEOFCALL)) as IEnumerable<BO.TYPEOFCALL>)!;

        public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
        public BO.TYPEOFCALL volunteer { get; set; } = BO.TYPEOFCALL.REDRIVE;
    }

    //public class TypeOfCallCollection : IEnumerable<TYPEOFCALL>
    //{
    //    private readonly List<TYPEOFCALL> _items; // רשימה של TYPEOFCALL

    //    public TypeOfCallCollection()
    //    {
    //        _items = new List<TYPEOFCALL>
    //        {
    //            // הוסף את הפריטים כאן
    //        };
    //    }

    //    // מימוש של GetEnumerator עבור IEnumerable<T>
    //    public IEnumerator<TYPEOFCALL> GetEnumerator() => _items.GetEnumerator();

    //    // מימוש של GetEnumerator עבור IEnumerable
    //    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    //}
}
