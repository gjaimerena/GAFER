using System.ComponentModel.DataAnnotations;

namespace GAFER.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }
    }

    public class ManageUserViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "La {0} debe contener al menos {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "Las contraseñas ingresadas no coinciden.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "La {0} debe contener al menos {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [StringLength(5, ErrorMessage = "el {0} debe contener al menos {2} caracteres.", MinimumLength = 1)]
        [Display(Name = "Codigo Colegio")]
        public string CodigoColegio { get; set; }

        
        [StringLength(50, ErrorMessage = "el {0} debe contener al menos {2} caracteres.", MinimumLength = 1)]
        [Display(Name = "Discriminator")]
        public string Discriminator { get; set; }

        [Required]
        [Display(Name = "Cantidad Vencimientos")]
        public int CantidadVencimientos { get; set; }

        //[Required]
        //[Display(Name = "Cantidad Vencimientos")]
        //public int CondicionIVA { get; set; }

        [Required]
        [StringLength(40, ErrorMessage = "el {0} debe contener al menos {2} caracteres.", MinimumLength = 1)]
        [Display(Name = "Denominacion")]
        public string Denominacion { get; set; }

        
        [StringLength(50, ErrorMessage = "el {0} debe contener al menos {2} caracteres.", MinimumLength = 1)]
        [Display(Name = "CUIT")]
        public string CUIT { get; set; }

        
        [StringLength(50, ErrorMessage = "el {0} debe contener al menos {2} caracteres.", MinimumLength = 1)]
        [Display(Name = "Contacto")]
        public string Contacto { get; set; }

        
        [StringLength(50, ErrorMessage = "el {0} debe contener al menos {2} caracteres.", MinimumLength = 1)]
        [Display(Name = "Direccion")]
        public string Direccion { get; set; }

       
        [StringLength(50, ErrorMessage = "el {0} debe contener al menos {2} caracteres.", MinimumLength = 1)]
        [Display(Name = "Observaciones")]
        public string Observaciones { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Mail")]
        public string Mail { get; set; }


        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Las contraseñas ingresadas no coinciden.")]
        public string ConfirmPassword { get; set; }



      
    }
}
