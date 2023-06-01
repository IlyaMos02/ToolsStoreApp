using System;
using System.Collections.Generic;

namespace ToolsStoreLab.Models;

public partial class WorkTime
{
    public int WorkTimeId { get; set; }

    public string Time { get; set; } = null!;

    public virtual ICollection<Store> Stores { get; set; } = new List<Store>();
}
