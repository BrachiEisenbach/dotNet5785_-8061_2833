
using BO;
using DO;
using PL.Call;
using PL.Vol;
using System;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        private int id = 0;

        private CallListWindow? _callsWindow;
        private VolunteerListWindow? _volunteersWindow;

        private bool _isSimulatorRunning = false;
        public bool IsSimulatorRunning
        {
            get => _isSimulatorRunning;
            set
            {
                if (_isSimulatorRunning != value)
                {
                    _isSimulatorRunning = value;
                    OnPropertyChanged(new DependencyPropertyChangedEventArgs(
                        IsSimulatorRunningProperty, !_isSimulatorRunning, _isSimulatorRunning));
                }
            }
        }

        public static readonly DependencyProperty IsSimulatorRunningProperty =
            DependencyProperty.Register("IsSimulatorRunning", typeof(bool), typeof(AdminWindow));

        public string SimulatorButtonText
        {
            get { return (string)GetValue(SimulatorButtonTextProperty); }
            set { SetValue(SimulatorButtonTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SimulatorButtonText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SimulatorButtonTextProperty =
            DependencyProperty.Register("SimulatorButtonText", typeof(string), typeof(AdminWindow));

        public int Interval
        {
            get { return (int)GetValue(IntervalProperty); }
            set { SetValue(IntervalProperty, value); }
        }


        public static readonly DependencyProperty IntervalProperty =
            DependencyProperty.Register("Interval", typeof(int), typeof(AdminWindow));
        public bool SimFlag
        {
            get { return (bool)GetValue(SimFlagProperty); }
            set { SetValue(SimFlagProperty, value); }
        }


        public static readonly DependencyProperty SimFlagProperty =
            DependencyProperty.Register("SimFlag", typeof(bool), typeof(AdminWindow));


        public BO.Volunteer CurrentVolunteer
        {
            get { return (BO.Volunteer)GetValue(CurrentVolunteerProperty); }
            set { SetValue(CurrentVolunteerProperty, value); }
        }
        public static readonly DependencyProperty CurrentVolunteerProperty =
            DependencyProperty.Register("CurrentVolunteer", typeof(BO.Volunteer), typeof(AdminWindow), new PropertyMetadata(null));


        public DateTime CurrentTime
        {
            get { return (DateTime)GetValue(CurrentTimeProperty); }
            set { SetValue(CurrentTimeProperty, value); }
        }
        public static readonly DependencyProperty CurrentTimeProperty =
        DependencyProperty.Register("CurrentTime", typeof(DateTime), typeof(AdminWindow));




        public DateTime Clock
        {
            get { return (DateTime)GetValue(ClockProperty); }
            set { SetValue(ClockProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ClockProperty =
            DependencyProperty.Register("Clock", typeof(DateTime), typeof(AdminWindow));

        public TimeSpan RiskRange
        {
            get { return (TimeSpan)GetValue(RiskRangeProperty); }
            set { SetValue(RiskRangeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RiskRangeProperty =
            DependencyProperty.Register("RiskRange", typeof(TimeSpan), typeof(AdminWindow));
        public int InTreatmentCount
        {
            get => (int)GetValue(InTreatmentCountProperty);
            set => SetValue(InTreatmentCountProperty, value);
        }
        public static readonly DependencyProperty InTreatmentCountProperty =
            DependencyProperty.Register("InTreatmentCount", typeof(int), typeof(AdminWindow));

        public int OpenCount
        {
            get => (int)GetValue(OpenCountProperty);
            set => SetValue(OpenCountProperty, value);
        }
        public static readonly DependencyProperty OpenCountProperty =
            DependencyProperty.Register("OpenCount", typeof(int), typeof(AdminWindow));

        public int ExpiredCount
        {
            get => (int)GetValue(ExpiredCountProperty);
            set => SetValue(ExpiredCountProperty, value);
        }
        public static readonly DependencyProperty ExpiredCountProperty =
            DependencyProperty.Register("ExpiredCount", typeof(int), typeof(AdminWindow));

        private void clockObserver()
        {
            Dispatcher.Invoke(() =>
            {
                SetCurrentValue(ClockProperty, s_bl.Admin.GetClock());
            });
        }

        private void configObserver()
        {
            RiskRange = s_bl.Admin.GetRiskRange();
        }




        private void btnAddOneMinute_Click(object sender, RoutedEventArgs e)
        {
            s_bl.Admin.ClockPromotion(BO.TIMEUNIT.MINUTE);
        }
        private void btnAddOneDay_Click(object sender, RoutedEventArgs e)
        {
            s_bl.Admin.ClockPromotion(BO.TIMEUNIT.DAY);
        }
        private void btnAddOneHour_Click(object sender, RoutedEventArgs e)
        {
            s_bl.Admin.ClockPromotion(BO.TIMEUNIT.HOUR);
        }
        private void btnAddOneMonth_Click(object sender, RoutedEventArgs e)
        {
            s_bl.Admin.ClockPromotion(BO.TIMEUNIT.MONTH);
        }
        private void btnAddOneYear_Click(object sender, RoutedEventArgs e)
        {
            s_bl.Admin.ClockPromotion(BO.TIMEUNIT.YEAR);
        }

        public AdminWindow(int AdminId)
        {
            InitializeComponent();
            this.DataContext = this;
            id = AdminId;
            try
            {
                this.CurrentVolunteer = s_bl.Volunteer.GetVolunteerDetails(AdminId);
            }
            catch (BO.BlDoesNotExistException ex)
            {
                MessageBox.Show($"Volunteer with ID {AdminId} not found: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }

            UpdateCallStatusCounts();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                s_bl.Admin.AddClockObserver(clockObserver);
                s_bl.Admin.AddConfigObserver(configObserver);
                Clock = s_bl.Admin.GetClock();
                RiskRange = s_bl.Admin.GetRiskRange();
                CurrentTime = s_bl.Admin.GetClock();
                UpdateCallStatusCounts();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during window load: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            try
            {
                s_bl.Admin.RemoveClockObserver(clockObserver);
                s_bl.Admin.RemoveConfigObserver(configObserver);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during window close: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void btnUpdate_RiskRange(object sender, RoutedEventArgs e)
        {
            s_bl.Admin.SetRiskRange(RiskRange);
        }

        private void btnUpdate_Clock(object sender, RoutedEventArgs e)
        {
            s_bl.Admin.SetClock(Clock);

        }
        private void btnVolunteers_Click(object sender, RoutedEventArgs e)
        {
            if (_volunteersWindow == null || !_volunteersWindow.IsVisible)
            {
                _volunteersWindow = new VolunteerListWindow(id);
                _volunteersWindow.Closed += (s, e) => _volunteersWindow = null;
                _volunteersWindow.Show();
            }
            else
            {
                _volunteersWindow.Activate();
            }
        }
        private void btnCalls_Click(object sender, RoutedEventArgs e)
        {
            if (_callsWindow == null || !_callsWindow.IsVisible)
            {
                _callsWindow = new CallListWindow(CurrentVolunteer.Id);
                _callsWindow.Closed += (s, e) => _callsWindow = null;
                _callsWindow.Show();
            }
            else
            {
                _callsWindow.Activate();
            }

        }
        private void btnInitialize_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Confirmation message to the user
                MessageBoxResult result = MessageBox.Show("Are you sure you want to initialize the database?",
                                                          "Initialization Confirmation",
                                                          MessageBoxButton.YesNo,
                                                          MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    // Close all open windows (except the main window)
                    CloseOpenWindows();
                }
                Mouse.OverrideCursor = Cursors.Wait;
                s_bl.Admin.InitializeDB();
                UpdateCallStatusCounts();
                Mouse.OverrideCursor = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing DB: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }

        }
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to reset the database?",
                                                   "Reset Confirmation",
                                                   MessageBoxButton.YesNo,
                                                   MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    // Close all open windows
                    CloseOpenWindows();
                }
                Mouse.OverrideCursor = Cursors.Wait;
                s_bl.Admin.ResetDB();
                UpdateCallStatusCounts();
                Mouse.OverrideCursor = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error reset DB: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }

        }
        private void UpdateCallStatusCounts()
        {
            try
            {
                var counts = s_bl.Call.GetCallCountsByStatus();

                InTreatmentCount = counts.ContainsKey(BO.STATUS.InTreatment) ? counts[BO.STATUS.InTreatment] : 0;
                OpenCount = counts.ContainsKey(BO.STATUS.Open) ? counts[BO.STATUS.Open] : 0;
                ExpiredCount = counts.ContainsKey(BO.STATUS.Expired) ? counts[BO.STATUS.Expired] : 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating call counts: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void CloseOpenWindows()
        {
            // סגירת כל החלונות הפתוחים חוץ מהחלון הראשי
            foreach (Window window in Application.Current.Windows)
            {
                if (window != this) // 'this' הוא החלון הראשי
                {
                    window.Close();
                }
            }
        }
        private void btnInTreatmentCalls_Click(object sender, RoutedEventArgs e)
        {
            if (_callsWindow == null || !_callsWindow.IsVisible)
            {
                _callsWindow = new CallListWindow(BO.STATUS.InTreatment); // <-- סינון לפי סטטוס
                _callsWindow.Closed += (s, e) => _callsWindow = null;
                _callsWindow.Show();
            }
            else
            {
                _callsWindow.Activate();
            }
        }
        private void btnOpenCalls_Click(object sender, RoutedEventArgs e)
        {
            if (_callsWindow == null || !_callsWindow.IsVisible)
            {
                _callsWindow = new CallListWindow(BO.STATUS.Open); // <-- סינון לפי סטטוס
                _callsWindow.Closed += (s, e) => _callsWindow = null;
                _callsWindow.Show();
            }
            else
            {
                _callsWindow.Activate();
            }
        }

        private void btnExpired_Click(object sender, RoutedEventArgs e)
        {
            if (_callsWindow == null || !_callsWindow.IsVisible)
            {
                _callsWindow = new CallListWindow(BO.STATUS.Expired); // <-- סינון לפי סטטוס
                _callsWindow.Closed += (s, e) => _callsWindow = null;
                _callsWindow.Show();
            }
            else
            {
                _callsWindow.Activate();
            }
        }
    }
}


//        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

//        private bool _isSimulatorRunning = false;
//        public bool IsSimulatorRunning
//        {
//            get => _isSimulatorRunning;
//            set
//            {
//                if (_isSimulatorRunning != value)
//                {
//                    _isSimulatorRunning = value;
//                    OnPropertyChanged(new DependencyPropertyChangedEventArgs(
//                        IsSimulatorRunningProperty, !_isSimulatorRunning, _isSimulatorRunning));
//                }
//            }
//        }

//        public static readonly DependencyProperty IsSimulatorRunningProperty =
//            DependencyProperty.Register("IsSimulatorRunning", typeof(bool), typeof(AdminWindow));

//        public string SimulatorButtonText
//        {
//            get { return (string)GetValue(SimulatorButtonTextProperty); }
//            set { SetValue(SimulatorButtonTextProperty, value); }
//        }

//        // Using a DependencyProperty as the backing store for SimulatorButtonText.  This enables animation, styling, binding, etc...
//        public static readonly DependencyProperty SimulatorButtonTextProperty =
//            DependencyProperty.Register("SimulatorButtonText", typeof(string), typeof(AdminWindow));

//        public int Interval
//        {
//            get { return (int)GetValue(IntervalProperty); }
//            set { SetValue(IntervalProperty, value); }
//        }


//        public static readonly DependencyProperty IntervalProperty =
//            DependencyProperty.Register("Interval", typeof(int), typeof(AdminWindow));
//        public bool SimFlag
//        {
//            get { return (bool)GetValue(SimFlagProperty); }
//            set { SetValue(SimFlagProperty, value); }
//        }


//        public static readonly DependencyProperty SimFlagProperty =
//            DependencyProperty.Register("SimFlag", typeof(bool), typeof(AdminWindow));

//        public DateTime Clock
//        {
//            get { return (DateTime)GetValue(ClockProperty); }
//            set { SetValue(ClockProperty, value); }
//        }
//        //לא ידעתי אםצריך לעשות שגם השעון יהיה נתוןלעדכון ידני אז עשיתי בכל זאת
//        public static readonly DependencyProperty ClockProperty =
//            DependencyProperty.Register("Clock", typeof(DateTime), typeof(AdminWindow));



//        public TimeSpan RiskRange
//        {
//            get { return (TimeSpan)GetValue(RiskRangeProperty); }
//            set { SetValue(RiskRangeProperty, value); }
//        }

//        // Using a DependencyProperty as the backing store for RiskRange.  This enables animation, styling, binding, etc...
//        public static readonly DependencyProperty RiskRangeProperty =
//            DependencyProperty.Register("RiskRange", typeof(TimeSpan), typeof(AdminWindow));



//        private volatile DispatcherOperation? _observerOperation = null; //stage 7


//        private void clockObserver()

//        {
//            if (_observerOperation is null || _observerOperation.Status == DispatcherOperationStatus.Completed)
//                _observerOperation = Dispatcher.BeginInvoke(() =>
//                {
//                    Clock = s_bl.Admin.GetClock();
//                });

//        }
//        private void configObserver()
//        {

//            if (_observerOperation is null || _observerOperation.Status == DispatcherOperationStatus.Completed)
//                _observerOperation = Dispatcher.BeginInvoke(() =>
//                {
//                    RiskRange = s_bl.Admin.GetMaxRange();
//                });

//        }
//        //private void Loaded()
//        //{
//        //}



//        private void btnAddOneMinute_Click(object sender, RoutedEventArgs e)
//        {
//            s_bl.Admin.ForwardClock(BO.TimeUnit.Minute);
//        }
//        private void btnAddOneHour_Click(object sender, RoutedEventArgs e)
//        {
//            s_bl.Admin.ForwardClock(BO.TimeUnit.Hour);
//        }
//        private void btnAddOneDay_Click(object sender, RoutedEventArgs e)
//        {
//            s_bl.Admin.ForwardClock(BO.TimeUnit.Day);
//        }
//        private void btnAddOneMonth_Click(object sender, RoutedEventArgs e)
//        {
//            s_bl.Admin.ForwardClock(BO.TimeUnit.Month);
//        }
//        private void btnAddOneYear_Click(object sender, RoutedEventArgs e)
//        {
//            s_bl.Admin.ForwardClock(BO.TimeUnit.Year);
//        }

//        public BO.Volunteer? CurrentAdmin
//        {
//            get { return (BO.Volunteer?)GetValue(CurrentAdminProperty); }
//            set { SetValue(CurrentAdminProperty, value); }
//        }

//        // Using a DependencyProperty as the backing store for CurrentVolunteer.  This enables animation, styling, binding, etc...
//        public static readonly DependencyProperty CurrentAdminProperty =
//            DependencyProperty.Register("CurrentVolunteer", typeof(BO.Volunteer), typeof(AdminWindow));
//        public AdminWindow(int aid)
//        {

//            InitializeComponent();
//            CurrentAdmin = (aid != 0) ? s_bl.Volunteer.Read(aid)! : new BO.Volunteer() { };
//            SimulatorButtonText = "Start Simulator";

//        }



//        private void Update_RiskRange(object sender, RoutedEventArgs e)
//        {
//            s_bl.Admin.SetMaxRange(RiskRange);
//        }

//        private void Update_Clock(object sender, RoutedEventArgs e)
//        {
//            s_bl.Admin.SetClock(Clock);

//        }

//        private void Handle_Volunteers(object sender, RoutedEventArgs e)
//        {
//            new PL.Volunteer.VolunteerListWindow().Show();
//        }


//        async private void Initialize_DB(object sender, RoutedEventArgs e)
//        {
//            // Confirmation message to the user
//            MessageBoxResult result = MessageBox.Show("Are you sure you want to initialize the database?",
//                                                      "Initialization Confirmation",
//                                                      MessageBoxButton.YesNo,
//                                                      MessageBoxImage.Question);

//            if (result == MessageBoxResult.Yes)
//            {
//                // Logic to initialize the database
//                // For example: ResetDatabase();

//                // Close all open windows (except the main window)
//                CloseOpenWindows();
//            }

//            try
//            {
//                //there is no sandclock icon i used a different icon
//                Mouse.OverrideCursor = Cursors.Wait; // שינוי לאייקון של שעון חול

//                // כאן תכניס את הקוד לאיפוס בסיס הנתונים


//                s_bl.Admin.ResetDB();
//                s_bl.Admin.InitializeDB(); // ד simulate איפוס בסיס נתונים

//            }
//            finally
//            {
//                Mouse.OverrideCursor = null; // החזר את האייקון המקורי

//            }


//        }

//        private void Reset_DB(object sender, RoutedEventArgs e)
//        {
//            MessageBoxResult result = MessageBox.Show("Are you sure you want to reset the database?",
//                                               "Reset Confirmation",
//                                               MessageBoxButton.YesNo,
//                                               MessageBoxImage.Question);

//            if (result == MessageBoxResult.Yes)
//            {
//                // Logic to reset the database
//                // For example: ClearDatabase();

//                // Close all open windows
//                CloseOpenWindows();
//            }
//            try
//            {
//                //there is no sandclock icon i used a different icon
//                Mouse.OverrideCursor = Cursors.Wait; // שינוי לאייקון של שעון חול

//                // כאן תכניס את הקוד לאיפוס בסיס הנתונים


//                s_bl.Admin.ResetDB();

//            }
//            finally
//            {
//                Mouse.OverrideCursor = null; // החזר את האייקון המקורי

//            }
//        }
//        private void CloseOpenWindows()
//        {
//            // סגירת כל החלונות הפתוחים חוץ מהחלון הראשי
//            foreach (Window window in Application.Current.Windows)
//            {
//                if (window != this) // 'this' הוא החלון הראשי
//                {
//                    window.Close();
//                }
//            }
//        }

//        private void Window_Loaded(object sender, RoutedEventArgs e)
//        {

//            Clock = s_bl.Admin.GetClock();
//            RiskRange = s_bl.Admin.GetMaxRange();
//            s_bl.Admin.AddClockObserver(clockObserver);
//            s_bl.Admin.AddConfigObserver(configObserver);


//        }


//        private void Window_Closed(object sender, EventArgs e)
//        {
//            s_bl.Admin.RemoveClockObserver(clockObserver);
//            s_bl.Admin.RemoveConfigObserver(configObserver);

//        }

//        private void Handle_Calls(object sender, RoutedEventArgs e)
//        {
//            new PL.Call.CallListWindow(CurrentAdmin.Id).Show();

//        }

//        private void Handle_Silmulator(object sender, RoutedEventArgs e)
//        {
//            IsSimulatorRunning = !IsSimulatorRunning;
//            SimulatorButtonText = IsSimulatorRunning ? "Stop Simulator" : "Start Simulator";

//            if (IsSimulatorRunning)
//            {
//                s_bl.Admin.StartSimulator(Interval);
//            }
//            else
//            {
//                s_bl.Admin.StopSimulator();

//            }
//        }
//    }
//}








//using BO;
//using DO;
//using PL.Call;
//using PL.Vol;
//using System;
//using System.Diagnostics;
//using System.Text;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Controls.Primitives;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Navigation;
//using System.Windows.Shapes;
//using System.Windows.Threading; // ייבוא עבור DispatcherTimer
//using System.ComponentModel; // ייבוא עבור INotifyPropertyChanged

//namespace PL
//{
//    /// <summary>
//    /// Interaction logic for MainWindow.xaml
//    /// </summary>
//    public partial class AdminWindow : Window
//    {
//        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

//        private int id = 0;

//        private volatile bool _isClockUpdating = false; // הדגל עבור clockObserver
//        private volatile bool _isConfigUpdating = false; // הדגל עבור configObserver

//        private CallListWindow? _callsWindow;
//        private VolunteerListWindow? _volunteersWindow;

//        // --- Dependency Properties חדשים ומעודכנים עבור סעיף 5ב' ---

//        // 1. Dependency Property עבור קצב התקדמות השעון (Interval)
//        public int Interval
//        {
//            get { return (int)GetValue(IntervalProperty); }
//            set { SetValue(IntervalProperty, value); }
//        }
//        public static readonly DependencyProperty IntervalProperty =
//            DependencyProperty.Register("Interval", typeof(int), typeof(AdminWindow), new PropertyMetadata(1000)); // ערך ברירת מחדל, לדוגמה 1000 דקות

//        // 2. Dependency Property בוליאני שמסמן אם הסימולטור רץ או לא
//        public bool IsSimulatorRunning
//        {
//            get { return (bool)GetValue(IsSimulatorRunningProperty); }
//            set { SetValue(IsSimulatorRunningProperty, value); }
//        }
//        public static readonly DependencyProperty IsSimulatorRunningProperty =
//            DependencyProperty.Register("IsSimulatorRunning", typeof(bool), typeof(AdminWindow), new PropertyMetadata(false, OnIsSimulatorRunningChanged));

//        // Callback כאשר IsSimulatorRunning משתנה (כדי לעדכן את ה-UI)
//        private static void OnIsSimulatorRunningChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
//        {
//            AdminWindow adminWindow = (AdminWindow)d;
//            adminWindow.UpdateUIBasedOnSimulatorStatus();
//        }

//        // --- סוף Dependency Properties חדשים ---


//        public BO.Volunteer CurrentVolunteer
//        {
//            get { return (BO.Volunteer)GetValue(CurrentVolunteerProperty); }
//            set { SetValue(CurrentVolunteerProperty, value); }
//        }
//        public static readonly DependencyProperty CurrentVolunteerProperty =
//            DependencyProperty.Register("CurrentVolunteer", typeof(BO.Volunteer), typeof(AdminWindow), new PropertyMetadata(null));


//        public DateTime CurrentTime
//        {
//            get { return (DateTime)GetValue(CurrentTimeProperty); }
//            set { SetValue(CurrentTimeProperty, value); }
//        }
//        public static readonly DependencyProperty CurrentTimeProperty =
//        DependencyProperty.Register("CurrentTime", typeof(DateTime), typeof(AdminWindow));


//        public DateTime Clock
//        {
//            get { return (DateTime)GetValue(ClockProperty); }
//            set { SetValue(ClockProperty, value); }
//        }

//        public static readonly DependencyProperty ClockProperty =
//            DependencyProperty.Register("Clock", typeof(DateTime), typeof(AdminWindow), new PropertyMetadata(DateTime.Now)); // עם ערך ברירת מחדל


//        public TimeSpan RiskRange
//        {
//            get { return (TimeSpan)GetValue(RiskRangeProperty); }
//            set { SetValue(RiskRangeProperty, value); }
//        }

//        public static readonly DependencyProperty RiskRangeProperty =
//            DependencyProperty.Register("RiskRange", typeof(TimeSpan), typeof(AdminWindow));

//        public int InTreatmentCount
//        {
//            get => (int)GetValue(InTreatmentCountProperty);
//            set => SetValue(InTreatmentCountProperty, value);
//        }
//        public static readonly DependencyProperty InTreatmentCountProperty =
//            DependencyProperty.Register("InTreatmentCount", typeof(int), typeof(AdminWindow));

//        public int OpenCount
//        {
//            get => (int)GetValue(OpenCountProperty);
//            set => SetValue(OpenCountProperty, value);
//        }
//        public static readonly DependencyProperty OpenCountProperty =
//            DependencyProperty.Register("OpenCount", typeof(int), typeof(AdminWindow));

//        public int ExpiredCount
//        {
//            get => (int)GetValue(ExpiredCountProperty);
//            set => SetValue(ExpiredCountProperty, value);
//        }
//        public static readonly DependencyProperty ExpiredCountProperty =
//            DependencyProperty.Register("ExpiredCount", typeof(int), typeof(AdminWindow));


//        public AdminWindow(int AdminId)
//        {
//            InitializeComponent();
//            this.DataContext = this;
//            id = AdminId;
//            try
//            {
//                this.CurrentVolunteer = s_bl.Volunteer.GetVolunteerDetails(AdminId);
//            }
//            catch (BO.BlDoesNotExistException ex)
//            {
//                MessageBox.Show($"Volunteer with ID {AdminId} not found: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
//                this.Close();
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Unexpected error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
//                this.Close();
//            }

//            // Add observers on window load
//            this.Loaded += (s, e) =>
//            {
//                s_bl.Admin.AddClockObserver(clockObserver);
//                s_bl.Admin.AddConfigObserver(configObserver);
//                // הוסיפו את האובזרבר של הסימולטור
//                s_bl.Admin.AddSimulatorObserver(simulatorObserver);
//                UpdateCallStatusCounts(); // עדכון ראשוני של הספירות
//                // נסנכרן את מצב הדגל IsSimulatorRunning עם מצב הסימולטור בפועל ב BL
//                IsSimulatorRunning = s_bl.Admin.GetSimulatorStatus();
//            };

//            // Remove observers on window close
//            this.Closed += (s, e) =>
//            {
//                s_bl.Admin.RemoveClockObserver(clockObserver);
//                s_bl.Admin.RemoveConfigObserver(configObserver);
//                s_bl.Admin.RemoveSimulatorObserver(simulatorObserver);

//                // סגירה מסודרת של הסימולטור בעת סגירת החלון
//                if (IsSimulatorRunning)
//                {
//                    s_bl.Admin.StopSimulator(); // קוראים למתודה ללא פרמטר אם היא לא דורשת
//                }
//            };

//            // עדכון ראשוני של שעון המערכת וטווח הסיכון
//            Clock = s_bl.Admin.GetClock();
//            RiskRange = s_bl.Admin.GetRiskRange();
//        }

//        // --- מתודות Observers קיימות ---
//        private void clockObserver()
//        {
//            if (_isClockUpdating)
//            {
//                return; // התעלם אם עדכון קודם עדיין בעיצומו
//            }

//            _isClockUpdating = true; // הדלק את הדגל

//            Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
//            {
//                try
//                {
//                    SetCurrentValue(ClockProperty, s_bl.Admin.GetClock());
//                }
//                catch (Exception ex)
//                {
//                    MessageBox.Show("Error updating clock: " + ex.Message);
//                }
//                finally
//                {
//                    _isClockUpdating = false; // כבה את הדגל בסיום פעולת ה-Dispatcher
//                }
//            }));
//        }

//        private void configObserver()
//        {
//            if (_isConfigUpdating)
//            {
//                return; // התעלם אם עדכון קודם עדיין בעיצומו
//            }

//            _isConfigUpdating = true; // הדלק את הדגל

//            Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
//            {
//                try
//                {
//                    RiskRange = s_bl.Admin.GetRiskRange();
//                    // בנוסף לעדכון RiskRange, נדאג שכל שאר המידע מבסיס הנתונים יתעדכן
//                    UpdateCallStatusCounts(); // לדוגמה, ספירות הקריאות
//                }
//                catch (Exception ex)
//                {
//                    MessageBox.Show("Error updating config: " + ex.Message);
//                }
//                finally
//                {
//                    _isConfigUpdating = false; // כבה את הדגל בסיום פעולת ה-Dispatcher
//                }
//            }));
//        }

//        // --- מתודת Observer חדשה עבור מצב הסימולטור ---
//        private void simulatorObserver()
//        {
//            // המתודה הזו תופעל על ידי ה-BL כאשר מצב הסימולטור משתנה
//            // אנחנו נעדכן כאן את הדגל IsSimulatorRunning
//            // ונדאג שה-UI יתעדכן בהתאם (על ידי ה-PropertyMetadata וה-callback שלו)
//            Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
//            {
//                IsSimulatorRunning = s_bl.Admin.GetSimulatorStatus();
//            }));
//        }

//        // --- מתודות טיפול בלחיצות כפתור קיימות ---
//        private void btnAddOneMinute_Click(object sender, RoutedEventArgs e)
//        {
//            s_bl.Admin.ClockPromotion(BO.TIMEUNIT.MINUTE);
//        }
//        private void btnAddOneDay_Click(object sender, RoutedEventArgs e)
//        {
//            s_bl.Admin.ClockPromotion(BO.TIMEUNIT.DAY);
//        }
//        private void btnAddOneHour_Click(object sender, RoutedEventArgs e)
//        {
//            s_bl.Admin.ClockPromotion(BO.TIMEUNIT.HOUR);
//        }
//        private void btnAddOneMonth_Click(object sender, RoutedEventArgs e)
//        {
//            s_bl.Admin.ClockPromotion(BO.TIMEUNIT.MONTH);
//        }
//        private void btnAddOneYear_Click(object sender, RoutedEventArgs e)
//        {
//            s_bl.Admin.ClockPromotion(BO.TIMEUNIT.YEAR);
//        }

//        private void btnUpdate_RiskRange(object sender, RoutedEventArgs e)
//        {
//            s_bl.Admin.SetRiskRange(RiskRange);
//        }

//        private void btnUpdate_Clock(object sender, RoutedEventArgs e)
//        {
//            // הערה: עם הסימולטור, ייתכן שכפתור זה כבר לא יהיה רלוונטי
//            // או שיהיה צריך להפוך אותו ללא זמין בזמן שהסימולטור פועל.
//            // במקרה הנוכחי, ההוראות לא ביקשו לחסום אותו.
//            s_bl.Admin.SetClock(Clock);
//        }
//        private void btnVolunteers_Click(object sender, RoutedEventArgs e)
//        {
//            if (_volunteersWindow == null || !_volunteersWindow.IsVisible)
//            {
//                _volunteersWindow = new VolunteerListWindow(id);
//                _volunteersWindow.Closed += (s, e) => _volunteersWindow = null;
//                _volunteersWindow.Show();
//            }
//            else
//            {
//                _volunteersWindow.Activate();
//            }
//        }
//        private void btnCalls_Click(object sender, RoutedEventArgs e)
//        {
//            if (_callsWindow == null || !_callsWindow.IsVisible)
//            {
//                _callsWindow = new CallListWindow(CurrentVolunteer.Id);
//                _callsWindow.Closed += (s, e) => _callsWindow = null;
//                _callsWindow.Show();
//            }
//            else
//            {
//                _callsWindow.Activate();
//            }
//        }
//        private void btnInitialize_Click(object sender, RoutedEventArgs e)
//        {
//            try
//            {
//                // Confirmation message to the user
//                MessageBoxResult result = MessageBox.Show("Are you sure you want to initialize the database?",
//                                                            "Initialization Confirmation",
//                                                            MessageBoxButton.YesNo,
//                                                            MessageBoxImage.Question);

//                if (result == MessageBoxResult.Yes)
//                {
//                    // Close all open windows (except the main window)
//                    CloseOpenWindows();
//                    Mouse.OverrideCursor = Cursors.Wait;
//                    s_bl.Admin.InitializeDB();
//                    UpdateCallStatusCounts();
//                    MessageBox.Show("Database initialized successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Error initializing DB: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
//            }
//            finally
//            {
//                Mouse.OverrideCursor = null;
//            }

//        }
//        private void btnReset_Click(object sender, RoutedEventArgs e)
//        {
//            try
//            {
//                MessageBoxResult result = MessageBox.Show("Are you sure you want to reset the database?",
//                                                        "Reset Confirmation",
//                                                        MessageBoxButton.YesNo,
//                                                        MessageBoxImage.Question);

//                if (result == MessageBoxResult.Yes)
//                {
//                    // Close all open windows
//                    CloseOpenWindows();
//                    Mouse.OverrideCursor = Cursors.Wait;
//                    s_bl.Admin.ResetDB();
//                    UpdateCallStatusCounts();
//                    MessageBox.Show("Database reset successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Error reset DB: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
//            }
//            finally
//            {
//                Mouse.OverrideCursor = null;
//            }

//        }
//        private void UpdateCallStatusCounts()
//        {
//            try
//            {
//                var counts = s_bl.Call.GetCallCountsByStatus();

//                InTreatmentCount = counts.ContainsKey(BO.STATUS.InTreatment) ? counts[BO.STATUS.InTreatment] : 0;
//                OpenCount = counts.ContainsKey(BO.STATUS.Open) ? counts[BO.STATUS.Open] : 0;
//                ExpiredCount = counts.ContainsKey(BO.STATUS.Expired) ? counts[BO.STATUS.Expired] : 0;
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Error updating call counts: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
//            }
//        }


//        private void CloseOpenWindows()
//        {
//            // סגירת כל החלונות הפתוחים חוץ מהחלון הראשי
//            foreach (Window window in Application.Current.Windows)
//            {
//                if (window != this) // 'this' הוא החלון הראשי
//                {
//                    window.Close();
//                }
//            }
//        }
//        private void btnInTreatmentCalls_Click(object sender, RoutedEventArgs e)
//        {
//            if (_callsWindow == null || !_callsWindow.IsVisible)
//            {
//                _callsWindow = new CallListWindow(BO.STATUS.InTreatment); // <-- סינון לפי סטטוס
//                _callsWindow.Closed += (s, e) => _callsWindow = null;
//                _callsWindow.Show();
//            }
//            else
//            {
//                _callsWindow.Activate();
//            }
//        }
//        private void btnOpenCalls_Click(object sender, RoutedEventArgs e)
//        {
//            if (_callsWindow == null || !_callsWindow.IsVisible)
//            {
//                _callsWindow = new CallListWindow(BO.STATUS.Open); // <-- סינון לפי סטטוס
//                _callsWindow.Closed += (s, e) => _callsWindow = null;
//                _callsWindow.Show();
//            }
//            else
//            {
//                _callsWindow.Activate();
//            }
//        }

//        private void btnExpired_Click(object sender, RoutedEventArgs e)
//        {
//            if (_callsWindow == null || !_callsWindow.IsVisible)
//            {
//                _callsWindow = new CallListWindow(BO.STATUS.Expired); // <-- סינון לפי סטטוס
//                _callsWindow.Closed += (s, e) => _callsWindow = null;
//                _callsWindow.Show();
//            }
//            else
//            {
//                _callsWindow.Activate();
//            }
//        }

//        // --- מתודות חדשות עבור סעיף 5ב' ---

//        /// <summary>
//        /// מטפלת בלחיצה על כפתור הפעל/עצור סימולטור.
//        /// </summary>
//        private void btnToggleSimulator_Click(object sender, RoutedEventArgs e)
//        {
//            try
//            {
//                if (IsSimulatorRunning)
//                {
//                    s_bl.Admin.StopSimulator();
//                }
//                else
//                {
//                    // יש לבדוק שה-Interval תקין לפני הפעלה
//                    if (Interval <= 0)
//                    {
//                        MessageBox.Show("קצב התקדמות השעון חייב להיות מספר חיובי.", "שגיאת קלט", MessageBoxButton.OK, MessageBoxImage.Warning);
//                        return;
//                    }
//                    s_bl.Admin.StartSimulator(Interval);
//                }
//                // מצב IsSimulatorRunning יעודכן על ידי ה-simulatorObserver
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"שגיאה בפעולת הסימולטור: {ex.Message}", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
//            }
//        }

//        /// <summary>
//        /// מעדכנת את מצב ה-UI של הפקדים השונים בהתאם למצב הסימולטור.
//        /// נקראת אוטומטית כאשר IsSimulatorRunning משתנה.
//        /// </summary>
//        private void UpdateUIBasedOnSimulatorStatus()
//        {
//            // כאן נגדיר את הפקדים שצריכים להיות מושבתים/מופעלים
//            // או לשנות את הטקסט שלהם.
//            // עדיף לבצע את ה-Enabling/Disabling ב-XAML באמצעות Binding ו-Converter
//            // אך ניתן גם לבצע זאת כאן ישירות.

//            // דוגמה לשינוי טקסט של כפתור ההפעלה/עצירה (אם לא עשינו זאת בקונברטר)
//            // אם היינו נותנים שם ל-Button ב-XAML, למשל x:Name="btnSimulatorToggle"
//            // if (btnSimulatorToggle != null)
//            // {
//            //     btnSimulatorToggle.Content = IsSimulatorRunning ? "עצור סימולטור" : "הפעל סימולטור";
//            // }

//            // קוראים ל-UpdateCallStatusCounts כדי לוודא שכל המידע מעודכן
//            UpdateCallStatusCounts();
//        }

//        private void Handle_Silmulator(object sender, RoutedEventArgs e)
//        {
//            IsSimulatorRunning = !IsSimulatorRunning;
//            SimulatorButtonText = IsSimulatorRunning ? "Stop Simulator" : "Start Simulator";

//            if (IsSimulatorRunning)
//            {
//                s_bl.Admin.StartSimulator(Interval);
//            }
//            else
//            {
//                s_bl.Admin.StopSimulator();

//            }
//        }
//    }

//    // --- Converters חדשים עבור סעיף 5ב' ---

//    /// <summary>
//    /// ממיר ערך בוליאני לטקסט עבור כפתור הפעל/עצור סימולטור.
//    /// </summary>
//    public class SimulatorButtonTextConverter : IValueConverter
//    {
//        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
//        {
//            if (value is bool isRunning)
//            {
//                return isRunning ? "עצור סימולטור" : "הפעל סימולטור";
//            }
//            return "הפעל סימולטור"; // ברירת מחדל
//        }

//        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
//        {
//            throw new NotImplementedException();
//        }
//    }

//    /// <summary>
//    /// ממיר ערך בוליאני ל-Visibility (הפוך).
//    /// משמש להסתרה/הצגה של פקדים.
//    /// </summary>
//    public class InvertBooleanToVisibilityConverter : IValueConverter
//    {
//        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
//        {
//            if (value is bool booleanValue)
//            {
//                return booleanValue ? Visibility.Collapsed : Visibility.Visible;
//            }
//            return Visibility.Visible; // ברירת מחדל
//        }

//        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
//        {
//            throw new NotImplementedException();
//        }
//    }

//    /// <summary>
//    /// ממיר ערך בוליאני לאפשרות קלט (IsEnabled - הפוך).
//    /// משמש לחסימה/הפעלה של פקדים.
//    /// </summary>
//    public class InvertBooleanToEnabledConverter : IValueConverter
//    {
//        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
//        {
//            if (value is bool booleanValue)
//            {
//                return !booleanValue; // אם הסימולטור רץ (true), הפקד יהיה מושבת (false)
//            }
//            return true; // ברירת מחדל: מופעל
//        }

//        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
//        {
//            throw new NotImplementedException();
//        }
//    }

//    /// <summary>
//    /// ממיר ערך בוליאני לאפשרות קלט (IsEnabled - ישיר).
//    /// משמש לחסימה/הפעלה של פקדים.
//    /// </summary>
//    public class BooleanToEnabledConverter : IValueConverter
//    {
//        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
//        {
//            if (value is bool booleanValue)
//            {
//                return booleanValue; // אם הסימולטור רץ (true), הפקד יהיה מופעל (true)
//            }
//            return true; // ברירת מחדל: מופעל
//        }

//        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
