//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TiendaElectronica
{
    using System;
    using System.Collections.Generic;
    
    public partial class Usuario
    {
        public int id { get; set; }
        public int idEmpleado { get; set; }
        public string usuario1 { get; set; }
        public string clave { get; set; }
        public string usuarioRegistro { get; set; }
        public System.DateTime fechaRegistro { get; set; }
        public short estado { get; set; }
    
        public virtual Empleado Empleado { get; set; }
    }
}
