using System.Threading.Tasks;

namespace RWD.Toolbox.SMTP
{  
    /// <summary>
    /// A tool used to assist with sending messages from a SMTP email server.
    /// </summary>
    public interface IEmailAgent
    {
        /// <summary>
        /// Regular Expression used to validate IP property.
        /// </summary>
        string RegEx_Ip { get; set; }

        /// <summary>
        /// Regular Expression used to validate URL property (Domains).
        /// </summary>
        string RegEx_Url { get; set; }

        /// <summary>
        /// Regular Expression used to validate emails.
        /// </summary>
        string RegEx_Email { get; set; }

        /// <summary>
        /// Domain name or IP address used to connect to the SMTP server.
        /// </summary>
        string SMTP_IP { get; set; }

        /// <summary>
        /// Password used to authenticate connection to SMTP server.
        /// </summary>
        string SMTP_Password { get; set; }

        /// <summary>
        /// Port use to connect to the SMTP server.
        /// </summary>
        int SMTP_Port { get; set; }

        /// <summary>
        /// Use SSL to connect to server.
        /// </summary>
        bool SMTP_Ssl { get; set; }

        /// <summary>
        /// User name used to authenticate connection to SMTP server.
        /// </summary>
        string SMTP_User { get; set; }

        /// <summary>
        /// Determine if a string is a properly formatted email address.
        /// </summary>
        /// <param name="email">Email Address to Validate as <see cref="string"/></param>
        /// <returns>Is Properly Formated Email Address as <see cref="bool"/></returns>
        bool IsValidEmailAddress(string email);
        
        /// <summary>
        /// Determine if a string is a properly formatted IP address.
        /// </summary>
        /// <param name="ip">IP Address to Validate as <see cref="string"/></param>
        /// <returns>Is Properly Formated Email Address as <see cref="bool"/></returns>
        bool IsValidIpAddress(string ip);

        /// <summary>
        /// Send and email to a single recipient.
        /// </summary>
        /// <param name="fromAddress">From: header of an Email.  Used with reader hits Reply. <see cref="string"/></param>
        /// <param name="fromName">User friendly display name associated with From: header of an Email. <see cref="string"/></param>
        /// <param name="toEmailAddress">From: header of an Email. This is the intended recipient of email. <see cref="string"/></param>
        /// <param name="subjectLine">Subject: header of an Email. This should be a one line summary of email. <see cref="string"/></param>
        /// <param name="emailBody">This is Body or content of the email.  Can be written in plain text or HTML. <see cref="string"/></param>
        /// <param name="bodyIsHTML">This tells SMTP server that content is in HTML format. <see cref="bool"/></param>
        /// <param name="fileAttachmentPaths">This is an OPTIONAL array of full paths pointing to files attachments. <see cref="T:string[]"/> </param>
        void SendEmail(string fromAddress, string fromName, string toEmailAddress, string subjectLine, string emailBody, bool bodyIsHTML, string[] fileAttachmentPaths = null);

        /// <summary>
        /// Send and email to multiple recipients.
        /// </summary>
        /// <param name="fromAddress">From: header of an Email.  Used with reader hits Reply. <see cref="string"/></param>
        /// <param name="fromName">User friendly display name associated with From: header of an Email. <see cref="string"/></param>
        /// <param name="toEmailAddresses">From: header of an Email. This is an array of intended recipients of the email. <see cref="T:string[]"/></param>
        /// <param name="subjectLine">Subject: header of an Email. This should be a one line summary of email. <see cref="string"/></param>
        /// <param name="emailBody">This is Body or content of the email.  Can be written in plain text or HTML. <see cref="string"/></param>
        /// <param name="bodyIsHTML">This tells SMTP server that content is in HTML format. <see cref="bool"/></param>
        /// <param name="fileAttachmentPaths">This is an OPTIONAL array of full paths pointing to files attachments. <see cref="T:string[]"/> </param>
        void SendEmail(string fromAddress, string fromName, string[] toEmailAddresses, string subjectLine, string emailBody, bool bodyIsHTML, string[] fileAttachmentPaths = null);

        /// <summary>
        /// Send and email to a single recipient asynchronously.
        /// </summary>
        /// <param name="fromAddress">From: header of an Email.  Used with reader hits Reply. <see cref="string"/></param>
        /// <param name="fromName">User friendly display name associated with From: header of an Email. <see cref="string"/></param>
        /// <param name="toEmailAddress">From: header of an Email. This is the intended recipient of email. <see cref="string"/></param>
        /// <param name="subjectLine">Subject: header of an Email. This should be a one line summary of email. <see cref="string"/></param>
        /// <param name="emailBody">This is Body or content of the email.  Can be written in plain text or HTML. <see cref="string"/></param>
        /// <param name="bodyIsHTML">This tells SMTP server that content is in HTML format. <see cref="bool"/></param>
        /// <param name="fileAttachmentPaths">This is an OPTIONAL array of full paths pointing to files attachments. <see cref="T:string[]"/> </param>
        /// <returns></returns>
        Task SendEmailAsync(string fromAddress, string fromName, string toEmailAddress, string subjectLine, string emailBody, bool bodyIsHTML, string[] fileAttachmentPaths = null);

        /// <summary>
        /// Send and email to multiple recipients asynchronously.
        /// </summary>
        /// <param name="fromAddress">From: header of an Email.  Used with reader hits Reply. <see cref="string"/></param>
        /// <param name="fromName">User friendly display name associated with From: header of an Email. <see cref="string"/></param>
        /// <param name="toEmailAddresses">From: header of an Email. This is an array of intended recipients of the email. <see cref="T:string[]"/></param>
        /// <param name="subjectLine">Subject: header of an Email. This should be a one line summary of email. <see cref="string"/></param>
        /// <param name="emailBody">This is Body or content of the email.  Can be written in plain text or HTML. <see cref="string"/></param>
        /// <param name="bodyIsHTML">This tells SMTP server that content is in HTML format. <see cref="bool"/></param>
        /// <param name="fileAttachmentPaths">This is an OPTIONAL array of full paths pointing to files attachments. <see cref="T:string[]"/> </param>
        /// <returns></returns>
        Task SendEmailAsync(string fromAddress, string fromName, string[] toEmailAddresses, string subjectLine, string emailBody, bool bodyIsHTML, string[] fileAttachmentPaths = null);
    }
}