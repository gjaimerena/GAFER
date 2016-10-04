using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GAFER.Models
{
    public class Talon
    {
        public int idAlumno { get; set; }
        public string importe1 { get; set; }
        public string importe2 { get; set; }
        public string importe3 { get; set; }
        public string fechaVenc1 { get; set; }
        public string fechaVenc2 { get; set; }
        public string fechaVenc3 { get; set; }
        public string fechaVenc1_customPF { get; set; }
        public string fechaVenc2_customPF { get; set; }
        public string fechaVenc3_customPF { get; set; }
    }
}