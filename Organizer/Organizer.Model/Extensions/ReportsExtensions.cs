using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

using Organizer.Model.DTO;

namespace Organizer.Model.Extensions
{
    public static class ReportsExtensions
    {
        public static IEnumerable<ProductivityReport> ProductivityReports(this List<TodoItem> todoItems, Goal goal = null)
        {
            CultureInfo culture = Thread.CurrentThread.CurrentCulture;
            var first = todoItems.FirstOrDefault();
            var last = todoItems.LastOrDefault();
            if (first == null || goal == null) return new List<ProductivityReport>();

            var startDate = first.Deadline.StartOfWeek(DayOfWeek.Monday);
            var dateDifference = (last.Deadline - startDate).Days / 7;
            var reports = Enumerable.Range(0, dateDifference + 1)
                   .Select(d => new { Start = startDate.AddDays(d * 7), End = startDate.AddDays((d * 7) + 6) })
                   .Select(r =>
                   {
                       var todos = todoItems.Where(t => t.Deadline >= r.Start.StartOfDay() && t.Deadline <= r.End.EndOfDay());
                       return new ProductivityReport
                       {
                           ActualTime = todos.Sum(y => y.Duration),
                           From = r.Start,
                           NumberOfTodos = todos.Count(),
                           MaxHoursPerWeek = goal.MaxHoursPerWeek,
                           MinHoursPerWeek = goal.MinHoursPerWeek
                       };
                   });
            return reports.ToList();
        }
    }
}
