using System.Collections.Generic;
using System.Linq;
using Organizer.Model;

namespace Model.DataProviders {
	public class TodoItemsProvider : DataProvider<TodoItem> {
        public TodoItemsProvider(DataContext db) : base(db) { }

        public IEnumerable<TodoItem> GetAll(int goalId)
        {
            return _dbSet.Include("Activity").Where(x => x.Activity.GoalId == goalId);
        }

        public void Resolve(int id, bool resolved)
        {
            var item = GetById(id);
            item.Resolved = resolved;
            Save();
        }
    }
}
