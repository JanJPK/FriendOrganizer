using System.Threading.Tasks;
using FriendOrganizer.Model;

namespace FriendOrganizer.UI.Data.Repositories
{
    public interface IFriendRepository
    {
        #region Public Methods and Operators

        Task<Friend> GetByIdAsync(int id);
        bool HasChanges();
        Task SaveAsync();

        #endregion
    }
}