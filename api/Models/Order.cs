using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int OrderQuantity { get; set; }
        public ICollection<Sheep> Sheeps { get; set; } // Danh sách các con cừu thuộc về đơn hàng
    }
}