using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ParsFile.Domain.Entities.Identity;

namespace ParsFile.Infrastructure.Persistence.FluentConfig
{
    public class FluentOrderHeaderConfig : IEntityTypeConfiguration<OrderHeader>
    {
        public void Configure(EntityTypeBuilder<OrderHeader> builder)
        {
            builder.Property(x => x.Id).HasDefaultValueSql("NEWID()");

            builder.HasOne(x => x.User)
                .WithMany(x => x.OrderHeaders)
                .HasForeignKey(x => x.UserName)
                .HasPrincipalKey(x => x.UserName);
        }
    }
}
