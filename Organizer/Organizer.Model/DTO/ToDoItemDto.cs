using System;
using System.Collections.Generic;
using System.Linq;

namespace Organizer.Model.DTO
{
    public class ToDoItemDto
    {
        private TodoItem data;

        public ToDoItemDto(TodoItem data)
        {
            Id = data.Id;
            Activity = data.Activity.Name;
            Goal = data.Activity.Goal == null ? null : data.Activity.Goal.Name;
            Description = data.Description;
            AddedOn = data.AddedOn;
            Deadline = data.Deadline;
            Resolved = data.Resolved;
            Duration = data.Duration;
            Notes = data.Notes;
            Tags = !data.Tags.Any() ? new List<string>() : data.Tags.Select(t => t.Name).ToList();
        }

        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime AddedOn { get; set; }
        public DateTime Deadline { get; set; }
        public string Activity { get; set; }
        public string Goal { get; set; }
        public bool Resolved { get; set; }
        public int Duration { get; set; }
        public string Notes { get; set; }
        public List<string> Tags { get; set; }
        public bool PickerOpened { get; set; }
    }
}
