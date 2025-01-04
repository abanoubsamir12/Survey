using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.CodeAnalysis.Scripting;
using Org.BouncyCastle.Crypto.Generators;

namespace Survey.Models
{
    public enum Role
    {
        User,
        Admin
    }

    public class User
    {
        [Key]
        public int Id { get; private set; }

        [Required]
        [MaxLength(100)]
        public string Username { get; private set; }
        public void UpdateUsername(string username) { this.Username = username; }

        [Required]
        [EmailAddress]
        public string Email { get;  private set; }

        public Boolean isActive { get; private set; }
        public void UpdateisActive(Boolean isActive) { this.isActive = isActive; }

        public void UpdateEmail(string email) { this.Email = email; }

        public string password { get; private set; }

        [Required]
        public Role Role { get; set; }

        public void UpdatePassword(string value)
        {
            password = value;
        }

        // Navigation property
        public ICollection<Answer> Answers { get; private set; }
        public ICollection<UserSurvey> UserSurveys { get; set; }
        public User(string username, string email,Role role, string password)
        {
            Username = username;
            Email = email;
            this.password = password;
            Role = role;
            if (role == Role.Admin)
            {
                isActive = false;
            }
            else
                isActive = true;
            Answers = new List<Answer>();
        }
        public User() { }

    }
}
