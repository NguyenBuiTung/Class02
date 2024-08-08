using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Interfaces
{
    public interface ISheepRepository
    {
        Task<List<Sheep>> GetAllSheepAsync();
        Task<List<Sheep>> GetSheepByUserIdAsync(string userId);
        Task<List<Sheep>> GetSheepByTimeRangeAsync(int startTime, int endTime);
        Task<List<Sheep>> GetSheepByOrderIdAsync(int orderId);
        Task<List<Sheep>> GetSheepByColorAsync(string color);
        Task AddSheepAsync(Sheep sheep);
    }
}