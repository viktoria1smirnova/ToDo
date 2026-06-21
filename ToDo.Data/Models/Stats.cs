using System;
using System.Collections.Generic;
using System.Text;

namespace ToDo.Data.Models
{
    public class Stats
    {
        public int TasksCount { get; set; }
        public int New { get; set; }
        public int InProgress { get; set; }
        public int Completed { get; set; }
        public int Important { get; set; }
        public int Expired { get; set; }
        public int Today { get; set; }
        public int DueThisWeek { get; set; }
    }
}
