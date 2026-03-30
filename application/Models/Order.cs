using System;
using System.Collections.Generic;

namespace ShoesShop.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public DateOnly OrderDate { get; set; }

    public DateOnly DeliveryDate { get; set; }

    public int PickupPointId { get; set; }

    public int ClientId { get; set; }

    public string CodeToReceive { get; set; } = null!;

    public string OrderStatus { get; set; } = null!;

    public virtual User Client { get; set; } = null!;

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual PickupPoint PickupPoint { get; set; } = null!;
}
