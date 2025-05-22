using System;
using System.Windows;

namespace PL.Volunteer
{
    /// <summary>
    /// Interaction logic for VolunteerWindow.xaml
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


        public VolunteerWindow(int id)
        {
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
                    TypeOfDistance = BO.TYPEOFDISTANCE.WALKINGDISTANCE
                };
            }

            if (CurrentVolunteer != null && CurrentVolunteer.Id != 0)
            {
                s_bl.Volunteer.AddObserver(CurrentVolunteer.Id, VolunteerObserver);
            }
        }


        private void VolunteerObserver()
        {
            Application.Current.Dispatcher.Invoke(() =>
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
                    s_bl.Volunteer.AddVolunteer(CurrentVolunteer);
                    MessageBox.Show("Volunteer added successfully");
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
                    s_bl.Volunteer.UpdateVolunteerDetails(CurrentVolunteer.Id, CurrentVolunteer);
                    MessageBox.Show("Volunteer updated successfully");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
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
