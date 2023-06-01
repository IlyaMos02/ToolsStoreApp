using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ToolsStoreLab.Models;

namespace ToolsStoreLab._Repository
{
    internal class CategoryRepository
    {
        private ToolsStoreLabContext _context;
        private QueryRepository Qrepository;

        public void Add(string categoryName)
        {
            _context = new ToolsStoreLabContext();
            Qrepository = new QueryRepository(_context);

            Category newCategory = new Category()
            {
                CategoryId = 0,
                Title = categoryName
            };

            Qrepository.Add("Add", "Category", newCategory, MainWindow.Instance.User.Email);
            _context.Categories.Add(newCategory);

            try
            {
                _context.SaveChanges();
                MessageBox.Show("Збереження успішне");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
