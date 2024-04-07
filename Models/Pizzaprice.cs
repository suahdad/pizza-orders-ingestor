using System;
using System.Collections.Generic;

namespace pizza_orders_ingestor;

public partial class Pizzaprice
{
    public string Id { get; set; } = null!;

    public string? PizzaId { get; set; }

    public string? Size { get; set; }

    public double? Price { get; set; }
}
