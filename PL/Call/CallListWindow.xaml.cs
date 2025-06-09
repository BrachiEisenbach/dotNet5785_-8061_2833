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

        public IEnumerable<BO.VolunteerInList> CallList
        {
            get { return (IEnumerable<BO.VolunteerInList>)GetValue(CallListProperty); }
            set { SetValue(CallListProperty, value); }
        }

        public static readonly DependencyProperty CallListProperty =
            DependencyProperty.Register("CallList", typeof(IEnumerable<BO.VolunteerInList>), typeof(CallListWindow), new PropertyMetadata(null));

        public BO.TYPEOFCALL type { get; set; } = BO.TYPEOFCALL.NONE;

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void dgCallList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
