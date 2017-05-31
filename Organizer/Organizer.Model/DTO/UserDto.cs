using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.Model.DTO
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool Admin { get; set; }
        public string Password { get; set; }

        public UserDto(User entity)
        {
            Id = entity.Id;
            Name = entity.Username;
            Email = entity.Email;
            Admin = entity.IsAdmin;
            Password = entity.Password;
        }
    }
}
