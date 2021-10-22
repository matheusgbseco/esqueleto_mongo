using System.Collections.Generic;

namespace Common.Domain.Interfaces
{
    public interface IService
    {
        bool IsInvalid();
        bool IsValid();
        List<string> GetValidationErrors();
    }
}