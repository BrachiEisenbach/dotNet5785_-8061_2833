using System.Runtime.CompilerServices;

namespace Helpers;

/// <summary>
/// Internal BL manager for all Application's Clock logic policies
/// </summary>
internal static class AdminManager //stage 4
{
    #region Stage 4
    private static readonly DalApi.IDal s_dal = DalApi.Factory.Get; //stage 4
    #endregion Stage 4

    #region Stage 5
    internal static event Action? ConfigUpdatedObservers; //prepared for stage 5 - for config update observers
    internal static event Action? ClockUpdatedObservers; //prepared for stage 5 - for clock update observers
    #endregion Stage 5

    #region Stage 4
    /// <summary>
    /// Property for providing/setting current configuration variable value for any BL class that may need it
    /// </summary>
internal static TimeSpan  RiskRange
    {
        get => s_dal.Config.RiskRange;
        set
        {
            s_dal.Config.RiskRange = value;
            ConfigUpdatedObservers?.Invoke(); // stage 5
        }
    }
    /// <summary>
    /// Property for providing current application's clock value for any BL class that may need it
    /// </summary>
    internal static DateTime Now { get => s_dal.Config.Clock; } //stage 4

    internal static void ResetDB() //stage 4
    {
        lock (BlMutex) //stage 7
        {
            s_dal.ResetDB();
            AdminManager.UpdateClock(AdminManager.Now); //stage 5 - needed since we want the label on Pl to be updated
            AdminManager.RiskRange = AdminManager.RiskRange; // stage 5 - needed to update PL 
        }
    }

    internal static void InitializeDB() //stage 4
    {
        lock (BlMutex) //stage 7
        {
            DalTest.Initialization.Do();
            AdminManager.UpdateClock(AdminManager.Now);  //stage 5 - needed since we want the label on Pl to be updated
            AdminManager.RiskRange = AdminManager.RiskRange; // stage 5 - needed for update the PL 
        }
    }
    private static Task? _periodicTask = null;

    /// <summary>
    /// Method to perform application's clock from any BL class as may be required
    /// </summary>
    /// <param name="newClock">updated clock value</param>
   
        private static DateTime? _initialClockTime = null;
        
        private static bool _calledAfterTenYears = false; // דגל – שלא יקרה פעמיים

        internal static void UpdateClock(DateTime newClock)
        {
            var oldClock = s_dal.Config.Clock;

            if (_initialClockTime is null)
            {
                _initialClockTime = oldClock;
                Console.WriteLine($"[DEBUG] Initial clock time set to {_initialClockTime}");
            }

            s_dal.Config.Clock = newClock;

            double yearsPassed = (newClock - _initialClockTime.Value).TotalDays / 365.25;
            Console.WriteLine($"[DEBUG] Years passed since start: {yearsPassed:F2}");

            // אם עברו 10 שנים ועדיין לא זומנה המשימה – זימון
            if (!_calledAfterTenYears && yearsPassed >= 10)
            {
                Console.WriteLine("[DEBUG] 10 years passed – calling UpdateExpiredOpenCalls");
                _calledAfterTenYears = true;

                Task.Run(() => CallManager.UpdateExpiredOpenCalls(oldClock, newClock));
            Console.WriteLine($"[DEBUG] oldClock: {oldClock}, newClock: {newClock}");

        }

        // התנאי הקיים שלך
        if (_periodicTask is null || _periodicTask.IsCompleted)
            {
                Console.WriteLine("[DEBUG] _periodicTask is null or completed – assigning new task");
                _periodicTask = Task.Run(() => CallManager.UpdateExpiredOpenCalls(oldClock, newClock));
            }

            ClockUpdatedObservers?.Invoke();
        }
    

    #endregion Stage 4

    #region Stage 7 base

    /// <summary>    
    /// Mutex to use from BL methods to get mutual exclusion while the simulator is running
    /// </summary>
    internal static readonly object BlMutex = new (); // BlMutex = s_dal; // This field is actually the same as s_dal - it is defined for readability of locks
    /// <summary>
    /// The thread of the simulator
    /// </summary>
    private static volatile Thread? s_thread;
    /// <summary>
    /// The Interval for clock updating
    /// in minutes by second (default value is 1, will be set on Start())    
    /// </summary>
    private static int s_interval = 1;
    /// <summary>
    /// The flag that signs whether simulator is running
    /// 
    private static volatile bool s_stop = false;


    /// <summary>
    /// בודק אם סימולטור רץ כרגע, ואם כן זורק חריגה כדי למנוע ביצוע פעולה לא חוקית.
    /// </summary>
    /// <exception cref="BO.BLTemporaryNotAvailableException">נזרק כאשר הסימולטור רץ.</exception>
    [MethodImpl(MethodImplOptions.Synchronized)] // מונע גישה מקבילה כדי למנוע מצבים של Race Condition
    public static void ThrowOnSimulatorIsRunning()
    {
        if (s_thread is not null)
            throw new BO.BLTemporaryNotAvailableException("Cannot perform the operation since Simulator is running");
    }

    /// <summary>
    /// מפעיל את הסימולטור עם מרווח זמן נתון (בדקות).
    /// </summary>
    /// <param name="interval">המרווח בדקות לעדכון השעון בסימולטור.</param>
    [MethodImpl(MethodImplOptions.Synchronized)]
    internal static void Start(int interval)
    {
        if (s_thread is null)
        {
            s_interval = interval;
            s_stop = false;
            s_thread = new(clockRunner) { Name = "ClockRunner" };
            s_thread.Start();
        }
    }

    /// <summary>
    /// מפסיק את ריצת הסימולטור.
    /// </summary>
    [MethodImpl(MethodImplOptions.Synchronized)]
    internal static void Stop()
    {
        if (s_thread is not null)
        {
            s_stop = true;
            s_thread.Interrupt(); // מעיר את ה-thread אם הוא ישן
            s_thread.Name = "ClockRunner stopped";
            s_thread = null;
        }
    }

    private static Task? _simulateTask = null;

    /// <summary>
    /// לולאת ריצה של הסימולטור, מעדכנת את השעון ומריצה סימולציה של מתנדבים וקריאות.
    /// </summary>
    private static void clockRunner()
    {
        while (!s_stop)
        {
            // מעדכן את השעון על ידי הוספת אינטרבל לדקות לשעה הנוכחית
            UpdateClock(Now.AddMinutes(s_interval));

            // אם המשימה לא קיימת או שהסתיימה, מתחיל סימולציה חדשה
            if (_simulateTask is null || _simulateTask.IsCompleted) // stage 7
                _simulateTask = Task.Run(() => VolunteerManager.SimulateVolunteerAndCallLifecycle());

            try
            {
                Thread.Sleep(1000); // מחכה שנייה לפני החזרה ללולאה
            }
            catch (ThreadInterruptedException)
            {
                // טיפול בהפרעות שינה של ה-thread, פשוט ממשיכים
            }
        }
    }

    #endregion Stage 7 base
}
