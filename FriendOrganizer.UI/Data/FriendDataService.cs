using System.Collections.Generic;
using FriendOrganizer.Model;

namespace FriendOrganizer.UI.Data
{
    /// <summary>
    ///     Supplies data; "fake" database.
    /// </summary>
    public class FriendDataService : IFriendDataService
    {
        public IEnumerable<Friend> GetAll()
        {
            // yield return creates element only when need for it arises.
            yield return new Friend {FirstName = "Greg", LastName = "Green"};
            yield return new Friend {FirstName = "Paul", LastName = "Purple"};
            yield return new Friend {FirstName = "Bob", LastName = "Blue"};
        }
    }
}