using CefSharp.WinForms;
using Model.DataProviders;
using Organizer.Model;
using Organizer.Model.DataProviders;
using Organizer.Model.Extensions;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace Organizer.Client.API
{
    public class ReportsController : AppController
    {
        private readonly GoalsController goals;
        private readonly TodoItemsProvider _todoItemsProvider;
        private readonly TagsProvider _tagsProvider;

        public ReportsController(ChromiumWebBrowser originalBrowser, MainWindow mainForm) : base(originalBrowser, mainForm)
        {
            var dbContext = new DataContext();
            _todoItemsProvider = new TodoItemsProvider(dbContext);
            _tagsProvider = new TagsProvider(dbContext);
            goals = new GoalsController(originalBrowser, mainForm);
        }

        public string LoadProductivityReports(int id)
        {
            var goal = goals.Get(id);
            var todoItems = _todoItemsProvider.GetAll(goal.Id).Where(x => x.Resolved).OrderBy(x => x.Deadline).ToList();
            return todoItems.ProductivityReports(goal).Serialize();
        }

        public string LoadTagsReports(int tagId)
        {
            CultureInfo culture = Thread.CurrentThread.CurrentCulture;
            var todoItems = _tagsProvider.GetById(tagId).TodoItems.Where(x => x.Resolved).OrderBy(x => x.Deadline).ToList();
            return todoItems.ProductivityReports().Serialize();
        }
    }
}
