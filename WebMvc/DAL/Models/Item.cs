using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebMvc.DAL.Models;

[Table("Items")]
public class Item
{
    public int Id { get; set; }
    [Required]
    public int GroupId { get; set; }
    public virtual ItemGroup Group { get; set; }

    [Required]
    [Precision(18, 2)]
    public decimal Price { get; set; }
    [Required(AllowEmptyStrings = true)]
    public string PhotoUrl { get; set; }
}