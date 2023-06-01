using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ToolsStoreLab.Models;

namespace ToolsStoreLab.View.AutRegView
{
    public class ValidateUser
    {
        private static string RegexEmail = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
        private static string RegexPhone = @"^\+380\d{9}$";
        private static string RegexPassword = @"^(?=.*[A-Z])(?=.*\d).{8,}$";

        public static (bool, string) ValidateRegistration(User user)
        {
            StringBuilder message = new StringBuilder();
            bool result = true;

            if(user.Name.Trim() == string.Empty || user.Name.Length <=3 )
            {
                message.AppendLine("Name too short");
                result = false;
            }
            if(user.Surname.Trim() == string.Empty || user.Surname.Length <=3 )
            {
                message.AppendLine("Surname too short");
                result = false;
            }
            if(!Regex.IsMatch(user.Email, RegexEmail))
            {
                message.AppendLine("Invalid email adress");
                result = false;
            }
            if(!Regex.IsMatch(user.Phone, RegexPhone))
            {
                message.AppendLine("Invalid phone number");
                result = false;
            }
            if (!Regex.IsMatch(user.Password, RegexPassword))
            {
                message.AppendLine("Password must contain 8 letters where must be\nat least one big letter and one number");
                result = false;
            }

            return (result, message.ToString());
        }

        public static (bool, string) ValidateChangeData(User changedUser, User user)
        {
            StringBuilder message = new StringBuilder();
            bool result = true;

            if(changedUser.Name == user.Name && changedUser.Surname == user.Surname && changedUser.Email == user.Email && changedUser.Phone == user.Phone)
            {
                message.AppendLine("You must change at least one field to save");
                result = false;
            }

            var resultValidation = ValidateRegistration(changedUser);
            if(!resultValidation.Item1)
            {
                message.AppendLine(resultValidation.Item2);
                result = false;
            }

            return (result, message.ToString());
        }
    }
}
