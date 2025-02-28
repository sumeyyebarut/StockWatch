using System;
using System.Collections.Generic;

namespace StockWatch.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public bool? IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Category> CategoryCreatedByUsers { get; set; } = new List<Category>();

    public virtual ICollection<Category> CategoryUpdatedByUsers { get; set; } = new List<Category>();

    public virtual ICollection<Product> ProductCreatedByUsers { get; set; } = new List<Product>();

    public virtual ICollection<Product> ProductUpdatedByUsers { get; set; } = new List<Product>();

    public virtual ICollection<StockMovement> StockMovementCreatedByUsers { get; set; } = new List<StockMovement>();

    public virtual ICollection<StockMovement> StockMovementUpdatedByUsers { get; set; } = new List<StockMovement>();

    public virtual ICollection<Warehouse> WarehouseCreatedByUsers { get; set; } = new List<Warehouse>();

    public virtual ICollection<Warehouse> WarehouseUpdatedByUsers { get; set; } = new List<Warehouse>();
}
