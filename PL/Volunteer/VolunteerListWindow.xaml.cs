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

        public VolunteerListWindow()
        {
            InitializeComponent();
        }

        public IEnumerable<BO.VolunteerInList> VolunteerList
        {
            get { return (IEnumerable<BO.VolunteerInList>)GetValue(VolunteerListProperty); }
            set { SetValue(VolunteerListProperty, value); }
        }

        public static readonly DependencyProperty VolunteerListProperty =
            DependencyProperty.Register("VolunteerList", typeof(IEnumerable<BO.VolunteerInList>), typeof(VolunteerListWindow), new PropertyMetadata(null));

        public BO.TYPEOFCALL type { get; set; }=BO.TYPEOFCALL.NONE;

        private void queryVolunteerList()
        => VolunteerList = (type == BO.TYPEOFCALL.NONE) ?
        s_bl?.Volunteer.GetVolunteerInList(null, null)! : s_bl?.Volunteer.GetVolunteerInList(null, BO.VOLUNTEERFIELDSORT.CALLTYPE)!.Where(v => v.TypeOfCall == type);

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            queryVolunteerList();
        }
    }
}
