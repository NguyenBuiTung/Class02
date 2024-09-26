using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs
{
    public class GetOrderDtos
    {
        public int OrderId { get; set; }
        public int OrderQuantity { get; set; }
        public string StartDate { get; set; }
        public string? EndDate { get; set; }
        public string Status { get; set; }
    }
}