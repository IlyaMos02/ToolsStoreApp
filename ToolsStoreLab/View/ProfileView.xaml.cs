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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ToolsStoreLab._Repository;
using ToolsStoreLab.Models;
using ToolsStoreLab.View.AutRegView;

namespace ToolsStoreLab.View
{
    /// <summary>
    /// Логика взаимодействия для ProfileView.xaml
    /// </summary>
    public partial class ProfileView : Page
    {
        UserRepository URepository;
        User User { get; set; }

        private static ProfileView instance;

        public static ProfileView Instance
        {
            get
            {
                if (instance == null)
                    instance = new ProfileView();

                return instance;
            }
        }

        public static ProfileView GetInstance(User user)
        {
            Instance.User = user;

            if (!Instance.User.RoleId.Equals(1))
                Instance.btnDeleteAccount.Visibility = Visibility.Hidden;
            else
                Instance.btnDeleteAccount.Visibility = Visibility.Visible;

            SetTextBoxesWithUserData();

            return instance;
        }

        public ProfileView()
        {
            InitializeComponent();                        
        }

        private static void SetTextBoxesWithUserData()
        {
            Instance.txtName.Text = Instance.User.Name;
            Instance.txtSurname.Text = Instance.User.Surname;
            Instance.txtPhone.Text = Instance.User.Phone;
        }

        private void ChangeEnableStateOfTextBoxes(bool state)
        {
            txtName.IsEnabled = state;
            txtSurname.IsEnabled = state;
            txtPhone.IsEnabled = state;
        }

        private void btnChangeData_Click(object sender, RoutedEventArgs e)
        {
            ChangeEnableStateOfTextBoxes(true);
            SaveCancelPanel.Visibility = Visibility.Visible;
        }

        private void btnSaveUserData_Click(object sender, RoutedEventArgs e)
        {
            using(ToolsStoreLabContext _context = new ToolsStoreLabContext())
            {
                URepository = new UserRepository(_context);

                User changedUser = new()
                {
                    Name = txtName.Text,
                    Surname = txtSurname.Text,
                    Email = User.Email,
                    Phone = txtPhone.Text,
                    Password = User.Password
                };

                var validationResult = ValidateUser.ValidateChangeData(changedUser, User);

                if (!validationResult.Item1)
                {
                    MessageBox.Show(validationResult.Item2);
                }
                else
                {
                    User.Name = changedUser.Name;
                    User.Surname = changedUser.Surname;
                    User.Email = changedUser.Email;
                    User.Phone = changedUser.Phone;

                    bool result = URepository.Update(User);

                    if (!result)
                    {
                        User = URepository.GetUser(User.Email, User.Password);
                    }


                    ChangeEnableStateOfTextBoxes(false);
                }

                SetTextBoxesWithUserData();
                SaveCancelPanel.Visibility = Visibility.Hidden;
            }           
        }

        private void btnCancelChanges_Click(object sender, RoutedEventArgs e)
        {
            SetTextBoxesWithUserData();
            ChangeEnableStateOfTextBoxes(false);
            SaveCancelPanel.Visibility = Visibility.Hidden;
        }

        private void btnDeleteAccount_Click(object sender, RoutedEventArgs e)
        {
            using (ToolsStoreLabContext _context = new ToolsStoreLabContext())
            {
                URepository = new UserRepository(_context);

                if (MessageBox.Show("Видалити ваш акаунт?", "Увага", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    URepository.Delete(User);
                    MainWindow.Instance.Close();
                    Environment.Exit(0);
                }
            }           
        }
    }
}
