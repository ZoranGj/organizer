﻿using Organizer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DataProviders {
	public class TodoItemsProvider : DataProvider<TodoItem> {
        public TodoItemsProvider(DataContext db) : base(db) { }

        public IEnumerable<TodoItem> GetAll(int categoryId)
        {
            return _dbSet.Include("Activity").Where(x => x.Activity.CategoryId == categoryId);
        }

        public void InitRecurringTodos()
        {

        }

        public void Resolve(int id, bool resolved)
        {
            var item = GetById(id);
            item.Resolved = resolved;
            Save();
        }
    }
}
