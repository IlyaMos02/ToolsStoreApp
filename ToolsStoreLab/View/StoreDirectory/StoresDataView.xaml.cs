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
using ToolsStoreLab.Models.ConvertModel.NewModels;

namespace ToolsStoreLab.View
{
    /// <summary>
    /// Логика взаимодействия для StoresDataView.xaml
    /// </summary>
    public partial class StoresDataView : Page
    {
        StoreModel _currentStoreModel = new StoreModel();

        public StoresDataView(StoreModel selectedStore)
        {
            InitializeComponent();

            _currentStoreModel = selectedStore;

            DataContext = _currentStoreModel;

            if(!MainWindow.Instance.User.UserId.Equals(1))
            {
                btnEdit.Visibility = Visibility.Hidden;
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            MyContentControl.GetFrame().Content = new StoresEditView(_currentStoreModel);
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            MyContentControl.GetFrame().Content = StoresView.Instance;
        }
    }
}
