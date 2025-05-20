
using BlApi;
using BO;
using Helpers;
using System.Diagnostics;

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
            AdminManager.RiskRange = riskRange;
            Debug.WriteLine("The riskRange was seted");


        }

        /// <summary>
        /// Gets the risk range from the system as an integer value.
        /// </summary>

        public TimeSpan GetRiskRange()
        {
            TimeSpan riskRange;
            if (TimeSpan.TryParse(AdminManager.RiskRange.ToString(), out riskRange))
            {
                return riskRange;
            }
            return TimeSpan.Zero;
        } 

        /// <summary>
        /// Retrieves the current time from the system clock.
        /// </summary>

        public DateTime GetClock()
        {
            return AdminManager.Now;
        }

        /// <summary>
        /// Promotes the system's time by the specified unit (e.g., minute, hour, day, month, year).
        /// </summary>
        /// <param name="unit">The time unit to promote the system clock by (Minute, Hour, Day, Month, Year).</param>

        public void ClockPromotion(TIMEUNIT unit)
        {
            DateTime newTime = unit switch
            {
                BO.TIMEUNIT.MINUTE => AdminManager.Now.AddMinutes(1),
                BO.TIMEUNIT.HOUR => AdminManager.Now.AddHours(1),
                BO.TIMEUNIT.DAY => AdminManager.Now.AddDays(1),
                BO.TIMEUNIT.MONTH => AdminManager.Now.AddMonths(1),
                BO.TIMEUNIT.YEAR => AdminManager.Now.AddYears(1),
                _ => throw new ArgumentOutOfRangeException(nameof(unit), unit, null)
            };
            AdminManager.UpdateClock(newTime);
            Console.WriteLine($"The clock is forward one {unit}.");

        }

        /// <summary>
        /// Initializes the database. This method is not implemented yet.
        /// </summary>

        public void InitializeDB()
        {
            ResetDB();
            AdminManager.InitializeDB();
            AdminManager.UpdateClock(AdminManager.Now);
            Console.WriteLine("The Data Base was initialized");
            
        }

        /// <summary>
        /// Resets the database by clearing all relevant data.
        /// </summary>

        public void ResetDB()
        {
            try
            {
                //_dal.Config.Reset();
                //_dal.Volunteer.DeleteAll();
                //_dal.Call.DeleteAll();
                //_dal.Assignment.DeleteAll();
                AdminManager.ResetDB();
                Console.WriteLine("The Data Base was reseted");

            }
            catch (Exception ex)
            {
                throw new BlException("Error resetting database", ex);
            }
        }

        #region Stage 5
        public void AddClockObserver(Action clockObserver) =>
        AdminManager.ClockUpdatedObservers += clockObserver;
        public void RemoveClockObserver(Action clockObserver) =>
        AdminManager.ClockUpdatedObservers -= clockObserver;
        public void AddConfigObserver(Action configObserver) =>
       AdminManager.ConfigUpdatedObservers += configObserver;
        public void RemoveConfigObserver(Action configObserver) =>
        AdminManager.ConfigUpdatedObservers -= configObserver;

        public void SetClock(DateTime currentTime)
        {
            AddClockObserver(() => AdminManager.UpdateClock(currentTime));
        }

        



        #endregion Stage 5
    }

}


