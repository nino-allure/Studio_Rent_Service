using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Studio_Rent_Service.Models;

namespace Studio_Rent_Service.ViewModels
{
    public class ReservationViewModel
    {
        public ObservableCollection<Reservation> Reservations { get; set; }
        public ObservableCollection<Studio> Studios { get; set; }
        public ObservableCollection<Client> Clients { get; set; }

        public ReservationViewModel()
        {
            // Инициализация студий
            var studioVM = new StudioViewModel();
            Studios = studioVM.Studios;

            // Инициализация клиентов
            var clientVM = new ClientViewModel();
            Clients = clientVM.Clients;

            // Инициализация бронирований
            Reservations = new ObservableCollection<Reservation>
            {
                new Reservation
                {
                    Id = 1,
                    Code = "RS001",
                    BookingDate = DateTime.Today.AddDays(1),
                    StartTime = new TimeSpan(10, 0, 0),
                    DurationHours = 2,
                    Studio = Studios[0],
                    Client = Clients[0],
                    Status = "Активно",
                    Cost = 5000,
                    Comment = "Запись песни"
                },
                new Reservation
                {
                    Id = 2,
                    Code = "RS002",
                    BookingDate = DateTime.Today.AddDays(2),
                    StartTime = new TimeSpan(14, 0, 0),
                    DurationHours = 3,
                    Studio = Studios[1],
                    Client = Clients[1],
                    Status = "Активно",
                    Cost = 5400,
                    Comment = "Репетиция танцевального номера"
                },
                new Reservation
                {
                    Id = 3,
                    Code = "RS003",
                    BookingDate = DateTime.Today.AddDays(-1),
                    StartTime = new TimeSpan(16, 0, 0),
                    DurationHours = 1,
                    Studio = Studios[2],
                    Client = Clients[0],
                    Status = "Завершено",
                    Cost = 1200,
                    Comment = "Мастер-класс"
                }
            };
        }

        public ObservableCollection<Reservation> GetClientReservations(int clientId)
        {
            var filtered = new ObservableCollection<Reservation>(
                Reservations.Where(r => r.Client?.Id == clientId));
            return filtered;
        }
        public void AddReservation(Reservation reservation)
        {
            Reservations.Add(reservation);
        }

        public void UpdateReservation(Reservation reservation)
        {
            var existing = Reservations.FirstOrDefault(r => r.Id == reservation.Id);
            if (existing != null)
            {
                existing.Code = reservation.Code;
                existing.BookingDate = reservation.BookingDate;
                existing.StartTime = reservation.StartTime;
                existing.DurationHours = reservation.DurationHours;
                existing.Studio = reservation.Studio;
                existing.Client = reservation.Client;
                existing.Status = reservation.Status;
                existing.Cost = reservation.Cost;
                existing.Comment = reservation.Comment;
            }
        }

        public void DeleteReservation(int id)
        {
            var reservation = Reservations.FirstOrDefault(r => r.Id == id);
            if (reservation != null)
            {
                Reservations.Remove(reservation);
            }
        }

        public int GetNextId()
        {
            return Reservations.Count > 0 ? Reservations.Max(r => r.Id) + 1 : 1;
        }

        public string GetNextCode()
        {
            if (Reservations.Count == 0) return "RS001";

            var lastCode = Reservations.Max(r => r.Code);
            var number = int.Parse(lastCode.Substring(2)) + 1;
            return $"RS{number:D3}";
        }

        public bool IsStudioAvailable(Studio studio, DateTime date, TimeSpan startTime, int durationHours, int excludeReservationId = 0)
        {
            var endTime = startTime.Add(TimeSpan.FromHours(durationHours));

            return !Reservations.Any(r =>
                r.Id != excludeReservationId &&
                r.Status != "Отменено" &&
                r.Studio.Id == studio.Id &&
                r.BookingDate.Date == date.Date &&
                r.StartTime < endTime &&
                r.StartTime.Add(TimeSpan.FromHours(r.DurationHours)) > startTime);
        }

        public decimal CalculateCost(Studio studio, int durationHours)
        {
            return studio.RentalCost * durationHours;
        }
    }
}
