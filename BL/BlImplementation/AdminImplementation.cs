
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
            AdminManager.ThrowOnSimulatorIsRunning();  //stage 7
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
            AdminManager.ThrowOnSimulatorIsRunning(); //stage 7
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
            AdminManager.ThrowOnSimulatorIsRunning(); //stage 7
            AdminManager.InitializeDB(); //stage 7            
        }

        /// <summary>
        /// Resets the database by clearing all relevant data.
        /// </summary>

        public void ResetDB()
        {
            try
            {
                AdminManager.ThrowOnSimulatorIsRunning(); //stage 7
                AdminManager.ResetDB(); //stage 7
                Console.WriteLine("The Data Base was reseted");

            }
            catch (Exception ex)
            {
                throw new BlException("Error resetting database", ex);
            }
        }

        #region Stage 5

        /// <summary>
        /// מוסיף מתבונן (Observer) שמופעל כאשר השעון מתעדכן.
        /// </summary>
        /// <param name="clockObserver">פעולה שתופעל בעת עדכון השעון.</param>
        public void AddClockObserver(Action clockObserver) =>
            AdminManager.ClockUpdatedObservers += clockObserver;

        /// <summary>
        /// מסיר מתבונן (Observer) הקשור לעדכון השעון.
        /// </summary>
        /// <param name="clockObserver">המתבונן שיש להסיר.</param>
        public void RemoveClockObserver(Action clockObserver) =>
            AdminManager.ClockUpdatedObservers -= clockObserver;

        /// <summary>
        /// מוסיף מתבונן (Observer) שמופעל כאשר קונפיגורציית המערכת מתעדכנת.
        /// </summary>
        /// <param name="configObserver">פעולה שתופעל בעת עדכון הקונפיגורציה.</param>
        public void AddConfigObserver(Action configObserver) =>
            AdminManager.ConfigUpdatedObservers += configObserver;

        /// <summary>
        /// מסיר מתבונן (Observer) הקשור לעדכון הקונפיגורציה.
        /// </summary>
        /// <param name="configObserver">המתבונן שיש להסיר.</param>
        public void RemoveConfigObserver(Action configObserver) =>
            AdminManager.ConfigUpdatedObservers -= configObserver;

        /// <summary>
        /// מגדיר את השעה הנוכחית במערכת.
        /// לפני ההגדרה, מבוצעת בדיקה שמוודאת שהסימולטור אינו רץ.
        /// מוסיף מתבונן שיבצע את העדכון בזמן המתאים.
        /// </summary>
        /// <param name="currentTime">תאריך ושעה לעדכון.</param>
        public void SetClock(DateTime currentTime)
        {
            AdminManager.ThrowOnSimulatorIsRunning(); // בודק אם הסימולטור רץ ומזריק חריגה במידה וכן
            AddClockObserver(() => AdminManager.UpdateClock(currentTime));
        }

        #endregion Stage 5

        /// <summary>
        /// מפעיל את הסימולטור עם מרווח זמן מוגדר (במילישניות).
        /// לפני ההפעלה, מתבצעת בדיקה שהסימולטור אינו רץ.
        /// </summary>
        /// <param name="interval">מרווח זמן בין פעולות הסימולטור.</param>
        public void StartSimulator(int interval)
        {
            AdminManager.ThrowOnSimulatorIsRunning(); // בודק אם הסימולטור כבר רץ ומונע הפעלה כפולה
            AdminManager.Start(interval); // מפעיל את הסימולטור עם הפרמטר שנשלח
        }

        /// <summary>
        /// מפסיק את ריצת הסימולטור.
        /// </summary>
        public void StopSimulator() => AdminManager.Stop();
    }

}


