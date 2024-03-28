namespace WebMvc.Models.Requests;

public class CreateWarehouseRequest
{
    public string Name { get; set; }
    public string StorageLocation { get; set; }
    public string ContactPerson { get; set; }
}