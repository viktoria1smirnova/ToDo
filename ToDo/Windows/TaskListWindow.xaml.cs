using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Globalization;
using ToDo.Data.Models;
using ToDo.Helpers;
using ToDo.Data.Services;

namespace ToDo.Windows
{
    /// <summary>
    /// Логика взаимодействия для TaskListWindow.xaml
    /// </summary>
    public partial class TaskListWindow : Window
    {
        private TaskService _service;

        public TaskListWindow(TaskService service)
        {
            InitializeComponent();

            _service = service;

            FillCollection(false);
        }

        private void FillCollection(bool applyFilters)
        {
            int idx = 0;
            if (TasksList.SelectedIndex > 0)
                idx = TasksList.SelectedIndex;

            TasksList.ItemsSource = null;
            TasksList.Items.Clear();

            if (applyFilters)
            {
                TaskFilter filter = new TaskFilter
                {
                    SearchText = SearchField.Text,
                    Status = GetStatusFilter(),
                    Priority = GetPriorityFilter(),
                    Date = DateField.SelectedDate,
                    ImportantOnly = ImportantOnlyField.IsChecked.Value,
                };

                TasksList.ItemsSource = _service.Get(filter);
            }
            else
            {
                TasksList.ItemsSource = _service.Get();
            }

            if (TasksList.Items.Count > 0 && idx < TasksList.Items.Count)
            {
                TasksList.SelectedIndex = idx;
            }
            else
            {
                TasksList.SelectedIndex = -1;
            }  
        }

        private void FiltersChanged(object sender, EventArgs e)
        {
            FillCollection(true);
        }

        private void NewTask_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var add = new TaskWindow(_service);
            if (add.ShowDialog() == true)
            {
                FillCollection(true);
            }
        }

        private void TasksList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            EditTask();
        }

        private void EditTask_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            EditTask();
        }

        private void DeleteTask_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (TasksList.SelectedItem is Data.Models.Task selectedTask)
            {
                var res = _service.DeleteTask(selectedTask.Id);
                if (res)
                {
                    FillCollection(true);
                }
            }
        }

        private void EditTask()
        {
            if (TasksList.SelectedItem is Data.Models.Task selectedTask)
            {
                var editWindow = new TaskWindow(_service, selectedTask);
                if (editWindow.ShowDialog() == true)
                {
                    FillCollection(true);
                }
            }
        }

        private void GetStatistics_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var stats = _service.GetStatistics();
            var statsWindow = new StatisticsWindow(stats)
            {
                Owner = this
            };
            statsWindow.ShowDialog();
        }

        private void ClearFilters_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SearchField.Text = string.Empty;
            StatusField.SelectedIndex = 0;
            PriorityField.SelectedIndex = 0;
            DateField.SelectedDate = null;
            ImportantOnlyField.IsChecked = false;

            FillCollection(true);
        }

        private Status? GetStatusFilter()
        {
            var selectedItem = StatusField.SelectedItem as ComboBoxItem;

            if (selectedItem != null)
            {
                var conv = new StatusConverter();
                var result = conv.ConvertBack(selectedItem.Content?.ToString(), typeof(Status), null, CultureInfo.CurrentCulture);
                return result as Status?;
            }

            return null;
        }

        private Priority? GetPriorityFilter()
        {
            var selectedItem = PriorityField.SelectedItem as ComboBoxItem;

            if (selectedItem != null)
            {
                var conv = new PriorityConverter();
                var result = conv.ConvertBack(selectedItem.Content?.ToString(), typeof(Priority), null, CultureInfo.CurrentCulture);
                return result as Priority?;
            }

            return null;
        }
    }
}
