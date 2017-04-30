using Newtonsoft.Json;
using System;

namespace Organizer.Model.Extensions
{
    public static class ObjectExtensions
    {
        public static string Serialize(this object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented,
                                                new JsonSerializerSettings
                                                {
                                                    PreserveReferencesHandling = PreserveReferencesHandling.Objects
                                                });
        }
    }
}
