using System.Threading.Tasks;

namespace FriendOrganizer.UI.Data.Repositories
{
    public interface IGenericRepository<T>
    {
        #region Public Methods and Operators

        void Add(T model);
        Task<T> GetByIdAsync(int id);
        bool HasChanges();
        void Remove(T model);
        Task SaveAsync();

        #endregion
    }
}