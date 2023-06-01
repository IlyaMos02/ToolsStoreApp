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
using ToolsStoreLab.View;

namespace ToolsStoreLab._Repository
{
    //Scaffold-DbContext "Data Source=(local);Initial Catalog=ToolsStoreLab;Integrated Security=True;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -f
    public class ProductRepository
    {
        ToolsStoreLabContext _context;
        QueryRepository QRepository;
        ConvertEntities converter;
        public ProductRepository(ToolsStoreLabContext context) 
        {
            _context = context;
            converter = new ConvertEntities(_context);
            QRepository = new QueryRepository(_context);
        }

        public IEnumerable<Product> GetProducts()
        {
            var query = _context.Products.Include(p => p.Kind).Include(p => p.Characteristic)
                .ThenInclude(c => c.KindTool).ThenInclude(kt => kt.Category)
                .Include(p => p.Kind).Include(p => p.Characteristic)
                .ThenInclude(c => c.KindTool).ThenInclude(kt => kt.EnergySource);

            return query.ToList();
        }

        public IEnumerable<ProductModel> GetProductModels()
        {
            var query = GetProducts().Select(p => converter.GetProductModel(p));

            //QRepository.AddQuery(new Query { QueryText = query.ToQueryString(), Date = DateTime.Now, UserEmail = MainWindow.Instance.User.Email });           

            return query.ToList();
        }       

        public Product GetProduct(int id)
        {
            var product = _context.Products.Include(p => p.Kind).Include(p=>p.Characteristic).ThenInclude(c => c.KindTool).ThenInclude(kt => kt.Category)
                .Include(p=>p.Characteristic).ThenInclude(c => c.KindTool).ThenInclude(kt => kt.EnergySource).FirstOrDefault(p => p.ProductId == id);

            return product;
        }

        public void UpsertProduct(ProductModel product)
        {
            CharacteristicRepository CRepository = new CharacteristicRepository(_context);
            CRepository.UpsertCharacteristic(product.Characteristic);

            var prod = converter.GetProduct(product);            

            var isNewlyCreated = !_context.Products.Select(p=>p.ProductId).Contains(prod.ProductId);

            if (isNewlyCreated)
            {
                _context.Products.Add(prod);
                QRepository.Add("Add", "Product", prod, MainWindow.Instance.User.Email);
            }
            else
            {
                _context.Products.Update(prod);
                QRepository.Add("Update", "Product", prod, MainWindow.Instance.User.Email);
            }

            try
            {
                _context.SaveChanges();              
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        public void DeleteProduct(int productId)
        {
            var prod = GetProduct(productId);                  

            if (prod is null)
            {
                MessageBox.Show("Товар не знайдено");
                return;
            }

            _context.Products.Remove(prod);
            _context.Characteristics.Remove(prod.Characteristic);
            _context.KindTools.Remove(prod.Characteristic.KindTool);

            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        public IEnumerable<Product> GetProductsByString(string str)
        {
            var query = _context.Products.Include(p=>p.Kind).Include(p => p.Characteristic).ThenInclude(c=>c.KindTool).ThenInclude(kt=>kt.Category)
                .Include(p => p.Kind).Include(p => p.Characteristic).ThenInclude(c => c.KindTool).ThenInclude(kt => kt.EnergySource).Where(p=>p.Title.Contains(str) ||
            p.Characteristic.KindTool.Category.Title.Contains(str) || p.Characteristic.KindTool.EnergySource.Source.Contains(str) ||
            p.Kind.Title.Contains(str));

            QRepository.AddQuery(new Query { QueryText = query.ToQueryString(), Date = DateTime.Now, UserEmail = MainWindow.Instance.User.Email });

            return query.ToList();
        }

        public IEnumerable<Product> GetProductsByTitle(string title)
        {
            var query = GetProducts().Where(p => p.Title.Contains(title));

            return query.ToList();
        }

        public IEnumerable<Product> GetProductsByKind(string kind)
        {
            var query = GetProducts().Where(p => p.Kind.Title.Contains(kind));

            return query.ToList();
        }

        public IEnumerable<Product> GetProductsByCategory(string category)
        {
            var query = GetProducts().Where(p => p.Characteristic.KindTool.Category.Title.Contains(category));

            return query.ToList();
        }

        public IEnumerable<Product> GetProductsByEnergySource(string energySource)
        {
            var query = GetProducts().Where(p => p.Characteristic.KindTool.EnergySource.Source.Contains(energySource));

            return query.ToList();
        }

        public void LikeProduct(ProductModel product)
        {
            product.Grade++;

            UpsertProduct(product);
        }
    }
}
