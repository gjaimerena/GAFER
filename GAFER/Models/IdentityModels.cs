using Microsoft.AspNet.Identity.EntityFramework;

namespace GAFER.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        
        public string CodigoColegio { get; set; }
        public string Denominacion { get; set; }
        public int CondicionIVA { get; set; }
        public string CUIT { get; set; }
        public string Mail { get; set; }
        public string Contacto { get; set; }
        public string Direccion { get; set; }
        public int CantidadVencimientos { get; set; }
        public string Observaciones { get; set; }
        

    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }
    }
}