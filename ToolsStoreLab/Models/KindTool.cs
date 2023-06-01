using System;
using System.Collections.Generic;

namespace ToolsStoreLab.Models;

public partial class KindTool
{
    public int KindToolId { get; set; }

    public int CategoryId { get; set; }

    public double Power { get; set; }

    public int EnergySourceId { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<Characteristic> Characteristics { get; set; } = new List<Characteristic>();

    public virtual EnergySource EnergySource { get; set; } = null!;
}
