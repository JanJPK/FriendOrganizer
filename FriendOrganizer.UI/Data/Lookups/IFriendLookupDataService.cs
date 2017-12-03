using System.Collections.Generic;
using System.Threading.Tasks;
using FriendOrganizer.Model;

namespace FriendOrganizer.UI.Data.Lookups
{
    public interface IFriendLookupDataService
    {
        #region Public Methods and Operators

        Task<IEnumerable<LookupItem>> GetFriendLookupAsync();

        #endregion
    }
}