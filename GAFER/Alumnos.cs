//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GAFER
{
    using System;
    using System.Collections.Generic;
    
    public partial class Alumnos
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Alumnos()
        {
            this.Historial = new HashSet<Historial>();
        }
    
        public int IdAlumno { get; set; }
        public string IdColegio { get; set; }
        public string CodigoAlumno { get; set; }
        public string Apellido { get; set; }
        public string Nombre { get; set; }
        public string Mail { get; set; }
        public string Observaciones { get; set; }
        public Nullable<int> Condicion { get; set; }
        public string Telefono { get; set; }
        public Nullable<System.DateTime> FechaAlta { get; set; }
        public Nullable<decimal> Importe1 { get; set; }
        public Nullable<decimal> Importe2 { get; set; }
        public Nullable<decimal> Importe3 { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Historial> Historial { get; set; }
    }
}
