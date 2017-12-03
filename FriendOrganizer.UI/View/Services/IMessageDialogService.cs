namespace FriendOrganizer.UI.View.Services
{
    public interface IMessageDialogService
    {
        #region Public Methods and Operators

        MessageDialogResult ShowOkCancelDialog(string text, string title);

        #endregion
    }
}