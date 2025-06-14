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
    /// Interaction logic for ChooseCall.xaml
    /// </summary>
    public partial class ChooseCallWindow : Window
    {
        private static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public ChooseCallWindow()
        {
            InitializeComponent();
        }

        public IEnumerable<BO.CallInList> CallList
        {
            get => (IEnumerable<BO.CallInList>)GetValue(CallListProperty);
            set => SetValue(CallListProperty, value);
        }

        public static readonly DependencyProperty CallListProperty =
            DependencyProperty.Register("CallList", typeof(IEnumerable<BO.CallInList>), typeof(CallListWindow), new PropertyMetadata(null));


        public BO.TYPEOFCALL Type
        {
            get => (BO.TYPEOFCALL)GetValue(TypeProperty);
            set => SetValue(TypeProperty, value);
        }

        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(BO.TYPEOFCALL), typeof(CallListWindow), new PropertyMetadata(BO.TYPEOFCALL.NONE, OnTypeChanged));

        public BO.CallInList? SelectedCall { get; set; }


        private static void OnTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var window = d as ChooseCallWindow;
            window?.queryCallListChooseCall();
        }


        private void queryCallListChooseCall()
        {
            if(Type == BO.TYPEOFCALL.NONE)
            {
                CallList = s_bl.Call
                    .GetCallList(null, null, null)
                    .Where(c => c.Status == BO.STATUS.Open);
            }            
            else
                CallList = s_bl.Call
                    .GetCallList(Type, null, BO.VOLUNTEERFIELDSORT.CALLTYPE)
                    .Where(c => c.TypeOfCall == Type && c.Status == BO.STATUS.Open);
        }

        private void Updete_click(object sender, RoutedEventArgs e)
        {

        }

        private void DataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }
        private void DataGridRow_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        //private void Choose_click(object sender, RoutedEventArgs e)
        //{
        //    if (e.OriginalSource is Button button && button.CommandParameter is Tuple<int, int> parameters)
        //    {
        //        int callId = parameters.Item1;
        //        int volunteerId = parameters.Item2;

        //        try
        //        {
        //            var result = MessageBox.Show("Are you sure you want to choose this call?", "Confirm", MessageBoxButton.YesNo);
        //            if (result == MessageBoxResult.Yes)
        //            {
        //                s_bl.Call.chooseCall(callId, volunteerId); // שימי לב שפה צריך להתאים גם בצד ה-BL
        //                queryCallList();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show("Cannot choose call: " + ex.Message);
        //        }
        //    }
        //}
    }
//    public ICommand ChooseCallCommand => new RelayCommand<BO.CallInList>(call =>
//{
//    s_bl.Call.AssignCallToVolunteer(call.Id, volunteerId);
//    queryCallListChooseCall(); // לרענן את הרשימה
//});

}
