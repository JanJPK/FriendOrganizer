using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using FriendOrganizer.DataAccess;
using FriendOrganizer.Model;

namespace FriendOrganizer.UI.Data
{
    /// <summary>
    ///     Supplies data; "fake" database.
    /// </summary>
    public class FriendDataService : IFriendDataService
    {
        private Func<FriendOrganizerDbContext> contextCreator;

        public FriendDataService(Func<FriendOrganizerDbContext> contextCreator)
        {
            this.contextCreator = contextCreator;
        }

        public async Task<List<Friend>> GetAllAsync()
        {
            // yield return creates element only when need for it arises.
            //yield return new Friend {FirstName = "Greg", LastName = "Green"};
            //yield return new Friend {FirstName = "Paul", LastName = "Purple"};
            //yield return new Friend {FirstName = "Bob", LastName = "Blue"};

            using (var context = contextCreator())
            {
                return await context.Friends.AsNoTracking().ToListAsync();
            }
        }
    }
}