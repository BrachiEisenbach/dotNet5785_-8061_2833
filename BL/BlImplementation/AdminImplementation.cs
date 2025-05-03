
using BlApi;
using BO;
using Helpers;

namespace BlImplementation
{
    internal class AdminImplementation : IAdmin
    {
        private readonly DalApi.IDal _dal = DalApi.Factory.Get;

        /// <summary>
        /// Sets the risk range for the system.
        /// </summary>
        /// <param name="riskRange">The new time span for the risk range.</param>

        public void SetRiskRange(TimeSpan riskRange)
        {
            _dal.Config.RiskRange = riskRange;
        }

        /// <summary>
        /// Gets the risk range from the system as an integer value.
        /// </summary>

        public int GetRiskRange()
        {
            int riskRange;
            if (int.TryParse(_dal.Config.RiskRange.ToString(), out riskRange))
            {
                return riskRange;
            }
            return 0;
        }

        /// <summary>
        /// Retrieves the current time from the system clock.
        /// </summary>

        public DateTime GetTime()
        {
            return ClockManager.Now;
        }

        /// <summary>
        /// Promotes the system's time by the specified unit (e.g., minute, hour, day, month, year).
        /// </summary>
        /// <param name="unit">The time unit to promote the system clock by (Minute, Hour, Day, Month, Year).</param>

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

        /// <summary>
        /// Initializes the database. This method is not implemented yet.
        /// </summary>

        public void initializeDB()
        {
            ResetDB();
        }

        /// <summary>
        /// Resets the database by clearing all relevant data.
        /// </summary>

        public void ResetDB()
        {
            try
            {
                _dal.Config.Reset();
                _dal.Volunteer.DeleteAll();
                _dal.Call.DeleteAll();
                _dal.Assignment.DeleteAll();
            }
            catch (Exception ex)
            {
                throw new BlException("Error resetting database", ex);
            }
        }
    }


}


