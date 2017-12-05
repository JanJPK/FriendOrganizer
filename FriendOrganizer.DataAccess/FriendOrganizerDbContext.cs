using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using FriendOrganizer.Model;

namespace FriendOrganizer.DataAccess
{
    public class FriendOrganizerDbContext : DbContext
    {
        #region Constructors and Destructors

        // Base takes connecton string.
        // App.config modification also necessary.
        //public FriendOrganizerDbContext() : base()
        public FriendOrganizerDbContext() : base("FriendOrganizerDb")
        {
        }

        #endregion

        #region Public Properties

        public DbSet<Friend> Friends { get; set; }
        public DbSet<ProgrammingLanguage> ProgrammingLanguages { get; set; }
        public DbSet<FriendPhoneNumber> FriendPhoneNumbers { get; set; }

        #endregion

        #region Methods

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

        #endregion
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