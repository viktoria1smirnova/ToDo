using System;
using System.Collections.Generic;
using System.Text;

namespace ToDo.Data.Models
{
    public class TaskFilter
    {
        public int? Id { get; set; }
        public string SearchText { get; set; }
        public Priority? Priority { get; set; }
        public DateTime? Date { get; set; }
        public Status? Status { get; set; }
        public bool ImportantOnly { get; set; }
    }
}
