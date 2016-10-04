using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GAFER.Models
{
    public class TalonPagoFacil
    {

        public Historial datosTalon { get; set; }
        public decimal Importe1 { get; set; }
        public decimal Importe2 { get; set; }
        public decimal Importe3 { get; set; }
        public DateTime fechaVenc1 { get; set; }
        public DateTime fechaVenc2 { get; set; }
        public DateTime fechaVenc3 { get; set; }
        


    }
}