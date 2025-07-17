using BO;
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
    /// Interaction logic for CallsHistoryWindow.xaml
    /// </summary>
    public partial class CallsHistoryWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();


        public static readonly DependencyProperty SelectedCallTypeProperty =
        DependencyProperty.Register(
        nameof(SelectedCallType),
        typeof(BO.TYPEOFCALL),
        typeof(CallsHistoryWindow),
        new PropertyMetadata(default(BO.TYPEOFCALL), OnSelectedCallTypeChanged));

        public BO.TYPEOFCALL SelectedCallType
        {
            get => (BO.TYPEOFCALL)GetValue(SelectedCallTypeProperty);
            set => SetValue(SelectedCallTypeProperty, value);
        }


        public static readonly DependencyProperty SelectedSortFieldProperty =
        DependencyProperty.Register(
        nameof(SelectedSortField),
        typeof(BO.ClosedCallInListField),
        typeof(CallsHistoryWindow),
        new PropertyMetadata(default(BO.ClosedCallInListField), OnSelectedSortFieldChanged));

        public BO.ClosedCallInListField SelectedSortField
        {
            get => (BO.ClosedCallInListField)GetValue(SelectedSortFieldProperty);
            set => SetValue(SelectedSortFieldProperty, value);
        }


        public IEnumerable<BO.ClosedCallInList> ClosedCallsList
        {
            get { return (IEnumerable<BO.ClosedCallInList>)GetValue(ClosedCallsListProperty); }
            set { SetValue(ClosedCallsListProperty, value); }
        }
        public static readonly DependencyProperty ClosedCallsListProperty =
            DependencyProperty.Register("ClosedCallsList", typeof(IEnumerable<BO.ClosedCallInList>), typeof(CallsHistoryWindow), new PropertyMetadata(null));

        private BO.TYPEOFCALL type { get; set; } = BO.TYPEOFCALL.NONE;
        private BO.ClosedCallInListField? sort { get; set; } = null;

        private int volId { get; set; } = 0;
        public CallsHistoryWindow(int VId)
        {
            try
            {
                ClosedCallsList = s_bl.Call.GetClosedCallInList(VId, null, null)!;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load closed calls list: " + ex.Message);
            }

            volId = VId;
            InitializeComponent();
            this.Loaded += Window_Loaded;
            this.Closed += Window_Closed;
        }
        private static void OnSelectedCallTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as CallsHistoryWindow;
            if (control != null)
            {
                var newType = (BO.TYPEOFCALL)e.NewValue;
                control.type = newType;
                control.queryClosedCallList();
            }
        }
        private static void OnSelectedSortFieldChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as CallsHistoryWindow;
            if (control != null)
            {
                var newSortField = (BO.ClosedCallInListField)e.NewValue;
                control.sort = newSortField;
                control.queryClosedCallList();
            }
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                s_bl.Call.AddObserver(closedCallListObserver);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to initialize closed calls observer: " + ex.Message);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            try
            {
                s_bl.Call.RemoveObserver(closedCallListObserver);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to remove closed calls observer: " + ex.Message);
            }
        }
        //לוודא שה TYPE מתעדכן
        private void queryClosedCallList()
        {
            try
            {
                BO.TYPEOFCALL? filteredType = type == BO.TYPEOFCALL.NONE ? null : type;
                ClosedCallsList = s_bl?.Call.GetClosedCallInList(volId, filteredType, sort);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to query closed calls: " + ex.Message);
            }
        }


        private void closedCallListObserver()
        {
            try
            {
                queryClosedCallList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to refresh closed calls: " + ex.Message);
            }
        }

    }
}
