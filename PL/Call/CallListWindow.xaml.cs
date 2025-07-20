using BO;
using DO;
using PL.Vol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL.Call
{
    /// <summary>
    /// חלון להצגת רשימת קריאות עם סינון, מחיקה, ופתיחת קריאה חדשה
    /// כולל ספירת קריאות לפי סטטוסים לעמוד ניהול
    /// </summary>
    public partial class CallListWindow : Window
    {
        /// <summary>
        /// מופע של השכבה העסקית לקבלת נתונים
        /// </summary>
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        private volatile bool _callListObserverWorking = false;
        public int VolunteerId { get; set; }

        public BO.Volunteer? CurrentVolunteer
        {
            get => (BO.Volunteer?)GetValue(CurrentVolunteerProperty);
            set => SetValue(CurrentVolunteerProperty, value);
        }

        public static readonly DependencyProperty CurrentVolunteerProperty =
            DependencyProperty.Register("CurrentVolunteer", typeof(BO.Volunteer), typeof(CallListWindow), new PropertyMetadata(null));


        public CallListWindow(int id)
        {
            InitializeComponent();
            VolunteerId = id; // שמירת ה-ID של המתנדב
            CurrentVolunteer = s_bl.Volunteer.GetVolunteerDetails(id);

            CallListObserver();
            this.Loaded += Window_Loaded;
            this.Closed += Window_Closed;
        }

        /// <summary>
        /// קונסטרקטור עם אפשרות לסינון קריאות לפי סטטוס התחלתי
        /// </summary>
        /// <param name="statusFilter">סטטוס לסינון הקריאות</param>
        public CallListWindow(BO.STATUS? statusFilter)
        {
            InitializeComponent();
            StatusFilter=statusFilter;
            this.Loaded += Window_Loaded;
            this.Closed += Window_Closed;
        }
        // ----------- DP: CallList -------------
        /// <summary>
        /// סטטוס שלפיו תסתנן הרשימה
        /// </summary>
        public BO.STATUS? StatusFilter
        {
            get => (BO.STATUS?)GetValue(StatusFilterProperty);
            set => SetValue(StatusFilterProperty, value);
        }

        public static readonly DependencyProperty StatusFilterProperty =
            DependencyProperty.Register(nameof(StatusFilter), typeof(BO.STATUS?), typeof(CallListWindow),
                new PropertyMetadata(null, OnStatusChanged));

        // ----------- DP: CallList -------------
        /// <summary>
        /// רשימת הקריאות המוצגות בחלון
        /// </summary>
        public IEnumerable<BO.CallInList> CallList
        {
            get => (IEnumerable<BO.CallInList>)GetValue(CallListProperty);
            set => SetValue(CallListProperty, value);
        }

        /// <summary>
        /// DependencyProperty עבור רשימת הקריאות (CallList)
        /// </summary>
        public static readonly DependencyProperty CallListProperty =
            DependencyProperty.Register(nameof(CallList), typeof(IEnumerable<BO.CallInList>), typeof(CallListWindow), new PropertyMetadata(null));

        // ----------- DP: Type (ל־ComboBox לסינון סוג קריאה) -------------
        /// <summary>
        /// סוג הקריאה לסינון (לדוגמה: NONE, סוג מסוים)
        /// </summary>
        public BO.TYPEOFCALL Type
        {
            get => (BO.TYPEOFCALL)GetValue(TypeProperty);
            set => SetValue(TypeProperty, value);
        }

        /// <summary>
        /// DependencyProperty עבור סוג הקריאה (Type)
        /// </summary>
        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register(nameof(Type), typeof(BO.TYPEOFCALL), typeof(CallListWindow), new PropertyMetadata(BO.TYPEOFCALL.NONE, OnTypeChanged));

        /// <summary>
        /// callback שנקרא כאשר משתנה ה־Type, כדי לעדכן את רשימת הקריאות
        /// </summary>
        private static void OnTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var window = d as CallListWindow;
            window?.CallListObserver();
            
        }

        /// <summary>
        /// הקריאה שנבחרה כרגע ברשימה (בהקשר ללחיצה כפולה)
        /// </summary>
        public BO.CallInList? SelectedCall { get; set; }


        /// <summary>
        /// טוען את רשימת הקריאות מהשכבה העסקית בהתאם לסינונים ומעדכן את ה־DependencyProperties של ספירת הקריאות
        /// </summary>
        private void queryCallList()
        {
            try
            {
                IEnumerable<BO.CallInList> calls;

                if (StatusFilter != null && Type != BO.TYPEOFCALL.NONE)
                {
                    var callsByStatus = s_bl.Call.GetCallList(StatusFilter, StatusFilter, null);
                    calls = callsByStatus.Where(c => c.TypeOfCall == Type);
                }
                else if (StatusFilter != null)
                {
                    calls = s_bl.Call.GetCallList(StatusFilter, StatusFilter, null);
                }
                else if (Type != BO.TYPEOFCALL.NONE)
                {
                    calls = s_bl.Call.GetCallList(Type, Type, BO.VOLUNTEERFIELDSORT.CALLTYPE);
                }
                else
                {
                    calls = s_bl.Call.GetCallList(null, null, null);
                }

                CallList = calls.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load call list: " + ex.Message);
            }
        }

        private static void OnStatusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var window = d as CallListWindow;
            window?.queryCallList();
        }

        /// <summary>
        /// אירוע לחיצה כפולה על שורה ברשימת הקריאות לפתיחת חלון הקריאה
        /// </summary>
        private void dgCallList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (SelectedCall != null)
                {
                    MessageBox.Show("ID: " + SelectedCall.CallId); // בדיקה איזה קריאה נבחרה
                    new CallWindow(SelectedCall.CallId).Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to open new volunteer window: " + ex.Message);
            }
        }

        /// <summary>
        /// אירוע לחיצה על כפתור מחיקת קריאה, כולל אישור ושגיאות
        /// </summary>
        private void Delete_click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is Button button && button.CommandParameter is int CallId)
            {
                try
                {
                    var result = MessageBox.Show("Are you sure you want to delete call?", "Confirm Delete", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        s_bl.Call.DeleteCall(CallId);
                        CallListObserver();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Cannot delete volunteer: " + ex.Message);
                }
            }
        }

        // ----------------- ביטול הקצאה (טרם ממומש) -----------------
        private void CancelAssignment_Click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is Button button && button.CommandParameter is int id)
            {
                int volId = CurrentVolunteer.Id;
                try
                {

                    s_bl.Call.CancelTreat(volId, id);
                    CallListObserver();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Cannot cancel assignment: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// אירוע לחיצה על כפתור הוספת קריאה חדשה
        /// </summary>
        private void btnAddCall(object sender, RoutedEventArgs e)
        {
            try
            {
                new CallWindow(0).Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to open new call window: " + ex.Message);
            }
        }
        /// <summary>
        /// מתעדכן כאשר יש שינוי ברשימת הקריאות דרך המנגנון Observer
        /// </summary>
        private void CallListObserver()
        {
            if (!_callListObserverWorking)
            {
                _callListObserverWorking = true;
                _ = Dispatcher.BeginInvoke(() =>
                {
                    try
                    {
                        queryCallList();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Failed to refresh call list: " + ex.Message);
                    }
                    finally
                    {
                        _callListObserverWorking = false; // כבוי הדגל לאחר סיום העדכון ב-UI Thread
                    }
                });
            }
        }
        /// <summary>
        /// טען את ה־Observer בעת טעינת החלון
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                s_bl.Call.AddObserver(CallListObserver);
                CallWindow.CallsChanged += OnCallsChanged;
                queryCallList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to initialize call list window: " + ex.Message);
            }
        }

        /// <summary>
        /// הסר את ה־Observer בעת סגירת החלון
        /// </summary>
        private void Window_Closed(object sender, EventArgs e)
        {
            try
            {
                s_bl.Call.RemoveObserver(CallListObserver);
                CallWindow.CallsChanged -= OnCallsChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to clean up on window close: " + ex.Message);
            }
        }

        /// <summary>
        ///מטפל בשינוי ברשימת הקריאות (אירוע סטטי מ־  CallWindow )
        /// </summary>
        private void OnCallsChanged()
        {
            CallListObserver();       
        }
    }
}
