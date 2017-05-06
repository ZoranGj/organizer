using Organizer.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Model.DataProviders {
	public class TodoItemsProvider : DataProvider<TodoItem> {
        public TodoItemsProvider(DataContext db) : base(db) { }

        public IEnumerable<TodoItem> GetAll(int categoryId)
        {
            return _dbSet.Include("Activity").Where(x => x.Activity.CategoryId == categoryId);
        }

        public void Resolve(int id, bool resolved)
        {
            var item = GetById(id);
            item.Resolved = resolved;
            Save();
        }

        public IEnumerable<TodoItem> GetAllForDeadline(DateTime deadline)
        {
            return _dbSet.Where(item => DbFunctions.TruncateTime(item.Deadline) == deadline.Date);
        }

        public IEnumerable<TodoItem> GetAllForTag(int id)
        {
            return _dbSet.Where(x => x.Tags.Any(tag => tag.Id == id) && x.Resolved).OrderBy(x => x.Deadline).ToList();
        }
    }
}
