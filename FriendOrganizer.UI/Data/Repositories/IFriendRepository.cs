using System.Threading.Tasks;
using FriendOrganizer.Model;

namespace FriendOrganizer.UI.Data.Repositories
{
    public interface IFriendRepository
    {
        #region Public Methods and Operators

        void Add(Friend friend);

        Task<Friend> GetByIdAsync(int id);
        bool HasChanges();
        void Remove(Friend friendModel);

        void RemovePhoneNumber(FriendPhoneNumber model);
        Task SaveAsync();

        #endregion
    }
}