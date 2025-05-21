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
    /// Interaction logic for VolunteerWindow.xaml
    /// </summary>
    public partial class VolunteerWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public String ButtonText
        {
            get { return (String)GetValue(ButtonTextProperty); }
            set { SetValue(ButtonTextProperty, value); }
        }
        public static readonly DependencyProperty ButtonTextProperty =
        DependencyProperty.Register("ButtonText", typeof(String), typeof(VolunteerWindow), new PropertyMetadata(""));


        public BO.Volunteer? CurrentVolunteer
        {
            get { return (BO.Volunteer?)GetValue(CurrentVolunteerProperty); }
            set { SetValue(CurrentVolunteerProperty, value); }
        }

        public static readonly DependencyProperty CurrentVolunteerProperty =
            DependencyProperty.Register("CurrentVolunteer", typeof(BO.Volunteer), typeof(VolunteerWindow), new PropertyMetadata(null));



        public VolunteerWindow(int id)
        {
            ButtonText = id == 0 ? "Add" : "Update";
            InitializeComponent();
            this.DataContext = this;
            CurrentVolunteer = (id != 0) ? s_bl.Volunteer.GetVolunteerDetails(id)! : new BO.Volunteer()
            {
                Id = 0,
                FullName = "",
                Phone = "",
                Email = "",
                Password = "",
                FullAddress = "",
                Latitude = null,
                //        Longitude = null,

                //public bool Active { get; set; }
                //public double? MaxDistance { get; set; }
                //public TYPEOFDISTANCE TypeOfDistance { get; set; }
                //public int AllCallsThatTreated { get; init; }
                //public int AllCallsThatCanceled { get; init; }
                //public int AllCallsThatHaveExpired { get; init; }




            };
        }
    }
}
