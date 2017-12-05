using System.Data.Entity.Migrations;
using System.Linq;
using FriendOrganizer.Model;

namespace FriendOrganizer.DataAccess.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<FriendOrganizerDbContext>
    {
        #region Constructors and Destructors

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        #endregion

        #region Methods

        protected override void Seed(FriendOrganizerDbContext context)
        {
            // "Add-Migration name" => "Update-Database"
            context.Friends.AddOrUpdate(
                f => f.FirstName,
                new Friend {FirstName = "Greg", LastName = "Green"},
                new Friend {FirstName = "Paul", LastName = "Purple"},
                new Friend {FirstName = "Bob", LastName = "Blue"}
            );

            context.ProgrammingLanguages.AddOrUpdate(
                pl => pl.Name,
                new ProgrammingLanguage {Name = "C#"},
                new ProgrammingLanguage {Name = "C++"},
                new ProgrammingLanguage {Name = "F#"},
                new ProgrammingLanguage {Name = "Visual Basic"}
            );

            context.SaveChanges(); // So Friends exist; required for phone numbers.

            context.FriendPhoneNumbers.AddOrUpdate(
                pl => pl.Number,
                new FriendPhoneNumber{Number = "+48 12345678", FriendId = context.Friends.First().Id}
            );
        }

        #endregion
    }
}