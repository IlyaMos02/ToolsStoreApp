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
using ToolsStoreLab.Models.ConvertModel.NewModels;

namespace ToolsStoreLab.View
{
    /// <summary>
    /// Логика взаимодействия для ProductsDataView.xaml
    /// </summary>
    public partial class ProductsDataView : Page
    {
        ProductRepository PRepository;
        ProductModel _currentProductModel = new ProductModel();

        public ProductsDataView(ProductModel selectedProduct)
        {
            InitializeComponent();

            _currentProductModel = selectedProduct;

            DataContext = _currentProductModel;

            if(!MainWindow.Instance.User.UserId.Equals(1))
            {
                btnEdit.Visibility = Visibility.Hidden;
                btnDelete.Visibility = Visibility.Hidden;
            }
        }       

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            MyContentControl.GetFrame().Content = new AddEditProductView(_currentProductModel);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            using(ToolsStoreLabContext _context = new ToolsStoreLabContext())
            {
                if (MessageBox.Show("Видалити товар?", "Увага", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    PRepository = new ProductRepository(_context);                    
                    PRepository.DeleteProduct(_currentProductModel.ProductId);
                    MessageBox.Show("Товар видален");
                }                
            }
            MyContentControl.GetFrame().Content = ProductView.Instance;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            MyContentControl.GetFrame().Content = ProductView.Instance;
        }
    }
}
