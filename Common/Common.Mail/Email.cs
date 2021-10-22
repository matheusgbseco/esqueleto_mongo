
using Common.Domain.Model;
using Common.Domain.Interfaces;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;

namespace Common.Mail
{
    public class Email : IEmail
    {

        private string smtpServer;
        private int smtpPortNumber;
        private string smtpPassword;
        private string smtpUser;
        private string textFormat;
        private string fakeEmail;
        private List<MailboxAddress> addressFrom;
        private List<MailboxAddress> addressTo;
        private List<MailboxAddress> addressReply;

        public Email()
        {
            this.smtpPortNumber = 587;
            this.textFormat = TextFormat.Html.ToString();
            this.addressFrom = new List<MailboxAddress>();
            this.addressTo = new List<MailboxAddress>();
            this.addressReply = new List<MailboxAddress>();
        }

        public void Config(string smtpServer, string smtpUser, string smtpPassword, int smtpPortNumber = 587, string fakeEmail = null, string textFormat = "HTML")
        {
            this.smtpServer = smtpServer;
            this.smtpUser = smtpUser;
            this.smtpPassword = smtpPassword;
            this.smtpPortNumber = smtpPortNumber;
            this.fakeEmail = fakeEmail;
            this.textFormat = textFormat;
        }

        public void Config(ConfigEmailBase config)
        {
            this.smtpServer = config.SmtpServer;
            this.smtpUser = config.SmtpUser;
            this.smtpPassword = config.SmtpPassword;
            this.smtpPortNumber = config.SmtpPortNumber;
            this.fakeEmail = config.FakeEmail;
            this.textFormat = config.TextFormat ?? "HTML";
        }

        public void AddAddressFrom(string name, string email)
        {
            this.addressFrom.Add(new MailboxAddress(name, email));
        }

        public void AddAddressTo(string name, string email)
        {
            if (!string.IsNullOrEmpty(this.fakeEmail))
                this.addressTo.Add(new MailboxAddress(name, this.fakeEmail));
            else
                this.addressTo.Add(new MailboxAddress(name, email));
        }

        public void AddAddressReply(string name, string email)
        {
            this.addressReply.Add(new MailboxAddress(name, email));
        }

        public void ClearAddress()
        {
            this.addressTo.Clear();
            this.addressFrom.Clear();
            this.addressReply.Clear();
        }

        public void Send(String subject, String content)
        {
            try
            {
                var mimeMessage = new MimeMessage();

                mimeMessage.From.AddRange(this.addressFrom);
                mimeMessage.To.AddRange(this.addressTo);
                mimeMessage.ReplyTo.AddRange(this.addressReply);

                mimeMessage.Subject = subject;

                mimeMessage.Body = new TextPart(this.textFormat)
                {
                    Text = content
                };

                using (var client = new SmtpClient())
                {
                    client.Connect(this.smtpServer, this.smtpPortNumber, false);
                    client.Authenticate(this.smtpUser, this.smtpPassword);
                    client.Send(mimeMessage);
                    client.Disconnect(true);
                }
            }
            catch (Exception ex)
            {
                this.ClearAddress();
                throw ex;
            }
            finally
            {
                this.ClearAddress();
            }
        }

    }
}
