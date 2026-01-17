using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Studio_Rent_Service.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string AdditionalInfo { get; set; }

        public string FullName => $"{LastName} {FirstName} {MiddleName}".Trim();
        public string DisplayInfo => $"{Code} - {FullName}";
        public string ContactInfo => $"{Phone} {Email}";
    }
}
