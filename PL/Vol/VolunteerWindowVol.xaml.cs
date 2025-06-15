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
using BO; // ודא שה-namespace של שכבת ה-BO שלך נכלל
using BlApi;
using PL.Call; // ודא שה-namespace של ה-API של שכבת ה-BL שלך נכלל

namespace PL.Vol
{
    /// <summary>
    /// Interaction logic for VolunteerWindowVol.xaml
    /// </summary>
    public partial class VolunteerWindowVol : Window
    {
        // מופע של ה-BL interface (העדיפו IBl על פני Bl בהתאם ל-DI)
        private static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        // מאפיין התלות עבור המתנדב המוצג בחלון
        public static readonly DependencyProperty CurrentVolunteerProperty =
            DependencyProperty.Register("CurrentVolunteer", typeof(BO.Volunteer), typeof(VolunteerWindowVol), new PropertyMetadata(null));

        public BO.Volunteer CurrentVolunteer
        {
            get { return (BO.Volunteer)GetValue(CurrentVolunteerProperty); }
            set { SetValue(CurrentVolunteerProperty, value); }
        }

        // מאפיין התלות עבור טקסט הכפתור הראשי (הוספה/עדכון)
        public static readonly DependencyProperty ButtonTextProperty =
            DependencyProperty.Register("ButtonText", typeof(string), typeof(VolunteerWindowVol), new PropertyMetadata("Add Volunteer"));

        public string ButtonText
        {
            get { return (string)GetValue(ButtonTextProperty); }
            set { SetValue(ButtonTextProperty, value); }
        }

        // קונסטרקטור עבור מצב "הוסף מתנדב חדש"
        public VolunteerWindowVol()
        {
            InitializeComponent();
            this.CurrentVolunteer = new BO.Volunteer(); // אובייקט מתנדב ריק חדש
            this.ButtonText = "Add Volunteer"; // כפתור להוספה
            // בדרך כלל, במצב הוספה, ID אינו מוצג אוטומטית או שהוא מופק
            // אם ה-ID אוטומטי, לא צריך להגדיר כאן
            // במקרה זה, החלון נסגר לאחר הוספה, כך שאין צורך בהרשמה למשקיף כאן.

            // הירשם לאירועי טעינה וסגירה של החלון
            this.Loaded += Window_Loaded;
            this.Closed += Window_Closed;
        }

