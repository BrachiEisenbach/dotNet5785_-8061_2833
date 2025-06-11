
using BO;
using System;
using System.Windows;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Controls;
using DO;
using System.Windows.Data;
using System.Windows.Media;

namespace PL.Vol
{
    /// <summary>
    /// Interaction logic for Vol.xaml
    /// </summary>
    public partial class VolunteerWindow : Window
    { 
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public string ButtonText
        {
            get => (string)GetValue(ButtonTextProperty);
            set => SetValue(ButtonTextProperty, value);
        }
        public static readonly DependencyProperty ButtonTextProperty =
            DependencyProperty.Register("ButtonText", typeof(string), typeof(VolunteerWindow), new PropertyMetadata(""));


        public BO.Volunteer? CurrentVolunteer
        {
            get => (BO.Volunteer?)GetValue(CurrentVolunteerProperty);
            set => SetValue(CurrentVolunteerProperty, value);
        }
        public static readonly DependencyProperty CurrentVolunteerProperty =
            DependencyProperty.Register("CurrentVolunteer", typeof(BO.Volunteer), typeof(VolunteerWindow), new PropertyMetadata(null));
       
        
        public bool IsUpdateMode
        {
            get => (bool)GetValue(IsUpdateModeProperty);
            set => SetValue(IsUpdateModeProperty, value);
        }
        public static readonly DependencyProperty IsUpdateModeProperty =
            DependencyProperty.Register("IsUpdateMode", typeof(bool), typeof(VolunteerWindow), new PropertyMetadata(false));


        public VolunteerWindow(int id)
        {
            
            IsUpdateMode = id != 0; // כאן נשתמש בזה בבינדינג ל-IsReadOnly
            ButtonText = id == 0 ? "Add" : "Update";
            InitializeComponent();
            DataContext = this;

            if (id != 0)
            {
                CurrentVolunteer = s_bl.Volunteer.GetVolunteerDetails(id);
            }
            else
            {
                CurrentVolunteer = new BO.Volunteer()
                {
                    Id = 0,
                    FullName = "",
                    Phone = "",
                    Email = "",
                    Password = "",
                    FullAddress = "",
                    Role = BO.ROLE.VOLUNTEER,
                    MaxDistance = 0,
                    TypeOfDistance = BO.TYPEOFDISTANCE.WALKINGDISTANCE,
                    AllCallsThatTreated=0,
                    AllCallsThatCanceled=0,
                    AllCallsThatHaveExpired = 0,
                    Active=false,
                    TypeOfCall= BO.TYPEOFCALL.NONE,
                };
            }

            if (CurrentVolunteer != null && CurrentVolunteer.Id != 0)
            {
                s_bl.Volunteer.AddObserver(CurrentVolunteer.Id, VolunteerObserver);
            }
        }


        private void VolunteerObserver()
        {


            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                if (CurrentVolunteer != null)
                {
                    int id = CurrentVolunteer.Id;
                    CurrentVolunteer = null;
                    CurrentVolunteer = s_bl.Volunteer.GetVolunteerDetails(id);
                }
            });
        }

        private void btnAddUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentVolunteer == null)
            {
                MessageBox.Show("No volunteer data available.");
                return;
            }

            if (ButtonText == "Add")
            {
                try
                {
                    if (!IsFormValid(this))
                    {
                        MessageBox.Show("יש שגיאות בטופס. נא לבדוק את השדות המסומנים באדום.");
                        return;
                    }
                    s_bl.Volunteer.AddVolunteer(CurrentVolunteer);
                    MessageBox.Show("Volunteer added successfully");
                    this.Close(); // סגירת החלון
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                try
                {
                    if (!IsFormValid(this))
                    {
                        MessageBox.Show("יש שגיאות בטופס. נא לבדוק את השדות המסומנים באדום.");
                        return;
                    }
                    s_bl.Volunteer.UpdateVolunteerDetails(CurrentVolunteer.Id, CurrentVolunteer);
                    MessageBox.Show("Volunteer updated successfully");
                    this.Close(); // סגירת החלון
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        bool IsFormValid(DependencyObject parent)
        {
            // Force validation updates
            foreach (var control in FindVisualChildren<UIElement>(parent))
            {
                var expr = BindingOperations.GetBindingExpression(control, TextBox.TextProperty);
                expr?.UpdateSource();
            }
            //todo:
            return true;// !Validation.GetHasError(parent);
        }

        IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    var child = VisualTreeHelper.GetChild(depObj, i);
                    if (child is T t)
                        yield return t;

                    foreach (var childOfChild in FindVisualChildren<T>(child))
                        yield return childOfChild;
                }
            }
        }
        private void Delete_click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is Button button && button.CommandParameter is int volunteerId)
            {
                try
                {
                    var result = MessageBox.Show("Are you sure you want to delete volunteer?", "Confirm Delete", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        s_bl.Volunteer.DeleteVolunteerDetails(volunteerId);
                        this.Close(); // סגירת החלון
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Cannot delete volunteer: " + ex.Message);
                }
            }
        }


        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            if (CurrentVolunteer != null && CurrentVolunteer.Id != 0)
            {
                s_bl.Volunteer.RemoveObserver(CurrentVolunteer.Id, VolunteerObserver);
            }
        }
    }
}


