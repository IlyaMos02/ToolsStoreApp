using System;
using System.Collections.Generic;

namespace ToolsStoreLab.Models.ConvertModel.NewModels;

public partial class ProductModel
{
    public int ProductId { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Kind { get; set; }

    public CharacteristicModel Characteristic { get; set; }

    public double Price { get; set; }

    public int Count { get; set; }

    public double Grade { get; set; }

    public override string ToString()
    {
        return string.Format($"Title - {Title}, Description - {Description}, Kind - {Kind}, Characteristic - {Characteristic.ToString()}, " +
            $"Price - {Price}, Count - {Count}, Grade - {Grade}");
    }
}
