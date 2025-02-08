using SistemaGestionResiduos.Domain.Entities;
using SistemaGestionResiduos.Infrastructure.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaGestionResiduos.Application.Services
{
    public class ContenedorService
    {
        private readonly GenericRepository<Contenedor> _repository;

        public ContenedorService(GenericRepository<Contenedor> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Contenedor>> GetAllContenedoresAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Contenedor> GetContenedorByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Contenedor> CreateContenedorAsync(Contenedor contenedor)
        {
            return await _repository.AddAsync(contenedor);
        }

        public async Task UpdateContenedorAsync(Contenedor contenedor)
        {
            await _repository.UpdateAsync(contenedor);
        }

        public async Task DeleteContenedorAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
