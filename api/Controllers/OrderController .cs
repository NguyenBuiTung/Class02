using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using api.DTOs;
using api.Interfaces;
using api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ISheepRepository _sheepRepository;
        private readonly IMapper _mapper;
        private static readonly Random _random = new Random();

        public OrderController(IOrderRepository orderRepository, ISheepRepository sheepRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _sheepRepository = sheepRepository;
            _mapper = mapper;
        }

        [HttpGet("get-orders")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IEnumerable<GetOrderDtos>>> GetOrders()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.NameId)?.Value
                         ?? User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized("User ID claim not found");
            }

            var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);

            var orderDtos = orders.Select(order => new GetOrderDtos
            {
                OrderId = order.OrderId,
                OrderQuantity = order.OrderQuantity,
                StartDate = order.StartDate.ToString("yyyy-MM-dd HH:mm"),
                EndDate = order.EndDate.ToString("yyyy-MM-dd HH:mm"),
                Status = order.Status
            }).ToList();

            return Ok(orderDtos);
        }
        [HttpPost("create-order")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IEnumerable<GetOrderDtos>>> CreateOrder(OrderDto orderDto)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.NameId)?.Value
                         ?? User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized("User ID claim not found");
            }

            // Chuyển đổi chuỗi StartDate thành DateTime với định dạng 'yyyy-MM-dd HH:mm'
            DateTime startDate;
            if (!DateTime.TryParseExact(orderDto.StartDate, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate))
            {
                return BadRequest("Invalid date format. Please use 'yyyy-MM-dd HH:mm'.");
            }

            // Tạo mới Order và gán startDate
            var order = new Order
            {
                OrderQuantity = orderDto.OrderQuantity,
                StartDate = startDate,
                Status = "Pending", // Gán giá trị DateTime sau khi đã chuyển đổi
                UserId = userId
            };

            // Thêm đơn hàng mới vào cơ sở dữ liệu
            await _orderRepository.AddOrderAsync(order);

            // Lấy danh sách đơn hàng cập nhật cho user
            var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);
            var orderDtos = orders.Select(o => new GetOrderDtos
            {
                OrderId = o.OrderId,
                OrderQuantity = o.OrderQuantity,
                Status = o.Status,
                StartDate = o.StartDate.ToString("yyyy-MM-dd HH:mm"),
                EndDate = o.EndDate.ToString("yyyy-MM-dd HH:mm"),
            }).ToList();

            return Ok(orderDtos);
        }





        // [HttpPut("update-order/{id}")]
        // public async Task<ActionResult<IEnumerable<GetOrderDtos>>> UpdateOrder(int id, OrderDto orderDto)
        // {
        //     var existingOrder = await _orderRepository.GetOrderByIdAsync(id);
        //     if (existingOrder == null)
        //     {
        //         return NotFound();
        //     }

        //     // Update the order
        //     existingOrder.OrderQuantity = orderDto.OrderQuantity;
        //     await _orderRepository.UpdateOrderAsync(existingOrder);

        //     // Retrieve the updated list of orders
        //     // var updatedOrders = await _orderRepository.GetOrdersAsync();
        //     var updatedOrderDtos = updatedOrders.Select(order => new GetOrderDtos
        //     {
        //         OrderId = order.OrderId,
        //         OrderQuantity = order.OrderQuantity
        //     }).ToList();

        //     // Return the updated list of orders
        //     return Ok(updatedOrderDtos);
        // }



        [HttpDelete("delete-order/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IEnumerable<GetOrderDtos>>> DeleteOrder(int id)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.NameId)?.Value
                         ?? User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized("User ID claim not found");
            }

            var existingOrder = await _orderRepository.GetOrderByIdAsync(id);

            if (existingOrder == null)
            {
                return NotFound("Order not found");
            }

            if (existingOrder.UserId != userId)
            {
                return Forbid("You are not authorized to delete this order");
            }

            // Delete the order
            await _orderRepository.DeleteOrderAsync(id);

            // Retrieve the remaining orders for the user
            var remainingOrders = await _orderRepository.GetOrdersByUserIdAsync(userId);
            var remainingOrderDtos = remainingOrders.Select(order => new GetOrderDtos
            {
                OrderId = order.OrderId,
                OrderQuantity = order.OrderQuantity,
                StartDate = order.StartDate.ToString("yyyy-MM-dd HH:mm"),
                EndDate = order.EndDate.ToString("yyyy-MM-dd HH:mm"),
                Status = order.Status

            }).ToList();

            // Return the list of remaining orders
            return Ok(remainingOrderDtos);
        }



        [HttpPost("process-transaction/{orderId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> ProcessTransaction(int orderId)
        {
            // Lấy UserId từ token
            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.NameId)?.Value
                         ?? User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized("User ID claim not found");
            }

            // Lấy thông tin đơn hàng từ repository
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                return NotFound("Order not found");
            }

            // Kiểm tra xem đã có cừu nào được tạo cho đơn hàng này chưa
            var existingSheeps = await _sheepRepository.GetSheepByOrderIdAsync(orderId);
            if (existingSheeps.Any())
            {
                // Nếu đã có cừu, trả về dữ liệu đã có
                var existingSheepResponse = existingSheeps.Select(sheep => new SheepDto
                {
                    Color = sheep.Color,
                    MeatWeight = sheep.MeatWeight,
                    WoolWeight = sheep.WoolWeight,
                    Time = sheep.Time,
                }).ToList();

                return Ok(existingSheepResponse);
            }

            // Bắt đầu đo thời gian giao dịch
            var startTime = DateTime.Now;

            // Tạo cừu ngẫu nhiên dựa trên số lượng đơn hàng
            var sheeps = await GenerateSheepForOrder(order.OrderId, order.OrderQuantity, userId, startTime);

            // Cập nhật thời gian kết thúc đơn hàng: lấy thời gian bắt đầu cộng với tổng thời gian ngẫu nhiên
            var totalDurationInSeconds = sheeps.Sum(s => s.Time);
            var endTime = startTime.AddSeconds(totalDurationInSeconds);

            // Cập nhật trạng thái và thời gian kết thúc đơn hàng
            order.Status = "Completed";
            order.EndDate = endTime;
            await _orderRepository.UpdateOrderAsync(order);

            // Chuyển đổi danh sách cừu thành DTO để trả về cho client
            var newSheepResponse = sheeps.Select(sheep => new SheepDto
            {
                Color = sheep.Color,
                MeatWeight = sheep.MeatWeight,
                WoolWeight = sheep.WoolWeight,
                Time = sheep.Time,
            }).ToList();

            return Ok(newSheepResponse);
        }

        private async Task<List<Sheep>> GenerateSheepForOrder(int orderId, int quantity, string userId, DateTime startTime)
        {
            var sheeps = new List<Sheep>();

            for (int i = 0; i < quantity; i++)
            {
                var meatWeight = GetRandomMeatWeight();

                // Giả sử mỗi lần tạo cừu mất ngẫu nhiên từ 1-5 giây
                var individualDurationInSeconds = _random.Next(1, 6);

                var sheep = new Sheep
                {
                    Color = GetRandomColor(),
                    MeatWeight = meatWeight,
                    WoolWeight = GetRandomWoolWeight(meatWeight),
                    Time = individualDurationInSeconds, // Thời gian riêng cho từng con cừu
                    OrderId = orderId,
                    UserId = userId
                };

                // Thêm cừu vào cơ sở dữ liệu
                await _sheepRepository.AddSheepAsync(sheep);
                sheeps.Add(sheep);
            }
            return sheeps;
        }




        private string GetRandomColor()
        {
            var colors = new[] { "Đen", "Trắng", "Xám" };
            return colors[_random.Next(colors.Length)];
        }

        private int GetRandomMeatWeight()
        {
            return _random.Next(30, 61);
        }

        private double GetRandomWoolWeight(int meatWeight)
        {
            var woolPercentage = _random.Next(3, 8);
            return meatWeight * woolPercentage / 100.0;
        }






    }

}