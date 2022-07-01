using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ParsFile.Domain.Entities.Content;
using ParsFile.Domain.Entities.Identity;
using ParsFile.Infrastructure.Persistence.FluentConfig;

namespace ParsFile.Infrastructure.Persistence.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole, String>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> db) : base(db)
        {
        }

        public DbSet<Domain.Entities.Content.File> File { get; set; }
        public DbSet<Wallet> Wallet { get; set; }
        public DbSet<Basket> Basket { get; set; }
        public DbSet<OrderHeader> OrderHeader { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
        public DbSet<Withdrawal> Withdrawal { get; set; }
        public DbSet<Category> Category { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new FluentFileConfig());
            builder.ApplyConfiguration(new FluentWalletConfig());
            builder.ApplyConfiguration(new FluentBasketConfig());
            builder.ApplyConfiguration(new FluentOrderHeaderConfig());
            builder.ApplyConfiguration(new FluentOrderDetailConfig());
            builder.ApplyConfiguration(new FluentWithdrawalConfig());

            base.OnModelCreating(builder);
        }

    }
}
