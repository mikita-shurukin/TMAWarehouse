namespace WebMvc.Models.Requests.Orders
{
    public class RemoveOrderItemsRequest
    {
        public int OrderId { get; set; }
        public List<OrderItem> Items { get; set; }
    }
}
