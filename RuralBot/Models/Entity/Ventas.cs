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
    
    public partial class Ventas
    {
        public int ventaId { get; set; }
        public int userId { get; set; }
        public int casaId { get; set; }
        public int numeroPersonas { get; set; }
        public System.DateTime fechaEntrada { get; set; }
        public System.DateTime fechaSalida { get; set; }
    
        public virtual Casas Casas { get; set; }
        public virtual Users Users { get; set; }
    }
}
