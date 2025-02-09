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
            try
            {
                Console.WriteLine("RutaService: Intentando obtener todas las rutas");
                var rutas = await _repository.GetAllAsync();
                var rutasList = rutas.ToList();
                Console.WriteLine($"RutaService: Se encontraron {rutasList.Count} rutas");
                foreach (var ruta in rutasList)
                {
                    Console.WriteLine($"RutaService: Ruta encontrada - ID: {ruta.Id}, Nombre: {ruta.Nombre}");
                }
                return rutasList;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RutaService: Error al obtener rutas: {ex.Message}");
                Console.WriteLine($"RutaService: StackTrace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<Ruta> GetRutaByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Ruta> CreateRutaAsync(Ruta ruta)
        {
            try
            {
                Console.WriteLine($"\nRutaService: Intentando crear ruta con nombre '{ruta.Nombre}'");
                Console.WriteLine($"RutaService: Datos de la ruta a crear:");
                Console.WriteLine($"- ID: {ruta.Id}");
                Console.WriteLine($"- Nombre: {ruta.Nombre}");
                Console.WriteLine($"- Descripción: {ruta.Descripcion}");
                Console.WriteLine($"- Hora Inicio: {ruta.HoraInicio}");
                Console.WriteLine($"- Hora Fin: {ruta.HoraFin}");
                Console.WriteLine($"- Días Servicio: {ruta.DiasServicio}");
                Console.WriteLine($"- Activo: {ruta.Activo}");

                var rutaCreada = await _repository.AddAsync(ruta);
                
                if (rutaCreada != null)
                {
                    Console.WriteLine($"RutaService: Ruta creada exitosamente con ID {rutaCreada.Id}");
                    
                    // Verificar que la ruta se guardó correctamente
                    var rutaVerificada = await _repository.GetByIdAsync(rutaCreada.Id);
                    if (rutaVerificada != null)
                    {
                        Console.WriteLine($"RutaService: Verificación exitosa - Ruta encontrada en la base de datos con ID {rutaVerificada.Id}");
                    }
                    else
                    {
                        Console.WriteLine("RutaService: ¡ADVERTENCIA! No se pudo encontrar la ruta recién creada en la base de datos");
                    }
                }
                else
                {
                    Console.WriteLine("RutaService: ¡ERROR! El repositorio devolvió null al crear la ruta");
                }

                return rutaCreada;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RutaService: Error al crear ruta: {ex.Message}");
                Console.WriteLine($"RutaService: StackTrace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task UpdateRutaAsync(Ruta ruta)
        {
            try
            {
                Console.WriteLine($"RutaService: Intentando actualizar ruta con ID: {ruta.Id}");
                await _repository.UpdateAsync(ruta);
                Console.WriteLine($"RutaService: Ruta actualizada exitosamente");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RutaService: Error al actualizar ruta: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteRutaAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
