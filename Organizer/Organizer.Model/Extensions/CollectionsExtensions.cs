using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.Model.Extensions
{
    public static class CollectionsExtensions
    {
        public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }

        public static string Serialize<T>(this IEnumerable<T> list)
        {
            return JsonConvert.SerializeObject(list, Formatting.Indented,
                                                new JsonSerializerSettings
                                                {
                                                    PreserveReferencesHandling = PreserveReferencesHandling.Objects
                                                });
        }
    }
}
