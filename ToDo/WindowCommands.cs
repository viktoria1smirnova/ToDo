using System.Windows.Input;
using ToDo.Windows;

namespace ToDo
{
    public class WindowCommands
    {
        public static RoutedCommand TaskOk { get; set; }
        public static RoutedCommand TaskCancel { get; set; }
        public static RoutedCommand ClearFilters { get; set; }
        public static RoutedCommand NewTask { get; set; }
        public static RoutedCommand EditTask { get; set; }
        public static RoutedCommand DeleteTask { get; set; }
        public static RoutedCommand GetStatistics { get; set; }

        static WindowCommands()
        {
            TaskOk = new RoutedCommand("TaskOk", typeof(TaskWindow));
            TaskCancel = new RoutedCommand("TaskCancel", typeof(TaskWindow));
            NewTask = new RoutedCommand("NewTask", typeof(TaskListWindow));
            ClearFilters = new RoutedCommand("ClearFilters", typeof(TaskListWindow));
            GetStatistics = new RoutedCommand("GetStatistics", typeof(TaskListWindow));
            EditTask = new RoutedCommand("EditTask", typeof(TaskListWindow));
            DeleteTask = new RoutedCommand("DeleteTask", typeof(TaskListWindow));
        }
    }
}
