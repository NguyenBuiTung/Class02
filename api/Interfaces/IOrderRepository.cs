using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetOrdersAsync();
        Task AddOrderAsync(Order order);
        Task<Order> GetOrderByIdAsync(int id);
        Task UpdateOrderAsync(Order order);
        Task DeleteOrderAsync(int id);
    }
}