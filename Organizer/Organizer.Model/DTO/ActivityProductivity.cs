using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.Model.DTO
{
    public class ActivityProductivity
    {
        public int CategoryId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int PlannedTime { get; set; }
        public int ActualTime { get; set; }
        public string Name { get; set; }
        public int NumberOfTodos { get; set; }
        public int NumberOfActivities { get; set; }
    }
}
