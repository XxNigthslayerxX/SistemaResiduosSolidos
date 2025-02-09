using Microsoft.EntityFrameworkCore;
using SistemaGestionResiduos.Domain.Entities;
using SistemaGestionResiduos.Infrastructure.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaGestionResiduos.Infrastructure.Repositories
{
    public class GenericRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                Console.WriteLine($"Repository: Intentando obtener todas las entidades de tipo {typeof(T).Name}");
                var entities = await _dbSet.ToListAsync();
                Console.WriteLine($"Repository: Se encontraron {entities.Count} entidades de tipo {typeof(T).Name}");
                return entities;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Repository: Error al obtener entidades de tipo {typeof(T).Name}: {ex.Message}");
                Console.WriteLine($"Repository: StackTrace: {ex.StackTrace}");
                throw;
            }
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            try
            {
                Console.WriteLine($"\nRepository: Intentando obtener entidad de tipo {typeof(T).Name} con ID {id}");
                
                var entity = await _dbSet.FindAsync(id);
                
                if (entity != null)
                {
                    Console.WriteLine($"Repository: Entidad encontrada con ID {id}");
                    // Si es una Ruta, mostrar más detalles
                    if (entity is Ruta ruta)
                    {
                        Console.WriteLine($"Repository: Detalles de la Ruta - Nombre: {ruta.Nombre}");
                    }
                }
                else
                {
                    Console.WriteLine($"Repository: No se encontró la entidad de tipo {typeof(T).Name} con ID {id}");
                }
                
                return entity;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Repository: Error al obtener entidad por ID: {ex.Message}");
                Console.WriteLine($"Repository: StackTrace: {ex.StackTrace}");
                throw;
            }
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            try
            {
                Console.WriteLine($"\nRepository: Intentando agregar entidad de tipo {typeof(T).Name}");
                
                // Verificar si la entidad es null
                if (entity == null)
                {
                    Console.WriteLine("Repository: ¡ERROR! La entidad es null");
                    throw new ArgumentNullException(nameof(entity));
                }

                // Verificar el estado de la conexión
                var connection = _context.Database.GetDbConnection();
                Console.WriteLine($"Repository: Estado de la conexión: {connection.State}");
                
                // Intentar agregar la entidad
                var entry = _dbSet.Add(entity);
                Console.WriteLine($"Repository: Entidad agregada al contexto con estado: {entry.State}");
                
                // Guardar cambios
                Console.WriteLine("Repository: Intentando guardar cambios en la base de datos...");
                await _context.SaveChangesAsync();
                Console.WriteLine($"Repository: Cambios guardados exitosamente. ID de la entidad: {entry.Entity.GetType().GetProperty("Id")?.GetValue(entry.Entity)}");
                
                return entry.Entity;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Repository: Error al agregar entidad: {ex.Message}");
                Console.WriteLine($"Repository: StackTrace: {ex.StackTrace}");
                throw;
            }
        }

        public virtual async Task UpdateAsync(T entity)
        {
            try
            {
                Console.WriteLine($"Repository: Intentando actualizar entidad en la base de datos");
                _dbSet.Update(entity);
                await _context.SaveChangesAsync();
                Console.WriteLine($"Repository: Entidad actualizada exitosamente");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Repository: Error al actualizar entidad: {ex.Message}");
                Console.WriteLine($"Repository: StackTrace: {ex.StackTrace}");
                throw;
            }
        }

        public virtual async Task DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
