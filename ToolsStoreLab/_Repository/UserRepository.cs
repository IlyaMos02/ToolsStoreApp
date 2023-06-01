using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using ToolsStoreLab.Models;

namespace ToolsStoreLab._Repository
{
    public class UserRepository
    {
        private ToolsStoreLabContext _context;
        private QueryRepository QRepository;

        public UserRepository(ToolsStoreLabContext context)
        {
            _context = context;
            QRepository = new QueryRepository(_context);
        }

        public IEnumerable<User> GetAll()
        {
            var users = _context.Users.ToList();

            return users;
        }

        public User GetUser(string email, string password)
        {
            var query = _context.Users.Where(u => u.Email == email && u.Password == password);
            var user = query.Include(u => u.Role).FirstOrDefault();

            QRepository.AddQuery(new Query { QueryText = query.ToQueryString(), Date = DateTime.Now});

            if (user is null)
                return null;
            else
                return user;
        }

        public bool Add(User user)
        {
            bool isNewlyCreated = _context.Users.Select(u => u.Email).Equals(user.Email) ||
                _context.Users.Select(u => u.Phone).Equals(user.Phone);

            if (isNewlyCreated)
                return false;

            user.RoleId = 1;

            //QueryRepository.Add("Add", "User", user);

            _context.Users.Add(user);            

            try
            {
                _context.SaveChanges();
                //MessageBox.Show("Регістрація успішна");
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return false;
            }
        }

        public bool Update(User user)
        {
            QRepository.Add("Update", "User", user, user.Email);

            _context.Update(user);

            try
            {
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return false;
            }
        }

        public bool Delete(User user)
        {
            QRepository.Add("Delete", "User", user, user.Email);

            _context.Users.Remove(user);

            try
            {
                _context.SaveChanges();
                MessageBox.Show("Видалення успішне");
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return false;
            }
        }
    }
}
