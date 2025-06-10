using BO;
using DO;
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

namespace PL.Call
{
    /// <summary>
    /// Interaction logic for CallListWindow.xaml
    /// </summary>
    public partial class CallListWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();


        public CallListWindow()
        {
            InitializeComponent();
        }

        public IEnumerable<BO.CallInList> CallList
        {
            get { return (IEnumerable<BO.CallInList>)GetValue(CallListProperty); }
            set { SetValue(CallListProperty, value); }
        }

        public static readonly DependencyProperty CallListProperty =
            DependencyProperty.Register("CallList", typeof(IEnumerable<BO.CallInList>), typeof(CallListWindow), new PropertyMetadata(null));

        //סינון לפי סוג קריאה
        public BO.TYPEOFCALL type { get; set; } = BO.TYPEOFCALL.NONE;
        public BO.CallInList? SelectedCall { get; set; }


        private void queryCallList()
      => CallList = (type == BO.TYPEOFCALL.NONE)
        ? s_bl?.Call.GetCallList(null, null, null)!
        : s_bl?.Call.GetCallList(type, null, BO.VOLUNTEERFIELDSORT.CALLTYPE)!
            .Where(v => v.TypeOfCall == type);


        //סינון לפי סטטוס קריאה
        //public BO.STATUS status { get; set; } = BO.STATUS.none;


        //private void queryCallList()
        // => CallList = (status == BO.STATUS.none)
        //? s_bl?.Call.GetCallList(null, null, null)!
        //: s_bl?.Call.GetCallList(status, null, BO.TYPEOFCALL.FLATTIRE)!
        //    .Where(c => c.Status == status);

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //סינון לפי סוג קריאה

            var comboBox = sender as ComboBox;
            if (comboBox.SelectedValue is BO.TYPEOFCALL selectedType)
            {
                type = selectedType; // עדכון ה-type כאן
                queryCallList(); // קריאה לעדכון הרשימה
            }

            //סינון לפי סטטוס קריאה
            //var comboBox = sender as ComboBox;
            //if (comboBox.SelectedValue is BO.STATUS selectedStatus)
            //{
            //    status = selectedStatus; // עדכון ה-type כאן
            //    queryCallList(); // קריאה לעדכון הרשימה
            //}

        }

        private void dgCallList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SelectedCall != null)
            {
                new CallWindow(SelectedCall.Id).Show();
            }

        }

        private void Delete_click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null && button.Tag != null)
            {
                int CallId = (int)button.Tag;
                try
                {
                    MessageBoxResult result = MessageBox.Show
                        ("Are you sure you want to delete call?", "Confirm Delete", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        s_bl.Call.DeleteCall(CallId);
                        queryCallList();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);

                }
            }
        }

        private void CancelAssignment_Click(object sender, RoutedEventArgs e)
        {
            //אפשרי??????
            //if (sender is Button btn && btn.Tag is int callId)
            //{
            //    // בצעי כאן את ביטול ההקצאה בלוגיקה שלך
            //    MessageBox.Show($"ביטול הקצאה לקריאה {callId}");
            //    s_bl.Call.(callId);    

            //}
        }

        private void btnAddVolunteer(object sender, RoutedEventArgs e)
        {
            new CallWindow(0).Show();

        }
    }
}
