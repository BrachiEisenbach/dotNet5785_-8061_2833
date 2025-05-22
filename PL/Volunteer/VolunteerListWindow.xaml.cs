using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL.Volunteer
{
    /// <summary>
    /// Interaction logic for VolunteerListWindow.xaml
    /// </summary>
    public partial class VolunteerListWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public BO.VolunteerInList? SelectesVolunteer { get; set; }
        
        public VolunteerListWindow()
        {
            InitializeComponent();
            this.Loaded += Window_Loaded;
            this.Closed += Window_Closed;
        }

        public IEnumerable<BO.VolunteerInList> VolunteerList
        {
            get { return (IEnumerable<BO.VolunteerInList>)GetValue(VolunteerListProperty); }
            set { SetValue(VolunteerListProperty, value); }
        }

        public static readonly DependencyProperty VolunteerListProperty =
            DependencyProperty.Register("VolunteerList", typeof(IEnumerable<BO.VolunteerInList>), typeof(VolunteerListWindow), new PropertyMetadata(null));

        public BO.TYPEOFCALL type { get; set; } = BO.TYPEOFCALL.NONE;

        private void queryVolunteerList()
            => VolunteerList = (type == BO.TYPEOFCALL.NONE)
                ? s_bl?.Volunteer.GetVolunteerInList(null, null)!
                : s_bl?.Volunteer.GetVolunteerInList(null, BO.VOLUNTEERFIELDSORT.CALLTYPE)!
                    .Where(v => v.TypeOfCall == type);

        private void volunteerListObserver()
        => queryVolunteerList();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        => s_bl.Volunteer.AddObserver(volunteerListObserver);

        private void Window_Closed(object sender, EventArgs e)
        => s_bl.Volunteer.RemoveObserver(volunteerListObserver);


        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox.SelectedValue is BO.TYPEOFCALL selectedType)
            {
                type = selectedType; // עדכון ה-type כאן
                queryVolunteerList(); // קריאה לעדכון הרשימה
            }
        }

        private void Delete_click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null && button.Tag != null)
            {
                int volunteerId = (int)button.Tag;
                try
                {
                    MessageBoxResult result = MessageBox.Show
                        ("Are you sure you want to delete volunteer?", "Confirm Delete", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        s_bl.Volunteer.DeleteVolunteerDetails(volunteerId);
                        queryVolunteerList();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);

                }
            }
        }

        private void dgVolunteerList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(SelectesVolunteer!=null)
            {
                new VolunteerWindow(SelectesVolunteer.Id).Show();
            }
        }
    }
}