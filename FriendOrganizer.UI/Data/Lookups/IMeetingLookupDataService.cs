using System.Collections.Generic;
using System.Threading.Tasks;
using FriendOrganizer.Model;

namespace FriendOrganizer.UI.Data.Lookups
{
    public interface IMeetingLookupDataService
    {
        #region Public Methods and Operators

        Task<List<LookupItem>> GetMeetingLookupAsync();

        #endregion
    }
}