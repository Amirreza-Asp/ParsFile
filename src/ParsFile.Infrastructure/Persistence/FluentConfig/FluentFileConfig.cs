using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ParsFile.Infrastructure.Persistence.FluentConfig
{
    public class FluentFileConfig : IEntityTypeConfiguration<Domain.Entities.Content.File>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Content.File> builder)
        {
            builder.Property(b => b.Id).HasDefaultValueSql("NEWID()");

            builder.Property(b => b.CreateTime).HasDefaultValueSql("GETDATE()");

            builder.Property(b => b.Path).IsRequired().HasMaxLength(40);

            builder.HasOne(p => p.User)
                        .WithMany(p => p.Files)
                          .HasForeignKey(p => p.UserName)
                             .HasPrincipalKey(b => b.UserName);
        }
    }
}
