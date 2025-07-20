using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;


namespace BlApi
{
    public interface ICall : IObservable
    {
        public Dictionary<BO.STATUS, int> GetCallCountsByStatus();
        //public void UpdateCallStatus();
        public IEnumerable<BO.CallInList> GetCallList(Enum? STATUS, object? valFilter, Enum? TYPEOFCALL);
        public BO.Call GetCallDetails(int callId);
        public void UpdateCallDetails(BO.Call call);
        public void DeleteCall(int callId);
        public void AddCall(BO.Call call);
        public IEnumerable<ClosedCallInList> GetClosedCallInList(int volId, BO.TYPEOFCALL? tOfCall, BO.ClosedCallInListField? sortBy);
        public IEnumerable<BO.OpenCallInList> GetOpenCallInList(int volId, BO.TYPEOFCALL? tOfCall, BO.OpenCallInListField? sortBy);
        public void updateFinishTreat(int volId, int callId);
        public void cancelTreat(int volId, int assiId);
        public void ChooseCall(int volId, int callId);
    }
}
