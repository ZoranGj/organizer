using CefSharp.WinForms;
using Model.DataProviders;
using Organizer.Model;
using Organizer.Model.DataProviders;
using Organizer.Model.DTO;
using Organizer.Model.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.Client.API
{
    public class TodosController : AppController
    {
        private readonly TodoItemsProvider _todoItemsProvider;
        private readonly TagsProvider _tagsProvider;

        public TodosController(ChromiumWebBrowser originalBrowser, MainWindow mainForm) : base(originalBrowser, mainForm)
        {
            _todoItemsProvider = new TodoItemsProvider();
            _tagsProvider = new TagsProvider();
        }

        public string GetAll(int categoryId)
        {
            _todoItemsProvider.InitRecurringTodos();
            var data = categoryId == 0 ? _todoItemsProvider.GetAll() : _todoItemsProvider.GetAll(categoryId);
            return data.Select(x => new ToDoItemDto
            {
                Id = x.Id,
                Activity = x.Activity.Name,
                Description = x.Description,
                AddedOn = x.AddedOn,
                Deadline = x.Deadline,
                Resolved = x.Resolved,
                Duration = x.Duration,
                Notes = x.Notes,
                Tags = !x.Tags.Any() ? new List<string>() : x.Tags.Select(t => t.Name).ToList()
                //RecurringTypeId = x.Recurring
            }).ToList().Serialize();
        }

        public void Delete(int todoItemId)
        {
            _todoItemsProvider.Delete(todoItemId);
            _todoItemsProvider.Save();
        }

        public void Resolve(int id, bool resolved)
        {
            _todoItemsProvider.Resolve(id, resolved);
        }

        public void Update(int id, string notes, string tags)
        {
            var item = _todoItemsProvider.GetById(id);
            item.Notes = notes;
            item.Tags = string.IsNullOrEmpty(tags) ? null : tags.Split(',').Select(t => new Tag { Name = t }).ToList();
            _todoItemsProvider.Update(item);
            _todoItemsProvider.Save();
        }

        public void Add(string description, DateTime deadline, int activityId, int duration)
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

        public string GetTags()
        {
            var tagsProvider = new TagsProvider();
            var data = tagsProvider.GetAll().Select(x => new TagDto { Id = x.Id, Name = x.Name });
            return data.Serialize();
        }
    }
}
