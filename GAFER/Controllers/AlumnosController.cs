using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GAFER;
using GAFER.Models;
using iTextSharp.text.pdf;
using System.IO;
using iTextSharp.text;
using System.Net.Mail;
using System.Globalization;
using System.Web.Helpers;
using System.Text;
using GAFER.Helpers;

namespace GAFER.Controllers
{
    public class AlumnosController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private GAFEREntities db = new GAFEREntities();
       

        // GET: Alumnos
        public ActionResult Index()
        {
            //string IdColegio = db.AspNetUsers.Where(m => m.UserName == User.Identity.Name).FirstOrDefault().Id;
            //var alumnos = db.Alumnos.Include(a => a.AspNetUsers);
            //alumnos = alumnos.Where(m => m.IdColegio == IdColegio);
            //return View(alumnos.ToList());

            
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            

            if (Session["codigoColegio"] == null || Session["cantVencimientos"]==null || Session["idColegio"]== null)
            {
                AspNetUsers user = db.AspNetUsers.Where(m => m.UserName == User.Identity.Name).FirstOrDefault();
                Session["cantVencimientos"] = user.CantidadVencimientos;
                Session["codigoColegio"] = user.CodigoColegio;
                Session["Colegio"] = user.UserName;
                Session["idColegio"] = user.Id;
                Session["DenominacionColegio"] = user.Denominacion;
            }

            int? cantVencimientos = (int)Session["cantVencimientos"];

            if (cantVencimientos != null)
            {
                ViewBag.importe1_div= "visibility:hidden";
                ViewBag.importe2_div = "visibility:hidden";
                ViewBag.importe3_div = "visibility:hidden";
                ViewBag.importe1_input = "hidden";
                ViewBag.importe2_input = "hidden";
                ViewBag.importe3_input = "hidden";

                if (cantVencimientos > 0)
                {
                    ViewBag.importe1_div = "visibility:visible";
                    ViewBag.importe1_input = "text";

                }
                if (cantVencimientos > 1)
                {
                    ViewBag.importe2_div = "visibility:visible";
                    ViewBag.importe2_input = "text";
                }
                if (cantVencimientos > 2)
                {
                    ViewBag.importe3_div = "visibility:visible";
                    ViewBag.importe3_input = "text";
                }
            }


            return View();
        }

