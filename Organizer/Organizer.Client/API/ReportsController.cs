using CefSharp.WinForms;
using Model.DataProviders;
using Organizer.Model;
using Organizer.Model.DataProviders;
using Organizer.Model.DTO;
using Organizer.Model.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Organizer.Client.API
{
    public class ReportsController : AppController
    {
        private readonly CategoriesController categories;
        private readonly TodoItemsProvider _todoItemsProvider;
        private readonly TagsProvider _tagsProvider;

        public ReportsController(ChromiumWebBrowser originalBrowser, MainWindow mainForm) : base(originalBrowser, mainForm)
        {
            _todoItemsProvider = new TodoItemsProvider();
            _tagsProvider = new TagsProvider();
            categories = new CategoriesController(originalBrowser, mainForm);
        }

        public string LoadProductivityReports(int id)
        {
            if (id != 0)
            {
                var category = categories.Get(id);
                var productivityList = ActivityReports(category);
                return productivityList.Serialize();
            }

            return null;
        }

        public string LoadTagsReports(int tagId)
        {
            CultureInfo culture = Thread.CurrentThread.CurrentCulture;
            var todoItems = _tagsProvider.GetById(tagId).TodoItems.Where(x => x.Resolved).ToList();
            return TagsReports(todoItems).Serialize();
        }

        public List<ActivityReport> TagsReports(List<TodoItem> todoItems, Category category = null)
        {
            CultureInfo culture = Thread.CurrentThread.CurrentCulture;
            var first = todoItems.FirstOrDefault();
            var last = todoItems.LastOrDefault();
            if (first == null) return new List<ActivityReport>();

            var startDate = GetStartOfWeek(first.Deadline);
            var dateDifference = (last.Deadline - startDate).Days / 7;
            var reports = Enumerable.Range(0, dateDifference + 1)
                   .Select(d => new { Start = startDate.AddDays(d * 7), End = startDate.AddDays((d * 7) + 5) })
                   .Select(r =>
                   {
                       var todos = todoItems.Where(t => t.Deadline >= r.Start && t.Deadline <= r.End);
                       return new ActivityReport
                       {
                           ActualTime = todos.Sum(y => y.Duration),
                           From = r.Start,
                           NumberOfTodos = todos.Count(),
                           MaxHoursPerWeek = category == null ? 0 : category.MaxHoursPerWeek,
                           MinHoursPerWeek = category == null ? 0 : category.MinHoursPerWeek
                       };
                   });
            return reports.ToList();
        }

        public List<ActivityReport> ActivityReports(Category category)
        {
            var todoItems = _todoItemsProvider.GetAll(category.Id).Where(x => x.Resolved).OrderBy(x => x.Deadline).ToList();
            return TagsReports(todoItems, category);
        }

        public static DateTime GetStartOfWeek(DateTime value)
        {
            value = value.Date;
            int daysIntoWeek = (int)value.DayOfWeek - 1;
            return value.AddDays(-daysIntoWeek);
        }
    }
}
