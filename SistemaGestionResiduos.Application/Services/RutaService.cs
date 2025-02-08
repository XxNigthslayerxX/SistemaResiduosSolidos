using SistemaGestionResiduos.Domain.Entities;
using SistemaGestionResiduos.Infrastructure.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaGestionResiduos.Application.Services
{
    public class RutaService
    {
        private readonly GenericRepository<Ruta> _repository;

        public RutaService(GenericRepository<Ruta> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Ruta>> GetAllRutasAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Ruta> GetRutaByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Ruta> CreateRutaAsync(Ruta ruta)
        {
            return await _repository.AddAsync(ruta);
        }

        public async Task UpdateRutaAsync(Ruta ruta)
        {
            await _repository.UpdateAsync(ruta);
        }

        public async Task DeleteRutaAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
