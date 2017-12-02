using System.Collections.ObjectModel;
using System.Linq;
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
        #region Fields

        private readonly IFriendLookupDataService friendLookupService;
        private readonly IEventAggregator eventAggregator;

        private NavigationItemViewModel selectedFriend;

        #endregion

        #region Constructors and Destructors

        public NavigationViewModel(IFriendLookupDataService friendLookupService, IEventAggregator eventAggregator)
        {
            this.friendLookupService = friendLookupService;
            this.eventAggregator = eventAggregator;
            Friends = new ObservableCollection<NavigationItemViewModel>();
            eventAggregator.GetEvent<AfterFriendSavedEvent>().Subscribe(AfterFriendSaved);
        }

        #endregion

        #region Public Properties

        // This will be bound to the UI to display the friends.
        public ObservableCollection<NavigationItemViewModel> Friends { get; }

        public NavigationItemViewModel SelectedFriend
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

        #endregion

        #region Public Methods and Operators

        public async Task LoadAsync()
        {
            var lookup = await friendLookupService.GetFriendLookupAsync();
            Friends.Clear();
            foreach (var item in lookup)
            {
                Friends.Add(new NavigationItemViewModel(item.Id, item.DisplayMember));
            }
        }

        #endregion

        #region Methods

        private void AfterFriendSaved(AfterFriendSavedEventArgs obj)
        {
            var lookupItem = Friends.Single(l => l.Id == obj.Id);
            lookupItem.DisplayMember = obj.DisplayMember;
        }

        #endregion
    }
}