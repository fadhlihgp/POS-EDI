using System;
using System.Collections.Generic;

namespace POS_Backend.Entities;

public partial class PurchaseDetail
{
    public string Id { get; set; } = null!;

    public decimal Price { get; set; }

    public int Quantity { get; set; }

    public decimal Total { get; set; }

    public string PurchaseId { get; set; } = null!;

    public string ItemId { get; set; } = null!;

    public virtual Item Item { get; set; } = null!;

    public virtual Purchase Purchase { get; set; } = null!;
}
