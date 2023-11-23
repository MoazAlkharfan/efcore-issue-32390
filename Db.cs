// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp1
{
    public class Db : DbContext
    {
        public DbSet<Tag> Tags => Set<Tag>();
        public DbSet<TagSetting> TagSettings => Set<TagSetting>();

        public Db(DbContextOptions<Db> options)
            : base(options)
        {
            ChangeTracker.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tag>()
                .HasMany(x => x.Settings)
                .WithOne(x => x.Tag);

            modelBuilder.Entity<Tag>()
                .Navigation(x => x.Settings)
                .AutoInclude()
                // Is it still lazy loading?
                .EnableLazyLoading(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}