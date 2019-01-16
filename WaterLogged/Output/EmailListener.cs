using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Mail;

namespace WaterLogged.Output
{
    //TODO: 
    //Add throttling to the write. To allow this to support a large number of instances/apps, sending them in batches every 30 seconds, or whatever user sets
    //would be a nice feature add. Background task and collection of events pushed into one message at the interval would probably be best.

    /// <summary>
    /// A class to send logging updates via email. 
    /// </summary>
    public class EmailListener : Listener
    {        
        public string UserName { get; set; }
        public string UserPass { get; set; }
        public int Port { get; set; }
        public bool SSL { get; set; }
        public string EmailSubject { get; set; }  
        public string SMTPHost { get; set; }
        public string FromAddress { get; set; }

        //A hacky way to have a threadsafe collection with no duplicates, get it together C#.
        private ConcurrentDictionary<string, byte> _recipients;

        /// <summary>
        /// Contruct the email listener with host, port, security, netwrok crednetials, and a list of reciepients
        /// </summary>
        /// <param name="host">Location of the SMTP Server</param>        
        /// <param name="ssl">Use an SSL connection</param>
        /// <param name="userName">user name for netwrok credentials</param>
        /// <param name="userPass">Password for network credentials</param>
        /// <param name="recipients">A list of recipients to recieve email alerts</param>
        /// <param name="port">Port for the smpt server, defaults to 587</param>
        /// <param name="emailSubject">Email message subject</param>
        public EmailListener(string host, string fromAddress, bool ssl = false, int port = 587, string emailSubject = "WaterLogged Message", 
                                IEnumerable<string> recipients = null, string userName = null, string userPass = null)
        {
            SMTPHost = host;
            Port = port;
            SSL = ssl;
            UserName = userName;
            UserPass = userPass;
            EmailSubject = emailSubject;
            FromAddress = fromAddress;

            _recipients = new ConcurrentDictionary<string, byte>();

            if (recipients != null)
            {
                foreach (string val in recipients)
                {
                    _recipients.TryAdd(val, 0);
                }
            }
        }

        /// <summary>
        /// Add a recipient to the list of email addresses to get log updates
        /// </summary>
        /// <param name="email">Email address of the recipient</param>
        /// <returns></returns>
        public bool AddRecipient(string email)
        {
            return _recipients.TryAdd(email, 0);
        }

        /// <summary>
        /// Remove a recipient from the list of email addresses to get log updates 
        /// </summary>
        /// <param name="email">Email address of the recipient</param>
        /// <returns></returns>
        public bool RemoveRecipient(string email)
        {
            return _recipients.TryRemove(email, out byte val);
        }

        public async override void Write(string value, string tag)
        {
            string message = "Logging update: <br />" + value;

            try
            {
                using (var mailer = new SmtpClient())
                {
                    mailer.Host = SMTPHost;
                    mailer.Port = Port;
                    mailer.EnableSsl = SSL;

                    if (!string.IsNullOrEmpty(UserName))
                    {
                        mailer.Credentials = new System.Net.NetworkCredential(UserName, UserPass);
                    }

                    var mail = new MailMessage
                    {
                        Subject = EmailSubject,
                        From = new MailAddress(FromAddress),
                        Body = message,
                        IsBodyHtml = true
                    };

                    foreach (string toEmail in _recipients.Keys)
                    {
                        mail.To.Add(toEmail);
                    }

                    await mailer.SendMailAsync(mail);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
