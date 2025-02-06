using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi
{
    public interface ICall
    {
        public int[] GetCallCountsByStatus();
        public void UpdateCallStatus();
        public BO.CallInList GetCallList(Enum? STATUS, object? valFilter, Enum? TYPEOFCALL);
        public BO.Call GetCallDetails(int callId);
        public void UpdateCallDetails(BO.Call call);
        public void DeleteCall(int callId);
        public void AddCall(BO.Call call);


    }
}
