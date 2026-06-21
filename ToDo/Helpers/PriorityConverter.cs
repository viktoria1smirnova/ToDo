using System.Globalization;
using System.Windows.Data;
using ToDo.Data.Models;

namespace ToDo.Helpers
{
    class PriorityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Priority priority)
            {
                return priority switch
                {
                    Priority.Low => "Низкий",
                    Priority.Medium => "Средний",
                    Priority.High => "Высокий",
                    _ => priority.ToString()
                };
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Priority? res = null;

            if (value is Priority pr)
                res = pr;

            if (value is string s)
            {
                res = s switch
                {
                    "Низкий" => Priority.Low,
                    "Средний" => Priority.Medium,
                    "Высокий" => Priority.High,
                    _ => null,
                };
            }

            return res;
        }
    }
}
