using System;
using System.Collections.Generic;

namespace AapkaStore.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int? UserFid { get; set; }

    public DateTime? Date { get; set; }

    public TimeSpan? Time { get; set; }

    public string? Details { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual User? UserF { get; set; }
}
