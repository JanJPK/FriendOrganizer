using System.Collections.ObjectModel;
using System.Threading.Tasks;
using FriendOrganizer.Model;
using FriendOrganizer.UI.Data;
using FriendOrganizer.UI.Event;
using Prism.Events;

namespace FriendOrganizer.UI.ViewModel
{
    /// <summary>
    ///     Displays the list of friends. Publishes an event in event aggregator that FriendDetailVieModel is subscribed to.
    /// </summary>
    public class NavigationViewModel : ViewModelBase, INavigationViewModel
    {
        // This will be bound to the UI to display the friends.
        public ObservableCollection<LookupItem> Friends { get; }

        public LookupItem SelectedFriend
        {
            get => selectedFriend;
            set
            {
                selectedFriend = value;
                OnPropertyChanged();
                if (selectedFriend != null)
                {
                    eventAggregator.GetEvent<OpenFriendDetailViewEvent>().Publish(selectedFriend.Id);
                }
            }
        }

        private readonly IFriendLookupDataService _friendLookupService;
        private readonly IEventAggregator eventAggregator;

        private LookupItem selectedFriend;

        public NavigationViewModel(IFriendLookupDataService friendLookupService, IEventAggregator eventAggregator)
        {
            _friendLookupService = friendLookupService;
            this.eventAggregator = eventAggregator;
            Friends = new ObservableCollection<LookupItem>();
        }

        public async Task LoadAsync()
        {
            var lookup = await _friendLookupService.GetFriendLookupAsync();
            Friends.Clear();
            foreach (var item in lookup)
            {
                Friends.Add(item);
            }
        }
    }
}