using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs
{
    public class SheepDto
    {
        public string Color { get; set; }
        public double MeatWeight { get; set; }
        public double WoolWeight { get; set; }
        public int Time { get; set; }
    }
}