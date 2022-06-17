using System;
using System.Linq;

namespace Domain.Interfaces
{
    public interface ICpfValidator
    {
        bool IsValid(string? cpf);
    }

    public class CpfValidator : ICpfValidator
    {
        public bool IsValid(string? cpf)
        {
            return !string.IsNullOrWhiteSpace(cpf) && cpf.All(c => Char.IsDigit(c));
        }
    }
}