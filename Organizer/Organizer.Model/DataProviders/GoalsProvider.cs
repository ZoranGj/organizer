using System.Collections.Generic;
using System.Linq;

using Organizer.Model;

namespace Model.DataProviders {
	public class GoalsProvider : DataProvider<Goal> {
        public GoalsProvider(DataContext db) : base(db) { }

        public GoalsProvider(DataContext db, int userId) : base(db) {
            UserId = userId;
        }

        public new List<Goal> GetAll()
        {
            return _dbSet.OrderBy(x => x.Priority)
                         .ToList();
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

        public Goal UpdateHours(int id, short minHoursPerWeek, short maxHoursPerWeek, string color)
        {
            var goal = GetById(id);
            goal.MinHoursPerWeek = minHoursPerWeek;
            goal.MaxHoursPerWeek = maxHoursPerWeek;
            goal.Color = color;
            Save();

            return goal;
        }
    }
}
