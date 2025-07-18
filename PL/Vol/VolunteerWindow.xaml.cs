
using BO;
using System;
using System.Windows;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Controls;
using DO;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Threading;
using Application = System.Windows.Application;


namespace PL.Vol
{
    /// <summary>
    /// Interaction logic for Vol.xaml
    /// </summary>
    public partial class VolunteerWindow : Window
    { 
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        private int adminId;
        private volatile bool _isVolunteerUpdating = false;
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


        public VolunteerWindow(int AdminId,int id)
        {
            
            IsUpdateMode = id != 0; // כאן נשתמש בזה בבינדינג ל-IsReadOnly
            ButtonText = id == 0 ? "Add" : "Update";
            InitializeComponent();
            DataContext = this;
            adminId= AdminId;
            try {

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
                    
                };
            }

            if (CurrentVolunteer != null && CurrentVolunteer.Id != 0)
            {
                s_bl.Volunteer.AddObserver(CurrentVolunteer.Id, VolunteerObserver);
            }
            }
            catch(BlDoesNotExistException ex)
            {
                MessageBox.Show("Unable to load volunteer details: " + ex.Message);
            }catch(Exception ex)
            {
                MessageBox.Show("Unable to load volunteer details: " + ex.Message);

            }
        }


        private void VolunteerObserver()
        {
            if (_isVolunteerUpdating)
            {
                return; // התעלם אם עדכון קודם עדיין בעיצומו
            }

            _isVolunteerUpdating = true; // הדלק את הדגל

            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                try
                {
                    if (CurrentVolunteer != null)
                    {
                        int id = CurrentVolunteer.Id;
                        CurrentVolunteer = s_bl.Volunteer.GetVolunteerDetails(id);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error refreshing volunteer data: " + ex.Message);
                }
                finally
                {
                    _isVolunteerUpdating = false; // כבה את הדגל בסיום פעולת ה-Dispatcher
                }
            }));
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
                    s_bl.Volunteer.UpdateVolunteerDetails(adminId, CurrentVolunteer);
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
            bool hasError = System.Windows.Controls.Validation.GetHasError(obj);
            if (hasError)
                return true;

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
 
        private void Delete_click(object sender, RoutedEventArgs e)
        {
            try
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

                        s_bl.Volunteer.DeleteVolunteerDetails(volunteerId);
                        this.Close(); // סגירת החלון
                    }
                }
            }
            catch (BlVolunteerInProgressException ex)
            {
                Console.WriteLine("נכנסתי ל catch המתאים");
                MessageBox.Show(
                    ex.Message,
                    "Cannot Delete Volunteer",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                Console.WriteLine("נכנסתי ל catch ברירת מחדל");
                MessageBox.Show(
                    $"Unexpected error: {ex.GetType().Name}\n{ex.Message}",
                    "Unexpected Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
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


