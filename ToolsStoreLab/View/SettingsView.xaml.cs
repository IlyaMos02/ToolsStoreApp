using Microsoft.Data.SqlClient;
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
using ToolsStoreLab.Tests;

namespace ToolsStoreLab.View
{
    /// <summary>
    /// Логика взаимодействия для SettingsView.xaml
    /// </summary>
    public partial class SettingsView : Page
    {
        private Backup backup;

        private static SettingsView instanse;
        public static SettingsView Instanse
        {
            get
            {
                if(instanse == null)
                    instanse = new SettingsView();

                return instanse;
            }
        }
        private SettingsView()
        {
            InitializeComponent();
            backup = new Backup();
        }

        private void btnBackup_Click(object sender, RoutedEventArgs e)
        {
            backup.MakeBackup(backup.HandArchivePath, backup.HandArchiveName);
            MessageBox.Show("База даних збережена");
        }
        
        private void btnLogs_Click(object sender, RoutedEventArgs e)
        {
            MyContentControl.GetFrame().Content = new LogsView();
        }

        private void btnRestore_Click(object sender, RoutedEventArgs e)
        {
            backup.RestoreBackup();
        }

        private void btnChangePath_Click(object sender, RoutedEventArgs e)
        {
            backup.ChangeHandArchivePath();
        }

        private void btnCategories_Click(object sender, RoutedEventArgs e)
        {
            pnlAddCategory.Visibility = Visibility.Visible;
        }

        private void btnAddCategory_Click(object sender, RoutedEventArgs e)
        {
            if(txtAddCategory.Text != string.Empty)
            {
                CategoryRepository CRepository = new CategoryRepository();
                CRepository.Add(txtAddCategory.Text);
            }

            txtAddCategory.Text = string.Empty;
            pnlAddCategory.Visibility = Visibility.Hidden;
        }

        private void btnCancelAddCategory_Click(object sender, RoutedEventArgs e)
        {
            txtAddCategory.Text = string.Empty;
            pnlAddCategory.Visibility = Visibility.Hidden;
        }

        private void btnStressTest_Click(object sender, RoutedEventArgs e)
        {
            new StressTest().Show();
        }
    }
}
