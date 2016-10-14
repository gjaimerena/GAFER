using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace GAFER.Helpers
{
    class Mail
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //  string From = ""; //de quien procede, puede ser un alias
        string To;  //a quien vamos a enviar el mail
        string Message;  //mensaje
        string Subject; //asunto
        List<string> Archivo = new List<string>(); //lista de archivos a enviar

     

        string DE = System.Configuration.ConfigurationManager.AppSettings["UserMail"]; //nuestro usuario de smtp
        string PASS = System.Configuration.ConfigurationManager.AppSettings["PassUserMail"]; //nuestro password de smtp
        string ServerSMTP = System.Configuration.ConfigurationManager.AppSettings["ServidorSMTP"];
        string PortSMTP = System.Configuration.ConfigurationManager.AppSettings["PortSMTP"];
        System.Net.Mail.MailMessage Email;

        public string error = "";

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="FROM">Procedencia</param>
        /// <param name="Para">Mail al cual se enviara</param>
        /// <param name="Mensaje">Mensaje del mail</param>
        /// <param name="Asunto">Asunto del mail</param>
        /// <param name="ArchivoPedido_">Archivo a adjuntar, no es obligatorio</param>
        public Mail(string Para, string Mensaje, string Asunto, List<string> ArchivoPedido_ = null)
        {
           // From = FROM;
            To = Para;
            Message = Mensaje;
            Subject = Asunto;
            Archivo = ArchivoPedido_;

        }

        /// <summary>
        /// metodo que envia el mail
        /// </summary>
        /// <returns></returns>
        public bool enviaMail()
        {

            //una validación básica
            if (To.Trim().Equals("") || Message.Trim().Equals("") || Subject.Trim().Equals(""))
            {
                error = "El mail, el asunto y el mensaje son obligatorios";
                return false;
            }

            //aqui comenzamos el proceso
            //comienza-------------------------------------------------------------------------
            try
            {
                //creamos un objeto tipo MailMessage
                //este objeto recibe el sujeto o persona que envia el mail,
                //la direccion de procedencia, el asunto y el mensaje
                Email = new System.Net.Mail.MailMessage(DE, To, Subject, Message);

                //si viene archivo a adjuntar
                //realizamos un recorrido por todos los adjuntos enviados en la lista
                //la lista se llena con direcciones fisicas, por ejemplo: c:/pato.txt
                if (Archivo != null)
                {
                    //agregado de archivo
                    foreach (string archivo in Archivo)
                    {
                        //comprobamos si existe el archivo y lo agregamos a los adjuntos
                        if (System.IO.File.Exists(@archivo))
                            Email.Attachments.Add(new Attachment(@archivo));

                    }
                }

                Email.IsBodyHtml = true; //definimos si el contenido sera html
                Email.From = new MailAddress(DE); //definimos la direccion de procedencia

                //aqui creamos un objeto tipo SmtpClient el cual recibe el servidor que utilizaremos como smtp
                //en este caso me colgare de gmail
                System.Net.Mail.SmtpClient smtpMail = new System.Net.Mail.SmtpClient(ServerSMTP);

                smtpMail.EnableSsl = false;//le definimos si es conexión ssl
                smtpMail.UseDefaultCredentials = false; //le decimos que no utilice la credencial por defecto
                smtpMail.Host = ServerSMTP; //agregamos el servidor smtp
                smtpMail.Port = Convert.ToInt32(PortSMTP); //le asignamos el puerto, en este caso gmail utiliza el 465
                smtpMail.Credentials = new System.Net.NetworkCredential(DE, PASS); //agregamos nuestro usuario y pass de gmail

                //enviamos el mail
                smtpMail.Send(Email);

                //eliminamos el objeto
                smtpMail.Dispose();

                //regresamos true
                return true;
            }
            catch (Exception ex)
            {
                //si ocurre un error regresamos false y el error
                log.Error("Ocurrio un error: " + ex.Message);
                error = "Ocurrio un error: " + ex.Message;
                return false;
            }

            return false;

        }
    }
}