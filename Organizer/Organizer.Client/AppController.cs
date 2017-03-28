using CefSharp;
using CefSharp.WinForms;
using Model.DataProviders;
using Newtonsoft.Json;
using Organizer.Model;
using Organizer.Model.DTO;
using Organizer.Model.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.Client
{
    public class AppController
    {
        // Declare a local instance of chromium and the main form in order to execute things from here in the main thread
        private static ChromiumWebBrowser _instanceBrowser = null;
        // The form class needs to be changed according to yours
        private static MainWindow _instanceMainForm = null;


        public AppController(ChromiumWebBrowser originalBrowser, MainWindow mainForm)
        {
            _instanceBrowser = originalBrowser;
            _instanceMainForm = mainForm;
        }

        public void showDevTools()
        {
            _instanceBrowser.ShowDevTools();
        }

        public void opencmd()
        {
            ProcessStartInfo start = new ProcessStartInfo("cmd.exe", "/c pause");
            Process.Start(start);
        }

        #region Categories

        public string GetCategories()
        {
            var categoryProvider = new CategoriesProvider();
            var data = categoryProvider.GetAll();
            return SerializeObject(data);
        }

        public Category GetCategory(int id)
        {
            var categoryProvider = new CategoriesProvider();
            return categoryProvider.GetById(id);
        }

        public void AddCategory(string name, int priority)
        {
            var id = new Random().Next(10000);
            var categoryProvider = new CategoriesProvider();
            categoryProvider.Insert(new Category
            {
                Id = id,
                Name = name,
                Priority = priority
            });
            categoryProvider.Save();
        }

        public void DeleteCategory(int id)
        {
            var categoryProvider = new CategoriesProvider();
            categoryProvider.Delete(id);
            categoryProvider.Save();
        }

        public void UpdateCategoryPriority(int id, int newPriority)
        {
            var categoryProvider = new CategoriesProvider();
            categoryProvider.UpdatePriority(id, newPriority);
        }

        public void UpdateCategoryData(int id, int hoursPerWeek)
        {
            var categoryProvider = new CategoriesProvider();
            categoryProvider.UpdateCategoryData(id, (short)hoursPerWeek);
        }

        #endregion

        #region Activities

        public void SaveActivity(int categoryId, string name, int activityId)
        {
            var activityProvider = new ActivitiesProvider();
            if(activityId == 0)
            {
                var id = new Random().Next(100000);
                activityProvider.Insert(new Activity
                {
                    Id = id,
                    Name = name,
                    Description = null,
                    CategoryId = categoryId,
                    Priority = 2
                });
            }
            else
            {
                var activity = activityProvider.GetById(activityId);
                activity.Name = name;
                activityProvider.Update(activity);
            }
            activityProvider.Save();
        }

        public void DeleteActivity(int id)
        {
            var activityProvider = new ActivitiesProvider();
            activityProvider.Delete(id);
            activityProvider.Save();
        }

        public string GetActivityItems(int categoryId)
        {
            var activityProvider = new ActivitiesProvider();
            var dictionary = new Dictionary<int, string>();
            return SerializeObject(activityProvider.GetAll(categoryId).Select(x => new ActivityDto()
            {
                Name = x.Name,
                Id = x.Id
            }).ToList());
        }

        #endregion

        public void AddTodoItem(string description, DateTime deadline, int activityId, int duration)
        {
            var id = new Random().Next(100000);
            var todoItemProvider = new TodoItemsProvider();
            todoItemProvider.Insert(new TodoItem
            {
                Id = id,
                ActivityId = activityId,
                Deadline = deadline,
                AddedOn = DateTime.Now,
                Description = description,
                Duration = duration,
                //Recurring = (short)recurring
            });
            todoItemProvider.Save();
        }

        public string GetTodoItems(int categoryId)
        {
            var todoItemProvider = new TodoItemsProvider();
            todoItemProvider.InitRecurringTodos();
            var data = categoryId == 0 ? todoItemProvider.GetAll() : todoItemProvider.GetAll(categoryId);
            return SerializeObject(data.Select(x => new ToDoItemDto
            {
                Id = x.Id,
                Activity = x.Activity.Name,
                Description = x.Description,
                AddedOn = x.AddedOn,
                Deadline = x.Deadline,
                Resolved = x.Resolved,
                Duration = x.Duration,
                Notes = x.Notes
                //RecurringTypeId = x.Recurring
            }));
        }

        public void DeleteTodoItem(int todoItemId)
        {
            var todoItemProvider = new TodoItemsProvider();
            todoItemProvider.Delete(todoItemId);
            todoItemProvider.Save();
        }

        public void ResolveTodoItem(int id, bool resolved)
        {
            var todoItemProvider = new TodoItemsProvider();
            todoItemProvider.Resolve(id, resolved);
        }

        public void UpdateTodoItem(int id, string notes)
        {
            var todoItemProvider = new TodoItemsProvider();
            var item = todoItemProvider.GetById(id);
            item.Notes = notes;
            todoItemProvider.Update(item);
            todoItemProvider.Save();
        }

        private string SerializeObject(object data)
        {
            return JsonConvert.SerializeObject(data, Formatting.Indented,
                                                new JsonSerializerSettings
                                                {
                                                    PreserveReferencesHandling = PreserveReferencesHandling.Objects
                                                });
        }

        int randomCategoryId()
        {
            var list = new List<int> { 4464, 4907, 3709 };
            return list.ElementAt(new Random().Next(list.Count));
        }

        public string LoadProductivityReports(int id)
        {
            if (id != 0)
            {
            //    var categoryProvider = new CategoriesProvider();
            //    var categories = categoryProvider.GetAll();
            //    var productivityList = categories.Select(x =>
            //    new
            //    {
            //        Label = x.Name,
            //        Items = ActivityReports(x)
            //    });
            //    return SerializeObject(productivityList);
            //}
            //else
            //{
                var category = GetCategory(id);
                var productivityList = ActivityReports(category);
                return SerializeObject(productivityList);
            }

            return null;
        }

        public List<ActivityReport> ActivityReports(Category category)
        {
            var todoItemProvider = new TodoItemsProvider();
            var todoItems = todoItemProvider.GetAll(category.Id).Where(x => x.Resolved).OrderBy(x => x.Deadline).ToList();
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
                    todoItemsCopy = todoItemsCopy.Concat(Enumerable.Range(1, dateDiff - 1).Select(x => new TodoItem
                    {
                        Deadline = item.Deadline.AddDays(7 * x),
                        ActivityId = item.ActivityId,
                        AddedOn = item.AddedOn.AddDays(7 * x),
                        Resolved = true,
                        Duration = 0
                    })).ToList();
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
                PlannedTime = category.HoursPerWeek
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
