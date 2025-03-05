using System;
using System.Collections.Generic;

namespace StockWatch.Models;

public partial class StockMovement
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int FromWarehouseId { get; set; }

    public int ToWarehouseId { get; set; }

    public int Quantity { get; set; }

    public string MovementType { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public int CreatedBy { get; set; }

    public virtual Warehouse? FromWarehouse { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual Warehouse? ToWarehouse { get; set; }
}
