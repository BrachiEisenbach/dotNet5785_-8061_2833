using BO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;


namespace PL.Converters
{
    public class StatusToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is STATUS status)
            {
                return status == STATUS.Open ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class AssignedOnlyVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is STATUS status)
            {
                return status == STATUS.InTreatment ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
    public class BoolToVisibilityConverter : IValueConverter
    {
        public bool Invert { get; set; } = false;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool val = value is bool b && b;

            if (Invert)
                val = !val;

            return val ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
    public class NullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            value != null ? Visibility.Visible : Visibility.Collapsed;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
    public class CallInTreateToIsEnabledConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var call = values[0];
            return call == null; // אם אין קריאה – אפשר לסמן
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
    public class InverseBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b)
                return !b;
            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b)
                return !b;
            return false;
        }

    }
    public class StatusToGeneralEditabilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is STATUS status &&
                   (status == STATUS.Open || status == STATUS.OpenDangerZone);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
    public class StatusToDeadlineEditabilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is STATUS status &&
                       (status == STATUS.Open || status == STATUS.OpenDangerZone||status == STATUS.InTreatment || status == STATUS.InTreatmentDangerZone);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }

    public class TupleConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return Tuple.Create(values[0], values[1]); // אפשר גם ליצור מחלקה משלך במקום Tuple
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class IsNullToIsEnabledConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // הפקד יהיה מאופשר (IsEnabled=true) רק אם הערך הוא null
            return value == null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
    public class UpdateModeAndStatusToIsEnabledConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2)
                return false;

            if (!(values[0] is bool isUpdateMode))
                return false;

            if (!(values[1] is STATUS status))
                return false;

            if (!isUpdateMode) // במצב הוספה - תמיד פעיל
                return true;

            // במצב עדכון - פעיל רק אם סטטוס מתאים
            return status == STATUS.Open || status == STATUS.OpenDangerZone;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }

    public class CanChooseCallConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // ודא שיש שני ערכים: CallInTreate (values[0]) ו-Active (values[1])
            if (values != null && values.Length >= 2)
            {
                var callInTreate = values[0]; // אובייקט ה-CallInTreate של המתנדב
                bool isActive = values[1] is bool b && b; // האם המתנדב פעיל

                // הכפתור "בחר קריאה" יהיה פעיל רק אם:
                // 1. למתנדב אין קריאה בטיפול כרגע (callInTreate הוא null)
                // 2. המתנדב פעיל (isActive הוא true)
                return callInTreate == null && isActive;
            }
            return false; // אם הערכים לא תקינים, הכפתור מושבת
        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class TypeOfCallToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is BO.TYPEOFCALL callType)
            {
                // הגדר צבעים שונים לכל סוג קריאה
                return callType switch
                {
                    BO.TYPEOFCALL.CARBURGLARY => new SolidColorBrush(Color.FromRgb(0x90, 0xEE, 0x90)), // Salmon: FF=255, A0=160, 7A=122
                    BO.TYPEOFCALL.FLATTIRE => new SolidColorBrush(Color.FromRgb(0x87, 0xCE, 0xEB)),    // SkyBlue: 87=135, CE=206, EB=235
                    BO.TYPEOFCALL.REDRIVE => new SolidColorBrush(Color.FromRgb(0xFF, 0xA0, 0x7A)),    // LightGreen: 90=144, EE=238, 90=144
                    BO.TYPEOFCALL.NONE => new SolidColorBrush(Color.FromRgb(0xFF, 0x63, 0x47)),      // Tomato: FF=255, 63=99, 47=71                    // הוסף כאן מקרים נוספים אם יש לך סוגי קריאות אחרים
                    _ => new SolidColorBrush(Colors.Transparent) // צבע ברירת מחדל או שקוף
                };
            }
            return new SolidColorBrush(Colors.Transparent); // צבע שקוף אם הקלט אינו תקין
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException(); // המרה הפוכה אינה נחוצה במקרה זה
        }
    }

    public class StatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is BO.STATUS status)
            {
                return status switch
                {
                    BO.STATUS.InTreatment => new SolidColorBrush(Color.FromRgb(0x4C, 0xAF, 0x50)), // Green - בטיפול (4CAF50)
                    BO.STATUS.InTreatmentDangerZone => new SolidColorBrush(Color.FromRgb(0xFF, 0xC1, 0x07)), // Amber - בטיפול (אזור סיכון) (FFC107)
                    BO.STATUS.Open => new SolidColorBrush(Color.FromRgb(0x21, 0x96, 0xF3)), // Blue - פתוח (2196F3)
                    BO.STATUS.Closed => new SolidColorBrush(Color.FromRgb(0x9E, 0x9E, 0x9E)), // Grey - סגור (9E9E9E)
                    BO.STATUS.Expired => new SolidColorBrush(Color.FromRgb(0xF4, 0x43, 0x36)), // Red - פג תוקף (F44336)
                    BO.STATUS.OpenDangerZone => new SolidColorBrush(Color.FromRgb(0xFF, 0x98, 0x00)), // Orange - פתוח (אזור סיכון) (FF9800)
                    BO.STATUS.none => new SolidColorBrush(Colors.Transparent), // שקוף או צבע ברירת מחדל אחר
                    _ => new SolidColorBrush(Colors.Transparent)
                };
            }
            return new SolidColorBrush(Colors.Transparent); // צבע שקוף אם הקלט אינו תקין
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException(); // המרה הפוכה אינה נחוצה במקרה זה
        }
    }
}
