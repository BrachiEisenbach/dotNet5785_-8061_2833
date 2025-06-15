using System;
using System.Collections.Generic;
using System.Linq; // עבור ToList()
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BO;
using BlApi;

namespace PL.Call
{
    /// <summary>
    /// Interaction logic for ChooseCall.xaml
    /// </summary>
    public partial class ChooseCallWindow : Window
    {
        private static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public ChooseCallWindow(int id)
        {
            InitializeComponent();
            VolunteerId = id; // שמירת ה-ID של המתנדב
            CurrentVolunteer = s_bl.Volunteer.GetVolunteerDetails(id);
            // קריאה ראשונית לטעינת הקריאות, כרגע ללא סינון או מיון ספציפיים
            // (הפונקציה ב-BL מסננת לפתוחות ומחשבת מרחק)
            queryCallListChooseCall();
        }

        // שמירת ה-ID של המתנדב כחבר מחלקה (לא Dependency Property)
        public int VolunteerId { get; set; }

        public BO.Volunteer? CurrentVolunteer
        {
            get => (BO.Volunteer?)GetValue(CurrentVolunteerProperty);
            set => SetValue(CurrentVolunteerProperty, value);
        }

        public static readonly DependencyProperty CurrentVolunteerProperty =
            DependencyProperty.Register("CurrentVolunteer", typeof(BO.Volunteer), typeof(ChooseCallWindow), new PropertyMetadata(null));

        public IEnumerable<BO.OpenCallInList>? CallList
        {
            get => (IEnumerable<BO.OpenCallInList>?)GetValue(CallListProperty);
            set => SetValue(CallListProperty, value);
        }

        public static readonly DependencyProperty CallListProperty =
            DependencyProperty.Register("CallList", typeof(IEnumerable<BO.OpenCallInList>), typeof(ChooseCallWindow), new PropertyMetadata(null));

        // נשמר ה-DependencyProperty עבור Type למרות שה-UI עבורו הוסר, כפי שבקשת.
        // הפרמטר Type נחוץ כעת כפרמטר למתודת ה-BL.
        public BO.TYPEOFCALL Type
        {
            get => (BO.TYPEOFCALL)GetValue(TypeProperty);
            set => SetValue(TypeProperty, value);
        }

        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(BO.TYPEOFCALL), typeof(ChooseCallWindow), new PropertyMetadata(BO.TYPEOFCALL.NONE, OnTypeChanged));

        public BO.OpenCallInList? SelectedCall
        {
            get => (BO.OpenCallInList?)GetValue(SelectedCallProperty);
            set => SetValue(SelectedCallProperty, value);
        }

        public static readonly DependencyProperty SelectedCallProperty =
            DependencyProperty.Register("SelectedCall", typeof(BO.OpenCallInList), typeof(ChooseCallWindow), new PropertyMetadata(null));

        private static void OnTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var window = d as ChooseCallWindow;
            window?.queryCallListChooseCall();
        }

        private void queryCallListChooseCall()
        {
            try
            {
                // קוראים ישירות למתודה ב-BL שמחזירה את רשימת הקריאות הפתוחות
                // עבור המתנדב הנוכחי, כולל חישוב המרחק והסינון והמיון הדרושים.
                // הפרמטרים הם: VolunteerId, TypeOfCall (מאפיין ה-Type של החלון), ו-SortBy (ממיין לפי מרחק כברירת מחדל).
                CallList = s_bl.Call.GetOpenCallInList(VolunteerId, Type, BO.OpenCallInListField.Distance).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load calls: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Updete_click(object sender, RoutedEventArgs e)
        {
            try {

                s_bl.Volunteer.UpdateVolunteerDetails(CurrentVolunteer.Id, CurrentVolunteer);
                MessageBox.Show("Volunteer updated successfully");
                CurrentVolunteer = s_bl.Volunteer.GetVolunteerDetails(VolunteerId);
                queryCallListChooseCall(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


       private void Choose_click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is Button button && button.CommandParameter is int callId)
            {
                try
                {
                    var result = MessageBox.Show("Are you sure you want to choose this call?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        s_bl.Call.chooseCall(CurrentVolunteer.Id, callId);
                        queryCallListChooseCall(); // רענון הרשימה כך שהקריאה שנבחרה תיעלם
                        MessageBox.Show("Call chosen successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (BlException ex)
                {
                    MessageBox.Show($"Cannot choose call: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

    }
}