using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ParsFile.Domain.Entities.Identity;

namespace ParsFile.Infrastructure.Persistence.FluentConfig
{
    public class FluentWalletConfig : IEntityTypeConfiguration<Wallet>
    {
        public void Configure(EntityTypeBuilder<Wallet> builder)
        {
            builder.Property(x => x.Id).HasDefaultValueSql("NEWID()");

            builder.HasOne(x => x.User)
                    .WithOne(x => x.Wallet)
                    .HasForeignKey<Wallet>(x => x.UserName)
                    .HasPrincipalKey<User>(x => x.UserName);

            builder.Property(x => x.UserName).IsRequired();
        }
    }
}
