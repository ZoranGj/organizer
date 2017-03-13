using CefSharp;
using CefSharp.WinForms;
using Model.DataProviders;
using Newtonsoft.Json;
using Organizer.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public string GetActivityItems()
        {
            var activityProvider = new ActivitiesProvider();
            var dictionary = new Dictionary<int, string>();
            return SerializeObject(activityProvider.GetAll().Select(x => new ActivityDto()
            {
                Name = x.Name,
                Id = x.Id
            }).ToList());
        }

        #endregion

        public void AddTodoItem(int activityId)
        {
            var id = new Random().Next(100000);
            var todoItemProvider = new TodoItemsProvider();
            todoItemProvider.Insert(new TodoItem
            {
                Id = id,
                ActivityId = activityId,
                Deadline = new DateTime(2017, 04, 04).AddMinutes(id + new Random().Next(30)),
                AddedOn = DateTime.Now.AddMinutes(id),
                Description = "Simple todo item for activity " + id
            });
            todoItemProvider.Save();
        }

        public string GetTodoItems()
        {
            var todoItemProvider = new TodoItemsProvider();
            var data = todoItemProvider.GetAll();
            return SerializeObject(data.Select(x => new ToDoItemDto
            {
                Id = x.Id,
                Activity = x.Activity.Name,
                Description = x.Description,
                AddedOn = x.AddedOn,
                Deadline = x.Deadline
            }));
        }

        public void DeleteTodoItem(int todoItemId)
        {
            var todoItemProvider = new TodoItemsProvider();
            todoItemProvider.Delete(todoItemId);
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
    }

    public class ToDoItemDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime AddedOn { get; set; }
        public DateTime Deadline { get; set; }
        public string Activity { get; set; }
    }

    public class ActivityDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
