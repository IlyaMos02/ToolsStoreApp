using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ToolsStoreLab._Repository;
using ToolsStoreLab.Models;

namespace ToolsStoreLab.Tests
{
    /// <summary>
    /// Логика взаимодействия для StressTest.xaml
    /// </summary>
    public partial class StressTest : Window
    {
        [DllImport("Kernel32")]
        public static extern void AllocConsole();

        [DllImport("Kernel32")]
        public static extern void FreeConsole();

        private ToolsStoreLabContext _context;
        private UserRepository _userRepository;
        private ProductRepository _productRepository;
        private DataGenerator generator;
        private const string connectionString = "Data Source=(local);Initial Catalog=ToolsStoreLabTests;Integrated Security=True;TrustServerCertificate=True;";
        private const int First_Iteration_Count = 50;
        private const int Second_Iteration_Count = 100;
        private const int Third_Iteration_Count = 500;
        private const int Fourth_Iteration_Count = 2000;
        private const int Fifth_Iteration_Count = 100000;
        public StressTest()
        {
            InitializeComponent();
            AllocConsole();

            _context = new ToolsStoreLabContext();
            _userRepository = new UserRepository(_context);
            _productRepository = new ProductRepository(_context);
            generator = new DataGenerator();

            _context.ConnectionString = connectionString;

            GenerateUserTest(Third_Iteration_Count);
            GetUsersTest();
            ClearTable("Users");

            //GenerateProductTest(Fifth_Iteration_Count);
            GetAllProductsTest();
            string str = "Tasty";
            GetProductsByString(str);
            string kind = "Будівельний";
            GetProductsByKind(kind);
            string category = "Ручний";
            GetProductsByCategory(category);
            string energySource = "Батарея";
            GetProductsByEnergySource(energySource);
        }

        private void GenerateUserTest(int countOfObjects)
        {
            var users = generator.GenerateUsers().Take(countOfObjects);
            var stopWatch = new Stopwatch();

            stopWatch.Start();
            foreach(var user in users)
                _userRepository.Add(user);
            stopWatch.Stop();

            Console.WriteLine($"Add users takes {stopWatch.Elapsed.Seconds} s, {stopWatch.ElapsedMilliseconds} ms, {stopWatch.ElapsedTicks} ticks");
        }

        private void GetUsersTest()
        {
            var stopWatch = new Stopwatch();

            stopWatch.Start();
            var users = _userRepository.GetAll();
            stopWatch.Stop();

            Console.WriteLine($"Get users takes {stopWatch.Elapsed.Seconds} s, {stopWatch.ElapsedMilliseconds} ms, {stopWatch.ElapsedTicks} ticks");
        }

        private void GenerateProductTest(int countOfObjects)
        {
            var products = generator.GenerateProductModels().Take(countOfObjects);
            var stopWatch = new Stopwatch();

            stopWatch.Start();
            foreach(var product in products)
                _productRepository.UpsertProduct(product);
            stopWatch.Stop();

            Console.WriteLine($"Add {countOfObjects} products takes {stopWatch.Elapsed.Seconds} s, {stopWatch.ElapsedMilliseconds} ms, {stopWatch.ElapsedTicks} ticks");
        }

        private void GetAllProductsTest()
        {
            var stopWatch = new Stopwatch();

            stopWatch.Start();
            var products = _productRepository.GetProductModels();
            stopWatch.Stop();

            Console.WriteLine($"Get products with count {products.Count()} takes {stopWatch.Elapsed.Seconds} s, {stopWatch.ElapsedMilliseconds} ms, {stopWatch.ElapsedTicks} ticks");
        }

        private void GetProductsByString(string str)
        {
            var stopWatch = new Stopwatch();

            stopWatch.Start();
            var products = _productRepository.GetProductsByString(str);
            stopWatch.Stop();

            Console.WriteLine($"Get products by string {str} takes {stopWatch.Elapsed.Seconds} s, {stopWatch.ElapsedMilliseconds} ms, {stopWatch.ElapsedTicks} ticks");
        }

        private void GetProductsByKind(string kind)
        {
            var stopWatch = new Stopwatch();

            stopWatch.Start();
            var products = _productRepository.GetProductsByKind(kind);
            stopWatch.Stop();

            Console.WriteLine($"Get products by kind {kind} takes {stopWatch.Elapsed.Seconds} s, {stopWatch.ElapsedMilliseconds} ms, {stopWatch.ElapsedTicks} ticks");
        }

        private void GetProductsByCategory(string category)
        {
            var stopWatch = new Stopwatch();

            stopWatch.Start();
            var products = _productRepository.GetProductsByCategory(category);
            stopWatch.Stop();

            Console.WriteLine($"Get products by category {category} takes {stopWatch.Elapsed.Seconds} s, {stopWatch.ElapsedMilliseconds} ms, {stopWatch.ElapsedTicks} ticks");
        }

        private void GetProductsByEnergySource(string energySource)
        {
            var stopWatch = new Stopwatch();

            stopWatch.Start();
            var products = _productRepository.GetProductsByEnergySource(energySource);
            stopWatch.Stop();

            Console.WriteLine($"Get products by energy source {energySource} takes {stopWatch.Elapsed.Seconds} s, {stopWatch.ElapsedMilliseconds} ms, {stopWatch.ElapsedTicks} ticks");
        }

        private void ClearTable(string tableName)
        {
            _context.Database.ExecuteSqlRaw($"TRUNCATE TABLE [{tableName}]");
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            FreeConsole();
        }
    }
}
