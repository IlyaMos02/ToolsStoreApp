using System;
using System.Collections.Generic;

namespace ToolsStoreLab.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<KindTool> KindTools { get; set; } = new List<KindTool>();
}
