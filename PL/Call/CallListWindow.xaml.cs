using BO;
using PL.Vol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL.Call
{
    public partial class CallListWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public CallListWindow()
        {
            InitializeComponent();
            queryCallList();
        }

        // ----------- DP: CallList -------------
        public IEnumerable<BO.CallInList> CallList
        {
            get => (IEnumerable<BO.CallInList>)GetValue(CallListProperty);
            set => SetValue(CallListProperty, value);
        }

        public static readonly DependencyProperty CallListProperty =
            DependencyProperty.Register("CallList", typeof(IEnumerable<BO.CallInList>), typeof(CallListWindow), new PropertyMetadata(null));

        // ----------- DP: Type (ל־ComboBox) -------------
        public BO.TYPEOFCALL Type
        {
            get => (BO.TYPEOFCALL)GetValue(TypeProperty);
            set => SetValue(TypeProperty, value);
        }

        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(BO.TYPEOFCALL), typeof(CallListWindow), new PropertyMetadata(BO.TYPEOFCALL.NONE, OnTypeChanged));

        private static void OnTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var window = d as CallListWindow;
            window?.queryCallList();
        }

        public BO.CallInList? SelectedCall { get; set; }

        // ----------------- שליפת קריאות -----------------
        private void queryCallList()
        {
            if (Type == BO.TYPEOFCALL.NONE)
                CallList = s_bl.Call.GetCallList(null, null, null);
            else
                CallList = s_bl.Call
                    .GetCallList(Type, null, BO.VOLUNTEERFIELDSORT.CALLTYPE)
                    .Where(v => v.TypeOfCall == Type);
        }

        // ----------------- לחיצה כפולה על קריאה -----------------
        private void dgCallList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SelectedCall != null)
                new CallWindow(SelectedCall.Id).Show();
        }

        // ----------------- מחיקת קריאה -----------------
        private void Delete_click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int callId)
            {
                var result = MessageBox.Show("Are you sure you want to delete call?", "Confirm Delete", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        s_bl.Call.DeleteCall(callId);
                        queryCallList();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        // ----------------- ביטול הקצאה (טרם ממומש) -----------------
        private void CancelAssignment_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int callId)
            {
                MessageBox.Show($"(TODO) ביטול הקצאה לקריאה {callId}");
                // כאן תוכלי לממש את הביטול בפועל:
                // s_bl.Call.CancelAssignment(callId);
                queryCallList();
            }
        }

        // ----------------- פתיחת קריאה חדשה -----------------
        private void btnAddVolunteer(object sender, RoutedEventArgs e)
        {
            new CallWindow(0).Show();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
