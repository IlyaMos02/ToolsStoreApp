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
using ToolsStoreLab.Models.ConvertModel;
using ToolsStoreLab.Models.ConvertModel.NewModels;

namespace ToolsStoreLab.View
{
    /// <summary>
    /// Логика взаимодействия для StoresView.xaml
    /// </summary>
    public partial class StoresView : Page
    {
        private ToolsStoreLabContext _context;
        private StoreRepository SRepository;
        private ConvertEntities converter;
        private User User { get; set; }
        private static StoresView _instance;

        public static StoresView Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new StoresView();

                return _instance;
            }
        }

        public static StoresView GetInstance(User user)
        {
            Instance.User = user;

            return Instance;
        }

        public StoresView()
        {
            InitializeComponent();

            _context = new ToolsStoreLabContext();
            SRepository = new StoreRepository(_context);
            converter = new ConvertEntities(_context);

            var items = new List<string>() { string.Empty };
            comboWorkTimeFilter.ItemsSource = items.Union(_context.WorkTimes.Select(w => w.Time).ToList());
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string searchStr = txtSearch.Text;

            Stores.ItemsSource = SRepository.GetStoresByString(searchStr).Select(s => converter.GetStoreModel(s)).ToList();
        }

        private void comboFilter_DropDownClosed(object sender, EventArgs e)
        {
            string filterStr = (sender as ComboBox).Text;

            Stores.ItemsSource = SRepository.GetStoresByString(filterStr).Select(s => converter.GetStoreModel(s)).ToList();
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                _context.ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                Stores.ItemsSource = SRepository.GetStores();
            }
        }

        private void Stores_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            StoreModel selectedStore = (sender as ListView).SelectedItem as StoreModel;

            if(selectedStore is null)
                return;

            MyContentControl.GetFrame().Content = new StoresDataView(selectedStore);
        }
    }
}
