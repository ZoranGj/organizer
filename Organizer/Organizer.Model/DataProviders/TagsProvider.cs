using Model.DataProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.Model.DataProviders
{
    public class TagsProvider : DataProvider<Tag>
    {
        public TagsProvider(DataContext db) : base(db) { }

        public Tag Get(string name)
        {
            return _dbSet.FirstOrDefault(tag => tag.Name == name);
        }
    }
}
