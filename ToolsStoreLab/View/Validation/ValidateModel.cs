using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolsStoreLab.Models.ConvertModel.NewModels;

namespace ToolsStoreLab.View.Validation
{
    public static class ValidateModel
    {
        public static (bool, string) ValidateProduct(ProductModel productModel)
        {
            StringBuilder message = new StringBuilder();
            bool result = true;            

            var charact = ValidateCharacteristic(productModel.Characteristic);

            if (string.IsNullOrEmpty(productModel.Title) || productModel.Title.Length < 3)
            {
                message.AppendLine("Title too short");
                result = false;
            }
            if (string.IsNullOrEmpty(productModel.Kind))
            {
                message.AppendLine("Kind is required");
                result = false;
            }
            if (productModel.Price <= 0)
            {
                message.AppendLine("Price is required");
                result = false;
            }
            if (!charact.Item1)
            {
                message.AppendLine(charact.Item2);
                result = false;
            }
            if (productModel.Count <= 0)
            {
                message.AppendLine("Count of products too small");
                result = false;
            }

            return(result, message.ToString());
        }

        public static (bool, string) ValidateCharacteristic(CharacteristicModel characteristicModel)
        {
            StringBuilder message = new StringBuilder();
            bool result = true;            

            if (string.IsNullOrEmpty(characteristicModel.KindTool.Category))
            {
                message.AppendLine("Category is required");
                result = false;
            }
            if (double.IsNaN(characteristicModel.KindTool.Power))
            {
                message.AppendLine("Power is required");
                result = false;
            }
            if (string.IsNullOrEmpty(characteristicModel.KindTool.EnergySource))
            {
                message.AppendLine("Energy source is required");
                result = false;
            }
            if (characteristicModel.Weight <= 0)
            {
                message.AppendLine("Weight is required");
                result = false;
            }            

            return(result, message.ToString());
        }
    }
}
