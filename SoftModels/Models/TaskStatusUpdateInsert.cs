using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftModels.Models
{
    public class TaskStatusUpdateInsert
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public string DueDate { get; set; }
    }
}
