using System;
using System.Collections.Generic;

namespace ShoesShop.Models;

public partial class Tovar
{
    public string TovarArticle { get; set; } = null!;

    public string TovarName { get; set; } = null!;

    public decimal TovarPrice { get; set; }

    public string? UnitOfMeasurement { get; set; }

    public int SupplierId { get; set; }

    public int ManufacturerId { get; set; }

    public int CategoryId { get; set; }

    public decimal CurrentDiscount { get; set; }

    public int QuantityInStock { get; set; }

    public string TovarDescription { get; set; } = null!;

    public string? Photo { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual Manufacture Manufacturer { get; set; } = null!;

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual Supplier Supplier { get; set; } = null!;
    public string DisplayedImage => (string)("../Resources/" + ((Photo == null || string.IsNullOrEmpty(Photo)) ? "picture.png" : Photo));

}
