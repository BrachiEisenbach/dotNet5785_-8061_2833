using BO;
using PL.Call;
using PL.Vol;
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

namespace PL.Vol
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
            volunteerListObserver();
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


        public static readonly DependencyProperty TypeProperty =
         DependencyProperty.Register(nameof(Type), typeof(BO.TYPEOFCALL), typeof(VolunteerListWindow),
         new PropertyMetadata(BO.TYPEOFCALL.NONE, OnSelectedCallTypeChanged));

        public BO.TYPEOFCALL Type
        {
            get { return (BO.TYPEOFCALL)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        private BO.TYPEOFCALL type { get; set; } = BO.TYPEOFCALL.NONE;

        private void queryVolunteerList()
        {
            try
            {
                VolunteerList = (Type == BO.TYPEOFCALL.NONE)
                            ? s_bl?.Volunteer.GetVolunteerInList(null, null)!
                            : s_bl?.Volunteer.GetVolunteerInList(null, BO.VOLUNTEERFIELDSORT.CALLTYPE)!
                                  .Where(v => v.TypeOfCall == Type)!;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load volunteer list: " + ex.Message);
                VolunteerList = Enumerable.Empty<BO.VolunteerInList>();
            }
        }
        private void volunteerListObserver()
        => queryVolunteerList();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            s_bl.Volunteer.AddObserver(volunteerListObserver);
          
        }
        private void Window_Closed(object sender, EventArgs e)
        => s_bl.Volunteer.RemoveObserver(volunteerListObserver);


        private static void OnSelectedCallTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as VolunteerListWindow;
            if (control != null)
            {
                control.volunteerListObserver();
            }
        }
        private void Delete_click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is Button button && button.CommandParameter is int volunteerId)
            {
                var result = MessageBox.Show(
                    "Are you sure you want to delete this volunteer?",
                    "Confirm Delete",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        s_bl.Volunteer.DeleteVolunteerDetails(volunteerId);
                        volunteerListObserver();
                    }
                    catch (BlDoesNotExistException)
                    {
                        MessageBox.Show(
                            "The volunteer you are trying to delete does not exist.",
                            "Delete Failed",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show(
                            "An unexpected error occurred while trying to delete the volunteer. Please try again or contact support.",
                            "Unexpected Error",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                }
            }
        }

        private void btnAddVolunteer(object sender, RoutedEventArgs e)
        {
            try
            {
                new VolunteerWindow(0).Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to open new volunteer window: " + ex.Message);
            }
        }

        private void dgVolunteerList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (SelectesVolunteer != null)
                {
                    new VolunteerWindow(SelectesVolunteer.Id).Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to open volunteer window: " + ex.Message);
            }
        }

    }
}