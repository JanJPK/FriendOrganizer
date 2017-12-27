using System.Threading.Tasks;

namespace FriendOrganizer.UI.ViewModel.Navigation
{
    public interface INavigationViewModel
    {
        #region Public Methods and Operators

        Task LoadAsync();

        #endregion
    }
}