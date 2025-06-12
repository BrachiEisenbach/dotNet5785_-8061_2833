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
            DependencyProperty.Register("CurrentVolunteer", typeof(BO.Volunteer), typeof(VolunteerWindow), new PropertyMetadata(null));

        public VolunteerWindowVol()
        {
            InitializeComponent();
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


    }
}
