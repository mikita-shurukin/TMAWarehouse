using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebMvc.DAL.Models;
using WebMvc.DAL;
using Microsoft.EntityFrameworkCore;
using WebMvc.Models.Requests.Orders;
using WebMvc.Common;
using WebMvc.Models.Responses;

namespace WebMvc.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : Controller
    {
        private readonly MainDbContext _dbContext;
        private readonly IMapper _mapper;

        public OrdersController(MainDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var result = await _dbContext.Orders.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (result is null)
            {
                return NotFound();
            }

            return View(result);
        }

        [HttpGet("GetOrdersByStatus")]
        public IActionResult GetOrdersByStatus(string status)
        {
            IEnumerable<Order> orders;

            if (status == "All")
            {
                orders = _dbContext.Orders.ToList();
            }
            else if (Enum.TryParse(status, true, out OrderStatus orderStatus))
            {
                orders = _dbContext.Orders.Where(o => o.Status == orderStatus).ToList();
            }
            else
            {
                return BadRequest("Invalid status value");
            }

            return PartialView("_OrderList", orders); 
        }

        [HttpGet()]
        public async Task<IActionResult> GetAllOrders()
        {
            var cultureInfo = System.Globalization.CultureInfo.GetCultureInfo("uk-UA");

            var orders = await _dbContext.Orders
                .Include(o => o.OrderPositions)
                .AsNoTracking()
                .Select(o => new Order
                {
                    Id = o.Id,
                    Status = o.Status,
                    Comment = o.Comment,
                    OrderPositions = o.OrderPositions.Select(p => new OrderPosition
                    {

                        ItemId = p.ItemId,
                        Quantity = p.Quantity,
                        UnitOfMeasurement = p.UnitOfMeasurement,


                        FormattedPrice = (p.Item.Price * p.Quantity).ToString("C", cultureInfo)
                    }).ToList()
                })
                .ToListAsync();

            return View(orders);

        }


        [HttpGet("Create")]
        public IActionResult Create()
        {
            var model = new CreateOrderRequest();
            var warehouseItems = _dbContext.InventoryItems.ToList();
            model.Items = warehouseItems.Select(item => new OrderItem
            {
                ItemId = item.ItemId,
                Quantity = 0,
                UnitOfMeasurement = item.UnitOfMeasurement,
                WarehouseId = item.WarehouseId
            }).ToList();

            return View(model);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateOrder([FromForm] CreateOrderRequest request)
        {

            var items = request.Items
                .GroupBy(x => x.ItemId + " " + x.WarehouseId)
                .Select(x => new OrderItem
                {
                    Quantity = x.Select(i => i.Quantity).Aggregate((a, b) => a + b),
                    ItemId = x.First().ItemId,
                    UnitOfMeasurement = x.First().UnitOfMeasurement,
                    WarehouseId = x.First().WarehouseId,
                });

            foreach (var item in items)
            {
                var inventoryItem = await _dbContext.InventoryItems
                    .FirstOrDefaultAsync(x => x.ItemId == item.ItemId && x.WarehouseId == item.WarehouseId);

                if (inventoryItem != null)
                {
                    if (inventoryItem.Quantity - item.Quantity < 0)
                    {
                        return BadRequest($"Not enough stock for item with id={item.ItemId}");
                    }
                    else
                    {
                        inventoryItem.Quantity -= item.Quantity;
                        _dbContext.InventoryItems.Update(inventoryItem);
                    }
                }
                else
                {
                    return BadRequest($"Item with id={item.ItemId} not found in inventory");
                }
            }

            var order = new Order
            {
                Comment = request.Comment,
                Status = OrderStatus.New,
                OrderPositions = new List<OrderPosition>()
            };

            var resultOrder = _dbContext.Orders.Add(order);

            await _dbContext.SaveChangesAsync();


            var orderPositions = items.Select(x => new OrderPosition
            {
                OrderId = resultOrder.Entity.Id,
                ItemId = x.ItemId,
                Quantity = x.Quantity,
                UnitOfMeasurement = x.UnitOfMeasurement,
                WarehouseId = x.WarehouseId,
                FormattedPrice = ""
            }).ToList();

            _dbContext.OrderPositions.AddRange(orderPositions);

            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("UpdateOrder/{id}")]
        public async Task<IActionResult> UpdateOrder(int id)
        {
            var order = await _dbContext.GetOrderById(id);

            var model = new UpdateOrderRequest
            {
                OrderId = order.Id,
                Comment = order.Comment,
                Status = order.Status,
                Items = new List<OrderItem>()
            };

            return View(model);
        }



        [HttpPatch("UpdateOrder/{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromForm]UpdateOrderRequest request)
        {
            _dbContext.Orders.Update(new Order
            {
                Id = request.OrderId,
                Comment = request.Comment,
                Status = request.Status
            });

            await _dbContext.SaveChangesAsync();

            var updatedOrder = await _dbContext.GetOrderById(request.OrderId);

            return View("UpdateOrder", updatedOrder);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var itemToDelete = await _dbContext.OrderPositions.FindAsync();

            if (itemToDelete == null)
            {
                return NotFound(); 
            }

            _dbContext.OrderPositions.Remove(itemToDelete); 

            await _dbContext.SaveChangesAsync(); 

            return NoContent(); 
        }
    }
}
