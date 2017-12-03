using System.Threading.Tasks;

namespace FriendOrganizer.UI.ViewModel
{
    public interface INavigationViewModel
    {
        #region Public Methods and Operators

        Task LoadAsync();

        #endregion
    }
}