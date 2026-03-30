using System;
using System.Collections.Generic;

namespace ShoesShop.Models;

public partial class Manufacture
{
    public int ManufacturerId { get; set; }

    public string ManufacturerName { get; set; } = null!;

    public virtual ICollection<Tovar> Tovars { get; set; } = new List<Tovar>();
}
