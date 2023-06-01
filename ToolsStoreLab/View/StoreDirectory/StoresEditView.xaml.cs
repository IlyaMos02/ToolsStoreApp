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
    /// Логика взаимодействия для StoresEditView.xaml
    /// </summary>
    public partial class StoresEditView : Page
    {
        private ToolsStoreLabContext _context;
        private StoreRepository SRepository;
        private StoreModel _currentStoreModel;

        public StoresEditView(StoreModel storeModel)
        {
            InitializeComponent();

            _context = new ToolsStoreLabContext();
            SRepository = new StoreRepository(_context);

            _currentStoreModel = storeModel;

            DataContext = _currentStoreModel;

            comboWorkTime.ItemsSource = _context.WorkTimes.Select(wk => wk.Time).ToList();
            comboElectricity.ItemsSource = new List<string>() { "Yes", "No" };
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SRepository.Update(_currentStoreModel);
            MessageBox.Show("Збереження успішне!");
            MyContentControl.GetFrame().Content = StoresView.Instance;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            MyContentControl.GetFrame().Content = StoresView.Instance;
        }
    }
}
