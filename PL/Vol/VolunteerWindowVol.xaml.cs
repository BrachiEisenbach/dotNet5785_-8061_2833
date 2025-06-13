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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class VolunteerWindowVol : Window
    {
        private static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public BO.Volunteer? CurrentVolunteer
        {
            get => (BO.Volunteer?)GetValue(CurrentVolunteerProperty);
            set => SetValue(CurrentVolunteerProperty, value);
        }
        public static readonly DependencyProperty CurrentVolunteerProperty =
            DependencyProperty.Register("CurrentVolunteer", typeof(BO.Volunteer), typeof(VolunteerWindowVol), new PropertyMetadata(null));

        public VolunteerWindowVol(int Id)
        {
            InitializeComponent();
            //this.Loaded += Window_Loaded;
            //this.Closed += OnClosed;
            DataContext = this;
            CurrentVolunteer = s_bl.Volunteer.GetVolunteerDetails(Id);

        }
        bool IsFormValid(DependencyObject parent)
        {
            // עדכן את כל ה־Binding ל־Source
            foreach (var control in FindVisualChildren<UIElement>(parent))
            {
                var expr = BindingOperations.GetBindingExpression(control, TextBox.TextProperty);
                expr?.UpdateSource();

                var exprCombo = BindingOperations.GetBindingExpression(control, ComboBox.SelectedValueProperty);
                exprCombo?.UpdateSource();
            }

            // בדוק אם יש שגיאות
            return !HasValidationError(parent);
        }

        bool HasValidationError(DependencyObject obj)
        {
            //if (Validation.GetHasError(obj))
            //    return true;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i);
                if (HasValidationError(child))
                    return true;
            }

            return false;
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
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            if (CurrentVolunteer != null && CurrentVolunteer.Id != 0)
            {
                s_bl.Volunteer.RemoveObserver(CurrentVolunteer.Id, VolunteerObserver);
            }
        }

        private void btnHistory_click(object sender, RoutedEventArgs e)
        {
            if (CurrentVolunteer == null)
            {
                MessageBox.Show("No volunteer data available.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            new PL.Call.CallsHistoryWindow(CurrentVolunteer.Id).Show();
        }
    }
}
//public VolunteerWindow(int id)
//{

//    IsUpdateMode = id != 0; // כאן נשתמש בזה בבינדינג ל-IsReadOnly
//    ButtonText = id == 0 ? "Add" : "Update";
//    InitializeComponent();
//    DataContext = this;

//    if (id != 0)
//    {
//        CurrentVolunteer = s_bl.Volunteer.GetVolunteerDetails(id);
//    }
//    else
//    {
//        CurrentVolunteer = new BO.Volunteer()
//        {
//            Id = 0,
//            FullName = "",
//            Phone = "",
//            Email = "",
//            Password = "",
//            FullAddress = "",
//            Role = BO.ROLE.VOLUNTEER,
//            MaxDistance = 0,
//            TypeOfDistance = BO.TYPEOFDISTANCE.WALKINGDISTANCE,
//            AllCallsThatTreated = 0,
//            AllCallsThatCanceled = 0,
//            AllCallsThatHaveExpired = 0,
//            Active = false,
//            TypeOfCall = BO.TYPEOFCALL.NONE,
//        };
//    }


