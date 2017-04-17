using Organizer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DataProviders {
	public class ActivitiesProvider : DataProvider<Activity> {
        public ActivitiesProvider(DataContext db) : base(db) { }

        public IEnumerable<Activity> GetAll(int categoryId)
        {
            return _dbSet.Where(x => x.CategoryId == categoryId);
        }
    }
}
