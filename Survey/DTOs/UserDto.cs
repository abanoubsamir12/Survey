using System.ComponentModel.DataAnnotations;
using Survey.Models;

namespace Survey.DTOs
{
    public class UserDto
    {

        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
        public UserDto() { }
        public UserDto(int id, string username, string email,Role role)
        {
            Id = id;
            Username = username;
            Email = email;
            Role = role;
        }
    }
}
