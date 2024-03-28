namespace WebMvc.Models.Requests.Orders
{
    public class CreateOrderRequest
    {
        public IEnumerable<OrderItem> Items { get; set; }
        public string Comment { get; set; }
    }
}