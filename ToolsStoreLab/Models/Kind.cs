using System;
using System.Collections.Generic;

namespace ToolsStoreLab.Models;

public partial class Kind
{
    public int KindId { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
