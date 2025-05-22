using BO;
using DO;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class OpenCallInList
    {
        public int Id { get; init; }
        public TYPEOFCALL TypeOfCall { get; init; }
        public string? VerbalDescription { get; init;}
        public string FullAddress { get; init; }
        public DateTime OpenTime { get; init; }
        public DateTime? MaxTimeToFinish { get; init; }
        public double Distance { get; init; }
    }
}
