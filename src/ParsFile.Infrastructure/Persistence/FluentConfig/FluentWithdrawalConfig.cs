using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ParsFile.Domain.Entities.Identity;

namespace ParsFile.Infrastructure.Persistence.FluentConfig
{
    public class FluentWithdrawalConfig : IEntityTypeConfiguration<Withdrawal>
    {
        public void Configure(EntityTypeBuilder<Withdrawal> builder)
        {
            builder.Property(x => x.Id).HasDefaultValueSql("NEWID()");

            builder.HasOne(x => x.Wallet)
                .WithMany(x => x.Withdrawals)
                .HasForeignKey(x => x.WalletId)
                .HasPrincipalKey(x => x.Id);
        }
    }
}
