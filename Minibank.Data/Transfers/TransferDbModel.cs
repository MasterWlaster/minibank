using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Minibank.Data.Accounts;
using Minibank.Data.Users;

namespace Minibank.Data.Transfers
{
    [Table("transfer")]
    public class TransferDbModel
    {
        public int Id { get; set; }
        public decimal Money { get; set; }
        public string CurrencyCode { get; set; }
        public int FromAccountId { get; set; }
        public virtual AccountDbModel FromAccount { get; set; }
        public int ToAccountId { get; set; }
        public virtual AccountDbModel ToAccount { get; set; }

        internal class Map : IEntityTypeConfiguration<TransferDbModel>
        {
            public void Configure(EntityTypeBuilder<TransferDbModel> builder)
            {
                //id
                builder.HasKey(it => it.Id);

                //from_account_id
                builder.HasOne(it => it.FromAccount)
                    .WithMany()
                    .HasForeignKey(it => it.FromAccountId);

                //to_account_id
                builder.HasOne(it => it.ToAccount)
                    .WithMany()
                    .HasForeignKey(it => it.ToAccountId);
            }
        }
    }
}
