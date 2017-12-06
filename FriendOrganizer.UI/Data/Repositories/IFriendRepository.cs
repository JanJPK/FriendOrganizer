using FriendOrganizer.Model;

namespace FriendOrganizer.UI.Data.Repositories
{

    public interface IFriendRepository : IGenericRepository<Friend>
    {
        #region Public Methods and Operators

        void RemovePhoneNumber(FriendPhoneNumber model);

        #endregion
    }
}