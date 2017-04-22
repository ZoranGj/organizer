using System.Collections.Generic;
using System.Linq;

using Organizer.Model;

namespace Model.DataProviders {
	public class CategoriesProvider : DataProvider<Category> {
        public CategoriesProvider(DataContext db) : base(db) { }

        public new List<Category> GetAll()
        {
            return _dbSet.OrderBy(x => x.Priority).ToList();
        }

        public void UpdatePriority(int id, int newPriority)
        {
            var swappedCategory = _dbSet.FirstOrDefault(x => x.Priority == newPriority);
            if (swappedCategory == null) return;

            var category = GetById(id);
            swappedCategory.Priority = category.Priority;
            category.Priority = newPriority;

            Save();
        }

        public void Delete(Category category)
        {
            _dbSet.Remove(category);
        }

        public void UpdateCategoryData(int id, short minHoursPerWeek, short maxHoursPerWeek)
        {
            var category = GetById(id);
            category.MinHoursPerWeek = minHoursPerWeek;
            category.MaxHoursPerWeek = maxHoursPerWeek;
            Save();
        }
    }
}
