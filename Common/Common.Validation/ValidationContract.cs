using System;
using Flunt.Validations;

namespace Common.Validation
{
    public class ValidationContract : Contract
    {

        public Contract IsValidCPFCNPJ(string cpf, string property, string message)
        {
            if (!cpf.IsCpfValid() && !cpf.IsCnpjValid()) this.AddNotification(property, message);
            return this;
        }

    }
}
