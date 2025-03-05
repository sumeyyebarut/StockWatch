using System;
using System.Collections.Generic;

namespace StockWatch.Models;

public partial class Warehouse
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Location { get; set; } = null!;

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public int CreatedByUserId { get; set; }

    public int? UpdatedByUserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<ProductStock> ProductStocks { get; set; } = new List<ProductStock>();

    public virtual ICollection<StockMovement> StockMovementFromWarehouses { get; set; } = new List<StockMovement>();

    public virtual ICollection<StockMovement> StockMovementToWarehouses { get; set; } = new List<StockMovement>();
}
