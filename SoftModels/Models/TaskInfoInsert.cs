using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftModels.Models
{
    public class TaskInfoInsert
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public int UserId { get; set; }
        public int TeamId { get; set; }
        public string DueDate { get; set; }
        public string IsActive { get; set; }

    }
}
