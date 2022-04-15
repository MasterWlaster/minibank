using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Minibank.Data.Accounts;
using Minibank.Data.Transfers;

namespace Minibank.Data.Users
{
    [Table("user")]
    public class UserDbModel
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }

        public virtual List<AccountDbModel> Accounts { get; set; }

        internal class Map : IEntityTypeConfiguration<UserDbModel>
        {
            public void Configure(EntityTypeBuilder<UserDbModel> builder)
            {
                //id
                builder.Property(it => it.Id)
                    .HasColumnName("id");

                builder.HasKey(it => it.Id);//.HasName("pk_id");

                //login
                builder.Property(it => it.Login)
                    .HasColumnName("login");

                //email
                builder.Property(it => it.Email)
                    .HasColumnName("email");

                //builder.Ignore

                //dotnet ef database update
                //dotnet ef migrations add Initial

                //builder.HasOne(it => it.User)
                //    .WithMany(iterator => iterator.Message)
                //    .HasForeignKey(iterator => iterator.UserId);
            }
        }
    }
}
