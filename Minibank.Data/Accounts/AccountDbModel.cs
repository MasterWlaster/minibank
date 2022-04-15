using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Minibank.Data.Transfers;
using Minibank.Data.Users;

namespace Minibank.Data.Accounts
{
    [Table("account")]
    public class AccountDbModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public virtual UserDbModel User { get; set; }
        public decimal Money { get; set; }
        public string CurrencyCode { get; set; }
        public bool IsActive { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime CloseDate { get; set; }

        //public virtual List<TransferDbModel> TransfersOutgoing { get; set; }
        //public virtual List<TransferDbModel> TransfersIngoing { get; set; }
        //public virtual List<TransferDbModel> Transfers { get; set; }

        internal class Map : IEntityTypeConfiguration<AccountDbModel>
        {
            public void Configure(EntityTypeBuilder<AccountDbModel> builder)
            {
                //id
                builder.Property(it => it.Id)
                    .HasColumnName("id");

                builder.HasKey(it => it.Id);//.HasName("pk_id");

                //user_id
                builder.Property(it => it.UserId)
                    .HasColumnName("user_id");

                builder.HasOne(it => it.User)
                    .WithMany(it => it.Accounts)
                    .HasForeignKey(it => it.UserId);

                //money
                builder.Property(it => it.Money)
                    .HasColumnName("money");

                //currency_code
                builder.Property(it => it.CurrencyCode)
                    .HasColumnName("currency_code");

                //is_active
                builder.Property(it => it.IsActive)
                    .HasColumnName("is_active");

                //open_date
                builder.Property(it => it.OpenDate)
                    .HasColumnName("open_date");

                //close_date
                builder.Property(it => it.CloseDate)
                    .HasColumnName("close_date");
            }
        }
    }
}
