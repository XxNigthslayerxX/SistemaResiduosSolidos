using System;
using System.ComponentModel.DataAnnotations;

namespace SistemaGestionResiduos.Domain.Entities
{
    public class Recoleccion
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La fecha es requerida")]
        [Display(Name = "Fecha de Recolección")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "El peso es requerido")]
        [Display(Name = "Peso Recolectado (kg)")]
        [Range(0, double.MaxValue, ErrorMessage = "El peso debe ser mayor a 0")]
        public double PesoRecolectado { get; set; }

        [Display(Name = "Tipo de Residuo")]
        public string TipoResiduo { get; set; }

        [Display(Name = "Observaciones")]
        public string Observaciones { get; set; }

        [Required(ErrorMessage = "El contenedor es requerido")]
        [Display(Name = "Contenedor")]
        public int ContenedorId { get; set; }

        public virtual Contenedor Contenedor { get; set; }
    }
}
