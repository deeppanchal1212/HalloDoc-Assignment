using AssignmentEntity.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssignmentEntity.ViewModel
{
    public class AddTaskViewModel
    {
        public string TaskName { get; set; } = null!;

        public string Assignee { get; set; } = null!;

        public string TaskDescription { get; set; } = null!;

        public DateOnly DueDate { get; set; }

        public string Category { get; set; } = null!;

        public List<City> City { get; set; } = null!;

        public int SelectedCityId { get; set; }

        public int TaskId { get; set; }
    }
}
