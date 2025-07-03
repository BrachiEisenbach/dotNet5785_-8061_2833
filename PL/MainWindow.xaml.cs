
using PL.Vol;
using System.Windows;

namespace PL
{
    public partial class MainWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public int Id
        {
            get { return (int)GetValue(IdProperty); }
            set { SetValue(IdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IdProperty =
            DependencyProperty.Register("Id", typeof(int), typeof(MainWindow));
        public string Username
        {
            get { return (string)GetValue(UsernameProperty); }
            set { SetValue(UsernameProperty, value); }
        }

        public static readonly DependencyProperty UsernameProperty =
            DependencyProperty.Register("Username", typeof(string), typeof(MainWindow));
        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }

        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(MainWindow));


        private AdminWindow _adminWindow;


        public MainWindow()
        {
            //למחוק שורה זו לפני הגשה
            s_bl.Admin.InitializeDB();
            InitializeComponent();
            this.DataContext = this;
            // Debugging code to set default values for testing purposes

#if DEBUG
            var vol1 = s_bl.Volunteer.GetVolunteerInList(null, 0).ToList()[37];
            Id = vol1.Id;
            Password = s_bl.Volunteer.GetVolunteerDetails(vol1.Id).Password;
            Username = s_bl.Volunteer.GetVolunteerDetails(vol1.Id).FullName;
#endif

        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Id == 0)
                {
                    MessageBox.Show("Please enter a valid ID.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                BO.ROLE roleA = s_bl.Volunteer.GetUserRole(Username, Password);
                BO.ROLE roleB = s_bl.Volunteer.GetUserRoleById(Id);
                BO.ROLE? role = roleA == roleB ? roleA : null;
                if (role == null)
                {
                    MessageBox.Show("One of the details entered is incorrect.", "Authentication Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (role == BO.ROLE.ADMIN)
                {
                    MessageBoxResult result = MessageBox.Show(
                        "Would you like to enter as Admin?\nClick 'Yes' for Admin, 'No' for Volunteer.",
                        "Select Role",
                        MessageBoxButton.YesNoCancel,
                        MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        if (_adminWindow == null || !_adminWindow.IsVisible)
                        {
                            _adminWindow = new AdminWindow(Id);
                            _adminWindow.Show();
                        }
                        else
                        {
                            _adminWindow.Activate();
                        }
                    }
                    else if (result == MessageBoxResult.No)
                    {
                        VolunteerWindowVol userWindow = new Vol.VolunteerWindowVol(Id);
                        userWindow.Show();
                    }
                    // אם Cancel - לא עושים כלום
                }

                else if (role == BO.ROLE.VOLUNTEER)
                {
                    VolunteerWindowVol userWindow = new VolunteerWindowVol(Id); // תמיד נפתח חדש
                    userWindow.Show();
                }
                else
                {
                    MessageBox.Show("You are not authorized to access this application.", "Access Denied", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
            

    }
}
