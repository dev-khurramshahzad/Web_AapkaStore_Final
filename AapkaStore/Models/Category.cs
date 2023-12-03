using System;
using System.Collections.Generic;

namespace AapkaStore.Models;

public partial class Category
{
    public int CatId { get; set; }

    public string Name { get; set; } = null!;

    public string? Image { get; set; }

    public string? Details { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();
}
