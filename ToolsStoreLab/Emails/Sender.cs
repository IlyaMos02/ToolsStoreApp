using FluentEmail.Core;
using FluentEmail.Core.Models;
using FluentEmail.Razor;
using FluentEmail.Smtp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Windows;
using ToolsStoreLab.Models;
using ToolsStoreLab.Models.ConvertModel.NewModels;
using ToolsStoreLab.View;

namespace ToolsStoreLab.Emails
{
    public class Sender
    {
        SmtpSender sender;
        SendResponse email;
        User user;

        private string _applicationEmail = "ilyamos@gmail.com";
        private string _attachFileDirectory = Directory.GetCurrentDirectory() + @"\SendEmail.txt";

        public Sender(User user)
        {
            this.user = user;

            sender = new SmtpSender(new SmtpClient("localhost")
            {
                EnableSsl = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Port = 25
            });
            Email.DefaultSender = sender;
        }

        public void SendFilteredProducts(IEnumerable<ProductModel> products)
        {
            StringBuilder template = new StringBuilder();
            template.AppendLine($"Dear {user.Name},");
            template.AppendLine("This is file, which contains your filtered products");
            template.AppendLine("- The ToolsStore Team");

            CreateFileWithData(products);          

            email = Email
                .From(_applicationEmail)
                .To(user.Email)
                .Subject("Results")
                .AttachFromFilename(_attachFileDirectory)
                .Body(template.ToString())
                .Send();

            MessageBox.Show(email.Successful.ToString());
        }

        public void SendPurchasedProducts(IEnumerable<CartItem> products)
        {
            StringBuilder template = new StringBuilder();
            template.AppendLine($"Dear {user.Name},");
            template.AppendLine("Thanks you for purchase in our store, this is your product list:");
            foreach (var product in products)
                template.AppendLine(product.ToString());
            template.AppendLine("- The ToolsStore Team");

            email = Email
                .From(_applicationEmail)
                .To(user.Email)
                .Subject("Results")
                .Body(template.ToString())
                .Send();

            MessageBox.Show(email.Successful.ToString());
        }

        public void SendSearchedText(IEnumerable<string> text)
        {
            StringBuilder template = new StringBuilder();
            template.AppendLine($"Dear {user.Name},");
            template.AppendLine("This file contains paragraph with your request");
            template.AppendLine("- The ToolsStore Team");

            CreateFileWithData(text);

            email = Email
                .From(_applicationEmail)
                .To(user.Email)
                .Subject("Results")
                .AttachFromFilename(_attachFileDirectory)
                .Body(template.ToString())
                .Send();

            MessageBox.Show(email.Successful.ToString());
        }

        private void CreateFileWithData(IEnumerable<object> objects)
        {
            if(File.Exists(_attachFileDirectory))
            {
                File.Delete(_attachFileDirectory);
            }

            using(FileStream stream = File.Create(_attachFileDirectory))
            {
                StringBuilder template = new StringBuilder();
                foreach (var item in objects)
                    template.AppendLine(item.ToString());

                byte[] text = Encoding.UTF8.GetBytes(template.ToString());
                stream.Write(text, 0, text.Length);
            }
        }
    }
}
