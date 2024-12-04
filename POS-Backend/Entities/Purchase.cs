using System;
using System.Collections.Generic;

namespace POS_Backend.Entities;

public partial class Purchase
{
    public string Id { get; set; } = null!;

    public DateTime Date { get; set; }

    public string Invoice { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<PurchaseDetail> PurchaseDetails { get; set; } = new List<PurchaseDetail>();
}
