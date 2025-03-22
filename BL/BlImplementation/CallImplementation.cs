
using BlApi;
using BO;
using DalApi;

namespace BlImplementation
{
    internal class CallImplementation : ICall
    {
        private readonly DalApi.IDal _dal = DalApi.Factory.Get;
        public void AddCall(Call call)
        {
            throw new NotImplementedException();
        }

        public void cancelTreat(int volId, int callId)
        {
            throw new NotImplementedException();
        }

        public void chooseCall(int volId, int callId)
        {
            throw new NotImplementedException();
        }

        public void DeleteCall(int callId)
        {
            throw new NotImplementedException();
        }

        public int[] GetCallCountsByStatus()
        {
            throw new NotImplementedException();
        }

        public Call GetCallDetails(int callId)
        {
            throw new NotImplementedException();
        }

        public CallInList GetCallList(Enum? STATUS, object? valFilter, Enum? TYPEOFCALL)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ClosedCallInList> GetClosedCallInList(int volId, TYPEOFCALL? tOfCall)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<OpenCallInList> GetOpenCallInList(int volId, TYPEOFCALL? tOfCall, TYPEOFCALL? tOfCall2)
        {
            throw new NotImplementedException();
        }

        public void UpdateCallDetails(Call call)
        {
            throw new NotImplementedException();
        }

        public void UpdateCallStatus()
        {
            throw new NotImplementedException();
        }

        public void updateFinishTreat(int volId, int callId)
        {
            throw new NotImplementedException();
        }
    }
}
