using System.Threading.Tasks;

namespace FriendOrganizer.UI.ViewModel
{
    public interface IDetailViewModel
    {
        #region Public Properties

        bool HasChanges { get; }

        #endregion

        #region Public Methods and Operators

        Task LoadAsync(int? id);

        #endregion
    }
}