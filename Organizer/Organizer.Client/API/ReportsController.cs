using CefSharp.WinForms;
using Model.DataProviders;
using Organizer.Model;
using Organizer.Model.DataProviders;
using Organizer.Model.DTO;
using Organizer.Model.Extensions;
using System;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace Organizer.Client.API
{
    public class ReportsController : AppController
    {
        private readonly GoalsController goals;
        private readonly TodoItemsProvider _todoItemsProvider;
        private readonly GoalsProvider _goalsProvider;
        private readonly TagsProvider _tagsProvider;
        private const int expiredItemsDays = 7;
        private const int upcomingItemsDays = 3;

        public ReportsController(ChromiumWebBrowser originalBrowser, MainWindow mainForm, DataContext dbContext) : base(originalBrowser, mainForm)
        {
            _todoItemsProvider = new TodoItemsProvider(dbContext);
            _tagsProvider = new TagsProvider(dbContext);
            _goalsProvider = new GoalsProvider(dbContext);
        }

        public string LoadProductivityReports(int id)
        {
            var goal = _goalsProvider.GetById(id);
            var todoItems = _todoItemsProvider.GetAll(goal.Id).Where(x => x.Resolved).OrderBy(x => x.Deadline).ToList();
            return todoItems.ProductivityReports(goal).Serialize();
        }

        public string LoadTagsReports(int tagId)
        {
            CultureInfo culture = Thread.CurrentThread.CurrentCulture;
            var todoItems = _tagsProvider.GetById(tagId).TodoItems.Where(x => x.Resolved).OrderBy(x => x.Deadline).ToList();
            return todoItems.ProductivityReports().Serialize();
        }

        public string LoadTodoItemNotifications()
        {
            var upcomingItemsTreshold = DateTime.Now.AddDays(upcomingItemsDays);
            var expiredItemsTreshold = DateTime.Now.AddDays(-expiredItemsDays);

            var todoItems = _todoItemsProvider.GetAll(x => !x.Resolved
                                                        && x.Deadline >= expiredItemsTreshold
                                                        && x.Deadline <= upcomingItemsTreshold)
                                              .ToList();
            return todoItems.Select(x => new ToDoItemDto(x)).Serialize();
        }
    }
}
