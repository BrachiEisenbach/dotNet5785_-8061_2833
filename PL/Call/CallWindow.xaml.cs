using BO;
using PL.Vol;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Policy;
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

namespace PL.Call
{
    /// <summary>
    /// Interaction logic for CallWindow.xaml
    /// </summary>
    public partial class CallWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public string ButtonText
        {
            get => (string)GetValue(ButtonTextProperty);
            set => SetValue(ButtonTextProperty, value);
        }
        public static readonly DependencyProperty ButtonTextProperty =
            DependencyProperty.Register("ButtonText", typeof(string), typeof(CallWindow), new PropertyMetadata(""));


        public BO.Call? CurrentCall
        {
            get => (BO.Call?)GetValue(CurrentCallProperty);
            set => SetValue(CurrentCallProperty, value);
        }
        public static readonly DependencyProperty CurrentCallProperty =
            DependencyProperty.Register("CurrentCall", typeof(BO.Call), typeof(CallWindow), new PropertyMetadata(null));


        public bool IsUpdateMode
        {
            get => (bool)GetValue(IsUpdateModeProperty);
            set => SetValue(IsUpdateModeProperty, value);
        }
        public static readonly DependencyProperty IsUpdateModeProperty =
            DependencyProperty.Register("IsUpdateMode", typeof(bool), typeof(CallWindow), new PropertyMetadata(false));


        public CallWindow(int id)
        {

            InitializeComponent();
            DataContext = this;
            IsUpdateMode = id != 0;
            ButtonText = id == 0 ? "Add" : "Update";

            if (id != 0)
            {
                CurrentCall = s_bl.Call.GetCallDetails(id);
            }
            else
            {
                CurrentCall = new BO.Call()
                {
                    Id = 0,
                    TypeOfCall = BO.TYPEOFCALL.NONE,
                    VerbalDescription = "",
                    FullAddress = "",
                    MaxTimeToFinish = null

                };
            }

            if (CurrentCall != null && CurrentCall.Id != 0)
            {
                s_bl.Call.AddObserver(CurrentCall.Id, CallObserver);
            }
        }


        private void CallObserver()
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                if (CurrentCall != null)
                {
                    int id = CurrentCall.Id;
                    CurrentCall = null;
                    CurrentCall = s_bl.Call.GetCallDetails(id);
                }
            });
        }

        private void btnAddUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentCall == null)
            {
                MessageBox.Show("No all data available.");
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
                    s_bl.Call.AddCall(CurrentCall);
                    MessageBox.Show("Call added successfully");
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
                    s_bl.Call.UpdateCallDetails(CurrentCall);
                    MessageBox.Show("Call updated successfully");
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


        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            if (CurrentCall != null && CurrentCall.Id != 0)
            {
                s_bl.Call.RemoveObserver(CurrentCall.Id, CallObserver);
            }
        }

    }
}
