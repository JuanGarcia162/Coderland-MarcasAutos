using Coderland.API.Application.Interfaces;
using Coderland.API.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using Microsoft.AspNetCore.Http;

namespace Coderland.API.Presentation.Controllers
{
    // API REST para gestionar las marcas de autos
    // Expone endpoints para consultar el catálogo de marcas
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class MarcasAutosController : ControllerBase
    {
        private readonly IMarcaAutoService _marcaAutoService;

        // Inyectamos el servicio que necesitamos para las operaciones de negocio
        public MarcasAutosController(IMarcaAutoService marcaAutoService)
        {
            _marcaAutoService = marcaAutoService;
        }

        // GET /api/MarcasAutos
        // Devuelve la lista completa de marcas disponibles
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<MarcaAuto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<MarcaAuto>>> GetAll()
        {
            var marcas = await _marcaAutoService.GetAllMarcasAutoAsync();
            return Ok(marcas);
        }

        // GET /api/MarcasAutos/{id}
        // Busca una marca específica por su ID
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MarcaAuto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<MarcaAuto>> GetById(int id)
        {
            var marca = await _marcaAutoService.GetMarcaAutoByIdAsync(id);
            
            if (marca == null)
            {
                return NotFound(); // 404 si no existe la marca
            }
            
            return Ok(marca); // 200 con los datos si existe
        }
    }
}
