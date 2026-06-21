using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace ToDo.Data.Context
{
    public class XmlContext
    {
        public List<Models.Task> Tasks;
        private readonly string _filePath;

        public XmlContext(string newFilePath)
        {
            Tasks = new List<Models.Task>();
            _filePath = newFilePath;
            FillTasks();
        }

        private void FillTasks()
        {
            if (!File.Exists(_filePath))
                return;

            try
            {
                var serializer = new XmlSerializer(typeof(List<Models.Task>));

                using var stream = File.OpenRead(_filePath);
                var loadedTasks = (List<Models.Task>?)serializer.Deserialize(stream);

                if (loadedTasks != null && loadedTasks.Any())
                {
                    Tasks = new List<Models.Task>(loadedTasks);

                    var maxId = Tasks.Max(t => t.Id);
                    if (maxId >= Models.Task.Counter)
                    {
                        Models.Task.Counter = maxId + 1;
                    }
                }  
            }
            catch (InvalidOperationException)
            { }
        }

        public void WriteToFile()
        {
            var serializer = new XmlSerializer(typeof(List<Models.Task>));
            var tasksToSave = Tasks.ToList();

            using var stream = File.Open(_filePath, FileMode.Create, FileAccess.Write, FileShare.None);
            serializer.Serialize(stream, tasksToSave);
        }
    }
}
