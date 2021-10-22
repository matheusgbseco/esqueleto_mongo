
using Common.Domain.Model;
using Common.Domain.Interfaces;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using RestSharp;
using System.Linq;
using RestSharp.Serialization.Json;
using sib_api_v3_sdk.Client;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Model;
using Newtonsoft.Json.Linq;

namespace Common.Mail
{
    public class EmailSendGridApi : IEmailApi
    {

        private string smtpServer;
        private int smtpPortNumber;
        private string smtpPassword;
        private string smtpUser;
        private string textFormat;
        private string fakeEmail;
        private string apiKey;
        private List<MailboxAddress> addressFrom;
        private List<MailboxAddress> addressTo;
        private List<MailboxAddress> addressReply;

        public EmailSendGridApi()
        {
            this.smtpPortNumber = 587;
            this.textFormat = TextFormat.Html.ToString();
            this.addressFrom = new List<MailboxAddress>();
            this.addressTo = new List<MailboxAddress>();
            this.addressReply = new List<MailboxAddress>();
        }

        public void Config(ConfigEmailBase config)
        {
            this.fakeEmail = config.FakeEmail;
            this.apiKey = config.ApiKey;
        }

        public void Config(string apiKey, string fakeEmail = null)
        {
            this.fakeEmail = fakeEmail;
            this.apiKey = apiKey;
        }

        public void AddAddressFrom(string name, string email)
        {
            if (email.IsEmail())
                this.addressFrom.Add(new MailboxAddress(name, email));
        }

        public void AddAddressTo(string name, string email)
        {
            if (!string.IsNullOrEmpty(this.fakeEmail))
                this.addressTo.Add(new MailboxAddress(name, this.fakeEmail));
            else if (email.IsEmail())
                this.addressTo.Add(new MailboxAddress(name, email));
        }

        public void AddAddressReply(string name, string email)
        {
            if (email.IsEmail())
                this.addressReply.Add(new MailboxAddress(name, email));
        }

        public void ClearAddress()
        {
            this.addressTo.Clear();
            this.addressFrom.Clear();
            this.addressReply.Clear();
        }

        public void Send(int templateId, dynamic parameters)
        {
            if (!Configuration.Default.ApiKey.ContainsKey("api-key"))
                Configuration.Default.ApiKey.Add("api-key", this.apiKey);

            if (this.addressTo.IsNotAny())
                return;

            foreach (var item in this.addressTo)
            {
                try
                {
                    var apiInstanceContacts = new ContactsApi();
                    var attributes = new JObject { { "NOME", item.Name } };
                    var listIds = new List<long?> { 2 };
                    var createContact = new CreateContact(item.Address, attributes, null, null, listIds, true);
                    CreateUpdateContactModel result = apiInstanceContacts.CreateContact(createContact);
                }
                catch { continue; }
            }

            var apiInstance = new TransactionalEmailsApi();

            var _to = new List<SendSmtpEmailTo>();
            foreach (var item in addressTo)
                _to.Add(new SendSmtpEmailTo(item.Address, item.Name));

            var _reply = new List<SendSmtpEmailReplyTo>();
            foreach (var item in addressReply)
                _reply.Add(new SendSmtpEmailReplyTo(item.Address, item.Name));

            try
            {
                var sendSmtpEmail = new SendSmtpEmail(null, _to, null, null, null, null, null, _reply.FirstOrDefault(), null, null, templateId, parameters, null, null);
                CreateSmtpEmail result = apiInstance.SendTransacEmail(sendSmtpEmail);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}