        // קונסטרקטור עבור מצב "הצג/עדכן מתנדב קיים"
        public VolunteerWindowVol(int volunteerId)
        {
            InitializeComponent();
            try
            {
                this.CurrentVolunteer = s_bl.Volunteer.GetVolunteerDetails(volunteerId); // טען את המתנדב מה-BL
                this.ButtonText = "Update Volunteer"; // כפתור לעדכון
                // אם רוצים ש-ID לא יהיה ניתן לשינוי במצב עדכון, ב-XAML נגדיר IsReadOnly="True"

                // הירשם לאירועי טעינה וסגירה של החלון
                this.Loaded += Window_Loaded;
                this.Closed += Window_Closed;
            }
            catch (BO.BlDoesNotExistException ex)
            {
                MessageBox.Show($"Volunteer with ID {volunteerId} not found: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close(); // סגור את החלון אם המתנדב לא נמצא
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }

        // --- הוספות עבור מנגנון המשקיפים ---

        // מתודת העדכון של המשקיף
        private void VolunteerDataObserver()
        {
            // המתודה הזו נקראת כאשר המתנדב בשכבת ה-BL משתנה.
            // עלינו לרענן את ה-CurrentVolunteer מחדש מה-BL.
            try
            {
                // וודא שה-ID של המתנדב קיים לפני שמנסים לשלוף אותו (עבור מצב עדכון)
                if (CurrentVolunteer != null && CurrentVolunteer.Id != 0)
                {
                    // זה יעדכן את ה-CurrentVolunteer ויגרום ל-UI להתעדכן
                    this.CurrentVolunteer = s_bl.Volunteer.GetVolunteerDetails(CurrentVolunteer.Id);
                }
            }
            catch (BO.BlDoesNotExistException)
            {
                // אם המתנדב לא נמצא (למשל, נמחק על ידי מנהל בחלון אחר), סגור את החלון.
                MessageBox.Show("Volunteer no longer exists. Closing window.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error refreshing volunteer data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // הרשמה למשקיף עם טעינת החלון
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // נרשום רק אם זה חלון קיים של מתנדב (לא חלון הוספה חדש)
            if (CurrentVolunteer != null && CurrentVolunteer.Id != 0)
            {
                // הירשם כמשקיף על המתנדב הספציפי הזה ב-BL
                // הנחה: ל-s_bl.Volunteer יש מתודה AddObserver שמקבלת ID ו-Action
                s_bl.Volunteer.AddObserver(CurrentVolunteer.Id, VolunteerDataObserver);
            }
        }

        // הסרת ההרשמה מהמשקיף עם סגירת החלון
        private void Window_Closed(object sender, EventArgs e)
        {
            // חשוב מאוד להסיר את ההרשמה כדי למנוע דליפות זיכרון
            if (CurrentVolunteer != null && CurrentVolunteer.Id != 0)
            {
                s_bl.Volunteer.RemoveObserver(CurrentVolunteer.Id, VolunteerDataObserver);
            }
            // גם נסיר את ההרשמה לאירועי החלון עצמם כדי למנוע קריאות כפולות
            this.Loaded -= Window_Loaded;
            this.Closed -= Window_Closed;
        }

        // --- סוף הוספות עבור מנגנון המשקיפים ---


        // לחיצה על כפתור "הוסף/עדכן מתנדב"
        private void btnAddUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // בהתאם להערה שלך, חלון זה משמש רק לעדכון מתנדב קיים
                s_bl.Volunteer.UpdateVolunteerDetails(CurrentVolunteer.Id, this.CurrentVolunteer);
                MessageBox.Show("Volunteer updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                this.Close(); // סגור את החלון לאחר הפעולה
            }
            catch (BO.BlAlreadyExistException ex)
            {
                MessageBox.Show($"Volunteer with this ID already exists: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // לחיצה על כפתור "בחר קריאה"
        private void btnChooseCall_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CurrentVolunteer.CallInTreate != null)
                {
                    MessageBox.Show("Volunteer is already treating a call. Cannot choose a new one.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!CurrentVolunteer.Active)
                {
                    MessageBox.Show("Volunteer is not active. Cannot choose a call.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // פתח חלון בחירת קריאות פנויות
                ChooseCallWindow chooseCallWin = new ChooseCallWindow(CurrentVolunteer.Id);
                chooseCallWin.ShowDialog(); // פתח כחלון דיאלוג

                // **הסרת שורת הרענון הידנית**
                // this.CurrentVolunteer = s_bl.Volunteer.GetVolunteerDetails(CurrentVolunteer.Id);
                // המשקיף ידאג לרענון כאשר ה-BL יודיע על שינוי במתנדב.
            }
            catch (BO.BlDoesNotExistException ex) // שיניתי את שם החריגה לכללית יותר לפי המקרה הזה
            {
                MessageBox.Show($"No suitable calls found: {ex.Message}", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred while choosing a call: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // לחיצה על כפתור "סיום טיפול"
        private void btnFinishTreat_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CurrentVolunteer.CallInTreate == null)
                {
                    MessageBox.Show("No call is currently in treatment to finish.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // קבל את ה-CallId של הקריאה בטיפול
                int callIdToFinish = CurrentVolunteer.CallInTreate.CallId;

                s_bl.Call.updateFinishTreat(CurrentVolunteer.Id, callIdToFinish); // קרא לפעולה ב-BL
                MessageBox.Show($"Call {callIdToFinish} finished successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                // **הסרת שורת הרענון הידנית**
                // this.CurrentVolunteer = s_bl.Volunteer.GetVolunteerDetails(CurrentVolunteer.Id);
                // המשקיף ידאג לרענון כאשר ה-BL יודיע על שינוי במתנדב.
            }
            catch (BO.BlDoesNotExistException ex)
            {
                MessageBox.Show($"Call not found: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while finishing treatment: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // הפונקציה המקורית של ביטול טיפול (מוערת בקוד ששלחת, אני מניח שאתה הולך לשחרר אותה)
        // אני מניח שאתה תשלב כאן את הפתרון ל-cancelTreat (עם ה-AssignmentId או CallId)
        // כפי שדיברנו בשיחות קודמות, לאחר שתוודא את המבנה של ה-BL.
        // הקוד הבא הוא רק placeholder אם אתה הולך להשתמש בו.
        //private void btnCancelTreat_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        if (CurrentVolunteer.CallInTreate == null)
        //        {
        //            MessageBox.Show("No call is currently in treatment to cancel.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        //            return;
        //        }

        //        // **כאן תשלב את הלוגיקה הנכונה ל-cancelTreat.**
        //        // אפשרות 1 (מומלצת, אם מותר לשנות BL):
        //        // s_bl.Call.cancelTreat(CurrentVolunteer.Id, CurrentVolunteer.CallInTreate.CallId);

        //        // אפשרות 2 (אם אסור לשנות BL):
        //        var activeAssignment = s_bl.Assignment.GetAll(assignment =>
        //                                                        assignment.VolunteerId == CurrentVolunteer.Id &&
        //                                                        assignment.CallId == CurrentVolunteer.CallInTreate.CallId &&
        //                                                        assignment.EndTimeOfTreatment == null // או כל תנאי אחר שמגדיר "פעיל"
        //                                                        ).FirstOrDefault();

        //        if (activeAssignment == null)
        //        {
        //            MessageBox.Show("Could not find the active assignment for this call. Cannot cancel.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //            return;
        //        }
        //        s_bl.Call.cancelTreat(CurrentVolunteer.Id, activeAssignment.Id); // שימוש ב-AssignmentId

        //        MessageBox.Show($"Call treatment canceled!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

        //        // **הסרת שורת הרענון הידנית**
        //        // this.CurrentVolunteer = s_bl.Volunteer.GetVolunteerDetails(CurrentVolunteer.Id);
        //        // המשקיף ידאג לרענון כאשר ה-BL יודיע על שינוי במתנדב.
        //    }
        //    catch (BO.BlDoesNotExistException ex)
        //    {
        //        MessageBox.Show($"Cancellation failed: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //    catch (BO.BlUnauthorizedException ex)
        //    {
        //        MessageBox.Show($"Permission denied: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //    catch (BO.BlInvalidOperationException ex)
        //    {
        //        MessageBox.Show($"Operation not allowed: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"An unexpected error occurred while canceling treatment: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}

        // לחיצה על כפתור "היסטוריית קריאות"
        private void btnHistory_click(object sender, RoutedEventArgs e)
        {
            try
            {
                // פתח חלון חדש להצגת היסטוריית הקריאות של המתנדב
                PL.Call.CallsHistoryWindow historyWin = new PL.Call.CallsHistoryWindow(CurrentVolunteer.Id);
                historyWin.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading call history: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}