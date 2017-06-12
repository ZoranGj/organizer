using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.Mvc;

using Model.DataProviders;
using Organizer.Model;
using Organizer.Model.DataProviders;
using Organizer.Model.DTO;
using Organizer.Model.Extensions;

namespace Organizer.WebClient.Controllers
{
    [Authorize]
    public class ReportsController : BaseController
    {
        private readonly GoalsController goals;
        private readonly TodoItemsProvider _todoItemsProvider;
        private readonly GoalsProvider _goalsProvider;
        private readonly TagsProvider _tagsProvider;
        private const int expiredItemsDays = 7;
        private const int upcomingItemsDays = 3;

        public ReportsController()
        {
            var dbContext = new DataContext();
            _todoItemsProvider = new TodoItemsProvider(dbContext, UserId);
            _tagsProvider = new TagsProvider(dbContext);
            _goalsProvider = new GoalsProvider(dbContext, UserId);
        }

        [HttpGet]
        public ActionResult LoadProductivityReports(int goalId)
        {
            var goal = _goalsProvider.GetById(goalId);
            var todoItems = _todoItemsProvider.GetAll(goal.Id).Where(x => x.Resolved).OrderBy(x => x.Deadline).ToList();
            return Json(todoItems.ProductivityReports(goal));
        }

        [HttpGet]
        public ActionResult LoadTagsReports(int tagId)
        {
            CultureInfo culture = Thread.CurrentThread.CurrentCulture;
            var todoItems = _tagsProvider.GetById(tagId).TodoItems.Where(x => x.Resolved).OrderBy(x => x.Deadline).ToList();
            return Json(todoItems.ProductivityReports());
        }

        [HttpGet]
        public ActionResult LoadTodoItemNotifications()
        {
            var upcomingItemsTreshold = DateTime.Now.AddDays(upcomingItemsDays);
            var expiredItemsTreshold = DateTime.Now.AddDays(-expiredItemsDays);

            var todoItems = _todoItemsProvider.GetAll(x => !x.Resolved
                                                        && x.Deadline >= expiredItemsTreshold
                                                        && x.Deadline <= upcomingItemsTreshold)
                                              .ToList();
            return Json(todoItems.Select(x => new ToDoItemDto(x)));
        }
    }
}