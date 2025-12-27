using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Studio_Rent_Service.Models;

namespace Studio_Rent_Service.ViewModels
{
    public class ClientViewModel
    {
        public ObservableCollection<Client> Clients { get; set; }

        public ClientViewModel()
        {
            Clients = new ObservableCollection<Client>
            {
                new Client
                {
                    Id = 1,
                    Code = "CL001",
                    LastName = "Петров",
                    FirstName = "Иван",
                    MiddleName = "Сергеевич",
                    Phone = "+7 (912) 345-67-89",
                    Email = "ivan.petrov@example.com",
                    AdditionalInfo = "Постоянный клиент"
                },
                new Client
                {
                    Id = 2,
                    Code = "CL002",
                    LastName = "Сидорова",
                    FirstName = "Мария",
                    MiddleName = "Ивановна",
                    Phone = "+7 (923) 456-78-90",
                    Email = "maria.sidorova@example.com",
                    AdditionalInfo = "Предпочитает утренние сеансы"
                }
            };
        }

        public void AddClient(Client client)
        {
            Clients.Add(client);
        }

        public void UpdateClient(Client client)
        {
            var existing = Clients.FirstOrDefault(c => c.Id == client.Id);
            if (existing != null)
            {
                existing.Code = client.Code;
                existing.LastName = client.LastName;
                existing.FirstName = client.FirstName;
                existing.MiddleName = client.MiddleName;
                existing.Phone = client.Phone;
                existing.Email = client.Email;
                existing.AdditionalInfo = client.AdditionalInfo;
            }
        }

        public void DeleteClient(int id)
        {
            var client = Clients.FirstOrDefault(c => c.Id == id);
            if (client != null)
            {
                Clients.Remove(client);
            }
        }

        public int GetNextId()
        {
            return Clients.Count > 0 ? Clients.Max(c => c.Id) + 1 : 1;
        }

        public string GetNextCode()
        {
            if (Clients.Count == 0) return "CL001";

            var lastCode = Clients.Max(c => c.Code);
            var number = int.Parse(lastCode.Substring(2)) + 1;
            return $"CL{number:D3}";
        }
    }
}
