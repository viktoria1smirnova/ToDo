using System.Windows;
using System.Windows.Input;
using ToDo.Data.Models;
using ToDo.Data.Services;

namespace ToDo.Windows
{
    /// <summary>
    /// Логика взаимодействия для TaskWindow.xaml
    /// </summary>
    public partial class TaskWindow : Window
    {
        private readonly TaskService _service;
        private Data.Models.Task? _task;
        private readonly bool _editFlag;

        public TaskWindow(TaskService service, Data.Models.Task? taskToEdit = null)
        {
            InitializeComponent();
            _service = service;

            _task = taskToEdit;
            _editFlag = taskToEdit != null;

            Title = _editFlag ? "Редактирование задачи" : "Новая задача";

            DataContext = _task;
            FillFields();
        }

        private void FillFields()
        {
            PriorityField.ItemsSource = Enum.GetValues(typeof(Priority));
            PriorityField.SelectedItem = Priority.Low;
            StatusField.ItemsSource = Enum.GetValues(typeof(Status));
            StatusField.SelectedItem = Status.New;

            if (_editFlag)
            {
                PriorityField.SelectedItem = _editFlag ? _task.Priority : Priority.Low;
                StatusField.SelectedItem = _editFlag ? _task.Status : Status.New;
                NameField.Text = _task.Name;
                DescriptionField.Text = _task.Description;
                DateField.SelectedDate = _task.Date;
                ImportantField.IsChecked = _task.ImportantFlag;
            } 
        }

        private void BtnOk_Click(object sender, ExecutedRoutedEventArgs e)
        {
            ErrorField.Visibility = Visibility.Collapsed;
            SaveTask();
        }
        private void BtnCancel_Click(object sender, ExecutedRoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void SaveTask()
        {
            if (!_editFlag)
            {
                _task = new Data.Models.Task();
            }

            _task.Name = NameField.Text.Trim();
            _task.Description = DescriptionField.Text.Trim();
            _task.Priority = (Priority)PriorityField.SelectedItem;
            _task.Status = (Status)StatusField.SelectedItem;
            _task.Date = DateField.SelectedDate ?? DateTime.Now.AddDays(1);
            _task.ImportantFlag = ImportantField.IsChecked == true;

            if (string.IsNullOrWhiteSpace(_task.Name))
            {
                ErrorField.Text = "Название задачи не заполнено";
                ErrorField.Visibility = Visibility.Visible;
                return;
            }

            bool res = _editFlag ? _service.EditTask(_task) : _service.SaveTask(_task);

            if (res)
            {
                DialogResult = true;
                Close();
            }
            else
            {
                ErrorField.Text = "Ошибка сохранения";
                ErrorField.Visibility = Visibility.Visible;
            }
        }
    }
}
