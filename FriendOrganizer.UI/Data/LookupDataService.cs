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
    ///     Loads the data from database.
    /// </summary>
    public class LookupDataService : IFriendLookupDataService
    {
        private readonly Func<FriendOrganizerDbContext> contextCreator;

        public LookupDataService(Func<FriendOrganizerDbContext> contextCreator)
        {
            this.contextCreator = contextCreator;
        }

        public async Task<IEnumerable<LookupItem>> GetFriendLookupAsync()
        {
            using (var ctx = contextCreator())
            {
                return await ctx.Friends.AsNoTracking()
                    .Select(f =>
                        new LookupItem
                        {
                            Id = f.Id,
                            DisplayMember = f.FirstName + " " + f.LastName
                        })
                    .ToListAsync();
            }
        }
    }
}