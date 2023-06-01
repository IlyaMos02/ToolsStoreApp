using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using ToolsStoreLab._Repository;
using ToolsStoreLab.Emails;
using ToolsStoreLab.Models;
using ToolsStoreLab.Models.ConvertModel.NewModels;

namespace ToolsStoreLab.View
{
    /// <summary>
    /// Логика взаимодействия для CartView.xaml
    /// </summary>
    public partial class CartView : Page
    {
        private User User;
        private ToolsStoreLabContext _context;
        private UserRepository URepository;
        private ProductRepository PRepository;
        private List<CartItem> cartProducts;
        public CartView(User user)
        {
            InitializeComponent();

            _context = new ToolsStoreLabContext();
            URepository = new UserRepository(_context);
            PRepository = new ProductRepository(_context);
            cartProducts = new List<CartItem>();

            this.User = user;

            FillUsersCart();
            Cart.ItemsSource = cartProducts;
        }

        private void FillUsersCart()
        {
            List<ProductModel> cart = JsonSerializer.Deserialize(User.Cart, typeof(List<ProductModel>)) as List<ProductModel>;

            foreach (var product in cart)
            {
                cartProducts.Add(new CartItem() { ProductModel = product, Count = 1 });
            }
        }

        private void btnBuy_Click(object sender, RoutedEventArgs e)
        {
            foreach(CartItem item in Cart.ItemsSource)
            {
                item.ProductModel.Count -= item.Count;
                PRepository.UpsertProduct(item.ProductModel);
            }
            User.Cart = null;
            URepository.Update(User);

            Sender emailSender = new Sender(User);
            emailSender.SendPurchasedProducts((List<CartItem>)Cart.ItemsSource);

            MessageBox.Show("Операція успішна");
            MyContentControl.GetFrame().Content = ProductView.GetInstance(User);
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            MyContentControl.GetFrame().Content = ProductView.GetInstance(User);
        }    

        private void txtNumberOfProd_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var result = int.TryParse((sender as TextBox).Text, out _) ? Convert.ToInt32((sender as TextBox).Text) : 1;

            if (result < 1)
                result = 1;

            var cartItemContext = (sender as TextBox).DataContext as CartItem;
            if (result > cartItemContext.ProductModel.Count)
                result = cartItemContext.ProductModel.Count;

            (sender as TextBox).Text = result.ToString();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if(Cart.SelectedItem is null)
            {
                MessageBox.Show("Оберіть товар");
                return;
            }
            cartProducts.Remove(Cart.SelectedItem as CartItem);

            List<ProductModel> cart = JsonSerializer.Deserialize(User.Cart, typeof(List<ProductModel>)) as List<ProductModel>;
            cart.Remove(cart.Find(c => c.ProductId == ((Cart.SelectedItem as CartItem).ProductModel).ProductId));
            User.Cart = JsonSerializer.Serialize(cart, cart.GetType());
            URepository.Update(User);

            ICollectionView view = CollectionViewSource.GetDefaultView(Cart.ItemsSource);
            view.Refresh();

            if(cart.Count == 0)
            {
                MessageBox.Show("Кошик порожній");
                MyContentControl.GetFrame().Content = ProductView.GetInstance(User);
            }
        }
    }

    public class CartItem
    {
        public ProductModel ProductModel { get; set; }
        public int Count { get; set; }

        public override string ToString()
        {
            return string.Format($"Product: {ProductModel.ToString()}, Count - {Count}");
        }
    }
}
