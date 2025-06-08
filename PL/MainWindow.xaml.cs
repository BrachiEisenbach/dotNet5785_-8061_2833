
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


        private AdminWindow _adminWindow;


        public MainWindow()
        {
            InitializeComponent();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (Id == 0)
            {
                MessageBox.Show("Please enter a valid ID.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            BO.ROLE role = s_bl.Volunteer.GetUserRoleById(Id);

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
                        _adminWindow = new AdminWindow();
                        _adminWindow.Show();
                    }
                    else
                    {
                        _adminWindow.Activate();
                    }
                }
                else if (result == MessageBoxResult.No)
                {
                    UserWindow userWindow = new UserWindow(); 
                    userWindow.Show();
                }
                // אם Cancel - לא עושים כלום
            }

            else if (role == BO.ROLE.VOLUNTEER)
            {
                UserWindow userWindow = new UserWindow(); // תמיד נפתח חדש
                userWindow.Show();
            }
            else
            {
                MessageBox.Show("You are not authorized to access this application.", "Access Denied", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
