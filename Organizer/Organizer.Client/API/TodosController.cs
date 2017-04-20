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
            var dbContext = new DataContext();
            _todoItemsProvider = new TodoItemsProvider(dbContext);
            _tagsProvider = new TagsProvider(dbContext);
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

        public void Add(string description, DateTime deadline, int activityId, int duration, bool resolved = false)
        {
            var id = new Random().Next(100000);
            _todoItemsProvider.Insert(new TodoItem
            {
                Id = id,
                ActivityId = activityId,
                Deadline = deadline,
                AddedOn = DateTime.Now,
                Description = description,
                Duration = duration,
                Resolved = resolved,
                //Recurring = (short)recurring
            });
            _todoItemsProvider.Save();
        }

        public string GetTags()
        {
            var data = _tagsProvider.GetAll().Select(x => new TagDto { Id = x.Id, Name = x.Name });
            return data.Serialize();
        }

        public string GetTagNames()
        {
            var data = _tagsProvider.GetAll().Select(x => x.Name ).ToList();
            return data.Serialize();
        }
    }
}
