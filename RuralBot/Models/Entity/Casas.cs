//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RuralBot.Models.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class Casas
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Casas()
        {
            this.Ventas = new HashSet<Ventas>();
        }
    
        public int casaId { get; set; }
        public string nombreCasa { get; set; }
        public int numeroCamas { get; set; }
        public int lugarId { get; set; }
        public string imagenUrl { get; set; }
        public int precio { get; set; }
    
        public virtual Lugares Lugares { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ventas> Ventas { get; set; }
    }
}
