using System;
using System.Collections.Generic;

namespace StockWatch.Models;

public partial class StockMovement
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int WarehouseId { get; set; }

    public int Quantity { get; set; }

    public string MovementType { get; set; } = null!;

    public bool? IsActive { get; set; }

    public int CreatedByUserId { get; set; }

    public int? UpdatedByUserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual User CreatedByUser { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;

    public virtual User? UpdatedByUser { get; set; }

    public virtual Warehouse Warehouse { get; set; } = null!;
}
