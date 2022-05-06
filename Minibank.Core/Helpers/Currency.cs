﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minibank.Core.Helpers
{
    public static class Currency
    {
        private static readonly List<string> currencies = new(){ "RUB", "USD", "EUR"};

        public static string DefaultCurrency => "RUB";
        
        public static string Normalize(string currencyCode)
        {
            return currencyCode.Trim().ToUpper();
        }

        public static bool IsValid(string currencyCode)
        {
            currencyCode = Validate(currencyCode);

            return currencyCode != null;
        }
        
        public static string Validate(string currencyCode)
        {
            currencyCode = Normalize(currencyCode);

            return !currencies.Contains(currencyCode) ? null : currencyCode;
        }
    }
}
