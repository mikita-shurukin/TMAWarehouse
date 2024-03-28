using WebMvc.Common;
using WebMvc.DAL.Models;

namespace WebMvc.Models.Requests.Orders
{
    public class UpdateOrderRequest
    {
        public UpdateOrderRequest()
        {
            Items = new List<OrderItem>();
        }
        public int OrderId { get; set; }
        public List<OrderItem> Items { get; set; }
        public string Comment { get; set; }
        public OrderStatus Status { get; set; }
    }
}
