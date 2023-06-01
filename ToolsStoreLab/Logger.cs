using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ToolsStoreLab.Models;

namespace ToolsStoreLab
{
    public static class Logger
    {
        private static string path = @"C:\Users\GameMax\source\repos\ToolsStoreLab\ToolsStoreLab\log.txt";

        public static void Log(string command, object model)
        {
            string text = command + "\n" + JsonSerializer.Serialize(model, model.GetType(), new JsonSerializerOptions() 
            { ReferenceHandler = ReferenceHandler.Preserve, WriteIndented = true}) + " " + DateTime.Now;

            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine(text);
            }
        }

        public static void Log(string command)
        {
            string text = command + " " + DateTime.Now;

            using(StreamWriter writer = new StreamWriter(path,true))
            {
                writer.WriteLine(text);
            }
        }

        /*public static void LogProduct(string command, Product product)
        {
            string text = command + "\n" + JsonSerializer.Serialize(product) + DateTime.Now;

            using(StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine(text);
            }
        }

        public static void LogCharacteristic(string command, Characteristic characteristic)
        {
            string text = command + "\n" + JsonSerializer.Serialize(characteristic) + DateTime.Now;

            using(StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine(text);
            }
        }*/
    }
}
