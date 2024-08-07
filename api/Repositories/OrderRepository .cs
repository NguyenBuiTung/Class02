using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddOrderAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }




        public async Task<Order> GetOrderByIdAsync(int id)
        {
            return await _context.Orders
                                 .Include(o => o.Sheeps) // Nếu bạn muốn bao gồm các đối tượng liên quan, như Sheep
                                 .FirstOrDefaultAsync(o => o.OrderId == id);
        }



        public async Task<IEnumerable<Order>> GetOrdersAsync()
        {
            return await _context.Orders.ToListAsync();
        }
        public async Task UpdateOrderAsync(Order order)
        {
            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrderAsync(int id)
        {
            var order = await _context.Orders.Include(o => o.Sheeps).FirstOrDefaultAsync(o => o.OrderId == id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }

    }
}