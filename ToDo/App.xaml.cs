using System;
using System.IO;
using System.Text.Json;
using System.Windows;
using ToDo.Data.Context;
using ToDo.Data.Services;
using ToDo.Windows;

namespace ToDo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static string _xmlFile { get; set; }
        public TaskService service { get; set; }

        public App()
        {
            SetXmlFilePath();

            service = new TaskService(new XmlContext(_xmlFile));

            TaskListWindow orgsView = new TaskListWindow(service);
            orgsView.Show();
        }

        private void SetXmlFilePath()
        {
            var settingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.json");

            var json = File.ReadAllText(settingsPath);
            using var document = JsonDocument.Parse(json);

            if (!document.RootElement.TryGetProperty("XmlFile", out var xmlFileElement) ||
                xmlFileElement.ValueKind != JsonValueKind.String ||
                string.IsNullOrWhiteSpace(_xmlFile = xmlFileElement.GetString()))
            {
                throw new InvalidDataException("Указан неправильный путь до xml файла.");
            }
        }
    }
}
