using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ToolsStoreLab._Repository;
using ToolsStoreLab.Models;
using ToolsStoreLab.Models.ConvertModel.NewModels;
using ToolsStoreLab.View.Validation;

namespace ToolsStoreLab.View
{
    /// <summary>
    /// Логика взаимодействия для AddProduct.xaml
    /// </summary>
    public partial class AddEditProductView : Page
    {       
        private ToolsStoreLabContext _context;
        private ProductRepository PRepository;

        private ProductModel _currentProductModel = new ProductModel();

        public AddEditProductView(ProductModel selectedProduct)
        {
            InitializeComponent();

            _context = new ToolsStoreLabContext();
            PRepository = new ProductRepository(_context);

            if (selectedProduct != null)
                _currentProductModel = selectedProduct;
            else
            {
                _currentProductModel.Characteristic = new CharacteristicModel();
                _currentProductModel.Characteristic.KindTool = new KindToolModel();
            }

            DataContext = _currentProductModel;
            
            comboCategory.ItemsSource = _context.Categories.Select(c => c.Title).ToList();
            comboKind.ItemsSource = _context.Kinds.Select(k => k.Title).ToList();
            comboEnergySource.ItemsSource = _context.EnergySources.Select(es => es.Source).ToList();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            MyContentControl.GetFrame().Content = ProductView.Instance;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var resultOfValidation = ValidateModel.ValidateProduct(_currentProductModel);

            if (!resultOfValidation.Item1)
                MessageBox.Show(resultOfValidation.Item2);
            else
            {
                PRepository.UpsertProduct(_currentProductModel);
                MessageBox.Show("Збереження успішне!");
                MyContentControl.GetFrame().Content = ProductView.Instance;
            }
        }
    }
}