        // GET: Alumnos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Alumnos alumnos = db.Alumnos.Find(id);
            if (alumnos == null)
            {
                return HttpNotFound();
            }
            return View(alumnos);
        }

        // GET: Alumnos/Create
        public ActionResult Create()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.IdColegio = new SelectList(db.AspNetUsers, "Id", "UserName");
            return View();
            //  return View();
        }

        // POST: Alumnos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Alumnos alumnos)
        {
            if (ModelState.IsValid)
            {
                db.Alumnos.Add(alumnos);
                db.SaveChanges();
                //    return RedirectToAction("Index");
                //}
                //else
                //{
                //    ViewBag.IdColegio = new SelectList(db.AspNetUsers, "Id", "UserName", alumnos.IdColegio);
                    return RedirectToAction("Index");
                }
            
                ViewBag.IdColegio = new SelectList(db.AspNetUsers, "Id", "UserName", alumnos.IdColegio);
             
                return View(alumnos);
            }

        public ActionResult AddAlumno(Alumnos alumno)
        {

            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            if (alumno.CodigoAlumno == "" || alumno.CodigoAlumno == null)
            {
                return Json("El código de alumno es obligatorio.", JsonRequestBehavior.AllowGet);
            }
            if (alumno != null)
            {
                try
                {
                   //si recibo valores nulos los seteo de forma generica para que sean validos
                    alumno.Apellido = (alumno.Apellido == null) ? "" : alumno.Apellido;
                    alumno.Condicion = (alumno.Condicion == null) ? 0 : alumno.Condicion;
                    alumno.Importe1 = (alumno.Importe1 == null) ? 0 : alumno.Importe1;
                    alumno.Importe2 = (alumno.Importe2 == null) ? 0 : alumno.Importe2;
                    alumno.Importe3 = (alumno.Importe3 == null) ? 0 : alumno.Importe3;
                    alumno.Mail = (alumno.Apellido == null) ? "" : alumno.Mail;
                    alumno.Nombre = (alumno.Nombre == null) ? "" : alumno.Nombre;
                    alumno.Observaciones = (alumno.Observaciones == null) ? "" : alumno.Observaciones;
                    alumno.Telefono = (alumno.Telefono == null) ? "" : alumno.Telefono;

                    //agrego valores necesarios
                    alumno.FechaAlta = DateTime.Now;
                    alumno.IdColegio = (string)Session["idColegio"];//db.AspNetUsers.Where(m => m.UserName == User.Identity.Name).FirstOrDefault().Id;

                    db.Alumnos.Add(alumno);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    return Json("Error al generar el alta de alumno" + ex.Message, JsonRequestBehavior.AllowGet);
                }

                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Error, parametro inválido", JsonRequestBehavior.AllowGet);
            }
        }
        // GET: Alumnos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            Alumnos alumnos = db.Alumnos.Find(id);
            if (alumnos == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdColegio = new SelectList(db.AspNetUsers, "Id", "UserName", alumnos.IdColegio);
            return View(alumnos);
        }

        // POST: Alumnos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Alumnos alumnos)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                db.Entry(alumnos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdColegio = new SelectList(db.AspNetUsers, "Id", "UserName", alumnos.IdColegio);
            return View(alumnos);
        }

        // GET: Alumnos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Alumnos alumnos = db.Alumnos.Find(id);
            if (alumnos == null)
            {
                return HttpNotFound();
            }
            return View(alumnos);
        }

        // POST: Alumnos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            Alumnos alumnos = db.Alumnos.Find(id);
            db.Alumnos.Remove(alumnos);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public string GenerarPDF(TalonPagoFacil talon, Talon talonweb)
        {
           
            try
            {

                string CodColegio = db.AspNetUsers.Where(m => m.UserName == User.Identity.Name).FirstOrDefault().CodigoColegio;
                string templateFile = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["templates"]) + "\\" + CodColegio + ".pdf"; 
                //string oldFile = @"C:/Projects/AspMvcIdentity-master/GAFER/pdfs/template/ordenpago.pdf";
                string newFile = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["repositorioTalones"]) + "\\" + CodColegio + "-"+ talon.datosTalon.IdAlumno +".pdf";

                string formatoDecimal = System.Configuration.ConfigurationManager.AppSettings["formatoDecimal"];
                //existo pdf previo
                if (System.IO.File.Exists(newFile))
                {
                    System.IO.File.Delete(newFile);
                }

                // open the reader
                PdfReader reader = new PdfReader(templateFile);
                Rectangle size = reader.GetPageSizeWithRotation(1);
                Document document = new Document(size);

                // open the writer
                FileStream fs = new FileStream(newFile, FileMode.Create, FileAccess.Write);
                PdfWriter writer = PdfWriter.GetInstance(document, fs);
                document.Open();

                // the pdf content

                PdfContentByte cb = writer.DirectContent;

                // select the font properties
                string pathFontArial = System.Configuration.ConfigurationManager.AppSettings["fontArial"];
                BaseFont bf = BaseFont.CreateFont(Server.MapPath(pathFontArial), BaseFont.CP1252, BaseFont.EMBEDDED);
                cb.SetColorFill(BaseColor.DARK_GRAY);
                cb.SetFontAndSize(bf, 11);

                //alumno Nombre
                cb.BeginText();
                int posX = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["alumnoX"]);
                int posY = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["alumnoY"]);

                string text = talon.datosTalon.Alumnos.Apellido.ToUpper() + " " + talon.datosTalon.Alumnos.Nombre;
                // put the alignment and coordinates here
                cb.ShowTextAligned(1, text, posX, posY, 0);
                cb.EndText();


                //alumno legajo
                cb.BeginText();
                posX = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["legajoAlumnoX"]);
                posY = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["legajoAlumnoY"]);

                text = talon.datosTalon.Alumnos.CodigoAlumno;
                // put the alignment and coordinates here
                cb.ShowTextAligned(1, text, posX, posY, 0);
                cb.EndText();

                //Nº talon - usa el nro de historial
                cb.BeginText();
                posX = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["nroX"]);
                posY = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["nroY"]);

                text = talon.datosTalon.Concepto;
                // put the alignment and coordinates here
                cb.ShowTextAligned(1, talon.datosTalon.IdHistorial.ToString(), posX, posY, 0);
                cb.EndText();


                //fecha Emision
                cb.BeginText();
                posX = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["fechaX"]);
                posY = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["fechaY"]);

                //text = talon.datosTalon.Concepto;
                // put the alignment and coordinates here
                cb.ShowTextAligned(1, talon.datosTalon.FechaEmision.Value.ToString("dd/MM/yyyy"), posX, posY, 0);
                cb.EndText();

                //Concepto
                cb.BeginText();
                posX = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["conceptoX"]);
                posY = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["conceptoY"]);

                text = talon.datosTalon.Concepto;
                // put the alignment and coordinates here
                cb.ShowTextAligned(1, text, posX, posY, 0);
                cb.EndText();

                cb.BeginText();
                text = talon.fechaVenc1.ToString("dd/MM/yyyy");
                posX = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["1VenPosX"]);
                posY = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["1VenPosY"]);
                // put the alignment and coordinates here
                cb.ShowTextAligned(1, text, posX, posY, 0);
                cb.EndText();

                //importe 1er venc
                cb.BeginText();

                text = talonweb.importe1;
                //ext = Convert.ToDouble(talon.Importe1).ToString("#.##");
                posX = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["1ImpPosX"]);
                posY = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["1ImpPosY"]);
                // put the alignment and coordinates here
                cb.ShowTextAligned(1, text, posX, posY, 0);
                cb.EndText();


                if (talon.datosTalon.AspNetUsers.CantidadVencimientos > 1)
                {

                    //2do vencimiento
                    cb.BeginText();
                    text = talon.fechaVenc2.ToString("dd/MM/yyyy");
                    posX = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["2VenPosX"]);
                    posY = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["2VenPosY"]);
                    // put the alignment and coordinates here
                    cb.ShowTextAligned(1, text, posX, posY, 0);
                    cb.EndText();

                    //2do vencimiento
                    cb.BeginText();
                    //text = talon.Importe2.ToString(nfi);
                    text = talonweb.importe2;
                    posX = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["2ImpPosX"]);
                    posY = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["2ImpPosY"]);
                    // put the alignment and coordinates here
                    cb.ShowTextAligned(1, text, posX, posY, 0);
                    cb.EndText();

                }

                if (talon.datosTalon.AspNetUsers.CantidadVencimientos > 2)
                {
                    //3er vencimiento
                    cb.BeginText();
                    text = talon.fechaVenc3.ToString("dd/MM/yyyy");
                    posX = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["3VenPosX"]);
                    posY = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["3VenPosY"]);
                    // put the alignment and coordinates here
                    cb.ShowTextAligned(1, text, posX, posY, 0);
                    cb.EndText();

                    //3er vencimiento
                    cb.BeginText();

                    // text = talon.Importe3.ToString(nfi);
                    text = talonweb.importe3;
                    posX = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["3ImpPosX"]);
                    posY = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["3ImpPosY"]);
                    // put the alignment and coordinates here
                    cb.ShowTextAligned(1, text, posX, posY, 0);
                    cb.EndText();
                }

                //CODIGO DE BARRA   
                string pathFontI25 = System.Configuration.ConfigurationManager.AppSettings["fontI25"];
                //bf = BaseFont.CreateFont(System.Configuration.ConfigurationManager.AppSettings["fontI2of5"], BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                bf = BaseFont.CreateFont(Server.MapPath(pathFontI25), BaseFont.CP1252, BaseFont.EMBEDDED);
                                cb.SetColorFill(BaseColor.BLACK);
                cb.SetFontAndSize(bf, 13);
                
                cb.BeginText();
              
                text = Helpers.I2of5.Interleaved25(talon.datosTalon.CodigoPagoFacil);

                //test
                //= Helpers.I2of5.Interleaved25("093702361750001619201653876317950016202184000075");
                // put the alignment and coordinates here
                posX = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["posCodigoX"]);
                posY = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["posCodigoY"]);
                cb.ShowTextAligned(Element.ALIGN_CENTER, text, posX, posY, 0);
                cb.EndText();

                // create the new page and add it to the pdf
                PdfImportedPage page = writer.GetImportedPage(reader, 1);
                cb.AddTemplate(page, 0, 0);

                // close the streams and voilá the file should be changed :)
                document.Close();
                fs.Close();
                writer.Close();
                reader.Close();

                return newFile;
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error al generar el PDF, intente nuevamente";
                log.Error("Error al generar PDF " + ex.Message);
                return null;
            }
            
        }

        public ActionResult SendMail(int id)
        {
            try
            {
                Alumnos alumno = db.Alumnos.Where(m => m.IdAlumno == id).FirstOrDefault();

                string CodColegio = (string)Session["codigoColegio"];

                string talon = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["repositorioTalones"]) + "\\" + CodColegio + "-" + alumno.IdAlumno + ".pdf";

                if (System.IO.File.Exists(@talon))
                {
                    string mensaje = "Se adjunta orden de pago por cuenta y orden de " +
                                    (string)Session["DenominacionColegio"] + "</br>" +
                                    "Muchas Gracias</br>" +
                                    "GAFER - Ordenes de Pago" ;
                    Mail oMail = new Mail(alumno.Mail, mensaje, "Orden de pago - " + (string)Session["DenominacionColegio"], talon);
                    
                    //y enviamos
                    if (oMail.enviaMail())
                    {
                        return Json("Success", JsonRequestBehavior.AllowGet);

                    }
                    else
                    {
                        log.Error("Error al intentar enviar el mail: " + oMail.error);
                        return Json("Error", JsonRequestBehavior.AllowGet);

                    }
                }
                else
                {
                    log.Error("No existe talon de pago para enviar, debe ser generardo antes.");
                    return Json("No hay disponible ningun talon de pago para enviar, debe ser generardo antes.", JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception ex)
            {

                log.Error("Error en metodo SENDMAIL - " + ex);
                return Json("Error en Send Mail", JsonRequestBehavior.AllowGet);
            }
            
        }
    
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult GetDataAlumnos(DataTableParams param)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                //Traer registros
                string IdColegio = (string)Session["idColegio"];
                IQueryable<Alumnos> memberCol = db.Alumnos.Where(m => m.IdColegio == IdColegio).AsQueryable();

                //Manejador de filtros
                int totalCount = memberCol.Count();
                IEnumerable<Alumnos> filteredMembers = memberCol;
                if (!string.IsNullOrEmpty(param.sSearch))
                {
                    filteredMembers = memberCol
                            .Where(m => m.Apellido.Contains(param.sSearch) ||
                               m.CodigoAlumno.Contains(param.sSearch) ||
                               m.Condicion.ToString().Contains(param.sSearch) ||
                               m.FechaAlta.ToString().Contains(param.sSearch) ||
                               m.Mail.Contains(param.sSearch) ||
                               m.Nombre.Contains(param.sSearch) ||
                               m.Observaciones.Contains(param.sSearch) ||
                               m.Telefono.Contains(param.sSearch));
                }
                //Manejador de orden
                var sortIdx = Convert.ToInt32(Request["iSortCol_0"]);
                Func<Alumnos, string> orderingFunction =
                                    (
                                 m => sortIdx == 0 ? m.CodigoAlumno :
                                      sortIdx == 1 ? m.Apellido :
                                      sortIdx == 2 ? m.Nombre :
                                      sortIdx == 3 ? m.Mail :
                                      sortIdx == 4 ? m.Observaciones :
                                      sortIdx == 5 ? m.Condicion.ToString() :
                                      sortIdx == 6 ? m.Telefono :
                                      sortIdx == 7 ? m.FechaAlta.ToString() :
                                      m.IdAlumno.ToString()
                                      );
                var sortDirection = Request["sSortDir_0"]; // asc or desc  
                if (sortDirection == "asc")
                    filteredMembers = filteredMembers.OrderBy(orderingFunction);
                else
                    filteredMembers = filteredMembers.OrderByDescending(orderingFunction);
                var displayedMembers = filteredMembers
                         .Skip(param.iDisplayStart)
                         .Take(param.iDisplayLength);

                //Manejardo de resultados
                var result = from a in displayedMembers
                             select new
                             {
                                 a.IdAlumno,
                                 a.CodigoAlumno,
                                 a.Apellido,
                                 a.Nombre,
                                 a.Mail,
                                 a.Observaciones,
                                 a.Condicion,
                                 a.Telefono,
                                 a.FechaAlta,
                                 a.Importe1,
                                 a.Importe2,
                                 a.Importe3

                             };
                //Se devuelven los resultados por json
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = totalCount,
                    iTotalDisplayRecords = filteredMembers.Count(),
                    aaData = result
                },
                   JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                log.Error("Error GETDataAlumno - " + ex);
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            
        }

        public ActionResult EliminarAlumnos(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                Alumnos alumnos = db.Alumnos.Where(m=> m.IdAlumno == id).FirstOrDefault();
                
                var childData = alumnos.Historial.ToList();
                foreach (var data in childData)
                {
                    db.Historial.Remove(data);
                }
                db.SaveChanges();

                if (alumnos != null)
                {
                   db.Alumnos.Remove(alumnos);
                   db.SaveChanges();
                }

                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetDataAlumnoForEdit(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {

                Alumnos alumnos = db.Alumnos.Where(m => m.IdAlumno == id).FirstOrDefault();

                if (alumnos != null)
                {
                    return Json(new {alumnos.Apellido,alumnos.IdAlumno,alumnos.CodigoAlumno, alumnos.Condicion, alumnos.Nombre,
                    alumnos.Importe1, alumnos.Importe2,alumnos.Importe3,alumnos.Mail, alumnos.Observaciones, alumnos.Telefono} , JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Error al traer Datos", JsonRequestBehavior.AllowGet);
                }

            }
            catch
            {
                return Json("Exception Error al conectarse", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult EditarAlumno(Alumnos alumno)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            if (alumno != null)
            {
                Alumnos updateAlumno = db.Alumnos.Find(alumno.IdAlumno);

                updateAlumno.Apellido = alumno.Apellido;
                updateAlumno.Nombre = alumno.Nombre;
                updateAlumno.CodigoAlumno = alumno.CodigoAlumno;
                updateAlumno.Condicion = alumno.Condicion;
                updateAlumno.Importe1 = alumno.Importe1;
                updateAlumno.Importe2 = alumno.Importe2;
                updateAlumno.Importe3 = alumno.Importe3;
                updateAlumno.Mail = alumno.Mail;
                updateAlumno.Observaciones = alumno.Observaciones;
                updateAlumno.Telefono = alumno.Telefono;

                try {
                    db.Entry(updateAlumno).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    return Json("Error al actualizar el alumno" + ex.Message, JsonRequestBehavior.AllowGet);
                }

                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Error parametro nulo", JsonRequestBehavior.AllowGet);
            }
                

        }

        public ActionResult GenerarTalon(Talon talon)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                if (talon != null){

                    Alumnos alum = db.Alumnos.Where(m => m.IdAlumno == talon.idAlumno).FirstOrDefault();

                    ///GENERO CODIGO PAGO FACIL CON LOS IMPORTES, FECHAS, CODIGO COLEGIO Y ALUMNO
                    AspNetUsers colegio = db.AspNetUsers.Where(m => m.UserName == User.Identity.Name).FirstOrDefault();

                    int? cantVencimientos = colegio.CantidadVencimientos;

                    //secuencia de digito verificador
                    string secuencia = "1";
                    
                    for (int i = 0; i < 56; i = i + 4)
                    {
                        secuencia = secuencia + "3579";
                    }

                    //armo codigo de barra

                    //codigo colegio 5 digitos
                    string codcolegio = colegio.CodigoColegio;
                    while (codcolegio.Length < 5) { codcolegio = "0" + codcolegio; }

                    string codigoBarra = "093" + codcolegio;

                    string imp1 = "", imp2 = "", imp3 = "";

                    //los importes tienen 6 digitos, 4 parte entera y dos para el decimal, sin separador
                    if (cantVencimientos > 0)
                    {
                        decimal importe1 = Convert.ToDecimal(talon.importe1.Replace(".", ","));
                        importe1 = Math.Round(importe1, 2, MidpointRounding.AwayFromZero);

                        var imp1_ = importe1.ToString().Split(',');
                        //imp1 = (Convert.ToDecimal(talon.importe1)).ToString();
                        while (imp1_[0].Length < 4) { imp1_[0] = "0" + imp1_[0]; }

                        if (imp1_.Length > 1)
                        {
                            
                            while (imp1_[1].Length < 2) { imp1_[1] = imp1_[1] + "0"; }
                        }
                        else
                        {
                            Array.Resize(ref imp1_, imp1_.Length + 1);
                            imp1_[1] = "00";
                        }
                        
                        imp1 = imp1_[0] + imp1_[1];
                    }
                    if (cantVencimientos > 1 )
                    {
                        decimal importe2 = Convert.ToDecimal(talon.importe2.Replace(".", ","));
                        importe2 = Math.Round(importe2, 2, MidpointRounding.AwayFromZero);

                        var imp2_ = importe2.ToString().Split(',');
                        //imp1 = (Convert.ToDecimal(talon.importe1)).ToString();
                        while (imp2_[0].Length < 4) { imp2_[0] = "0" + imp2_[0]; }

                        if (imp2_.Length > 1)
                        {
                            while (imp2_[1].Length < 2) { imp2_[1] = imp2_[1] + "0"; }
                        }
                        else
                        {
                            Array.Resize(ref imp2_, imp2_.Length + 1);
                            imp2_[1] = "00";
                        }
                        
                        imp2 = imp2_[0] + imp2_[1];
                    }
                    if (cantVencimientos > 2)
                    {
                        decimal importe3 = Convert.ToDecimal(talon.importe3.Replace(".", ","));
                        importe3 = Math.Round(importe3, 2, MidpointRounding.AwayFromZero);

                        var imp3_ = importe3.ToString().Split(',');
                        //imp1 = (Convert.ToDecimal(talon.importe1)).ToString();
                        while (imp3_[0].Length < 4) { imp3_[0] = "0" + imp3_[0]; }

                        if (imp3_.Length > 1)
                        {
                            while (imp3_[1].Length < 2) { imp3_[1] = imp3_[1] + "0"; }
                        }
                        else
                        {
                            Array.Resize(ref imp3_, imp3_.Length + 1);
                            imp3_[1] = "00";
                        }
                       
                        imp3 = imp3_[0] + imp3_[1];
                    }

                    //codgio alumno debe tener 9 digitos, completo con ceros delante
                    string codAlumno = alum.CodigoAlumno;
                    while (codAlumno.Length < 9) { codAlumno = "0" + codAlumno; }


                    if (cantVencimientos == 1) codigoBarra = codigoBarra + imp1 + talon.fechaVenc1_customPF + codAlumno;

                    if (cantVencimientos == 2) codigoBarra = codigoBarra + imp1 + talon.fechaVenc1_customPF + codAlumno
                                                            + imp2 + talon.fechaVenc2_customPF;

                    if (cantVencimientos == 3) codigoBarra = codigoBarra + imp1 + talon.fechaVenc1_customPF + codAlumno
                                                            + imp2 + talon.fechaVenc2_customPF + imp3 + talon.fechaVenc3_customPF;

                    //genero digito verificador
                    int dv = 0;
                    for (int j=0; j < codigoBarra.Length; j++)
                    {
                        dv = dv + Convert.ToInt32(secuencia[j]) * Convert.ToInt32(codigoBarra[j]);
                    }
                    dv = dv / 2;
                    dv = dv - (dv / 10) * 10;

                    //lo inserto al final del codigo barra
                    codigoBarra = codigoBarra + dv.ToString();

                    TalonPagoFacil talonPF = new TalonPagoFacil
                    {
                        datosTalon = new Historial
                        {
                            AspNetUsers = colegio,
                            Concepto = talon.concepto,
                            Alumnos = alum,
                            CodigoPagoFacil = codigoBarra,
                            FechaEmision = DateTime.Now,
                            IdColegio = colegio.Id,
                            IdAlumno = alum.IdAlumno,


                        },
                        Importe1 = Decimal.Round(Convert.ToDecimal(talon.importe1.Replace(".", ",")),2,MidpointRounding.AwayFromZero),
                        Importe2 = Decimal.Round(Convert.ToDecimal(talon.importe2.Replace(".", ",")), 2, MidpointRounding.AwayFromZero),
                        Importe3 = Decimal.Round(Convert.ToDecimal(talon.importe3.Replace(".", ",")), 2, MidpointRounding.AwayFromZero),
                        fechaVenc1 = DateTime.ParseExact(talon.fechaVenc1, "dd/MM/yyyy", CultureInfo.InvariantCulture), //talon.fechaVenc1,
                        fechaVenc2 = DateTime.ParseExact(talon.fechaVenc2, "dd/MM/yyyy", CultureInfo.InvariantCulture),//talon.fechaVenc2,
                        fechaVenc3 = DateTime.ParseExact(talon.fechaVenc3, "dd/MM/yyyy", CultureInfo.InvariantCulture)//talon.fechaVenc3,


                    };



                    //Guardo el historial del Talon generado                
                    db.Historial.Add(talonPF.datosTalon);
                    db.SaveChanges();

                

                    string urlNewPdf = GenerarPDF(talonPF, talon);

                    if (urlNewPdf== null)
                    {
                        return Json("Error al generar Pdf", JsonRequestBehavior.AllowGet);
                    }

                    return Json("Success", JsonRequestBehavior.AllowGet);

                }
                else
                {
                    return Json("Error talon nulo", JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                log.Error("Error al generar talon - " + ex.Message);
                return Json("Error " + ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DownloadPDF(int id)
        {
 
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            //directorio de talones generados
            string repositorio = System.Configuration.ConfigurationManager.AppSettings["repositorioTalones"];

            List<string> lstAllFileName = (from itemFile in Directory.GetFiles(Server.MapPath(repositorio), "*.pdf")select Path.GetFileNameWithoutExtension(itemFile)).Cast<string>().ToList();

            //busco y devuelve path del talon correspondiente al alumno
            foreach (string dir in lstAllFileName)
            {
                string nameFile = Session["codigoColegio"] + "-" + id.ToString();
                if (nameFile == dir)
                {
                    string pdfPath = Session["codigoColegio"] + "-" + id + ".pdf";
                    return Json(pdfPath, JsonRequestBehavior.AllowGet);
                }
            }

            log.Error("Error al intentar descargar el PDF");
            return Json("notFound", JsonRequestBehavior.AllowGet);
        }
       
    }
}