using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebMvc.DAL.Models;

[Table("OrderPositions")]
public class OrderPosition
{
    [Required]
    public int OrderId { get; set; }
    public virtual Order Order { get; set; }
    [Required]
    public int WarehouseId { get; set; }
    public virtual Warehouse Warehouse { get; set; }
    [Required]
    public int ItemId { get; set; }
    public virtual Item Item { get; set; }
    [Required]
    public string UnitOfMeasurement { get; set; }
    [Required]
    public int Quantity { get; set; }
    public string FormattedPrice { get; set; }
}
