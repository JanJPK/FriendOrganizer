using System.Threading.Tasks;
using FriendOrganizer.Model;
using FriendOrganizer.UI.Data;
using FriendOrganizer.UI.Event;
using Prism.Events;

namespace FriendOrganizer.UI.ViewModel
{
    /// <summary>
    ///     Displays details of selected friend.
    /// </summary>
    public class FriendDetailViewModel : ViewModelBase, IFriendDetailViewModel
    {
        public Friend Friend
        {
            get => friend;
            private set
            {
                friend = value;
                OnPropertyChanged();
            }
        }

        public IFriendDataService dataService;
        public IEventAggregator eventAggregator;

        private Friend friend;

        public FriendDetailViewModel(IFriendDataService dataService, IEventAggregator eventAggregator)
        {
            this.dataService = dataService;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.GetEvent<OpenFriendDetailViewEvent>().Subscribe(OnOpenFriendDetailView);
        }

        public async Task LoadAsync(int id)
        {
            Friend = await dataService.GetByIdAsync(id);
        }

        private async void OnOpenFriendDetailView(int id)
        {
            await LoadAsync(id);
        }
    }
}