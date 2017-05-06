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
        private CategoriesProvider _categoriesProvider;
        private ActivitiesProvider _acivitiesProvider;
        private TodoItemsProvider _todoItemsProvider;

        public CategoriesController(ChromiumWebBrowser originalBrowser, MainWindow mainForm) : base(originalBrowser, mainForm)
        {
            var dbContext = new DataContext();
            Setup(dbContext);
        }

        public CategoriesController() { }

        public void Setup(DataContext dbContext) {
            _categoriesProvider = new CategoriesProvider(dbContext);
            _acivitiesProvider = new ActivitiesProvider(dbContext);
            _todoItemsProvider = new TodoItemsProvider(dbContext);
        }

        #region Categories

        public string GetAll()
        {
            var data = _categoriesProvider.GetAll();
            return data.Serialize();
        }

        public Category Get(int id)
        {
            return _categoriesProvider.GetById(id);
        }

        public string GetCategory(int id)
        {
            return _categoriesProvider.GetById(id).Serialize();
        }

        public void Add(string name, int priority)
        {
            var id = new Random().Next(100000);
            _categoriesProvider.Insert(new Category
            {
                //Id = id,
                Name = name,
                Priority = priority
            });
            _categoriesProvider.Save();
        }

        public void Delete(int id)
        {
            var category = Get(id);
            category.Activities.ToList().ForEach(a => {
                a.TodoItems.ToList().ForEach(t => _todoItemsProvider.Delete(t.Id));
                _acivitiesProvider.Delete(a.Id);
            });

            _categoriesProvider.Delete(category);
            _categoriesProvider.Save();
        }

        public void UpdatePriority(int id, int newPriority)
        {
            _categoriesProvider.UpdatePriority(id, newPriority);
        }

        public void UpdateSetting(int id, int minHoursPerWeek, int maxHoursPerWeek)
        {
            _categoriesProvider.UpdateCategoryData(id, (short)minHoursPerWeek, (short)maxHoursPerWeek);
        }

        #endregion

        #region Activities

        public void SaveActivity(int categoryId, string name, int activityId)
        {
            if (activityId == 0)
            {
                var id = new Random().Next(100000);
                _acivitiesProvider.Insert(new Activity
                {
                    //Id = id,
                    Name = name,
                    Description = null,
                    CategoryId = categoryId,
                    Priority = 2
                });
            }
            else
            {
                var activity = _acivitiesProvider.GetById(activityId);
                activity.Name = name;
                _acivitiesProvider.Update(activity);
            }
            _acivitiesProvider.Save();
        }

        public void DeleteActivity(int id)
        {
            var activity = _acivitiesProvider.GetById(id);
            activity.TodoItems.ToList().ForEach(t => _todoItemsProvider.Delete(t.Id));

            _acivitiesProvider.Delete(activity.Id);
            _acivitiesProvider.Save();
        }

        public string GetActivityItems(int categoryId)
        {
            var dictionary = new Dictionary<int, string>();
            return _acivitiesProvider.GetAll(categoryId).Select(x => new ActivityDto()
            {
                Name = x.Name,
                Id = x.Id
            }).ToList().Serialize();
        }

        #endregion
    }
}