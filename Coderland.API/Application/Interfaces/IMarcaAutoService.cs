using System.Collections.Generic;
using System.Threading.Tasks;
using Coderland.API.Domain.Entities;

namespace Coderland.API.Application.Interfaces
{
    // Define los servicios de negocio para trabajar con marcas de autos
    // Esta capa actúa como intermediaria entre los controladores y los repositorios
    public interface IMarcaAutoService
    {
        // Obtiene el listado completo de marcas disponibles
        Task<IEnumerable<MarcaAuto>> GetAllMarcasAutoAsync();
        
        // Busca una marca por su ID
        // Retorna la marca si existe, null en caso contrario
        Task<MarcaAuto> GetMarcaAutoByIdAsync(int id);
    }
}
