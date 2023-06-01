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
using ToolsStoreLab.Emails;
using ToolsStoreLab.Models;

namespace ToolsStoreLab.View.AutRegView
{
    /// <summary>
    /// Логика взаимодействия для AutorizationWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window
    {
        private UserRepository URepository;

        private static AuthorizationWindow instance;

        public static AuthorizationWindow Instance
        {
            get
            {
                if (instance is null)
                    instance = new AuthorizationWindow();

                return instance;
            }
        }

        public AuthorizationWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            using (ToolsStoreLabContext _context = new ToolsStoreLabContext())
            {
                URepository = new UserRepository(_context);

                string email = txtEmail.Text;
                string password = txtPassword.Password;

                var user = URepository.GetUser(email, password);

                if (user is null)
                    MessageBox.Show("Користувач не знайден");
                else
                {
                    MainWindow.GetInstance(user).Show();
                    this.Hide();
                }
            }         
        }

        private void btnRegistr_Click(object sender, RoutedEventArgs e)
        {
            RegistrationWindow.Instance.Show();
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
