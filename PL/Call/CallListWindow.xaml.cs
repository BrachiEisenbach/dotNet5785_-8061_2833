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
        public int VolunteerId { get; set; }

        public BO.Volunteer? CurrentVolunteer
        {
            get => (BO.Volunteer?)GetValue(CurrentVolunteerProperty);
            set => SetValue(CurrentVolunteerProperty, value);
        }

        public static readonly DependencyProperty CurrentVolunteerProperty =
            DependencyProperty.Register("CurrentVolunteer", typeof(BO.Volunteer), typeof(ChooseCallWindow), new PropertyMetadata(null));


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
            IEnumerable<BO.CallInList> calls;

            // אם קיים סינון לפי סטטוס
            if (StatusFilter != null && Type != BO.TYPEOFCALL.NONE)
            {
                // שולחים ל-BL סטטוס כפילטר וגם את אותו סטטוס כערך לסינון,
                // ואז מסננים לפי סוג הקריאה ידנית, כי BL לא תומך בשני הפילטרים יחד
                var callsByStatus = s_bl.Call.GetCallList(StatusFilter, StatusFilter, null);
                calls = callsByStatus.Where(c => c.TypeOfCall == Type);
            }
            else if (StatusFilter != null)
            {
                // סינון לפי סטטוס בלבד
                calls = s_bl.Call.GetCallList(StatusFilter, StatusFilter, null);
            }
            else if (Type != BO.TYPEOFCALL.NONE)
            {
                // סינון לפי סוג קריאה בלבד
                calls = s_bl.Call.GetCallList(Type, Type, BO.VOLUNTEERFIELDSORT.CALLTYPE);
            }
            else
            {
                // ללא סינון
                calls = s_bl.Call.GetCallList(null, null, null);
            }

            CallList = calls.ToList();
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

                    s_bl.Call.cancelTreat(volId, id);
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
            new CallWindow(0).Show();
        }

        /// <summary>
        /// מתעדכן כאשר יש שינוי ברשימת הקריאות דרך המנגנון Observer
        /// </summary>
        private void CallListObserver()
            => queryCallList();

        /// <summary>
        /// טען את ה־Observer בעת טעינת החלון
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            s_bl.Call.AddObserver(CallListObserver);
            CallWindow.CallsChanged += OnCallsChanged;
            queryCallList();
        }

        /// <summary>
        /// הסר את ה־Observer בעת סגירת החלון
        /// </summary>
        private void Window_Closed(object sender, EventArgs e)
        {
            s_bl.Call.RemoveObserver(CallListObserver);
            CallWindow.CallsChanged -= OnCallsChanged;
        }

        /// <summary>
        ///מטפל בשינוי ברשימת הקריאות (אירוע סטטי מ־  CallWindow )
        /// </summary>
        private void OnCallsChanged()
        {
            Dispatcher.Invoke(queryCallList);
        }
    }
}
