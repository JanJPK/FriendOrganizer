using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using FriendOrganizer.UI.Data.Lookups;
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

        private readonly IEventAggregator eventAggregator;

        private readonly IFriendLookupDataService friendLookupService;

        #endregion

        #region Constructors and Destructors

        public NavigationViewModel(IFriendLookupDataService friendLookupService, IEventAggregator eventAggregator)
        {
            this.friendLookupService = friendLookupService;
            this.eventAggregator = eventAggregator;
            Friends = new ObservableCollection<NavigationItemViewModel>();
            eventAggregator.GetEvent<AfterFriendSavedEvent>().Subscribe(AfterFriendSaved);
            eventAggregator.GetEvent<AfterFriendDeletedEvent>().Subscribe(AfterFriendDeleted);
        }



        #endregion

        #region Public Properties

        // This will be bound to the UI to display the friends.
        public ObservableCollection<NavigationItemViewModel> Friends { get; }

        #endregion

        #region Public Methods and Operators

        public async Task LoadAsync()
        {
            var lookup = await friendLookupService.GetFriendLookupAsync();
            Friends.Clear();
            foreach (var item in lookup)
            {
                Friends.Add(new NavigationItemViewModel(item.Id, item.DisplayMember, eventAggregator));
            }
        }

        #endregion

        #region Methods

        private void AfterFriendSaved(AfterFriendSavedEventArgs obj)
        {
            // SingleOrDefault -> now we can have null ids (new friends), and this method returns null if id does not exist, unlike Single which throws an exception.
            var lookupItem = Friends.SingleOrDefault(l => l.Id == obj.Id);
            if (lookupItem == null)
            {
                Friends.Add(new NavigationItemViewModel(obj.Id, obj.DisplayMember, eventAggregator));
            }
            else
            {
                lookupItem.DisplayMember = obj.DisplayMember;
            }
        }

        #endregion
        private void AfterFriendDeleted(int id)
        {
            var friend = Friends.SingleOrDefault(f => f.Id == id);
            if (friend != null)
            {
                Friends.Remove(friend);
            }
        }
    }
}