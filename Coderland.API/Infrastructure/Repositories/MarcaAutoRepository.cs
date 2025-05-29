using Coderland.API.Domain.Entities;
using Coderland.API.Domain.Interfaces;
using Coderland.API.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coderland.API.Infrastructure.Repositories
{
    // Implementación concreta del repositorio usando EF Core
    // Esta clase se encarga de toda la comunicación con la base de datos
    public class MarcaAutoRepository : IMarcaAutoRepository
    {
        private readonly ApplicationDbContext _context;

        // Inyectamos el contexto de EF Core para poder acceder a la BD
        public MarcaAutoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Obtiene todas las marcas de la tabla MarcasAutos
        // Usamos ToListAsync() para no bloquear el hilo mientras se consulta la BD
        public async Task<IEnumerable<MarcaAuto>> GetAllAsync()
        {
            return await _context.MarcasAutos.ToListAsync();
        }

        // Busca una marca por su ID usando el método FindAsync de EF Core
        // FindAsync es más eficiente que FirstOrDefaultAsync porque usa la clave primaria
        public async Task<MarcaAuto> GetByIdAsync(int id)
        {
            return await _context.MarcasAutos.FindAsync(id);
        }
    }
}
