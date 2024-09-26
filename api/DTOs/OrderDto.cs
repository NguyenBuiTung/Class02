using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs
{
    public class OrderDto
    {
        // public int OrderId { get; set; }
        public int OrderQuantity { get; set; }
        public string StartDate { get; set; }
        // public List<SheepDto> Sheeps { get; set; }
    }
}