using System;
using System.Collections.Generic;

namespace ToolsStoreLab.Models;

public partial class EnergySource
{
    public int EnergySourceId { get; set; }

    public string Source { get; set; } = null!;

    public virtual ICollection<KindTool> KindTools { get; set; } = new List<KindTool>();
}
