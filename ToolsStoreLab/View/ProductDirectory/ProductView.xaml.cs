using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using ToolsStoreLab._Repository;
using ToolsStoreLab.Emails;
using ToolsStoreLab.Models;
using ToolsStoreLab.Models.ConvertModel;
using ToolsStoreLab.Models.ConvertModel.NewModels;

namespace ToolsStoreLab.View
{
    /// <summary>
    /// Логика взаимодействия для ProductView.xaml
    /// </summary>
    public partial class ProductView : Page
    {        
        private ToolsStoreLabContext _context;
        private ProductRepository PRepository;
        private UserRepository URepository;
        private ConvertEntities converter;
        private List<ProductModel> UsersCart;

        private User User { get; set; }

        private static ProductView instance;

        public static ProductView Instance
        {
            get {
                if (instance == null)
                    instance = new ProductView();

                return instance;
            }
        }

        public static ProductView GetInstance(User user)
        {
            Instance.User = user;
            Instance.UsersCart.Clear();
            

            if (Instance.User is not null)
            {
                if(Instance.User.Cart is not null)
                    Instance.UsersCart = JsonSerializer.Deserialize(Instance.User.Cart, typeof(List<ProductModel>)) as List<ProductModel>;

                if (Instance.User.RoleId.Equals(1))
                    Instance.AddProduct.Visibility = Visibility.Hidden;
                else
                    Instance.AddProduct.Visibility = Visibility.Visible;
            }

            return Instance;
        }

        public ProductView()
        {
            InitializeComponent();

            _context = new ToolsStoreLabContext();
            PRepository = new ProductRepository(_context);
            URepository = new UserRepository(_context);
            converter = new ConvertEntities(_context);
            UsersCart = new List<ProductModel>();

            FillFilterComboBoxes();                     
        }

        private void FillFilterComboBoxes()
        {
            var items = new List<string>() { string.Empty };
            comboCategoryFilter.ItemsSource = items.Union(_context.Categories.Select(c => c.Title).ToList());
            comboKindFilter.ItemsSource = items.Union(_context.Kinds.Select(k => k.Title).ToList());
            comboEnergySourceFilter.ItemsSource = items.Union(_context.EnergySources.Select(es => es.Source).ToList());
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            MyContentControl.GetFrame().Content = new AddEditProductView(null);
        }       

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if(Visibility == Visibility.Visible)
            {
                _context.ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                Products.ItemsSource = PRepository.GetProductModels();
                FillFilterComboBoxes();                
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string searchStr = txtSearch.Text;

            Products.ItemsSource = PRepository.GetProductsByString(searchStr).Select(p => converter.GetProductModel(p)).ToList();
        }

        private void comboFilter(object sender, EventArgs e)
        {
            string filterStr = (sender as ComboBox).Text;

            if((sender as ComboBox) == comboCategoryFilter)
            {
                comboEnergySourceFilter.SelectedIndex = 0;
                comboKindFilter.SelectedIndex = 0;
                Products.ItemsSource = PRepository.GetProductsByCategory(filterStr).Select(p => converter.GetProductModel(p)).ToList();
            }
            else if((sender as ComboBox) == comboEnergySourceFilter)
            {
                comboKindFilter.SelectedIndex = 0;
                comboCategoryFilter.SelectedIndex = 0;
                Products.ItemsSource = PRepository.GetProductsByEnergySource(filterStr).Select(p => converter.GetProductModel(p)).ToList();
            }
            else if((sender as ComboBox) == comboKindFilter)
            {
                comboCategoryFilter.SelectedIndex = 0;
                comboEnergySourceFilter.SelectedIndex = 0;
                Products.ItemsSource = PRepository.GetProductsByKind(filterStr).Select(p => converter.GetProductModel(p)).ToList();
            }

            Sender emailSender = new Sender(User);
            emailSender.SendFilteredProducts((List<ProductModel>)Products.ItemsSource);
        }

        private void btnLike_Click(object sender, RoutedEventArgs e)
        {
            var likedProduct = (sender as Button).DataContext as ProductModel;
            likedProduct.Grade++;
            PRepository.UpsertProduct(likedProduct);
            (sender as Button).IsEnabled = false;
        }

        private void Products_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ProductModel selectedProduct = (sender as ListView).SelectedItem as ProductModel;

            if (selectedProduct is null)
                return;

            MyContentControl.GetFrame().Content = new ProductsDataView(selectedProduct);
        }

        private void Cart_Click(object sender, RoutedEventArgs e)
        {
            if (User.Cart is null)
                MessageBox.Show("Кошик порожній");
            else
            {
                MyContentControl.GetFrame().Content = new CartView(User);
            }
        }

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            if (Products.SelectedItem == null)
                return;
            if((Products.SelectedItem as ProductModel).Count == 0)
            {
                MessageBox.Show("Товару немає на складі");
                return;
            }

            ProductModel selectedProduct = Products.SelectedItem as ProductModel;

            if (UsersCart.Find(uc => uc.ProductId == selectedProduct.ProductId) is null)
            {
                UsersCart.Add(selectedProduct);
                User.Cart = JsonSerializer.Serialize(UsersCart, UsersCart.GetType());
                URepository.Update(User);
            }
            else
                MessageBox.Show("Товар вже в кошику");
        }
    }
}
