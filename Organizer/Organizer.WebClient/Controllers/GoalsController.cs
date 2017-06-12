using Model.DataProviders;
using Organizer.Model;
using Organizer.Model.DataProviders;
using Organizer.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Organizer.WebClient.Controllers
{
    [Authorize]
    public class GoalsController : BaseController
    {
        private GoalsProvider _goalsProvider;
        private ActivitiesProvider _acivitiesProvider;
        private TodoItemsProvider _todoItemsProvider;
        private UsersProvider _usersProvider;
        private DataContext _dbContext;

        public GoalsController()
        {
            _dbContext = new DataContext();
            _goalsProvider = new GoalsProvider(_dbContext, UserId);
            _acivitiesProvider = new ActivitiesProvider(_dbContext, UserId);
            _todoItemsProvider = new TodoItemsProvider(_dbContext, UserId);
            _usersProvider = new UsersProvider(_dbContext);
        }

        #region Goals

        [HttpGet]
        public ActionResult GetAll()
        {
            var data = _goalsProvider.GetAll().Select(g => new GoalDto(g)).ToList();
            return Json(data);
        }

        [HttpGet]
        public ActionResult Get(int id, bool reload = true)
        {
            var goal = _goalsProvider.GetById(id);
            if (reload) _dbContext.Entry<Goal>(goal).Reload();

            return Json(goal);
        }

        [HttpPost]
        public void Add(string name, int priority)
        {
            var user = _usersProvider.GetById(UserId);

            _goalsProvider.Insert(new Goal
            {
                Name = name,
                Priority = priority,
                Color = "rgb(92,184,92)",
                MinHoursPerWeek = 2,
                MaxHoursPerWeek = 8,
                User = user
            });
            _goalsProvider.Save();
        }

        [HttpPost]
        public void Delete(int id)
        {
            var goal = _goalsProvider.GetById(id);
            goal.Activities.ToList().ForEach(a => {
                a.TodoItems.ToList().ForEach(t => _todoItemsProvider.Delete(t.Id));
                _acivitiesProvider.Delete(a.Id);
            });

            _goalsProvider.Delete(goal);
            _goalsProvider.Save();
        }

        [HttpPost]
        public void UpdatePriority(int id, int newPriority)
        {
            _goalsProvider.UpdatePriority(id, newPriority);
        }

        [HttpPost]
        public void UpdateSetting(int id, int minHoursPerWeek, int maxHoursPerWeek, string color)
        {
            _goalsProvider.UpdateHours(id, (short)minHoursPerWeek, (short)maxHoursPerWeek, color);
        }

        [HttpPost]
        public void UpdateName(int id, string value)
        {
            var goal = _goalsProvider.GetById(id);
            if (!goal.Name.Equals(value))
            {
                goal.Name = value;
                _goalsProvider.Save();
            }
        }

        #endregion

        #region Activities

        [HttpPost]
        public void SaveActivity(int goalId, string name, int activityId)
        {
            if (activityId == 0)
            {
                _acivitiesProvider.Insert(new Activity
                {
                    Name = name,
                    Description = null,
                    GoalId = goalId,
                    Priority = 2,
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

        [HttpPost]
        public void DeleteActivity(int id)
        {
            var activity = _acivitiesProvider.GetById(id);
            activity.TodoItems.ToList().ForEach(t => _todoItemsProvider.Delete(t.Id));

            _acivitiesProvider.Delete(activity.Id);
            _acivitiesProvider.Save();
        }

        [HttpGet]
        public ActionResult GetActivityItems(int goalId)
        {
            var dictionary = new Dictionary<int, string>();
            return Json(_acivitiesProvider.GetAll(goalId).Select(x => new ActivityDto()
            {
                Name = x.Name,
                Id = x.Id
            }).ToList());
        }

        [HttpPost]
        public void UpdateActivity(int id, string name, DateTime? startDate = null, DateTime? plannedCompletionDate = null)
        {
            var activity = _acivitiesProvider.GetById(id);
            if (activity != null)
            {
                activity.Name = name;
                activity.StartDate = startDate;
                activity.PlannedCompletionDate = plannedCompletionDate;
                _goalsProvider.Save();
            }
        }

        #endregion
    }
}