using System.Collections.Generic;
using System.Threading.Tasks;
using Coderland.API.Domain.Entities;

namespace Coderland.API.Domain.Interfaces
{
    // Interfaz que define las operaciones de acceso a datos para las marcas de autos
    // La implementación real estará en la capa de infraestructura
    public interface IMarcaAutoRepository
    {
        // Trae todas las marcas de la base de datos
        Task<IEnumerable<MarcaAuto>> GetAllAsync();
        
        // Busca una marca específica por su ID
        // Devuelve null si no existe
        Task<MarcaAuto> GetByIdAsync(int id);
    }
}
