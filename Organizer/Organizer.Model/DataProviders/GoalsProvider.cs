using System.Collections.Generic;
using System.Linq;

using Organizer.Model;

namespace Model.DataProviders {
	public class GoalsProvider : DataProvider<Goal> {
        public GoalsProvider(DataContext db) : base(db) { }

        public new List<Goal> GetAll()
        {
            return _dbSet.OrderBy(x => x.Priority).ToList();
        }

        public void UpdatePriority(int id, int newPriority)
        {
            var swappedGoal = _dbSet.FirstOrDefault(x => x.Priority == newPriority);
            if (swappedGoal == null) return;

            var goal = GetById(id);
            swappedGoal.Priority = goal.Priority;
            goal.Priority = newPriority;

            Save();
        }

        public void Delete(Goal goal)
        {
            _dbSet.Remove(goal);
        }

        public void UpdateHours(int id, short minHoursPerWeek, short maxHoursPerWeek)
        {
            var goal = GetById(id);
            goal.MinHoursPerWeek = minHoursPerWeek;
            goal.MaxHoursPerWeek = maxHoursPerWeek;
            Save();
        }
    }
}
