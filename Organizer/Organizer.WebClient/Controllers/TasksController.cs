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
    //[Authorize]
    public class TasksController : BaseController
    {
        private readonly GoalsProvider _goalsProvider;
        private readonly ActivitiesProvider _activitiesProvider;
        private readonly TodoItemsProvider _todoItemsProvider;
        private readonly TagsProvider _tagsProvider;

        public TasksController()
        {
            var dbContext = new DataContext();
            _goalsProvider = new GoalsProvider(dbContext);
            _activitiesProvider = new ActivitiesProvider(dbContext);
            _todoItemsProvider = new TodoItemsProvider(dbContext);
            _tagsProvider = new TagsProvider(dbContext);
        }

        [HttpGet]
        public ActionResult GetAll(int goalId)
        {
            var data = goalId == 0 ? _todoItemsProvider.GetAll() : _todoItemsProvider.GetAll(goalId);
            return Json(data.Select(x => new ToDoItemDto(x)).ToList());
        }

        [HttpGet]
        public ActionResult Get(int todoItemId)
        {
            var data = _todoItemsProvider.GetById(todoItemId);
            var dto = new ToDoItemDto(data);
            return Json(dto);
        }

        [HttpPost]
        public void Delete(int todoItemId)
        {
            _todoItemsProvider.Delete(todoItemId);
            _todoItemsProvider.Save();
        }

        [HttpPost]
        public void Resolve(int id, bool resolved)
        {
            _todoItemsProvider.Resolve(id, resolved);
        }

        [HttpPost]
        public void Update(int id, string notes, string tags)
        {
            var tagList = new List<Tag>();
            var item = _todoItemsProvider.GetById(id);
            item.Notes = notes;

            if (string.IsNullOrEmpty(tags) || tags == " ")
            {
                item.Tags = new List<Tag>();
            }
            else
            {
                item.Tags = new List<Tag>();

                foreach (var tag in tags.Split(','))
                {
                    var dbTag = _tagsProvider.Get(tag);
                    if (dbTag != null)
                    {
                        item.Tags.Add(dbTag);
                    }
                    else
                    {
                        item.Tags.Add(new Tag
                        {
                            Name = tag,
                        });
                    }
                }
            }

            _todoItemsProvider.Update(item);
            _todoItemsProvider.Save();
        }

        [HttpPost]
        public void Add(string description, DateTime deadline, int activityId, int duration, bool resolved = false)
        {
            _todoItemsProvider.Insert(new TodoItem
            {
                ActivityId = activityId,
                Deadline = deadline,
                AddedOn = DateTime.Now,
                Description = description,
                Duration = duration,
                Resolved = resolved,
            });

            var activity = _activitiesProvider.GetById(activityId);
            if (activity != null && activity.StartDate == null)
            {
                var date = activity.TodoItems.Min(t => t.AddedOn);
                activity.StartDate = date;
                var goal = _goalsProvider.GetById(activity.GoalId);
                if (goal != null && goal.Start == null)
                {
                    goal.Start = date;
                }
            }

            _todoItemsProvider.Save();
        }

        [HttpPost]
        public void UpdateDescription(int id, string value)
        {
            var todo = _todoItemsProvider.GetById(id);
            if (!todo.Description.Equals(value))
            {
                todo.Description = value;
                _todoItemsProvider.Save();
            }
        }

        [HttpPost]
        public void UpdateDuration(int id, int value)
        {
            var todo = _todoItemsProvider.GetById(id);
            if (todo.Duration != value)
            {
                todo.Duration = value;
                _todoItemsProvider.Save();
            }
        }

        [HttpGet]
        public ActionResult GetTags()
        {
            var data = _tagsProvider.GetAll().Select(tag => new TagDto(tag));
            return Json(data);
        }

        [HttpGet]
        public ActionResult GetTagNames()
        {
            var data = _tagsProvider.GetAll().Select(x => x.Name).ToList();
            return Json(data);
        }
    }
}