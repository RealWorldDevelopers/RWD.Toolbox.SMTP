using System;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RWD.Toolbox.SMTP
{
    /// <inheritdoc />
    public class EmailAgent : IEmailAgent
    {
        /// <summary>
        /// Constructor without parameters.
        /// </summary>
        /// <remarks>Using this constructor be sure to set the needed properties to insure a good connection to the server.</remarks>
        public EmailAgent()
        {           
        }

        /// <summary>
        /// Recommended Constructor.  This constructor take the needed properties for a good connection to the SMTP server as parameters.
        /// </summary>
        /// <param name="ip">Domain name or IP address to the SMTP server as <see cref="string"/>.</param>
        /// <param name="port">Port to use when connecting as <see cref="int"/>.</param>
        /// <param name="useSsl">Should connection use SSL as <see cref="bool"/>.</param>
        /// <param name="userName">Authorized user name to connect to server with as <see cref="string"/>.</param>
        /// <param name="userPassword">Password to authenticate with as <see cref="string"/>.</param>
        public EmailAgent(string ip, int port, bool useSsl, string userName, string userPassword)
        {
            SMTP_IP = ip;
            SMTP_Port = port;
            SMTP_Ssl = useSsl;
            SMTP_User = userName;
            SMTP_Password = userPassword;
        }

        /// <inheritdoc />
        public string RegEx_Email { get; set; } = @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";

        /// <inheritdoc />
        public string RegEx_Ip { get; set; } = @"(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])";

        /// <inheritdoc />
        public string RegEx_Url { get; set; } = @"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$";

        /// <inheritdoc />
        public string SMTP_IP { get; set; }

        /// <inheritdoc />
        public int SMTP_Port { get; set; }

        /// <inheritdoc />
        public bool SMTP_Ssl { get; set; } 

        /// <inheritdoc />
        public string SMTP_User { get; set; } 

        /// <inheritdoc />
        public string SMTP_Password { get; set; } 

             

        /// <inheritdoc />
        public void SendEmail(string fromAddress, string fromName, string toEmailAddress, string subjectLine, string emailBody, bool bodyIsHTML, string[] fileAttachmentPaths = null)
        {
            // change single To: email into array and process
            var toArray = new string[] { toEmailAddress };
            SendEmail(fromAddress, fromName, toArray, subjectLine, emailBody, bodyIsHTML, fileAttachmentPaths);
        }

        /// <inheritdoc />
        public async Task SendEmailAsync(string fromAddress, string fromName, string toEmailAddress, string subjectLine, string emailBody, bool bodyIsHTML, string[] fileAttachmentPaths = null)
        {
            // change single To: email into array and process
            var toArray = new string[] { toEmailAddress };
            await SendEmailAsync(fromAddress, fromName, toArray, subjectLine, emailBody, bodyIsHTML, fileAttachmentPaths);
        }

        /// <inheritdoc />
        public void SendEmail(string fromAddress, string fromName, string[] toEmailAddresses, string subjectLine, string emailBody, bool bodyIsHTML, string[] fileAttachmentPaths = null)
        {
            // build the message object
            var msg = BuildMessageObject(fromAddress, fromName, toEmailAddresses, subjectLine, emailBody, bodyIsHTML, fileAttachmentPaths);

            // send the email
            SendMessage(msg);
        }

        /// <inheritdoc />
        public async Task SendEmailAsync(string fromAddress, string fromName, string[] toEmailAddresses, string subjectLine, string emailBody, bool bodyIsHTML, string[] fileAttachmentPaths = null)
        {
            // build the message object
            var msg = BuildMessageObject(fromAddress, fromName, toEmailAddresses, subjectLine, emailBody, bodyIsHTML, fileAttachmentPaths);

            // send the email
            await SendMessageAsync(msg);
        }

        /// <inheritdoc />
        public bool IsValidEmailAddress(string email)
        {
            bool isValid = false;
            if (!string.IsNullOrWhiteSpace(email))
            {
                isValid = Regex.IsMatch(email, RegEx_Email, RegexOptions.IgnoreCase);
            }
            return isValid;
        }

        /// <inheritdoc />
        public bool IsValidIpAddress(string ip)
        {
            bool isValid = false;
            if (!string.IsNullOrWhiteSpace(ip))
            {
                if (Regex.IsMatch(ip, RegEx_Url, RegexOptions.IgnoreCase) || Regex.IsMatch(ip, RegEx_Ip, RegexOptions.IgnoreCase))
                    isValid = true;
            }
            return isValid;
        }

        /// <summary>
        /// Build an message object for the SMTP client to send.
        /// </summary>
        /// <param name="fromAddress">From: header of an Email.  Used with reader hits Reply. <see cref="string"/></param>
        /// <param name="fromName">User friendly display name associated with From: header of an Email. <see cref="string"/></param>
        /// <param name="toEmailAddresses">From: header of an Email. This is an array of intended recipients of the email. <see cref="T:string[]"/></param>
        /// <param name="subjectLine">Subject: header of an Email. This should be a one line summary of email. <see cref="string"/></param>
        /// <param name="emailBody">This is Body or content of the email.  Can be written in plain text or HTML. <see cref="string"/></param>
        /// <param name="bodyIsHTML">This tells SMTP server that content is in HTML format. <see cref="bool"/></param>
        /// <param name="fileAttachmentPaths">This is an OPTIONAL array of full paths pointing to files attachments. <see cref="T:string[]"/> </param>
        /// <returns>Message Object as <see cref="MailMessage"/></returns>
        private MailMessage BuildMessageObject(string fromAddress, string fromName, string[] toEmailAddresses, string subjectLine, string emailBody, bool bodyIsHTML, string[] fileAttachmentPaths = null)
        {
            // validate inputs
            if (!IsValidEmailAddress(fromAddress))
                throw new Exception("You must supply a valid 'FromTo: Email Address");

            if (string.IsNullOrWhiteSpace(fromName))
                fromName = fromAddress;

            if (string.IsNullOrWhiteSpace(subjectLine))
                throw new NullReferenceException("Your must supply a valid 'Subject:' line");

            if (string.IsNullOrWhiteSpace(emailBody))
                throw new NullReferenceException("Your must supply content for the email body");

            // email object
            var mailMsg = new MailMessage()
            {
                From = new System.Net.Mail.MailAddress(fromAddress, fromName),
                Subject = subjectLine,
                IsBodyHtml = bodyIsHTML,
                Body = emailBody
            };

            // validate To Addresses
            if (toEmailAddresses == null)
                throw new NullReferenceException("Your must supply at least one valid 'To: Email Address");

            // handle additional recipients
            foreach (var address in toEmailAddresses)
            {
                if (!string.IsNullOrWhiteSpace(address))
                {
                    if (IsValidEmailAddress(address.Trim()))
                        mailMsg.To.Add(address.Trim());
                }
            }

            if (mailMsg.To.Count < 1)
                throw new IndexOutOfRangeException("You must supply at least one valid 'To: Email Address");

            // check for attachments and handle
            if (fileAttachmentPaths != null)
            {
                foreach (var filePath in fileAttachmentPaths)
                {
                    if (!string.IsNullOrWhiteSpace(filePath))
                    {
                        var strFile = new System.Net.Mail.Attachment(filePath);
                        mailMsg.Attachments.Add(strFile);
                    }
                }
            }

            return mailMsg;

        }

        /// <summary>
        /// Create a new SMTP Client and send a message asynchronously.
        /// </summary>
        /// <param name="mailMsg">Message Object as <see cref="MailMessage"/></param>
        /// <returns></returns>
        private async Task SendMessageAsync(MailMessage mailMsg)
        {
            // validate inputs
            if (!IsValidIpAddress(SMTP_IP))
                throw new Exception("A properly formatted IP Address to the SMTP server must be submitted.");

            var SMTP_MailClient = new SmtpClient
            {
                Host = SMTP_IP,
                Port = SMTP_Port,
                EnableSsl = SMTP_Ssl,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(SMTP_User, SMTP_Password)
            };
            await SMTP_MailClient.SendMailAsync(mailMsg);
            SMTP_MailClient.Dispose();
        }

        /// <summary>
        /// Create a new SMTP Client and send a message.
        /// </summary>
        /// <param name="mailMsg">Message Object as <see cref="MailMessage"/></param>
        private void SendMessage(MailMessage mailMsg)
        {
            if (!IsValidIpAddress(SMTP_IP))
                throw new Exception("A properly formatted IP Address to the SMTP server must be submitted.");

            var SMTP_MailClient = new SmtpClient
            {
                Host = SMTP_IP,
                Port = SMTP_Port,
                EnableSsl = SMTP_Ssl,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(SMTP_User, SMTP_Password)
            };
            SMTP_MailClient.Send(mailMsg);
            SMTP_MailClient.Dispose();
        }

    }
}
