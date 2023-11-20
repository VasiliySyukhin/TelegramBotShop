using System;
using System.Collections.Generic;

namespace Telegram_Bot_Project.Categories;

public partial class Categories2
{
    public decimal Id { get; set; }

    public string Categories { get; set; } = null!;

    public string ProductName { get; set; } = null!;

    public decimal Price { get; set; }

    public string LoginPassword { get; set; } = null!;
}
