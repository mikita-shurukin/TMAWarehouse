using System.Text.Json.Serialization;

namespace WebMvc.Models.Requests.ItemGroup
{
    public class CreateItemGroupRequest
    {
        [JsonIgnore]
        public string? Name { get; set; }
    }
}
