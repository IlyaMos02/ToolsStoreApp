using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ToolsStoreLab._Repository;
using ToolsStoreLab.Models;

namespace ToolsStoreLab.View.AutRegView
{
    /// <summary>
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        UserRepository URepository;

        private static RegistrationWindow instance;
        public static RegistrationWindow Instance
        {
            get
            {
                if (instance is null)
                    instance = new RegistrationWindow();

                return instance;
            }
        }

        public RegistrationWindow()
        {
            InitializeComponent();
        }

        private void btnRegistr_Click(object sender, RoutedEventArgs e)
        {
            using(ToolsStoreLabContext _context = new ToolsStoreLabContext())
            {
                URepository = new(_context);

                if (txtPassword.Password != txtRepeatPass.Password)
                {
                    MessageBox.Show("Повторіть пароль правильно");
                    return;
                }

                var user = new User()
                {
                    Name = txtName.Text,
                    Surname = txtSurname.Text,
                    Email = txtEmail.Text,
                    Phone = txtPhone.Text,
                    Password = txtPassword.Password
                };

                var resultOfValidation = ValidateUser.ValidateRegistration(user);

                if (!resultOfValidation.Item1)
                {
                    MessageBox.Show(resultOfValidation.Item2);
                    return;
                }
                else
                {
                    var result = URepository.Add(user);

                    if (result)
                    {
                        MessageBox.Show("Регістрація успішна");
                        MainWindow.GetInstance(user).Show();
                        this.Hide();
                        AuthorizationWindow.Instance.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Користувач вже існує");
                        return;
                    }
                }
            }           
        }

        private void btnLogIn_Click(object sender, RoutedEventArgs e)
        {
            AuthorizationWindow.Instance.Show();
            this.Hide();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Backup backup = new Backup();
            backup.MakeBackup(backup.AutoArchivePath, backup.AutoArchiveName);

            Close();
            Environment.Exit(0);
        }
    }
}
