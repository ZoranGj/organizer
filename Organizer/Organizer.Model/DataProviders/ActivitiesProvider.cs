using System.Collections.Generic;
using System.Linq;

using Organizer.Model;

namespace Model.DataProviders {
	public class ActivitiesProvider : DataProvider<Activity> {
        public ActivitiesProvider(DataContext db) : base(db) { }
        public ActivitiesProvider(DataContext db, int userId) : base(db) {
            UserId = userId;
        }

        public IEnumerable<Activity> GetAll(int goalId)
        {
            return _dbSet.Where(x => x.GoalId == goalId && x.Goal.User.Id == UserId);
        }

        public new void Delete(object Id)
        {
            var activity = _dbSet.Find(Id);
            _dbSet.Remove(activity);
        }
    }
}
