﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using FriendOrganizer.DataAccess;
using FriendOrganizer.Model;

namespace FriendOrganizer.UI.Data.Lookups
{
    /// <summary>
    ///     Loads the data from database.
    /// </summary>
    public class LookupDataService : IFriendLookupDataService,
        IProgrammingLanguageLookupDataService,
        IMeetingLookupDataService
    {
        #region Fields

        private readonly Func<FriendOrganizerDbContext> contextCreator;

        #endregion

        #region Constructors and Destructors

        public LookupDataService(Func<FriendOrganizerDbContext> contextCreator)
        {
            this.contextCreator = contextCreator;
        }

        #endregion

        #region Public Methods and Operators

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

        public async Task<List<LookupItem>> GetMeetingLookupAsync()
        {
            using (var ctx = contextCreator())
            {
                var items = await ctx.Meetings.AsNoTracking()
                    .Select(m =>
                        new LookupItem
                        {
                            Id = m.Id,
                            DisplayMember = m.Title
                        })
                    .ToListAsync();
                return items;
            }
        }

        public async Task<IEnumerable<LookupItem>> GetProgrammingLanguageLookupAsync()
        {
            using (var ctx = contextCreator())
            {
                return await ctx.ProgrammingLanguages.AsNoTracking()
                    .Select(f =>
                        new LookupItem
                        {
                            Id = f.Id,
                            DisplayMember = f.Name
                        })
                    .ToListAsync();
            }
        }

        #endregion
    }
}