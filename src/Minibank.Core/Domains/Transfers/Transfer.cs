using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minibank.Core.Domains.Transfers
{
    public class Transfer
    {
        public int Id { get; set; }
        public decimal Money { get; set; }
        public string CurrencyCode { get; set; }
        public int FromAccountId { get; set; }
        public int ToAccountId { get; set; }
    }
}
