using System.Data.Entity;
using System.Threading.Tasks;
using FriendOrganizer.DataAccess;
using FriendOrganizer.Model;

namespace FriendOrganizer.UI.Data.Repositories
{
    /// <summary>
    ///     Loads the data from database.
    /// </summary>
    public class FriendRepository : IFriendRepository
    {
        #region Fields

        private readonly FriendOrganizerDbContext context;

        #endregion

        #region Constructors and Destructors

        public FriendRepository(FriendOrganizerDbContext context)
        {
            this.context = context;
        }

        #endregion

        #region Public Methods and Operators

        //public async Task<List<Friend>> GetAllAsync()
        //{
        //    // yield return creates element only when need for it arises.
        //    //yield return new Friend {FirstName = "Greg", LastName = "Green"};
        //    //yield return new Friend {FirstName = "Paul", LastName = "Purple"};
        //    //yield return new Friend {FirstName = "Bob", LastName = "Blue"};

        //    using (var context = contextCreator())
        //    {
        //        return await context.Friends.AsNoTracking().ToListAsync();
        //    }
        //}

        public async Task<Friend> GetByIdAsync(int id)
        {
            return await context.Friends.SingleAsync(f => f.Id == id);
        }

        public bool HasChanges()
        {
            return context.ChangeTracker.HasChanges();
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }

        public void Add(Friend friend)
        {
            context.Friends.Add(friend);
        }

        public void Remove(Friend friendModel)
        {
            context.Friends.Remove(friendModel);
        }

        #endregion
    }
}