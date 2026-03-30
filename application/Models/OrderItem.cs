using System;
using System.Collections.Generic;

namespace ShoesShop.Models;

public partial class OrderItem
{
    public int OrderItemId { get; set; }

    public int OrderId { get; set; }

    public string TovarArticle { get; set; } = null!;

    public int TovarQuantity { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Tovar TovarArticleNavigation { get; set; } = null!;
}
