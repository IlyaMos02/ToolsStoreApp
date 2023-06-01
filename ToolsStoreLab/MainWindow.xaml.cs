using System;
using System.Windows;
using ToolsStoreLab.Models;
using ToolsStoreLab.View;
using ToolsStoreLab.View.AutRegView;
using ToolsStoreLab.Models.ConvertModel.NewModels;
using System.Collections.Generic;
using ToolsStoreLab._Repository;
using System.Linq;

namespace ToolsStoreLab
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public User User;     

        private static MainWindow instance;

        public static MainWindow Instance
        {
            get
            {
                if(instance == null)
                    instance = new MainWindow();

                return instance;
            }
        }

        public static MainWindow GetInstance(User user)
        {         
            Instance.User = user;
            if (Instance.User.RoleId == 1)
                Instance.Settings.Visibility = Visibility.Hidden;
            else
            {
                Instance.Settings.Visibility = Visibility.Visible;
                ReplenishStock();
            }
                

            return Instance;
        }

        private static void ReplenishStock()
        {
            using(ToolsStoreLabContext _context = new ToolsStoreLabContext())
            {
                ProductRepository repository = new ProductRepository(_context);
                List<ProductModel> products = repository.GetProductModels().ToList();

                if (products.Find(p => p.Count == 0) is not null)
                {
                    Random randomCount = new Random();
                    foreach (var product in products)
                    {
                        if (product.Count == 0)
                        {
                            product.Count = randomCount.Next(10, 100);
                            repository.UpsertProduct(product);
                        }
                    }
                    System.Windows.MessageBox.Show("Products purchased");
                }
            }                                  
        }

        public MainWindow()
        {
            InitializeComponent();           

            AuthorizationWindow.Instance.Show();
            this.Hide();

            MyContentControl.SetFrame(this.MainFrame);            
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            AuthorizationWindow.Instance.Show();
            MyContentControl.GetFrame().Content = null;
            Hide();
        }        
       

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            MyContentControl.GetFrame().Content = ProductView.GetInstance(User);
        }

        private void Contuct_Click(object sender, RoutedEventArgs e)
        {
            MyContentControl.GetFrame().Content = new ContactView(User);
        }

        private void btnStores_Click(object sender, RoutedEventArgs e)
        {
            MyContentControl.GetFrame().Content = StoresView.GetInstance(User);
        }       

        private void btnProfile_Click(object sender, RoutedEventArgs e)
        {
            MyContentControl.GetFrame().Content = ProfileView.GetInstance(User);
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            MyContentControl.GetFrame().Content = SettingsView.Instanse;
        }
    }
}
