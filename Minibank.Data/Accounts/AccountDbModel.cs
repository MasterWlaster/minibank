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

        internal class Map : IEntityTypeConfiguration<AccountDbModel>
        {
            public void Configure(EntityTypeBuilder<AccountDbModel> builder)
            {
                //id
                builder.HasKey(it => it.Id);

                //user_id
                builder.HasOne(it => it.User)
                    .WithMany(it => it.Accounts)
                    .HasForeignKey(it => it.UserId);
            }
        }
    }
}
