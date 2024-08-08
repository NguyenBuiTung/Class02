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
    public class SheepRepository : ISheepRepository
    {
        private readonly ApplicationDbContext _context;

        public SheepRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Sheep>> GetAllSheepAsync()
        {
            return await _context.Sheeps.ToListAsync();
        }

        public async Task<List<Sheep>> GetSheepByTimeRangeAsync(int startTime, int endTime)
        {
            return await _context.Sheeps
                .Where(s => s.Time >= startTime && s.Time <= endTime)
                .ToListAsync();
        }

        public async Task<List<Sheep>> GetSheepByColorAsync(string color)
        {
            var colorLower = color.ToLower(); // Hoặc .ToUpper()
            return await _context.Sheeps
                .Where(s => s.Color.ToLower() == colorLower) // Hoặc .ToUpper()
                .ToListAsync();
        }

        public async Task<List<Sheep>> GetSheepByOrderIdAsync(int orderId)
        {
            return await _context.Sheeps
                .Where(s => s.OrderId == orderId)
                .ToListAsync();
        }


        public async Task AddSheepAsync(Sheep sheep)
        {
            _context.Sheeps.Add(sheep);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Sheep>> GetSheepByUserIdAsync(string userId)
        {
          return await _context.Sheeps.Where(o => o.UserId == userId).ToListAsync();
        }
    }
}