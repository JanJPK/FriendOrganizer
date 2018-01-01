using System.Threading.Tasks;

namespace FriendOrganizer.UI.View.Services
{
    public interface IMessageDialogService
    {
        #region Public Methods and Operators

        void ShowInfoDialogAsync(string text);

        Task<MessageDialogResult> ShowOkCancelDialogAsync(string text, string title);

        #endregion
    }
}