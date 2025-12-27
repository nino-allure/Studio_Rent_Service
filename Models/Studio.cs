using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Studio_Rent_Service.Models
{
    public class Studio
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Equipment { get; set; }
        public decimal RentalCost { get; set; }

        public string DisplayInfo => $"{Code} - {Name} ({RentalCost:N0} ₽/час)";
        public string ShortInfo => $"{Name}";
    }
}
