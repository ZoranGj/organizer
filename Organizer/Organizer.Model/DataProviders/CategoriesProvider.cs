using Organizer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DataProviders {
	public class CategoriesProvider : DataProvider<Category> {
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

            //Update(category);
            Update(GetById(swappedCategory.Id));
            Save();
        }
    }
}
