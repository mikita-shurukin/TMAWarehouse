namespace WebMvc.Models.Responses;

public class WarehouseItemResponse
{
    public int WarehouseId { get; set; }
    public int ItemId { get; set; }
    public int GroupId { get; set; }
    public string GroupName { get; set; }
    //TODO: поразмыслить над типом данных
    public string UnitOfMeasurement { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public string StorageLocation { get; set; }
    public string ContactPerson { get; set; }
    public string PhotoUrl { get; set; }
}
