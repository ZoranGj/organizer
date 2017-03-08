using Organizer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DataProviders {
	public class TodoItemsProvider : DataProvider<TodoItem> {
        public new IEnumerable<TodoItem> GetAll()
        {
            return _dbSet.Include("Activity");
        }
    }
}
