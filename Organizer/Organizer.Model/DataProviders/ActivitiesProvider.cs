using System.Collections.Generic;
using System.Linq;

using Organizer.Model;

namespace Model.DataProviders {
	public class ActivitiesProvider : DataProvider<Activity> {
        public ActivitiesProvider(DataContext db) : base(db) { }

        public IEnumerable<Activity> GetAll(int categoryId)
        {
            return _dbSet.Where(x => x.CategoryId == categoryId);
        }

        public new void Delete(object Id)
        {
            var activity = _dbSet.Find(Id);
            //var todoItems = activity.TodoItems.ToList();
            //todoItems.ForEach(a => activity.TodoItems.Remove(a));

            _dbSet.Remove(activity);
        }
    }
}
