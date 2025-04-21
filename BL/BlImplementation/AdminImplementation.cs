
using BlApi;
using BO;
using Helpers;

namespace BlImplementation
{
    internal class AdminImplementation : IAdmin
    {
        private readonly DalApi.IDal _dal = DalApi.Factory.Get;

        //done
        public void SetRiskRange(TimeSpan riskRange)
        {
            _dal.Config.RiskRange = riskRange;
        }

        //done
        public int GetRiskRange()
        {
            int riskRange;
            if (int.TryParse(_dal.Config.RiskRange.ToString(), out riskRange))
            { 
                return riskRange; 
            }
            return 0;
        }

        //done
        public DateTime GetTime()
        {
            return ClockManager.Now;
        }

        //done
        public void ClockPromotion(TIMEUNIT unit)
        {
            DateTime newTime = unit switch
            {
                BO.TIMEUNIT.MINUTE => ClockManager.Now.AddMinutes(1),
                BO.TIMEUNIT.HOUR => ClockManager.Now.AddHours(1),
                BO.TIMEUNIT.DAY => ClockManager.Now.AddDays(1),
                BO.TIMEUNIT.MONTH => ClockManager.Now.AddMonths(1),
                BO.TIMEUNIT.YEAR => ClockManager.Now.AddYears(1),
                _ => throw new ArgumentOutOfRangeException(nameof(unit), unit, null)
            };

        }

        // to do:!!!
        public void initializeDB()
        {
            
        }

        //done
        public void ResetDB()
        {
            try
            {
                _dal.Config.Reset();
                _dal.Volunteer.DeleteAll();
                _dal.Call.DeleteAll();
                _dal.Assignment.DeleteAll();
            }
            catch(Exception ex) 
            {
                throw new BlException("Error resetting database", ex);
            }
        }
    }
}
