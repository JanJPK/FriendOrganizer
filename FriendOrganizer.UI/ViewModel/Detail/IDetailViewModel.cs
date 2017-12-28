using System.Threading.Tasks;

namespace FriendOrganizer.UI.ViewModel.Detail
{
    public interface IDetailViewModel
    {
        #region Public Properties

        bool HasChanges { get; }

        int Id { get;}

        #endregion

        #region Public Methods and Operators

        Task LoadAsync(int id);

        #endregion
    }
}