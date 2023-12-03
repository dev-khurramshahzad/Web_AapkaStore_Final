using System;
using System.Collections.Generic;

namespace AapkaStore.Models;

public partial class Item
{
    public int ItemId { get; set; }

    public string Name { get; set; } = null!;

    public int? CatFid { get; set; }

    public double SalePrice { get; set; }

    public double CostPrice { get; set; }

    public string? Image1 { get; set; }

    public string? Image2 { get; set; }

    public string? Type { get; set; }

    public int Quantity { get; set; }

    public int? Rating { get; set; }

    public string? Details { get; set; }

    public string? Status { get; set; }

    public virtual Category? CatF { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
