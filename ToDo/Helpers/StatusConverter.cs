using System.Globalization;
using System.Windows.Data;
using ToDo.Data.Models;

namespace ToDo.Helpers
{
    class StatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Status status)
            {
                return status switch
                {
                    Status.New => "Новая",
                    Status.InProgress => "В процессе",
                    Status.Completed => "Завершена",
                    _ => status.ToString()
                };
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Status? res = null;

            if (value is Status st)
                res = st;

            if (value is string s)
            {
                res = s switch
                {
                    "Новые" => Status.New,
                    "В процессе" => Status.InProgress,
                    "Завершены" => Status.Completed,
                    _ => null,
                };
            }

            return res;
        }
    }
}
