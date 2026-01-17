using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Studio_Rent_Service.Models
{
    public enum UserRole
    {
        Admin,
        Client
    }

    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; } 
        public UserRole Role { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLogin { get; set; }

        // Для связи с клиентом (если пользователь - клиент)
        public int? ClientId { get; set; }
        public Client Client { get; set; }
    }
}