using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.Model.DTO
{
    public class ActivityDto
    {
        public int Id { get; set; }
        public int GoalId { get; set; }
        public string Name { get; set; }
        public int ActiveTodosNumber { get; set; }
        public int ResolvedTodosNumber { get; set; }

        public ActivityDto(Activity entity, int goalId)
        {
            Id = entity.Id;
            Name = entity.Name;
            ActiveTodosNumber = entity.TodoItems.Count(t => !t.Resolved);
            ResolvedTodosNumber = entity.TodoItems.Count(t => t.Resolved);
            GoalId = goalId;
        }

        public ActivityDto() { }
    }
}
