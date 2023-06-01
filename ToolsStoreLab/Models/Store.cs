using System;
using System.Collections.Generic;

namespace ToolsStoreLab.Models;

public partial class Store
{
    public int StoreId { get; set; }

    public string Adress { get; set; } = null!;

    public int WorkTimeId { get; set; }

    public string WorkWithoutElectricity { get; set; } = null!;

    public virtual WorkTime WorkTime { get; set; } = null!;
}
