using System;
using System.IO;
using System.Linq;
using ToDo.Data.Context;
using ToDo.Data.Models;
using ToDo.Data.Services;
using Xunit;

namespace ToDo.Test
{
    public class TaskServiceTests : IDisposable
    {
        private readonly string _tempFile;
        private readonly XmlContext _context;
        private readonly TaskService _service;

        public TaskServiceTests()
        {
            ToDo.Data.Models.Task.Counter = 0;

            _tempFile = Path.Combine(Path.GetTempPath(), $"todo_test_{Guid.NewGuid()}.xml");
            _context = new XmlContext(_tempFile);
            _service = new TaskService(_context);

            _context.Tasks.Add(new ToDo.Data.Models.Task { Name = "Task A", Description = "One", Priority = Priority.Low, Status = Status.New, ImportantFlag = false });
            _context.Tasks.Add(new ToDo.Data.Models.Task { Name = "Task B", Description = "Two", Priority = Priority.High, Status = Status.InProgress, ImportantFlag = true });
            _context.Tasks.Add(new ToDo.Data.Models.Task { Name = "Task C", Description = "Three", Priority = Priority.Medium, Status = Status.Completed, ImportantFlag = true });
        }

        [Fact]
        public void Get_NoFilter_ReturnsAll()
        {
            var all = _service.Get();
            Assert.Equal(3, all.Count);
        }

        [Fact]
        public void Get_FilterByPriority_ReturnsMatching()
        {
            var filter = new TaskFilter { Priority = Priority.High };
            var result = _service.Get(filter);
            Assert.Single(result);
            Assert.Equal(Priority.High, result.First().Priority);
        }

        [Fact]
        public void Get_ImportantOnly_ReturnsOnlyImportant()
        {
            var filter = new TaskFilter { ImportantOnly = true };
            var result = _service.Get(filter);
            Assert.Equal(2, result.Count);
            Assert.All(result, t => Assert.True(t.ImportantFlag));
        }

        [Fact]
        public void SaveTask_PersistsAndAdds()
        {
            var newTask = new ToDo.Data.Models.Task { Name = "Saved Task", Description = "saved", Priority = Priority.Low, Status = Status.New, ImportantFlag = false };
            var ok = _service.SaveTask(newTask);
            Assert.True(ok);
            Assert.Contains(_context.Tasks, t => t.Name == "Saved Task");
            Assert.True(File.Exists(_tempFile));
        }

        [Fact]
        public void DeleteTask_RemovesExisting()
        {
            var t = new ToDo.Data.Models.Task { Name = "ToDelete", Description = "del", Priority = Priority.Low, Status = Status.New, ImportantFlag = false };
            _service.SaveTask(t);
            var id = t.Id;
            var res = _service.DeleteTask(id);
            Assert.True(res);
            Assert.DoesNotContain(_context.Tasks, x => x.Id == id);
        }

        [Fact]
        public void EditTask_UpdatesFields()
        {
            var t = new ToDo.Data.Models.Task { Name = "EditMe", Description = "orig", Priority = Priority.Low, Status = Status.New, ImportantFlag = false };
            _service.SaveTask(t);
            t.Name = "Edited";
            t.Description = "Changed";
            t.Priority = Priority.High;

            var res = _service.EditTask(t);
            Assert.True(res);

            var updated = _context.Tasks.First(x => x.Id == t.Id);
            Assert.Equal("Edited", updated.Name);
            Assert.Equal("Changed", updated.Description);
            Assert.Equal(Priority.High, updated.Priority);
        }

        [Fact]
        public void Get_FilterByStatus_ReturnsMatching()
        {
            var filter = new TaskFilter { Status = Status.InProgress };
            var result = _service.Get(filter);
            Assert.Single(result);
            Assert.Equal(Status.InProgress, result.First().Status);
        }

        [Fact]
        public void Get_FilterByDate_ReturnsMatching()
        {
            var targetDate = new DateTime(2026, 6, 21);
            var t = new ToDo.Data.Models.Task { Name = "DateTask", Description = "d", Priority = Priority.Low, Status = Status.New, ImportantFlag = false, Date = targetDate };
            _context.Tasks.Add(t);

            var filter = new TaskFilter { Date = targetDate };
            var result = _service.Get(filter);
            Assert.Contains(result, x => x.Date == targetDate);
        }

        [Fact]
        public void Get_SearchText_ReturnsMatching()
        {
            var filter = new TaskFilter { SearchText = "Task A" };
            var result = _service.Get(filter);
            Assert.Single(result);
            Assert.Contains("Task A", result.First().Name);
        }

        public void Dispose()
        {
            try
            {
                if (File.Exists(_tempFile)) File.Delete(_tempFile);
            }
            catch { }
        }
    }
}
