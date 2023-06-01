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
    public class KindToolRepository
    {
        ToolsStoreLabContext _context;
        ConvertEntities converter;
        public KindToolRepository(ToolsStoreLabContext context)
        {
            _context = context;
            converter = new ConvertEntities(_context);
        }

        public IEnumerable<KindToolModel> GetKindToolModels()
        {
            var kindToolModels = _context.KindTools
                .Include(kt=>kt.EnergySource)
                .Include(kt=>kt.Category)
                .Select(kt=>converter.GetKindToolModel(kt)).ToList();

            return kindToolModels;
        }

        public KindToolModel GetKindToolModel(int id)
        {
            var kindToolModel = GetKindToolModels().FirstOrDefault(kt=>kt.Id == id);

            return kindToolModel;
        }

        public void UpsertKindTool(KindToolModel kindToolModel)
        {
            var kindTool = converter.GetKindTool(kindToolModel);

            var isNewlyCreated = !_context.KindTools.Select(kt => kt.KindToolId).Contains(kindTool.KindToolId);

            if (isNewlyCreated)
                _context.KindTools.Add(kindTool);
            else
                _context.KindTools.Update(kindTool);

            try
            {
                _context.SaveChanges();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        public void DeleteKindTool(int id)
        {
            var kindTool = converter.GetKindTool(GetKindToolModel(id));

            if(kindTool is null)
                return;

            _context.KindTools.Remove(kindTool);

            try
            {
                _context.SaveChanges();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
