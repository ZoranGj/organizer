using Organizer.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Setup {
	public class DbInitializer {
		public static void Seed() {
            var dbContext = new DataContext();

            var predefinedTags = new List<Tag>
            {
                new Tag { Name = "Machine learning" },
                new Tag { Name = "Coursera" },
                new Tag { Name = "Pluralsight" },
                new Tag { Name = "Web development" },
                new Tag { Name = "Software architecture" },
                new Tag { Name = "Algorithms" },
            };

            foreach(var tag in predefinedTags)
            {
                if (!dbContext.Tags.Any(t => t.Name == tag.Name))
                {
                    dbContext.Tags.Add(tag);
                }
            }

            dbContext.SaveChanges();
		}
	}
}
