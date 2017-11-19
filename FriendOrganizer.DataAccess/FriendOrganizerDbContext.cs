using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using FriendOrganizer.Model;

namespace FriendOrganizer.DataAccess
{
    public class FriendOrganizerDbContext : DbContext
    {
        public DbSet<Friend> Friends { get; set; }

        // Base takes connecton string.
        // App.config modification also necessary.
        //public FriendOrganizerDbContext() : base()
        public FriendOrganizerDbContext() : base("FriendOrganizerDb")
        {
                
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // This override prevents EF from creating table with plural names.
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            // Fluent API constraints example below (required + maximum length of 50):
            //modelBuilder.Entity<Friend>()
            //    .Property(f => f.FirstName)
            //    .IsRequired()
            //    .HasMaxLength(50);
            // Alternative approach (bundled with FriendConfiguration class below):
            //modelBuilder.Configurations.Add(new FriendConfiguration());
        }
    }

    //public class FriendConfiguration : EntityTypeConfiguration<Friend>
    //{
    //    public FriendConfiguration()
    //    {
    //        Property(f => f.FirstName)
    //            .IsRequired()
    //            .HasMaxLength(50);
    //    }
    //}
}