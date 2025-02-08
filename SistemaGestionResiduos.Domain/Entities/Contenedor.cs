using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SistemaGestionResiduos.Domain.Entities
{
    public class Contenedor
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La ubicación es requerida")]
        [Display(Name = "Ubicación")]
        public string Ubicacion { get; set; }

        [Required(ErrorMessage = "La capacidad es requerida")]
        [Display(Name = "Capacidad (kg)")]
        [Range(0, double.MaxValue, ErrorMessage = "La capacidad debe ser mayor a 0")]
        public double Capacidad { get; set; }

        [Required(ErrorMessage = "El nivel de llenado es requerido")]
        [Display(Name = "Nivel de Llenado (%)")]
        [Range(0, 100, ErrorMessage = "El nivel de llenado debe estar entre 0 y 100")]
        public double NivelLlenado { get; set; }

        [Required(ErrorMessage = "El tipo de residuo es requerido")]
        [Display(Name = "Tipo de Residuo")]
        public string TipoResiduo { get; set; }

        [Display(Name = "Última Recolección")]
        public DateTime UltimaRecoleccion { get; set; }

        [Required(ErrorMessage = "La ruta es requerida")]
        [Display(Name = "Ruta")]
        public int RutaId { get; set; }

        public virtual Ruta Ruta { get; set; }
        public virtual ICollection<Recoleccion> Recolecciones { get; set; }
    }
}
