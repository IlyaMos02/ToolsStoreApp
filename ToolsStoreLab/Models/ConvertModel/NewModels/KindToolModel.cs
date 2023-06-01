using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolsStoreLab.Models.ConvertModel.NewModels
{
    public  class KindToolModel
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public double Power { get; set; }
        public string EnergySource { get; set; }

        public override string ToString()
        {
            return string.Format($"Category - {Category}, Power - {Power}, Energy source - {EnergySource}");
        }
    }
}
