using System;
using System.Collections.Generic;

namespace ToolsStoreLab.Models;

public partial class Characteristic
{
    public int CharacteristicId { get; set; }

    public int KindToolId { get; set; }

    public double Weight { get; set; }

    public virtual KindTool KindTool { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
