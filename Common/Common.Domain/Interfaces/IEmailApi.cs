using Common.Domain.Base;
using Common.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Domain.Interfaces
{
    public interface IEmailApi
    {
        void Config(ConfigEmailBase config);
        void Config(string apiKey, string fakeEmail = null);
        void AddAddressFrom(string name, string email);
        void AddAddressTo(string name, string email);
        void AddAddressReply(string name, string email);
        void Send(int templateId, dynamic parameters);
    }
}
