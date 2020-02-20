using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLayer.General
{
    public class Email
    {
        public string EmailHost;
        public string EmailPort;
        public string EmailUsername;
        public string EmailPwd;
        public bool EmailSsl;

        public void bindCredentials()
        {
            EmailHost = System.Configuration.ConfigurationManager.AppSettings["EmailHost"].ToString();
            EmailPort = System.Configuration.ConfigurationManager.AppSettings["EmailPort"].ToString();
            EmailUsername = System.Configuration.ConfigurationManager.AppSettings["EmailUsername"].ToString();
            EmailPwd = System.Configuration.ConfigurationManager.AppSettings["EmailPwd"].ToString();
            EmailSsl = bool.Parse(System.Configuration.ConfigurationManager.AppSettings["EmailSsl"].ToString());
        }
        public string SendMail(bool isDefaultCredentials, string ToAddress, string CCAddress, string msg, string subject, string EmailName, MemoryStream attachment = null)
        {
            if (isDefaultCredentials)
                bindCredentials();

            string val = "";
            if (ToAddress != "")
            {
                try
                {
                    System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
                    client.Credentials = new System.Net.NetworkCredential(EmailUsername, EmailPwd);
                    client.Port = Convert.ToInt32(EmailPort);
                    client.Host = EmailHost;
                    client.EnableSsl = EmailSsl;
                    System.Net.Mail.MailMessage MyMsg = new System.Net.Mail.MailMessage();
                    MyMsg.From = new System.Net.Mail.MailAddress(EmailUsername, EmailName);
                    MyMsg.IsBodyHtml = true;
                    MyMsg.Body = msg.ToString();
                    MyMsg.Subject = subject;
                    MyMsg.To.Add(ToAddress);
                    if (CCAddress != "")
                        MyMsg.CC.Add(CCAddress);
                    if (attachment != null)
                        MyMsg.Attachments.Add(new System.Net.Mail.Attachment(attachment, "Appointment.ics", "text/calendar"));
                    //client.Send(MyMsg);
                    SendEmailDelegate sd = new SendEmailDelegate(client.Send);
                    AsyncCallback cb = new AsyncCallback(SendEmailResponse);
                    sd.BeginInvoke(MyMsg, cb, sd);

                    val = "1";
                }
                catch (Exception ex)
                {
                    val = ex.Message;
                }

            }
            return val;
        }

        private delegate void SendEmailDelegate(System.Net.Mail.MailMessage m);

        private static void SendEmailResponse(IAsyncResult ar)
        {
            try
            {
                SendEmailDelegate sd = (SendEmailDelegate)(ar.AsyncState);
                sd.EndInvoke(ar);
            }
            catch (Exception ex)
            { }
        }
    }
}
