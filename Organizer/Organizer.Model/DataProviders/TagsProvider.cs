using System;
using System.Linq;

using Model.DataProviders;

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
