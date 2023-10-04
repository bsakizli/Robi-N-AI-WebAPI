using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;
using EmptorUtility.Models.Response;

namespace MailEntity
{
    public class MailService
    {
        //private readonly IConfiguration _configuration;

        //public MailService(AIServiceDbContext db, IConfiguration configuration)
        //{
        //    _db = db;
        //    _configuration = configuration;
        //}

        //_configuration.GetValue<string>("IronBarCode.LicenseKey");


        public bool WaitingEmptorSendMail(string TicketId, r_getMainResponsibleInfo _request, string CompanyName, DateTime WaitingDate)
        {
            try
            {
                var message = new MimeMessage();
                message.To.Add(MailboxAddress.Parse(_request.SubResponsibleEmail));
                message.Cc.Add(MailboxAddress.Parse(_request.MainResponsibleEmail));
                //message.From.AddRange(emailMessage.FromAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));

                message.From.Add(MailboxAddress.Parse("robin@bdh.com.tr"));

                message.Subject = String.Format("{0} numaralı kayıt beklemeye alınmıştır.",TicketId);


                //We will say we are sending HTML. But there are options for plaintext etc. 
                message.Body = new TextPart(TextFormat.Html)
                {

                    Text = String.Format(@"Sayın <b>{0}</b>,</br></br>

Talebiniz üzerine <b>{1}</b> numaralı ilgili firması <b>{2}</b> olan kayıt bekleme prosedürü sebebi ile <font color='red'><b>{3}</b></font>'a kadar beklemeye alınmıştır.
</br></br>
İyi Çalışmalar.</br>
Robi-N", _request.SubResponsibleFullName, TicketId, CompanyName, WaitingDate)
                };

                //Be careful that the SmtpClient class is the one from Mailkit not the framework!
                using (var emailClient = new SmtpClient())
                {
                    //The last parameter here is to use SSL (Which you should!)
                    emailClient.Connect("smtp-mail.outlook.com", 587, false);

                    //Remove any OAuth functionality as we won't be using it. 
                    emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

                    emailClient.Authenticate("robin@bdh.com.tr", "ea3zCPD998");

                    var tt = emailClient.Send(message);

                    emailClient.Disconnect(true);

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool SendMail()
        {
            try
            {
               
                var message = new MimeMessage();
                message.To.Add(MailboxAddress.Parse("baris.sakizli@bdh.com.tr"));
                message.To.Add(MailboxAddress.Parse("hakan.dansik@bdh.com.tr"));
                //message.From.AddRange(emailMessage.FromAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));

                message.From.Add(MailboxAddress.Parse("robin@bdh.com.tr"));

                message.Subject = "Deneme Mail";
                
                
                //We will say we are sending HTML. But there are options for plaintext etc. 
                message.Body = new TextPart(TextFormat.Html)
                {
                   
                    Text = @"<b>Deneme İçerik</b> <img src='https://www.bdh.com.tr/wp-content/uploads/2019/08/BDHLogo2019_160.png'>"
                };

                //Be careful that the SmtpClient class is the one from Mailkit not the framework!
                using (var emailClient = new SmtpClient())
                {
                    //The last parameter here is to use SSL (Which you should!)
                    emailClient.Connect("smtp-mail.outlook.com", 587, false);

                    //Remove any OAuth functionality as we won't be using it. 
                    emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

                    emailClient.Authenticate("robin@bdh.com.tr", "ea3zCPD998");

                    var tt = emailClient.Send(message);

                    emailClient.Disconnect(true);

                    return true;
                }
            } catch(Exception ex)
            {
                return false;
            }

           
        }


        public bool SendMailHtml(string html, string subject)
        {
            try
            {
                var bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = html;
                var message = new MimeMessage();
                message.To.Add(MailboxAddress.Parse("gamze.ozen@bdh.com.tr"));
                message.To.Add(MailboxAddress.Parse("baris.sakizli@bdh.com.tr"));
                message.To.Add(MailboxAddress.Parse("hakan.dansik@bdh.com.tr"));
                //message.From.AddRange(emailMessage.FromAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));

                message.From.Add(MailboxAddress.Parse("robin@bdh.com.tr"));

                message.Subject = subject;
                //We will say we are sending HTML. But there are options for plaintext etc. 
                message.Body = bodyBuilder.ToMessageBody();

                //Be careful that the SmtpClient class is the one from Mailkit not the framework!
                using (var emailClient = new SmtpClient())
                {
                    //The last parameter here is to use SSL (Which you should!)
                    emailClient.Connect("smtp-mail.outlook.com", 587, false);

                    //Remove any OAuth functionality as we won't be using it. 
                    emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

                    emailClient.Authenticate("robin@bdh.com.tr", "ea3zCPD998");


                    var tt = emailClient.Send(message);

                    emailClient.Disconnect(true);

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }


        }




    }
}