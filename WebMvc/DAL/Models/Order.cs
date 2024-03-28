using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebMvc.Common;

namespace WebMvc.DAL.Models;

[Table("Orders")]
public class Order
{
    public int Id { get; set; }
    [Required]
    public OrderStatus Status { get; set; }
    public virtual IEnumerable<OrderPosition> OrderPositions { get; set; }

    [Required(AllowEmptyStrings = true)]
    public string Comment { get; set; }
}

