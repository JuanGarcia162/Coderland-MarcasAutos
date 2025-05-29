using System;

namespace Coderland.API.Domain.Entities
{
    // Clase que almacena la info de las marcas de autos que manejamos en el sistema
    public class MarcaAuto
    {
        // ID único para cada marca en la base de datos
        public int Id { get; set; }
        
        // Nombre comercial de la marca (Toyota, Ford, etc.)
        public string Nombre { get; set; }
        
        // País donde se fundó la marca
        public string PaisOrigen { get; set; }
        
        // Año en que se fundó la empresa
        public int AnioFundacion { get; set; }
        
        // Flag para saber si la marca está activa o no en nuestro catálogo
        public bool Activo { get; set; }
    }
}
