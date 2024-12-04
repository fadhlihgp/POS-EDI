using System;
using System.Collections.Generic;

namespace POS_Backend.Entities;

public partial class Item
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public int Stok { get; set; }

    public string? PictureUrl { get; set; }

    public bool IsDeleted { get; set; }

    public string CategoryId { get; set; } = null!;

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<PurchaseDetail> PurchaseDetails { get; set; } = new List<PurchaseDetail>();
}
