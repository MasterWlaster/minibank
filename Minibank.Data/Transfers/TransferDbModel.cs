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
                builder.Property(it => it.Id)
                    .HasColumnName("id");

                builder.HasKey(it => it.Id);//.HasName("pk_id");

                //money
                builder.Property(it => it.Money)
                    .HasColumnName("money");

                //currency_code
                builder.Property(it => it.CurrencyCode)
                    .HasColumnName("currency_code");

                //from_account_id
                builder.Property(it => it.FromAccountId)
                    .HasColumnName("from_account_id");

                builder.HasOne(it => it.FromAccount)
                    .WithMany()
                    .HasForeignKey(it => it.FromAccountId);

                //to_account_id
                builder.Property(it => it.ToAccountId)
                    .HasColumnName("to_account_id");

                builder.HasOne(it => it.ToAccount)
                    .WithMany()
                    .HasForeignKey(it => it.ToAccountId);
            }
        }
    }
}
