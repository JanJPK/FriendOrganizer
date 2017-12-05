using System.Data.Entity.Migrations;

namespace FriendOrganizer.DataAccess.Migrations
{
    public partial class InitialDatabase : DbMigration
    {
        #region Public Methods and Operators

        public override void Down()
        {
            DropTable("dbo.Friend");
        }

        public override void Up()
        {
            CreateTable(
                    "dbo.Friend",
                    c => new
                    {
                        Id = c.Int(false, true),
                        FirstName = c.String(false, 50),
                        LastName = c.String(maxLength: 50),
                        Email = c.String(maxLength: 50)
                    })
                .PrimaryKey(t => t.Id);
        }

        #endregion
    }
}