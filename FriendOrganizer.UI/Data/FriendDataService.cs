using System;
using System.Data.Entity;
using System.Threading.Tasks;
using FriendOrganizer.DataAccess;
using FriendOrganizer.Model;

namespace FriendOrganizer.UI.Data
{
    /// <summary>
    ///     Supplies data; "fake" database (later changed).
    /// </summary>
    public class FriendDataService : IFriendDataService
    {
        #region Fields

        private readonly Func<FriendOrganizerDbContext> contextCreator;

        #endregion

        #region Constructors and Destructors

        public FriendDataService(Func<FriendOrganizerDbContext> contextCreator)
        {
            this.contextCreator = contextCreator;
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
            using (var context = contextCreator())
            {
                return await context.Friends.AsNoTracking().SingleAsync(f => f.Id == id);
            }
        }

        public async Task SaveAsync(Friend friend)
        {
            using (var ctx = contextCreator())
            {
                ctx.Friends.Attach(friend);
                ctx.Entry(friend).State = EntityState.Modified;
                await ctx.SaveChangesAsync();
            }
        }

        #endregion
    }
}