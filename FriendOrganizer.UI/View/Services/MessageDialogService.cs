using System.Windows;

namespace FriendOrganizer.UI.View.Services
{
    public class MessageDialogService : IMessageDialogService
    {
        #region Public Methods and Operators

        public void ShowInfoDialog(string text)
        {
            MessageBox.Show(text);
        }

        public MessageDialogResult ShowOkCancelDialog(string text, string title)
        {
            var result = MessageBox.Show(text, title, MessageBoxButton.OKCancel);
            return result == MessageBoxResult.OK ? MessageDialogResult.OK : MessageDialogResult.Cancel;
        }

        #endregion
    }

    public enum MessageDialogResult
    {
        OK,
        Cancel
    }
}