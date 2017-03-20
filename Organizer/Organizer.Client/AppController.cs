using CefSharp;
using CefSharp.WinForms;
using Model.DataProviders;
using Newtonsoft.Json;
using Organizer.Model;
using Organizer.Model.DTO;
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

        public void AddTodoItem(string description, DateTime deadline, int activityId)
        {
            var id = new Random().Next(100000);
            var todoItemProvider = new TodoItemsProvider();
            todoItemProvider.Insert(new TodoItem
            {
                Id = id,
                ActivityId = activityId,
                Deadline = deadline,
                AddedOn = DateTime.Now,
                Description = description
            });
            todoItemProvider.Save();
        }

        public string GetTodoItems(int categoryId)
        {
            var todoItemProvider = new TodoItemsProvider();
            var data = categoryId == 0 ? todoItemProvider.GetAll() : todoItemProvider.GetAll(categoryId);
            return SerializeObject(data.Select(x => new ToDoItemDto
            {
                Id = x.Id,
                Activity = x.Activity.Name,
                Description = x.Description,
                AddedOn = x.AddedOn,
                Deadline = x.Deadline,
                Resolved = x.Resolved,
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
            var todoItemProvider = new TodoItemsProvider();
            var productivityList = new List<ActivityProductivity>();
            var category = GetCategory(id);

            var todoItems = todoItemProvider.GetAll(id).Where(x => x.Resolved);
            var groupedByWeek = todoItems.GroupBy(item => GetStartOfWeek(item.Deadline));
            productivityList = groupedByWeek.Select(x =>
            {
                return x.FirstOrDefault() == null ? new ActivityProductivity() : new ActivityProductivity
                {
                    ActualTime = x.Sum(y => 1),
                    From = x.Key,
                    NumberOfTodos = x.Count(),
                    PlannedTime = category.HoursPerWeek
                };
            }).ToList();
            return SerializeObject(productivityList);
        }

        public static DateTime GetStartOfWeek(DateTime value)
        {
            // Get rid of the time part first...
            value = value.Date;
            int daysIntoWeek = (int)value.DayOfWeek;
            return value.AddDays(-daysIntoWeek);
        }
    }

    public class ToDoItemDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime AddedOn { get; set; }
        public DateTime Deadline { get; set; }
        public string Activity { get; set; }
        public bool Resolved { get; set; }
    }

    public class ActivityDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
