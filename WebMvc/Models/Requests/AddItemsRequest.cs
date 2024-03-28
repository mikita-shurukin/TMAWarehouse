namespace WebMvc.Models.Requests;

public class AddItemsRequest
{
    public int WarehouseId { get; set; }
    public IEnumerable<ItemPosition> ItemPositions { get; set; }
}