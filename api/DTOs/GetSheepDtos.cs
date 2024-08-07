using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs
{
    public class GetSheepDtos
    {
        public int Time { get; set; }
        public string Color { get; set; }
        public double MeatWeight { get; set; }
        public double WoolWeight { get; set; }
    }
}