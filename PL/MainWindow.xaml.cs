using BO;
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
            try
            {
                s_bl.Admin.InitializeDB();
                InitializeComponent();
                this.DataContext = this;

#if DEBUG
                var vol1 = s_bl.Volunteer.GetVolunteerInList(null, null).ToList()[0];
                Id = vol1.Id;
                var details = s_bl.Volunteer.GetVolunteerDetails(vol1.Id);
                Password = details.Password;
                Username = details.FullName;
#endif
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Initialization error:\n{ex.Message}", "Startup Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!ValidateInputs()) return;

                BO.ROLE role = AuthenticateUser();

                if (role == BO.ROLE.ADMIN)
                    ShowAdminOrVolunteerChoice();
                else if (role == BO.ROLE.VOLUNTEER)
                    OpenVolunteerWindow();
                else
                    ShowAccessDenied();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Login error:\n{ex.Message}", "Authentication Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidateInputs()
        {
            if (Id == 0 || string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                MessageBox.Show("Please enter a valid ID, username, and password.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        private BO.ROLE AuthenticateUser()
        {
            try
            {
                BO.ROLE roleA = s_bl.Volunteer.GetUserRole(Username, Password);
                BO.ROLE roleB = s_bl.Volunteer.GetUserRoleById(Id);

                if (roleA != roleB)
                    throw new Exception("User credentials do not match.");

                return roleA;
            }
            catch (BlDoesNotExistException ex)
            {
                MessageBox.Show($"Authentication error:\n{ex.Message}", "Login Error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Authentication error:\n{ex.Message}", "Login Error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }

        private void ShowAdminOrVolunteerChoice()
        {
            try
            {
                MessageBoxResult result = MessageBox.Show(
                    "Do you want to enter as Admin?\nClick 'Yes' for Admin or 'No' for Volunteer.",
                    "Choose Role",
                    MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                    OpenAdminWindow();
                else if (result == MessageBoxResult.No)
                    OpenVolunteerWindow();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error choosing role:\n{ex.Message}", "Role Selection Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OpenAdminWindow()
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening admin window:\n{ex.Message}", "Admin Window Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OpenVolunteerWindow()
        {
            try
            {
                VolunteerWindowVol userWindow = new VolunteerWindowVol(Id);
                userWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening volunteer window:\n{ex.Message}", "Volunteer Window Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ShowAccessDenied()
        {
            MessageBox.Show("You are not authorized to access this application.", "Access Denied", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
