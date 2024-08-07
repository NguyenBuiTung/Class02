using System;
using System.Collections.Generic;
using System.Linq;
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
            var orders = await _orderRepository.GetOrdersAsync();
            var orderDtos = orders.Select(order => new GetOrderDtos
            {
                OrderId = order.OrderId,
                OrderQuantity = order.OrderQuantity
            }).ToList();
            return Ok(orderDtos);
        }
        [HttpPost("create-order")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IEnumerable<GetOrderDtos>>> CreateOrder(OrderDto orderDto)
        {
            // Create a new order
            var order = new Order
            {
                OrderQuantity = orderDto.OrderQuantity
            };

            // Add the new order to the database
            await _orderRepository.AddOrderAsync(order);

            // Retrieve the updated list of orders
            var orders = await _orderRepository.GetOrdersAsync();
            var orderDtos = orders.Select(o => new GetOrderDtos
            {
                OrderId = o.OrderId,
                OrderQuantity = o.OrderQuantity
            }).ToList();

            // Return the updated list of orders
            return Ok(orderDtos);
        }


        [HttpPut("update-order/{id}")]
        public async Task<ActionResult<IEnumerable<GetOrderDtos>>> UpdateOrder(int id, OrderDto orderDto)
        {
            var existingOrder = await _orderRepository.GetOrderByIdAsync(id);
            if (existingOrder == null)
            {
                return NotFound();
            }

            // Update the order
            existingOrder.OrderQuantity = orderDto.OrderQuantity;
            await _orderRepository.UpdateOrderAsync(existingOrder);

            // Retrieve the updated list of orders
            var updatedOrders = await _orderRepository.GetOrdersAsync();
            var updatedOrderDtos = updatedOrders.Select(order => new GetOrderDtos
            {
                OrderId = order.OrderId,
                OrderQuantity = order.OrderQuantity
            }).ToList();

            // Return the updated list of orders
            return Ok(updatedOrderDtos);
        }



        [HttpDelete("delete-order/{id}")]
         [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IEnumerable<GetOrderDtos>>> DeleteOrder(int id)
        {
            var existingOrder = await _orderRepository.GetOrderByIdAsync(id);
            if (existingOrder == null)
            {
                return NotFound();
            }

            // Delete the order
            await _orderRepository.DeleteOrderAsync(id);

            // Retrieve the remaining orders
            var remainingOrders = await _orderRepository.GetOrdersAsync();
            var remainingOrderDtos = remainingOrders.Select(order => new GetOrderDtos
            {
                OrderId = order.OrderId,
                OrderQuantity = order.OrderQuantity
            }).ToList();

            // Return the list of remaining orders
            return Ok(remainingOrderDtos);
        }


        [HttpPost("process-transaction/{orderId}")]
         [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> ProcessTransaction(int orderId)
        {
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

            // Nếu chưa có cừu, tạo cừu ngẫu nhiên dựa trên số lượng đơn hàng
            var sheeps = await GenerateSheepForOrder(order.OrderId, order.OrderQuantity);

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



        private async Task<List<Sheep>> GenerateSheepForOrder(int orderId, int quantity)
        {
            var sheeps = new List<Sheep>();
            for (int i = 0; i < quantity; i++)
            {
                var meatWeight = GetRandomMeatWeight();
                var randomSeconds = _random.Next(1, 6);
                var sheep = new Sheep
                {
                    Color = GetRandomColor(),
                    MeatWeight = meatWeight,
                    WoolWeight = GetRandomWoolWeight(meatWeight),
                    Time = randomSeconds,
                    OrderId = orderId
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