namespace WebMvc.Models.Requests;

public class UpdateItemRequest
{
    public int Id { get; set; }
    public int GroupId { get; set; }
    public decimal Price { get; set; }
    public string PhotoUrl { get; set; }
}