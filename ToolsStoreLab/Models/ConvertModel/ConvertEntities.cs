using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolsStoreLab.Models.ConvertModel.NewModels;

namespace ToolsStoreLab.Models.ConvertModel
{
    public class ConvertEntities
    {
        private ToolsStoreLabContext _context;

        public ConvertEntities(ToolsStoreLabContext context)
        {
            _context = context;
        }
        //Entity to Model
        public ProductModel GetProductModel(Product product)
        {
            var productModel = new ProductModel()
            {
                ProductId = product.ProductId,
                Title = product.Title,
                Description = product.Description,
                Kind = product.Kind.Title,
                Characteristic = GetCharacteristicModel(product.Characteristic),
                Price = product.Price,
                Count = product.Count,
                Grade = product.Grade
            };            

            return productModel;
        }

        public CategoryModel GetCategoryModel(Category category)
        {
            var categoryModel = new CategoryModel()
            {
                CategoryId = category.CategoryId,
                Title = category.Title
            };

            return categoryModel;
        }

        public CharacteristicModel GetCharacteristicModel(Characteristic characteristic)
        {
            var characteristicModel = new CharacteristicModel()
            {
                CharacteristicId = characteristic.CharacteristicId,
                KindTool = GetKindToolModel(characteristic.KindTool),                
                Weight= characteristic.Weight
            };

            return characteristicModel;
        }

        public KindToolModel GetKindToolModel(KindTool tool)
        {
            var kindToolModel = new KindToolModel()
            {
                Id = tool.KindToolId,
                Category = tool.Category.Title,
                Power = tool.Power,
                EnergySource = tool.EnergySource.Source
            };

            return kindToolModel;
        }

        public KindModel GetKindModel(Kind kind)
        {
            var kindModel = new KindModel()
            {
                KindId = kind.KindId,
                Title = kind.Title
            };

            return kindModel;
        }

        public RoleModel GetRoleModel(Role role)
        {
            var roleModel = new RoleModel()
            {
                RoleId = role.RoleId,
                Title = role.Title
            };

            return roleModel;
        }

        public StoreModel GetStoreModel(Store store)
        {
            var storeModel = new StoreModel()
            {
                StoreId = store.StoreId,
                Adress = store.Adress,
                WorkTime = store.WorkTime.Time,
                WorkWithoutElectricity = store.WorkWithoutElectricity
            };

            return storeModel;
        }

        public UserModel GetUserModel(User user)
        {
            var userModel = new UserModel()
            {
                UserId = user.UserId,
                Name = user.Name,
                Surname = user.Surname,
                Phone = user.Phone,
                Email = user.Email,
                Password = user.Password,
                Role = user.Role.Title
            };

            return userModel;
        }

        public WorkTimeModel GetWorkTimeModel(WorkTime workTime)
        {
            var workTimeModel = new WorkTimeModel()
            {
                WorkTimeId = workTime.WorkTimeId,
                Time = workTime.Time
            };

            return workTimeModel;
        }

        //Model to Entity
        public Product GetProduct(ProductModel productModel)
        {
            var product = new Product()
            {
                ProductId = productModel.ProductId,
                Title = productModel.Title,
                Description = productModel.Description,
                KindId = _context.Kinds.FirstOrDefault(k => k.Title == productModel.Kind).KindId,
                CharacteristicId = _context.Characteristics.OrderByDescending(c => c.CharacteristicId).FirstOrDefault().CharacteristicId,/*.FirstOrDefault(c => c == GetCharacteristic(productModel.Characteristic)).CharacteristicId,*/
                Price = productModel.Price,
                Count = productModel.Count,
                Grade = productModel.Grade
            };

            return product;
        }

        public Characteristic GetCharacteristic(CharacteristicModel characteristicModel)
        {
            var characteristic = new Characteristic()
            {
                CharacteristicId = characteristicModel.CharacteristicId,
                KindToolId = _context.KindTools.OrderByDescending(kt=>kt.KindToolId).FirstOrDefault().KindToolId,/*.FirstOrDefault(kt => kt == GetKindTool(characteristicModel.KindTool)).KindToolId,*/
                Weight = characteristicModel.Weight         
            };

            return characteristic;
        }

        public Category GetCategory(CategoryModel categoryModel)
        {           
            var category = new Category()
            {
                CategoryId = categoryModel.CategoryId,
                Title = categoryModel.Title,                
            };

            return category;
        }

        public KindTool GetKindTool(KindToolModel kindToolModel)
        {
            var tool = new KindTool()
            {
                KindToolId = kindToolModel.Id,
                CategoryId = _context.Categories.FirstOrDefault(c => c.Title == kindToolModel.Category).CategoryId,
                Power = kindToolModel.Power,
                EnergySourceId = _context.EnergySources.FirstOrDefault(e => e.Source == kindToolModel.EnergySource).EnergySourceId
            };

            return tool;
        }

        public Kind GetKind(KindModel kindModel)
        {
            var kind = new Kind()
            {
                KindId = kindModel.KindId,
                Title = kindModel.Title,
                //Products = context.Products.Where(p => p.KindId == kindModel.KindId).ToList()
            };

            return kind;
        }

        public Role GetRole (RoleModel roleModel)
        {
            var role = new Role()
            {
                RoleId = roleModel.RoleId,
                Title = roleModel.Title,
                //Users = context.Users.Where(u => u.RoleId == roleModel.RoleId).ToList()
            };

            return role;
        }

        public Store GetStore (StoreModel storeModel)
        {
            var store = new Store()
            {
                StoreId = storeModel.StoreId,
                Adress = storeModel.Adress,
                WorkTimeId = _context.WorkTimes.FirstOrDefault(w => w.Time == storeModel.WorkTime).WorkTimeId,
                WorkWithoutElectricity = storeModel.WorkWithoutElectricity,
                //WorkTime = context.WorkTimes.FirstOrDefault(w => w.Time == storeModel.WorkTime)
            };

            return store;
        }

        public User GetUser (UserModel userModel)
        {
            var user = new User()
            {
                UserId = userModel.UserId,
                Name = userModel.Name,
                Surname = userModel.Surname,
                Phone = userModel.Phone,
                Email = userModel.Email,
                Password = userModel.Password,
                RoleId = _context.Roles.FirstOrDefault(r => r.Title == userModel.Role).RoleId,
                //Role = context.Roles.FirstOrDefault(r => r.Title == userModel.Role)
            };

            return user;
        }

        public WorkTime GetWorkTime (WorkTimeModel workTimeModel)
        {
            var workTime = new WorkTime()
            {
                WorkTimeId = workTimeModel.WorkTimeId,
                Time = workTimeModel.Time,
                //Stores = context.Stores.Where(s => s.WorkTimeId == workTimeModel.WorkTimeId).ToList()
            };

            return workTime;
        }
    }
}
