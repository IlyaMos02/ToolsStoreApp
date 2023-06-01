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
    public class CharacteristicRepository
    {      
        ToolsStoreLabContext _context;
        ConvertEntities converter;
        public CharacteristicRepository(ToolsStoreLabContext context)
        {
            _context = context;
            converter = new ConvertEntities(_context);
        }

        public IEnumerable<CharacteristicModel> GetCharacteristics()
        {
            var characteristics = _context.Characteristics
                .Include(c => c.KindTool).ThenInclude(kt=>kt.Category)
                .Include(c => c.KindTool).ThenInclude(kt => kt.EnergySource)
                .Select(c => converter.GetCharacteristicModel(c)).ToList();

            return characteristics;
        }

        public CharacteristicModel GetCharacteristicModel(int id)
        {
            var characteristic = GetCharacteristics().FirstOrDefault(c => c.CharacteristicId == id);

            return characteristic;
        }

        public Characteristic GetCharacteristic(int id)
        {
            var characteristic = _context.Characteristics.Include(c => c.KindTool).ThenInclude(kt => kt.Category)
                .Include(c => c.KindTool).ThenInclude(kt => kt.EnergySource).FirstOrDefault(c => c.CharacteristicId == id);

            return characteristic;
        }

        public void UpsertCharacteristic(CharacteristicModel characteristicModel)
        {
            KindToolRepository KTRepository = new KindToolRepository(_context);
            KTRepository.UpsertKindTool(characteristicModel.KindTool);

            var charact = converter.GetCharacteristic(characteristicModel);

            var isNewlyCreated = !_context.Characteristics.Select(c=>c.CharacteristicId).Contains(characteristicModel.CharacteristicId);

            if(isNewlyCreated)
            {
                _context.Characteristics.Add(charact);
            }              
            else
                _context.Characteristics.Update(charact);

            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            
        }

        public void DeleteCharacteristic(int id)
        {
            var characteristic = converter.GetCharacteristic(GetCharacteristicModel(id));

            if (characteristic is null)
                return;

            _context.Characteristics.Remove(characteristic);
            _context.KindTools.Remove(characteristic.KindTool);
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
