namespace FriendOrganizer.UI.View.Services
{
    public interface IMessageDialogService
    {
        #region Public Methods and Operators

        void ShowInfoDialog(string text);

        MessageDialogResult ShowOkCancelDialog(string text, string title);

        #endregion
    }
}