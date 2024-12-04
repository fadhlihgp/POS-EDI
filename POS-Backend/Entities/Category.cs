using System;
using System.Collections.Generic;

namespace POS_Backend.Entities;

public partial class Category
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();
}
