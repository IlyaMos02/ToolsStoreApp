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
using ToolsStoreLab.Emails;
using ToolsStoreLab.Models;

namespace ToolsStoreLab.View
{
    /// <summary>
    /// Логика взаимодействия для ContactView.xaml
    /// </summary>
    public partial class ContactView : Page
    {
        private User User { get; set; }
        private List<TextBlock> textBlocks;

        public ContactView(User user)
        {
            InitializeComponent();

            User = user;
            textBlocks = InitializeListTextBlocks();
        }

        private void btnSearchInPage_Click(object sender, RoutedEventArgs e)
        {
            if (txtSearchInPage.Visibility == Visibility.Hidden)
            {
                txtSearchInPage.Visibility = Visibility.Visible;
            }
            else if (txtSearchInPage.Visibility == Visibility.Visible && txtSearchInPage.Text != string.Empty)
            {
                List<string> result = new List<string>();

                foreach (TextBlock block in textBlocks)
                    if (block.Text.Contains(txtSearchInPage.Text))
                        result.Add(block.Text);

                Sender emailSender = new Sender(User);
                emailSender.SendSearchedText(result);
            }
            else
                txtSearchInPage.Visibility = Visibility.Hidden;
        }

        private List<TextBlock> InitializeListTextBlocks()
        {
            List<TextBlock> blocks = new List<TextBlock>()
            {
                HeaderDesc,
                Desc,
                HeaderPhones,
                FirstPhone,
                SecondPhone,
                ThirdPhone,
                HeaderEmails,
                FirstEmail,
                SecondEmail,
                ThirdEmail
            };

            return blocks;
        }
    }
}
