using System;
using System.Collections.Generic;

namespace ToolsStoreLab.Models.ConvertModel.NewModels;

public partial class CharacteristicModel
{
    public int CharacteristicId { get; set; }

    public KindToolModel KindTool { get; set; }

    public double Weight { get; set; }

    public override string ToString()
    {
        return string.Format($"Kind Tool - {KindTool.ToString()}, Weight - {Weight}");
    }
}
