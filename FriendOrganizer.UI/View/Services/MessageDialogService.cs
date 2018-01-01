using System.Threading.Tasks;
using System.Windows;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace FriendOrganizer.UI.View.Services
{
    public class MessageDialogService : IMessageDialogService
    {
        #region Public Methods and Operators

        private MetroWindow MetroWindow => (MetroWindow)App.Current.MainWindow;
        public async void ShowInfoDialogAsync(string text)
        {
            await MetroWindow.ShowMessageAsync("Info", text);
        }

        public async Task<MessageDialogResult> ShowOkCancelDialogAsync(string text, string title)
        {
            var result = await MetroWindow.ShowMessageAsync(title, text, MessageDialogStyle.AffirmativeAndNegative);
            return result == MahApps.Metro.Controls.Dialogs.MessageDialogResult.Affirmative ? MessageDialogResult.OK : MessageDialogResult.Cancel;
        }

        #endregion
    }

    public enum MessageDialogResult
    {
        OK,
        Cancel
    }
}