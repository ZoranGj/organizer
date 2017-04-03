﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.Model.DTO
{
    public class ToDoItemDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime AddedOn { get; set; }
        public DateTime Deadline { get; set; }
        public string Activity { get; set; }
        public bool Resolved { get; set; }
        public int Duration { get; set; }
        public string Notes { get; set; }
        public List<string> Tags { get; set; }
        public int RecurringTypeId { get; set; }
        public string RecurringMode
        {
            get
            {
                switch ((RecurringType)RecurringTypeId)
                {
                    case RecurringType.Dayly:
                        return "Repeat daily";
                    case RecurringType.Weekend:
                        return "Repeat on weekends";
                    case RecurringType.Weekly:
                        return "Repeat weekly";
                    default:
                        return "One time";
                }
            }
        }
    }

    public enum RecurringType
    {
        Standard = 0,
        Dayly = 1,
        Weekly = 2,
        Weekend = 3
    }
}
