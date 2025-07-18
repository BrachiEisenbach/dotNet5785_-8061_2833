using BlApi;
using BO;
using PL.Call;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
namespace PL.Vol
{
    public partial class VolunteerWindowVol : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private static readonly IBl s_bl = Factory.Get();

        public BO.Volunteer? CurrentVolunteer
        {
            get { return (BO.Volunteer?)GetValue(CurrentVolunteerProperty); }
            set { SetValue(CurrentVolunteerProperty, value); }
        }
        public static readonly DependencyProperty CurrentVolunteerProperty =
            DependencyProperty.Register("CurrentVolunteer", typeof(BO.Volunteer), typeof(VolunteerWindowVol), new PropertyMetadata(null));

        public VolunteerWindowVol(int volunteerId)
        {
            InitializeComponent();
            try
            {
                this.CurrentVolunteer = s_bl.Volunteer.GetVolunteerDetails(volunteerId);
                if (this.CurrentVolunteer.CallInTreate != null)
                {
                    System.Diagnostics.Debug.WriteLine(CurrentVolunteer.CallInTreate.CallId);
                    System.Diagnostics.Debug.WriteLine(CurrentVolunteer.CallInTreate.VerbalDescription);
                    System.Diagnostics.Debug.WriteLine(CurrentVolunteer.CallInTreate.FullAddress);
                }

                this.Loaded += Window_Loaded;
                this.Closed += Window_Closed;
                this.DataContext = this;
            }
            catch (BO.BlDoesNotExistException ex)
            {
                MessageBox.Show($"Volunteer with ID {volunteerId} not found: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }

        #region Observer Pattern
        private void VolunteerDataObserver()
        {
            try
            {
                if (CurrentVolunteer != null && CurrentVolunteer.Id != 0)
                {
                    var localChanges = this.CurrentVolunteer;
                    var freshData = s_bl.Volunteer.GetVolunteerDetails(CurrentVolunteer.Id);

                    this.CurrentVolunteer = freshData;
                    OnPropertyChanged(nameof(CurrentVolunteer));
                }
            }
            catch (BO.BlDoesNotExistException)
            {
                MessageBox.Show("Volunteer no longer exists. Closing window.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error refreshing volunteer data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CurrentVolunteer != null && CurrentVolunteer.Id != 0)
                {
                    s_bl.Volunteer.AddObserver(CurrentVolunteer.Id, VolunteerDataObserver);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to observe volunteer changes: " + ex.Message);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            try
            {
                if (CurrentVolunteer != null && CurrentVolunteer.Id != 0)
                {
                    s_bl.Volunteer.RemoveObserver(CurrentVolunteer.Id, VolunteerDataObserver);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while detaching observer: " + ex.Message);
            }
        }
        #endregion

        //done!!!!
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                s_bl.Volunteer.UpdateVolunteerDetails(CurrentVolunteer.Id, this.CurrentVolunteer);
                MessageBox.Show("Volunteer updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                VolunteerDataObserver();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //done!!!!
        private void btnChooseCall_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ChooseCallWindow chooseCallWin = new ChooseCallWindow(CurrentVolunteer.Id);
                chooseCallWin.Closed += (s, args) => VolunteerDataObserver();
                chooseCallWin.Show();


            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred while choosing a call: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //done!!!
        private void btnFinishTreat_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CurrentVolunteer.CallInTreate == null)
                {
                    MessageBox.Show("This volunteer has no active call.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                int callIdToFinish = CurrentVolunteer.CallInTreate.CallId;
                s_bl.Call.updateFinishTreat(CurrentVolunteer.Id, callIdToFinish);
                MessageBox.Show($"Call {callIdToFinish} finished successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                VolunteerDataObserver();
            }
            catch (BO.BlDoesNotExistException ex)
            {
                MessageBox.Show($"Call not found: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while finishing treatment: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //done!!!
        private void btnCancelTreat_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CurrentVolunteer.CallInTreate == null)
                {
                    MessageBox.Show("This volunteer has no active call.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (MessageBox.Show("Are you sure you want to cancel treatment for this call?", "Confirm Cancellation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    int callIdToCancel = CurrentVolunteer.CallInTreate.CallId;

                    int assiIdToCancel = CurrentVolunteer.CallInTreate.Id;
                    s_bl.Call.cancelTreat(CurrentVolunteer.Id, assiIdToCancel);
                    MessageBox.Show($"Treatment for call {callIdToCancel} has been canceled.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    VolunteerDataObserver();               
                }
            }
            catch (BO.BlDoesNotExistException ex)
            {
                MessageBox.Show($"Cancellation failed: Call not found. {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (BO.BlInvalidOperationException ex)
            {
                MessageBox.Show($"Operation not allowed: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred while canceling treatment: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //done!!!!
        private void btnHistory_click(object sender, RoutedEventArgs e)
        {
            try
            {
                CallsHistoryWindow historyWin = new CallsHistoryWindow(CurrentVolunteer.Id);
                historyWin.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading call history: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


    }
}