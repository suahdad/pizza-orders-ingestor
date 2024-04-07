using System;
using System.Collections.Generic;

namespace pizza_orders_ingestor;

public partial class Orderdetail
{
    public uint Id { get; set; }

    public uint? OrderId { get; set; }

    public string? PizzaId { get; set; }

    public byte? Quantity { get; set; }

    public double? Price { get; set; }
}
