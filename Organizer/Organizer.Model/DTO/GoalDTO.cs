using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.Model.DTO
{
    public class GoalDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Priority { get; set; }
        public short MinHoursPerWeek { get; set; }
        public short MaxHoursPerWeek { get; set; }
        public string Color { get; set; }
        public List<ActivityDto> Activities { get; set; }

        public GoalDto(Goal entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Priority = entity.Priority;
            MinHoursPerWeek = entity.MinHoursPerWeek;
            MaxHoursPerWeek = entity.MaxHoursPerWeek;
            Color = entity.Color;
            Activities = entity.Activities.Select(a => new ActivityDto(a, entity.Id)).ToList();
        }
    }
}
