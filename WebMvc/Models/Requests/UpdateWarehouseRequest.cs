namespace WebMvc.Models.Requests;

public class UpdateWarehouseRequest
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string StorageLocation { get; set; }
    public string ContactPerson { get; set; }
}