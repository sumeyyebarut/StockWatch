using System;
using System.Collections.Generic;

namespace StockWatch.Models;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int CategoryId { get; set; }

    public string Barcode { get; set; } = null!;

    public bool? IsActive { get; set; }

    public int CreatedByUserId { get; set; }

    public int? UpdatedByUserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<ProductStock> ProductStocks { get; set; } = new List<ProductStock>();

    public virtual ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();
}
