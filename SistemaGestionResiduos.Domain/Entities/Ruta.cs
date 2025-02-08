using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SistemaGestionResiduos.Domain.Entities
{
    public class Ruta
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [Display(Name = "Nombre de la Ruta")]
        public string Nombre { get; set; }

        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "La hora de inicio es requerida")]
        [Display(Name = "Hora de Inicio")]
        public TimeSpan HoraInicio { get; set; }

        [Required(ErrorMessage = "La hora de fin es requerida")]
        [Display(Name = "Hora de Fin")]
        public TimeSpan HoraFin { get; set; }

        [Required(ErrorMessage = "Los días de servicio son requeridos")]
        [Display(Name = "Días de Servicio")]
        public string DiasServicio { get; set; }

        [Display(Name = "Estado Activo")]
        public bool Activo { get; set; }

        public virtual ICollection<Contenedor> Contenedores { get; set; }
    }
}
