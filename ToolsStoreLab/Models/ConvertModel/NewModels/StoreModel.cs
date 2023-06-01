using System;
using System.Collections.Generic;

namespace ToolsStoreLab.Models.ConvertModel.NewModels;

public partial class StoreModel
{
    public int StoreId { get; set; }

    public string Adress { get; set; } = null!;

    public string WorkTime { get; set; } = null!;

    public string WorkWithoutElectricity { get; set; } = null!;
}
