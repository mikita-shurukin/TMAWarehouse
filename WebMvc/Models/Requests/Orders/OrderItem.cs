namespace WebMvc.Models.Requests.Orders
{
    public class OrderItem
    {
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public string UnitOfMeasurement { get; set; }
        public int WarehouseId { get; set; }
    }
}
