using System;
using System.Collections.Generic;

namespace ToolsStoreLab.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int KindId { get; set; }

    public int CharacteristicId { get; set; }

    public double Price { get; set; }

    public int Count { get; set; }

    public double Grade { get; set; }

    public virtual Characteristic Characteristic { get; set; } = null!;

    public virtual Kind Kind { get; set; } = null!;
}
