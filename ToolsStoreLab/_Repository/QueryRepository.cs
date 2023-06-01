using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using ToolsStoreLab.Models;

namespace ToolsStoreLab._Repository
{
    public class QueryRepository
    {
        private ToolsStoreLabContext _context;

        public QueryRepository(ToolsStoreLabContext context)
        {
            _context = context;
        }

        public IEnumerable<Query> GetAllQueries()
        {
            var queries = _context.Queries.ToList();

            return queries;
        }

        public IEnumerable<Query> GetQueryByString(string str)
        {                      
            var queries = GetAllQueries()
                .Where(q => q.QueryText.Contains(str)).ToList();

            return queries;
        }  
        
        public IEnumerable<Query> GetQueryByUserEmail(string userEmail)
        {
            var queries = GetAllQueries().Where(q =>q.UserEmail != null && q.UserEmail.Contains(userEmail)).ToList();

            return queries;
        }

        public IEnumerable<Query> GetQueryByDate(string date)
        {
            var queries = GetAllQueries().Where(q => q.Date.ToShortDateString() == date).ToList();

            return queries;
        }

        public void Add(string methodName, string table, object model, string? userEmail = null)
        {
            var jModel = JsonSerializer.Serialize(model, model.GetType(), new JsonSerializerOptions()
            { ReferenceHandler = ReferenceHandler.Preserve, WriteIndented = true });

            if (methodName == "Add")
            {
                AddQuery(new Query { QueryText = $"INSERT into {table} values {jModel}", Date = DateTime.Now, UserEmail = userEmail });
            }
            else if (methodName == "Update")
            {
                AddQuery(new Query { QueryText = $"UPDATE {table} set {jModel}", Date = DateTime.Now, UserEmail = userEmail });
            }
            else if (methodName == "Delete")
            {
                AddQuery(new Query { QueryText = $"DELETE from {table} object {jModel}", Date = DateTime.Now, UserEmail = userEmail });
            }
        }

        public void AddQuery(Query query)
        {
            _context.Queries.Add(query);

            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
