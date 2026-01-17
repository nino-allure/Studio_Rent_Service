using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Studio_Rent_Service.Models;

namespace Studio_Rent_Service.ViewModels
{
    public class AuthViewModel
    {
        private List<User> users;
        public User CurrentUser { get; set; } // Изменено на public set для возможности сброса

        public AuthViewModel()
        {
            // Инициализация тестовых пользователей
            users = new List<User>
            {
                new User
                {
                    Id = 1,
                    Username = "admin",
                    Password = "admin123",
                    Role = UserRole.Admin,
                    FullName = "Администратор Системы",
                    Email = "admin@studio.com",
                    CreatedAt = DateTime.Now.AddMonths(-1)
                },
                new User
                {
                    Id = 2,
                    Username = "ivanov",
                    Password = "client123",
                    Role = UserRole.Client,
                    FullName = "Иванов Иван Иванович",
                    Email = "ivanov@example.com",
                    CreatedAt = DateTime.Now.AddDays(-15),
                    ClientId = 1 // Связь с клиентом ID=1
                },
                new User
                {
                    Id = 3,
                    Username = "sidorova",
                    Password = "client456",
                    Role = UserRole.Client,
                    FullName = "Сидорова Мария Ивановна",
                    Email = "sidorova@example.com",
                    CreatedAt = DateTime.Now.AddDays(-10),
                    ClientId = 2 // Связь с клиентом ID=2
                }
            };
        }

        // Метод InitializeTestUsers был удален, так как уже инициализируем в конструкторе

        public bool Authenticate(string username, string password, out string errorMessage)
        {
            errorMessage = "";

            if (string.IsNullOrWhiteSpace(username))
            {
                errorMessage = "Введите имя пользователя";
                return false;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                errorMessage = "Введите пароль";
                return false;
            }

            var user = users.FirstOrDefault(u =>
                u.Username.Equals(username, StringComparison.OrdinalIgnoreCase) &&
                u.Password == password);

            if (user == null)
            {
                errorMessage = "Неверное имя пользователя или пароль";
                return false;
            }

            // Обновляем время последнего входа
            user.LastLogin = DateTime.Now;
            CurrentUser = user;

            return true;
        }

        public bool IsAdmin => CurrentUser?.Role == UserRole.Admin;
        public bool IsClient => CurrentUser?.Role == UserRole.Client;

        public string WelcomeMessage => CurrentUser != null
            ? $"Добро пожаловать, {CurrentUser.FullName} ({CurrentUser.Role})"
            : "";
    }
}