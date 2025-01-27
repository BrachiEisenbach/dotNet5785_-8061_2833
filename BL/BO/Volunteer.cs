using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    internal class Volunteer
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }
        public string? FullAddress { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}
