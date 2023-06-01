using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ToolsStoreLab._Repository;
using ToolsStoreLab.Models;

namespace ToolsStoreLab.View
{
    /// <summary>
    /// Логика взаимодействия для LogsView.xaml
    /// </summary>
    public partial class LogsView : Page
    {
        private ToolsStoreLabContext _context;
        private QueryRepository QRepository;
        public LogsView()
        {
            InitializeComponent();
            _context = new ToolsStoreLabContext();
            QRepository = new QueryRepository(_context);         

            var items = new List<string>() { string.Empty };
            comboQueryTypeFilter.ItemsSource = items.Union(new string[] {"SELECT" , "INSERT", "UPDATE", "DELETE"});
            comboTableFilter.ItemsSource = items.Union(new string[] { "Users", "Products", "Stores" });
        }

        private void comboFilter_DropDownClosed(object sender, EventArgs e)
        {
            string filterStr = (sender as ComboBox).Text;

            Queries.ItemsSource = QRepository.GetQueryByString(filterStr);
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string searchStr = txtSearch.Text;
            string searchDateTime = dateFilter.Text;

            if (searchDateTime != string.Empty)
                Queries.ItemsSource = QRepository.GetQueryByDate(searchDateTime);
            else if (searchStr != string.Empty)
                Queries.ItemsSource = QRepository.GetQueryByString(searchStr);
            else
                Queries.ItemsSource = QRepository.GetAllQueries();
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                _context.ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                Queries.ItemsSource = QRepository.GetAllQueries();
            }
        }

        private void txtUser_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Queries.ItemsSource = QRepository.GetQueryByUserEmail(txtUser.Text);
        }
    }
}
