using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Setup {
	public class DbInitializer : DropCreateDatabaseIfModelChanges<DataContext> {
		protected override void Seed(DataContext context) {
			if (!context.Categories.Any()) {
				context.Categories.Add(new Category {
					Name = "Dummy Category",
					Notes = "Simple note.. "
				});
			}
		}
	}
}
