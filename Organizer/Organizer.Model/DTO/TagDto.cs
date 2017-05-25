using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.Model.DTO
{
    public class TagDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public TagDto(Tag entity)
        {
            Id = entity.Id;
            Name = entity.Name;
        }

        public TagDto() { }
    }
}
