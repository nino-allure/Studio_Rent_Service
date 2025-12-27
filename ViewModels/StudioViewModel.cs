using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Studio_Rent_Service.Models;

namespace Studio_Rent_Service.ViewModels
{
    public class StudioViewModel
    {
        public ObservableCollection<Studio> Studios { get; set; }

        public StudioViewModel()
        {
            Studios = new ObservableCollection<Studio>
            {
                new Studio
                {
                    Id = 1,
                    Code = "ST001",
                    Name = "Студия звукозаписи 'Волна'",
                    Address = "ул. Пушкина, д. 10",
                    Equipment = "Микрофоны Neumann, Акустическая обработка, MIDI-клавиатура",
                    RentalCost = 2500
                },
                new Studio
                {
                    Id = 2,
                    Code = "ST002",
                    Name = "Ultima Sounds",
                    Address = "пр. Ленина, д. 45",
                    Equipment = "Зеркала, Балетные станки, Звуковая система",
                    RentalCost = 1800
                },
                new Studio
                {
                    Id = 3,
                    Code = "ST003",
                    Name = "Luxury Waves",
                    Address = "ул. Советская, д. 23",
                    Equipment = "Мольберты, Освещение, Материалы для рисования",
                    RentalCost = 1200
                }
            };
        }

        public void AddStudio(Studio studio)
        {
            Studios.Add(studio);
        }

        public void UpdateStudio(Studio studio)
        {
            var existing = Studios.FirstOrDefault(s => s.Id == studio.Id);
            if (existing != null)
            {
                existing.Code = studio.Code;
                existing.Name = studio.Name;
                existing.Address = studio.Address;
                existing.Equipment = studio.Equipment;
                existing.RentalCost = studio.RentalCost;
            }
        }

        public void DeleteStudio(int id)
        {
            var studio = Studios.FirstOrDefault(s => s.Id == id);
            if (studio != null)
            {
                Studios.Remove(studio);
            }
        }

        public int GetNextId()
        {
            return Studios.Count > 0 ? Studios.Max(s => s.Id) + 1 : 1;
        }

        public string GetNextCode()
        {
            if (Studios.Count == 0) return "ST001";

            var lastCode = Studios.Max(s => s.Code);
            var number = int.Parse(lastCode.Substring(2)) + 1;
            return $"ST{number:D3}";
        }
    }
}
