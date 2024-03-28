using WebMvc.Common;

namespace WebMvc.Models.Responses
{
    public class OrderResponse
    {
        public int RequestId { get; set; }
        public int ItemId { get; set; }
        public string UnitOfMeasurement { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Comment { get; set; }
        public OrderStatus Status { get; set; }
    }
}
