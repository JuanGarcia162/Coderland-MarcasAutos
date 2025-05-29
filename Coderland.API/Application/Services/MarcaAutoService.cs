using System.Collections.Generic;
using System.Threading.Tasks;
using Coderland.API.Application.Interfaces;
using Coderland.API.Domain.Entities;
using Coderland.API.Domain.Interfaces;

namespace Coderland.API.Application.Services
{
    // Implementación concreta del servicio de marcas de autos
    // Aquí podríamos añadir lógica de negocio adicional si fuera necesario
    public class MarcaAutoService : IMarcaAutoService
    {
        private readonly IMarcaAutoRepository _marcaAutoRepository;

        // Inyectamos el repositorio a través del constructor
        public MarcaAutoService(IMarcaAutoRepository marcaAutoRepository)
        {
            _marcaAutoRepository = marcaAutoRepository;
        }

        // Simplemente delegamos al repositorio para obtener todas las marcas
        // En un futuro podríamos añadir filtros, ordenación, etc.
        public async Task<IEnumerable<MarcaAuto>> GetAllMarcasAutoAsync()
        {
            return await _marcaAutoRepository.GetAllAsync();
        }

        // Delegamos al repositorio para buscar por ID
        // Aquí podríamos añadir cache o validaciones adicionales
        public async Task<MarcaAuto> GetMarcaAutoByIdAsync(int id)
        {
            return await _marcaAutoRepository.GetByIdAsync(id);
        }
    }
}
