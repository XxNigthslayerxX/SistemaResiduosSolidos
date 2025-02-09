using SistemaGestionResiduos.Domain.Entities;
using SistemaGestionResiduos.Infrastructure.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaGestionResiduos.Application.Services
{
    public class ContenedorService
    {
        private readonly GenericRepository<Contenedor> _contenedorRepository;
        private readonly GenericRepository<Ruta> _rutaRepository;

        public ContenedorService(GenericRepository<Contenedor> contenedorRepository, GenericRepository<Ruta> rutaRepository)
        {
            _contenedorRepository = contenedorRepository;
            _rutaRepository = rutaRepository;
        }

        public async Task<IEnumerable<Contenedor>> GetAllContenedoresAsync()
        {
            return await _contenedorRepository.GetAllAsync();
        }

        public async Task<Contenedor> GetContenedorByIdAsync(int id)
        {
            return await _contenedorRepository.GetByIdAsync(id);
        }

        public async Task<Contenedor> CreateContenedorAsync(Contenedor contenedor)
        {
            try
            {
                Console.WriteLine($"\nContenedorService: Intentando crear contenedor en ubicación '{contenedor.Ubicacion}'");
                Console.WriteLine($"ContenedorService: Datos del contenedor a crear:");
                Console.WriteLine($"- ID: {contenedor.Id}");
                Console.WriteLine($"- Ubicación: {contenedor.Ubicacion}");
                Console.WriteLine($"- Capacidad: {contenedor.Capacidad}");
                Console.WriteLine($"- Nivel de Llenado: {contenedor.NivelLlenado}");
                Console.WriteLine($"- Tipo de Residuo: {contenedor.TipoResiduo}");
                Console.WriteLine($"- RutaId: {contenedor.RutaId}");

                // Verificar que la ruta existe
                var ruta = await _rutaRepository.GetByIdAsync(contenedor.RutaId);
                if (ruta == null)
                {
                    Console.WriteLine($"ContenedorService: Error - No se encontró la ruta con ID {contenedor.RutaId}");
                    throw new Exception($"No se encontró la ruta con ID {contenedor.RutaId}");
                }

                // Asegurarse de que la colección de recolecciones esté inicializada
                if (contenedor.Recolecciones == null)
                {
                    Console.WriteLine("ContenedorService: Inicializando la colección de recolecciones");
                    contenedor.Recolecciones = new List<Recoleccion>();
                }

                // Asegurarse de que UltimaRecoleccion tenga un valor
                if (contenedor.UltimaRecoleccion == default)
                {
                    Console.WriteLine("ContenedorService: Inicializando UltimaRecoleccion con la fecha actual");
                    contenedor.UltimaRecoleccion = DateTime.Now;
                }

                var contenedorCreado = await _contenedorRepository.AddAsync(contenedor);
                
                if (contenedorCreado != null)
                {
                    Console.WriteLine($"ContenedorService: Contenedor creado exitosamente con ID {contenedorCreado.Id}");
                    
                    // Verificar que el contenedor se guardó correctamente
                    var contenedorVerificado = await _contenedorRepository.GetByIdAsync(contenedorCreado.Id);
                    if (contenedorVerificado != null)
                    {
                        Console.WriteLine($"ContenedorService: Verificación exitosa - Contenedor encontrado en la base de datos con ID {contenedorVerificado.Id}");
                    }
                    else
                    {
                        Console.WriteLine("ContenedorService: ¡ADVERTENCIA! No se pudo encontrar el contenedor recién creado en la base de datos");
                    }
                }
                else
                {
                    Console.WriteLine("ContenedorService: ¡ERROR! El repositorio devolvió null al crear el contenedor");
                }

                return contenedorCreado;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ContenedorService: Error al crear contenedor: {ex.Message}");
                Console.WriteLine($"ContenedorService: StackTrace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task UpdateContenedorAsync(Contenedor contenedor)
        {
            await _contenedorRepository.UpdateAsync(contenedor);
        }

        public async Task DeleteContenedorAsync(int id)
        {
            await _contenedorRepository.DeleteAsync(id);
        }
    }
}
