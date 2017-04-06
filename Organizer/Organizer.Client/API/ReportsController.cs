using CefSharp.WinForms;
using Model.DataProviders;
using Organizer.Model;
using Organizer.Model.DataProviders;
using Organizer.Model.DTO;
using Organizer.Model.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public string LoadTagsReports(string tag)
        {
            if (!string.IsNullOrEmpty(tag))
            {
                var todoItems = _tagsProvider.GetAll().First(t => t.Name == tag).TodoItems.Where(t => t.Resolved).ToList();
                var todoItemsCopy = new List<TodoItem>();
                if (!todoItems.Any())
                {
                    return null;
                }

                for (var i = 0; i < todoItems.Count(); i++)
                {
                    var item = todoItems.ElementAt(i);
                    todoItemsCopy.Add(item);
                    if (todoItems.Count() > i + 1)
                    {

                        var nextItem = todoItems.ElementAt(i + 1);
                        int dateDiff = ((int)(nextItem.Deadline - item.Deadline).TotalDays / 7);
                        if (dateDiff > 0)
                        {
                            todoItemsCopy = todoItemsCopy.Concat(Enumerable.Range(1, dateDiff - 1).Select(x => new TodoItem
                            {
                                Deadline = item.Deadline.AddDays(7 * x),
                                ActivityId = item.ActivityId,
                                AddedOn = item.AddedOn.AddDays(7 * x),
                                Resolved = true,
                                Duration = 0
                            })).ToList();
                        }
                        todoItemsCopy.Add(nextItem);
                        i++;
                    }
                }

                var groupedByWeek = todoItemsCopy.OrderBy(x => x.Deadline).GroupBy(item => GetStartOfWeek(item.Deadline));
                var productivityList = groupedByWeek.Select(x => new ActivityReport
                {
                    ActualTime = x.Sum(y => y.Duration),
                    From = x.Key,
                    NumberOfTodos = x.Count(),
                }).ToList();
                return productivityList.Serialize();
            }

            return null;
        }

        public List<ActivityReport> ActivityReports(Category category)
        {
            var todoItems = _todoItemsProvider.GetAll(category.Id).Where(x => x.Resolved).OrderBy(x => x.Deadline).ToList();
            var todoItemsCopy = new List<TodoItem>();
            if (!todoItems.Any())
            {
                return null;
            }

            for (var i = 0; i < todoItems.Count(); i++)
            {
                var item = todoItems.ElementAt(i);
                todoItemsCopy.Add(item);
                if (todoItems.Count() > i + 1)
                {

                    var nextItem = todoItems.ElementAt(i + 1);
                    int dateDiff = ((int)(nextItem.Deadline - item.Deadline).TotalDays / 7);
                    if (dateDiff > 0)
                    {
                        todoItemsCopy = todoItemsCopy.Concat(Enumerable.Range(1, dateDiff - 1).Select(x => new TodoItem
                        {
                            Deadline = item.Deadline.AddDays(7 * x),
                            ActivityId = item.ActivityId,
                            AddedOn = item.AddedOn.AddDays(7 * x),
                            Resolved = true,
                            Duration = 0
                        })).ToList();
                    }
                    todoItemsCopy.Add(nextItem);
                    i++;
                }
            }

            var groupedByWeek = todoItemsCopy.OrderBy(x => x.Deadline).GroupBy(item => GetStartOfWeek(item.Deadline));
            return groupedByWeek.Select(x => new ActivityReport
            {
                ActualTime = x.Sum(y => y.Duration),
                From = x.Key,
                NumberOfTodos = x.Count(),
                MaxHoursPerWeek = category.MaxHoursPerWeek,
                MinHoursPerWeek = category.MinHoursPerWeek
            }).ToList();
        }

        public static DateTime GetStartOfWeek(DateTime value)
        {
            value = value.Date;
            int daysIntoWeek = (int)value.DayOfWeek - 1;
            return value.AddDays(-daysIntoWeek);
        }
    }
}
