using CefSharp.WinForms;
using Model.DataProviders;
using Organizer.Model;
using Organizer.Model.DTO;
using Organizer.Model.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.Client.API
{
    public class CategoriesController : AppController
    {
        private readonly CategoriesProvider _categoriesProvider;
        private readonly ActivitiesProvider _acivitiesProvider;

        public CategoriesController(ChromiumWebBrowser originalBrowser, MainWindow mainForm) : base(originalBrowser, mainForm)
        {
            _categoriesProvider = new CategoriesProvider();
            _acivitiesProvider = new ActivitiesProvider();
        }

        #region Categories

        public string GetAll()
        {
            var categoryProvider = new CategoriesProvider();
            var data = categoryProvider.GetAll();
            return data.Serialize();
        }

        public Category Get(int id)
        {
            var categoryProvider = new CategoriesProvider();
            return categoryProvider.GetById(id);
        }

        public void Add(string name, int priority)
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

        public void Delete(int id)
        {
            var categoryProvider = new CategoriesProvider();
            categoryProvider.Delete(id);
            categoryProvider.Save();
        }

        public void UpdatePriority(int id, int newPriority)
        {
            var categoryProvider = new CategoriesProvider();
            categoryProvider.UpdatePriority(id, newPriority);
        }

        public void UpdateSetting(int id, int minHoursPerWeek, int maxHoursPerWeek)
        {
            var categoryProvider = new CategoriesProvider();
            categoryProvider.UpdateCategoryData(id, (short)minHoursPerWeek, (short)maxHoursPerWeek);
        }

        #endregion

        #region Activities

        public void SaveActivity(int categoryId, string name, int activityId)
        {
            var activityProvider = new ActivitiesProvider();
            if (activityId == 0)
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
            return activityProvider.GetAll(categoryId).Select(x => new ActivityDto()
            {
                Name = x.Name,
                Id = x.Id
            }).ToList().Serialize();
        }

        #endregion
    }
}