using System;
using System.Collections.ObjectModel;
using ToDo.Data.Context;
using ToDo.Data.Models;

namespace ToDo.Data.Services
{
    public class TaskService
    {
        private XmlContext _context;

        public TaskService(XmlContext context)
        {
            if (context != null)
            {
                _context = context;
            }
        }

        public ObservableCollection<Models.Task> Get()
        {
            return new ObservableCollection<Models.Task>(_context.Tasks);
        }

        public ObservableCollection<Models.Task> Get(TaskFilter filter)
        {
            var searchText = filter.SearchText?.ToLower();

            var filteredTasks = _context.Tasks
                .Where(x => (string.IsNullOrEmpty(searchText)
                        || x.Name.ToLower().Contains(searchText)
                        || (!string.IsNullOrEmpty(x.Description) && x.Description.ToLower().Contains(searchText)))
                    && (!filter.Id.HasValue || x.Id == filter.Id.Value)
                    && (!filter.Status.HasValue || x.Status == filter.Status.Value)
                    && (!filter.Priority.HasValue || x.Priority == filter.Priority.Value)
                    && (!filter.Date.HasValue || x.Date == filter.Date.Value)
                    && (!filter.ImportantOnly || x.ImportantFlag))
                .ToList();

            return new ObservableCollection<Models.Task>(filteredTasks);
        }

        public bool SaveTask(Models.Task newTask)
        {
            if (newTask == null || string.IsNullOrEmpty(newTask.Name))
            {
                return false;
            }

            _context.Tasks.Add(newTask);
            _context.WriteToFile();
            return true;
        }

        public bool DeleteTask(int id)
        {
            Models.Task? target = _context.Tasks
                .FirstOrDefault(t => t.Id == id);

            if (target == null)
            {
                return false;
            }

            _context.Tasks.Remove(target);
            _context.WriteToFile();
            return true;
        }

        public bool EditTask(Models.Task task)
        {
            Models.Task? target = _context.Tasks
                .FirstOrDefault(t => t.Id == task.Id);


            if (target == null)
            {
                return false;
            }

            target.Priority = task.Priority;
            target.Description = task.Description;
            target.Status = task.Status;
            target.Date = task.Date;
            target.Name = task.Name;

            _context.WriteToFile();
            return true;
        }

        public Stats GetStatistics()
        {
            var tasks = _context.Tasks.ToList();

            var stats = new Stats
            {
                TasksCount = tasks.Count,
                New = tasks.Count(t => t.Status == Status.New),
                InProgress = tasks.Count(t => t.Status == Status.InProgress),
                Completed = tasks.Count(t => t.Status == Status.Completed),
                Important = tasks.Count(t => t.ImportantFlag),
                Expired = tasks.Count(t => t.Date.Date < DateTime.Now.Date && t.Status != Status.Completed),
                Today = tasks.Count(t => t.Date.Date == DateTime.Today),
                DueThisWeek = tasks.Count(t => t.Date <= DateTime.Today.AddDays(7)),
            };

            return stats;
        }
    }
}
