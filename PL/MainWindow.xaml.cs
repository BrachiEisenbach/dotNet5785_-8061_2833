using System.Windows;

namespace PL
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            string id = IdTextBox.Text;

            if (string.IsNullOrWhiteSpace(id))
            {
                MessageBox.Show("Please enter a valid ID.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


        }
    }
}
