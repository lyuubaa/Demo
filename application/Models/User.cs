using System;
using System.Collections.Generic;

namespace ShoesShop.Models;

public partial class User
{
    public int UserId { get; set; }

    public string UserRole { get; set; } = null!;

    public string UserSurname { get; set; } = null!;

    public string UserFirstname { get; set; } = null!;

    public string UserLastname { get; set; } = null!;

    public string UserEmail { get; set; } = null!;

    public string UserPassword { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
