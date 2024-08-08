using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Sheep
    {
        public int Id { get; set; }
        public int Time { get; set; }
        public string Color { get; set; }
        public double MeatWeight { get; set; }
        public double WoolWeight { get; set; }
        public string UserId { get; set; }
        public int? OrderId { get; set; } // Khóa ngoại
        public Order Order { get; set; } // Mối quan hệ với Order
    }
}