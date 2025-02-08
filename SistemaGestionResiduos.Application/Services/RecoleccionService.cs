using SistemaGestionResiduos.Domain.Entities;
using SistemaGestionResiduos.Infrastructure.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaGestionResiduos.Application.Services
{
    public class RecoleccionService
    {
        private readonly GenericRepository<Recoleccion> _repository;

        public RecoleccionService(GenericRepository<Recoleccion> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Recoleccion>> GetAllRecoleccionesAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Recoleccion> GetRecoleccionByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Recoleccion> CreateRecoleccionAsync(Recoleccion recoleccion)
        {
            return await _repository.AddAsync(recoleccion);
        }

        public async Task UpdateRecoleccionAsync(Recoleccion recoleccion)
        {
            await _repository.UpdateAsync(recoleccion);
        }

        public async Task DeleteRecoleccionAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
