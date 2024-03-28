using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebMvc.DAL.Models;

[Table("ItemGroups")]
public class ItemGroup
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
}
