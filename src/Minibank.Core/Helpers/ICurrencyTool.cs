using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minibank.Core.Helpers
{
    public interface ICurrencyTool
    {
        string DefaultCurrency { get; }
        string Normalize(string currencyCode);
        bool IsValid(string currencyCode);
        string Validate(string currencyCode);
    }
}
