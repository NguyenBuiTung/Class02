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
    }
}