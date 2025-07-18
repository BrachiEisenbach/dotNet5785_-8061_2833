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

        public ChooseCallWindow(int id)
        {
            InitializeComponent();
            VolunteerId = id;

            try
            {
                CurrentVolunteer = s_bl.Volunteer.GetVolunteerDetails(id);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading volunteer details: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
                return;
            }

            ChooseCallObserver();
            this.Loaded += Window_Loaded;
            this.Closed += Window_Closed;
        }

        private static void OnTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var window = d as ChooseCallWindow;
            window?.ChooseCallObserver();
        }

        private void queryCallListChooseCall()
        {
            try
            {
                CallList = s_bl.Call.GetOpenCallInList(VolunteerId, Type, BO.OpenCallInListField.Distance).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load calls: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Updete_click(object sender, RoutedEventArgs e)
        {
            try
            {
                s_bl.Volunteer.UpdateVolunteerDetails(CurrentVolunteer.Id, CurrentVolunteer);
                MessageBox.Show("Volunteer updated successfully");
                CurrentVolunteer = s_bl.Volunteer.GetVolunteerDetails(VolunteerId);
                ChooseCallObserver();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to update volunteer: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                        //ChooseCallObserver(); // רענון הרשימה כך שהקריאה שנבחרה תיעלם
                        MessageBox.Show("Call chosen successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);                        
                        this.Close();

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

        private void ChooseCallObserver()
         => queryCallListChooseCall();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                s_bl.Call.AddObserver(ChooseCallObserver);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error attaching observer: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            try
            {
                s_bl.Call.RemoveObserver(ChooseCallObserver);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error detaching observer: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


    }
}