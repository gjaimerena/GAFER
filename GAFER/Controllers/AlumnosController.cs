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

namespace GAFER.Controllers
{
    public class AlumnosController : Controller
    {
        private GAFEREntities db = new GAFEREntities();

        private static string codigoColegio = "";

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

            AspNetUsers user = db.AspNetUsers.Where(m => m.UserName == User.Identity.Name).FirstOrDefault();
            int? cantVencimientos = user.CantidadVencimientos;
            codigoColegio = user.CodigoColegio;

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
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                return Json("Codigo Alumno es obligatorio", JsonRequestBehavior.AllowGet);
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
                    alumno.IdColegio = db.AspNetUsers.Where(m => m.UserName == User.Identity.Name).FirstOrDefault().Id;

                    db.Alumnos.Add(alumno);
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
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        public string GenerarPDF(TalonPagoFacil talon)
        {
            
            try
            {

                string CodColegio = db.AspNetUsers.Where(m => m.UserName == User.Identity.Name).FirstOrDefault().CodigoColegio;
                string templateFile = System.Configuration.ConfigurationManager.AppSettings["templates"] + "\\" + CodColegio + ".pdf"; 
                //string oldFile = @"C:/Projects/AspMvcIdentity-master/GAFER/pdfs/template/ordenpago.pdf";
                string newFile = System.Configuration.ConfigurationManager.AppSettings["repositorioTalones"] + "\\" + CodColegio + "-"+ talon.datosTalon.IdAlumno +".pdf";

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
                BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                cb.SetColorFill(BaseColor.DARK_GRAY);
                cb.SetFontAndSize(bf, 8);

                // write the text in the pdf content
                cb.BeginText();
                string text = "Codigo Alumno: " + talon.datosTalon.Alumnos.CodigoAlumno;
                // put the alignment and coordinates here
                cb.ShowTextAligned(1, text, 520, 640, 0);
                cb.EndText();

                bf = BaseFont.CreateFont(@"C:\Projects\AspMvcIdentity-master\GAFER\fonts\I25HRE__.TTF", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                cb.SetColorFill(BaseColor.DARK_GRAY);
                cb.SetFontAndSize(bf, 12);


                cb.BeginText();
                text = "123456789123456789";
                // put the alignment and coordinates here
                cb.ShowTextAligned(1, text, 100, 200, 0);
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
                System.Console.WriteLine(ex.Message);
                return null;
            }
            
        }

        public ActionResult mostrarTalon()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            string ruta = @"../pdfs/newFile.pdf";
            if (ruta != null)
            {
                ViewBag.ruta = ruta;
                return View("Talon");
            }
            else
            {
                ViewBag.Message = "No se pudo mostrar el pdf, consulte al soporte";
                return View("Pago");
            }
        }

        public ActionResult SendMail(int id)
        {

            try
            {
                Alumnos alumno = db.Alumnos.Where(m => m.IdAlumno == id).FirstOrDefault();


                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);

                client.EnableSsl = true;

                MailAddress from = new MailAddress("gabrieljaimerena@gmail.com", "GAFER - Emision de Pago Fácil");

                MailAddress to = new MailAddress(alumno.Mail, alumno.Apellido + " " + alumno.Nombre);

                MailMessage message = new MailMessage(from, to);

                message.Body = "This is a test e-mail message sent using gmail as a relay server ";

                string user = db.AspNetUsers.Where(m => m.CodigoColegio == codigoColegio).FirstOrDefault().UserName;
                message.Subject = "Nueva solicitud de pago emitida por " + user;

                NetworkCredential myCreds = new NetworkCredential("gabrieljaimerena@gmail.com", "“CHICOtazo1541", "");

                client.Credentials = myCreds;
                client.Send(message);

                return Json("Success", JsonRequestBehavior.AllowGet);
               // ViewBag.message = "El correo se envio exitosamente";
            }

            catch (Exception ex)
            {
                Console.WriteLine("Exception is: " + ex.ToString());
                return Json("Error", JsonRequestBehavior.AllowGet);
               // ViewBag.message = "Hubo un error al enviar el correo, intente nuevamente";
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

            //Traer registros
            string IdColegio = db.AspNetUsers.Where(m => m.UserName == User.Identity.Name).FirstOrDefault().Id;
            IQueryable<Alumnos> memberCol = db.Alumnos.Where(m=> m.IdColegio == IdColegio).AsQueryable();

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
                        var imp1_ = talon.importe1.Split('.');
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
                        var imp2_ = talon.importe2.Split('.');
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
                        var imp3_ = talon.importe3.Split('.');
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
                            

                            Alumnos = alum,
                            CodigoPagoFacil = codigoBarra,
                            FechaEmision = DateTime.Now,
                            IdColegio = colegio.Id,
                            IdAlumno = alum.IdAlumno,


                        },
                        Importe1 = Convert.ToDecimal(talon.importe1),
                        Importe2 = Convert.ToDecimal(talon.importe2),
                        Importe3 = Convert.ToDecimal(talon.importe3),
                        fechaVenc1 = DateTime.ParseExact(talon.fechaVenc1, "dd/MM/yyyy", CultureInfo.InvariantCulture), //talon.fechaVenc1,
                        fechaVenc2 = DateTime.ParseExact(talon.fechaVenc2, "dd/MM/yyyy", CultureInfo.InvariantCulture),//talon.fechaVenc2,
                        fechaVenc3 = DateTime.ParseExact(talon.fechaVenc3, "dd/MM/yyyy", CultureInfo.InvariantCulture)//talon.fechaVenc3,


                    };



                    //Guardo el historial del Talon generado                
                    db.Historial.Add(talonPF.datosTalon);
                    db.SaveChanges();

                    string urlNewPdf = GenerarPDF(talonPF);

                    return Json("Success", JsonRequestBehavior.AllowGet);

                }
                else
                {
                    return Json("Error talon nulo", JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {

                return Json("Error " + ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DownloadPDF(int id)
        {
            //string[] dirs = Directory.GetFiles(@"C:\Projects\AspMvcIdentity-master\GAFER\pdfs\", "*.pdf");

            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            string repositorio = System.Configuration.ConfigurationManager.AppSettings["repositorioTalones"];

            List<string> lstAllFileName = (from itemFile in Directory.GetFiles(repositorio, "*.pdf")
                                           select Path.GetFileNameWithoutExtension(itemFile)).Cast<string>().ToList();
            foreach (string dir in lstAllFileName)
            {
                string nameFile = codigoColegio + "-" + id.ToString();
                if (nameFile == dir)
                {
                    string pdfPath = codigoColegio + "-" + id + ".pdf";
                    return Json(pdfPath, JsonRequestBehavior.AllowGet);
                }
            }
            return Json("notFound", JsonRequestBehavior.AllowGet);
        }

       
    }
}
