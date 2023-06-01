using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolsStoreLab.Models;
using ToolsStoreLab.Models.ConvertModel.NewModels;

namespace ToolsStoreLab.Tests
{
    internal class DataGenerator
    {
        private ToolsStoreLabContext _context;

        private Faker<User> userFake;
        private Faker<ProductModel> productFake;
        private Faker<CharacteristicModel> characteristicFake;
        private Faker<KindToolModel> kindToolFake;

        private const int MIN_INT = 90_000;
        private const int MAX_INT = 100_000;
        private const int min_int = 10;
        private const int max_int = 500;

        public DataGenerator()
        {
            Randomizer.Seed = new Random(123);
            _context = new ToolsStoreLabContext();

            userFake = new Faker<User>()
                .RuleFor(u => u.UserId, _ => 0)
                .RuleFor(u => u.Name, f => f.Name.FirstName())
                .RuleFor(u => u.Surname, f => f.Name.LastName())
                .RuleFor(u => u.Phone, f => f.Phone.PhoneNumber("+380#########"))
                .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.Name, u.Surname))
                .RuleFor(u => u.Password, f => f.Internet.Password())
                .RuleFor(u => u.RoleId, f => f.Random.Int(1, 2));

            kindToolFake = new Faker<KindToolModel>()
                .RuleFor(kd => kd.Id, _ => 0)
                .RuleFor(kd => kd.Category, f => f.Random.ListItem(_context.Categories.Select(c => c.Title).ToList()))
                .RuleFor(kd => kd.Power, f => f.Random.Int(min_int, max_int))
                .RuleFor(kd => kd.EnergySource, f => f.Random.ListItem(_context.EnergySources.Select(es => es.Source).ToList()));

            characteristicFake = new Faker<CharacteristicModel>()
                .RuleFor(c => c.CharacteristicId, _ => 0)
                .RuleFor(c => c.KindTool, _ => kindToolFake.Generate())
                .RuleFor(c => c.Weight, f => f.Random.Int(min_int, max_int));

            productFake = new Faker<ProductModel>()
                .RuleFor(p => p.ProductId, _ => 0)
                .RuleFor(p => p.Title, f => f.Commerce.ProductName())
                .RuleFor(p => p.Description, _ = "-")
                .RuleFor(p => p.Kind, f => f.Random.ListItem(_context.Kinds.Select(k => k.Title).ToList()))
                .RuleFor(p => p.Characteristic, _ => characteristicFake.Generate())
                .RuleFor(p => p.Price, f => f.Random.Int(min_int, max_int))
                .RuleFor(p => p.Count, f => f.Random.Int(min_int, max_int))
                .RuleFor(p => p.Grade, _ => 0);
        }

        public User GenerateUser()
        {
            return userFake.Generate();
        }

        public ProductModel GenerateProductModel()
        {
            return productFake.Generate();
        }

        public IEnumerable<User> GenerateUsers()
        {
            return userFake.GenerateForever();
        }

        public IEnumerable<ProductModel> GenerateProductModels()
        {
            return productFake.GenerateForever();
        }
    }
}
