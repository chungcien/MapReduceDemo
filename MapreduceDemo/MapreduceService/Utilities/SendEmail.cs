using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Mail;
using System.Net;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.IO;

namespace WS_FingerPrinter
{
    public class SendEmail
    {
        public static Exception Send_Email(List<string> ToEmail, List<string> CC, string title, string content, bool isHtml, string logpath, ref List<string> mail_content)
        {

            for (int j = 0; j < 5; j++)
            {
                try
                {
                    MailMessage mail = new MailMessage();
                    string FromEmail = ConfigurationManager.AppSettings["Email"].ToString();
                    string Pass = ConfigurationManager.AppSettings["Password"].ToString();

                    SmtpClient smtpServer = new SmtpClient(ConfigurationManager.AppSettings["SMTP_Server"].ToString());
                    mail.From = new MailAddress(FromEmail);

                    if (ToEmail == null || ToEmail.Count == 0)
                    {
                        return new Exception("To Email is Null.");
                    }
                    for (int i = 0; i < ToEmail.Count; i++)
                    {
                        if (string.IsNullOrEmpty(ToEmail[i]))
                        {
                            continue;
                        }
                        mail.To.Add(ToEmail[i]); // Email cần đến
                    }

                    if (CC != null)
                    {
                        for (int i = 0; i < CC.Count; i++)
                        {
                            if (string.IsNullOrEmpty(CC[i]))
                            {
                                continue;
                            }
                            mail.CC.Add(CC[i]); // Email cần CC
                        }
                    }


                    mail.Subject = (title == "") ? "No subject" : title;
                    mail.Body = content;
                    mail.IsBodyHtml = isHtml;
                    mail.Priority = MailPriority.High;
                    smtpServer.Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"].ToString());
                    smtpServer.Credentials = new NetworkCredential(FromEmail, Pass);
                    smtpServer.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"].ToString());

                    Disable_CertificateValidation();

                    smtpServer.Send(mail);

                    File_Read_Write.Write_File(logpath, DateTime.Now.ToString() + ": Sending email \"" + title + "\" TO " + String.Join(", ", ToEmail.ToArray()) + " and " + ((CC == null) ? "no CC" : "CC to " + String.Join(", ", CC.ToArray())) + " Succufully!", true, ref mail_content);

                    break;
                }
                catch (Exception er)
                {
                    File_Read_Write.Write_File(logpath, DateTime.Now.ToString() + ": Try " + (j + 1) + ": Error - Sending email \"" + title + "\" TO " + String.Join(", ", ToEmail.ToArray()) + " and " + ((CC == null) ? "no CC" : "CC to " + String.Join(", ", CC.ToArray())) + " error!" + er.Message, true, ref mail_content);
                }
            }
            return null;
        }

        public static Exception Send_Email(string FromName, List<string> ToEmail, List<string> CC, string title, string content, bool isHtml, string logpath, ref List<string> mail_content)
        {

            for (int j = 0; j < 5; j++)
            {
                try
                {
                    MailMessage mail = new MailMessage();
                    string FromEmail = ConfigurationManager.AppSettings["Email"].ToString();
                    string Pass = ConfigurationManager.AppSettings["Password"].ToString();

                    SmtpClient smtpServer = new SmtpClient(ConfigurationManager.AppSettings["SMTP_Server"].ToString());
                    mail.From = new MailAddress(FromEmail, FromName);

                    if (ToEmail == null || ToEmail.Count == 0)
                    {
                        return new Exception("To Email is Null.");
                    }
                    for (int i = 0; i < ToEmail.Count; i++)
                    {
                        if (string.IsNullOrEmpty(ToEmail[i]))
                        {
                            continue;
                        }
                        mail.To.Add(ToEmail[i]); // Email cần đến
                    }

                    if (CC != null)
                    {
                        for (int i = 0; i < CC.Count; i++)
                        {
                            if (string.IsNullOrEmpty(CC[i]))
                            {
                                continue;
                            }
                            mail.CC.Add(CC[i]); // Email cần CC
                        }
                    }


                    mail.Subject = (title == "") ? "No subject" : title;
                    mail.Body = content;
                    mail.IsBodyHtml = isHtml;
                    mail.Priority = MailPriority.High;
                    smtpServer.Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"].ToString());
                    smtpServer.Credentials = new NetworkCredential(FromEmail, Pass);
                    smtpServer.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"].ToString());

                    Disable_CertificateValidation();

                    smtpServer.Send(mail);

                    File_Read_Write.Write_File(logpath, DateTime.Now.ToString() + ": Sending email \"" + title + "\" TO " + String.Join(", ", ToEmail.ToArray()) + " and " + ((CC == null) ? "no CC" : "CC to " + String.Join(", ", CC.ToArray())) + " Succufully!", true, ref mail_content);

                    break;
                }
                catch (Exception er)
                {
                    File_Read_Write.Write_File(logpath, DateTime.Now.ToString() + ": Try " + (j + 1) + ": Error - Sending email \"" + title + "\" TO " + String.Join(", ", ToEmail.ToArray()) + " and " + ((CC == null) ? "no CC" : "CC to " + String.Join(", ", CC.ToArray())) + " error!" + er.Message, true, ref mail_content);
                }
            }
            return null;
        }



        static void Disable_CertificateValidation()
        {
            ServicePointManager.ServerCertificateValidationCallback =
                delegate (
                    object s,
                    X509Certificate certificate,
                    X509Chain chain,
                    SslPolicyErrors sslPolicyErrors
                )
                {
                    return true;
                };
        }
    }
}
