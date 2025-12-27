using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Studio_Rent_Service.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public DateTime BookingDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public int DurationHours { get; set; }
        public Studio Studio { get; set; }
        public Client Client { get; set; }
        public string Status { get; set; } // "Активно", "Завершено", "Отменено"
        public decimal Cost { get; set; }
        public string Comment { get; set; }

        public string TimeInfo => $"{StartTime:hh\\:mm} - {StartTime.Add(TimeSpan.FromHours(DurationHours)):hh\\:mm}";
        public bool IsActive => Status == "Активно";
        public string DisplayInfo => $"{Code} - {BookingDate:dd.MM.yyyy} {TimeInfo}";
    }
}
