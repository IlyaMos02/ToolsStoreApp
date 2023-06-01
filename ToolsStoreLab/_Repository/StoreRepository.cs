using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ToolsStoreLab.Models;
using ToolsStoreLab.Models.ConvertModel;
using ToolsStoreLab.Models.ConvertModel.NewModels;

namespace ToolsStoreLab._Repository
{
    public class StoreRepository
    {
        private ToolsStoreLabContext _context;
        private QueryRepository QRepository;
        private ConvertEntities converter;

        public StoreRepository(ToolsStoreLabContext context)
        {
            _context = context;
            converter = new ConvertEntities(_context);
            QRepository = new QueryRepository(_context);
        }

        public void Update(StoreModel storeModel)
        {
            var store = converter.GetStore(storeModel);

            _context.Stores.Update(store);
            QRepository.Add("Update", "Stores", storeModel, MainWindow.Instance.User.Email);

            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        public IEnumerable<StoreModel> GetStores()
        {
            var query = _context.Stores.Include(s => s.WorkTime).Select(s => converter.GetStoreModel(s));

            //Logger.Log("Get all stores");
            QRepository.AddQuery(new Query { QueryText = query.ToQueryString(), Date = DateTime.Now, UserEmail = MainWindow.Instance.User.Email });

            return query.ToList();
        }

        public IEnumerable<Store> GetStoresByString(string str)
        {
            var query = _context.Stores.Include(s => s.WorkTime).Where(s => s.Adress.Contains(str) || s.WorkTime.Time.Contains(str));

            //Logger.Log($"Search stores by string - {str}");
            QRepository.AddQuery(new Query { QueryText = query.ToQueryString(), Date = DateTime.Now, UserEmail = MainWindow.Instance.User.Email });

            return query.ToList();
        }
    }
}
