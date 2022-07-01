using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ParsFile.Domain.Entities.Content;

namespace ParsFile.Infrastructure.Persistence.FluentConfig
{
    public class FluentBasketConfig : IEntityTypeConfiguration<Basket>
    {
        public void Configure(EntityTypeBuilder<Basket> builder)
        {
            builder.Property(x => x.Id).HasDefaultValueSql("NEWID()");

            builder.HasOne(x => x.User)
                .WithMany(x => x.Baskets)
                .HasForeignKey(x => x.UserName)
                .HasPrincipalKey(x => x.UserName);


        }
    }
}
