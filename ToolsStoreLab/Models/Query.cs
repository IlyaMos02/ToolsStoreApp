using System;
using System.Collections.Generic;

namespace ToolsStoreLab.Models;

public partial class Query
{
    public int QueryId { get; set; }

    public string QueryText { get; set; } = null!;

    public DateTime Date { get; set; }

    public string? UserEmail { get; set; }
}
