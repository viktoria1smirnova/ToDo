using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace ToDo.Data.Models
{
    public class Task
    {
        public static int Counter = 0;
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public Priority Priority { get; set; }
        public DateTime Date { get; set; }
        public Status Status { get; set; }
        public bool ImportantFlag { get; set; }

        public Task()
        {
            Id = ++Counter;
            Priority = Priority.Low;
            Status = Status.New;
            Date = DateTime.Now.Date;
            ImportantFlag = false;
        }
    }
}
