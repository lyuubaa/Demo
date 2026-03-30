using System;
using System.Collections.Generic;

namespace ShoesShop.Models;

public partial class PickupPoint
{
    public int PickupPointId { get; set; }

    public string PickupPointIndex { get; set; } = null!;

    public string PickupPointCity { get; set; } = null!;

    public string PickupPointStreet { get; set; } = null!;

    public string PickupPointHouse { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public string DisplayAddress => (string)($"{PickupPointIndex}, {PickupPointCity}, {PickupPointStreet}, {PickupPointHouse}");
}
