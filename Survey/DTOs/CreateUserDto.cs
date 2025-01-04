using Survey.Models;

namespace Survey.DTOs
{
    public class CreateUserDto : UserDto
    {
       
        public string Password { get; set; }
        public CreateUserDto() { }

        public CreateUserDto(int id, string username, string email,Role role, string password):  base(id,username, email,role)
        {
            
            Password = password;
        }
    }
    
}
