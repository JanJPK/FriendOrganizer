using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using FriendOrganizer.UI.Data.Lookups;
using FriendOrganizer.UI.Event;
using Prism.Events;

namespace FriendOrganizer.UI.ViewModel
{
    /// <summary>
    ///     Displays the list of items. Publishes an event in event aggregator that FriendDetailVieModel is subscribed to.
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
            eventAggregator.GetEvent<AfterDetailSavedEvent>().Subscribe(AfterDetailSaved);
            eventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(AfterDetailDeleted);
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
                Friends.Add(new NavigationItemViewModel(item.Id, item.DisplayMember, eventAggregator,
                    nameof(FriendDetailViewModel)));
            }
        }

        #endregion

        #region Methods

        private void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            //var friend = Friends.SingleOrDefault(f => f.Id == id);
            //if (friend != null)
            //{
            //    Friends.Remove(friend);
            //}

            switch (args.ViewModelName)
            {
                case nameof(FriendDetailViewModel):
                {
                    var friend = Friends.SingleOrDefault(f => f.Id == args.Id);
                    if (friend != null)
                    {
                        Friends.Remove(friend);
                    }
                    break;
                }
            }
        }

        private void AfterDetailSaved(AfterDetailSavedEventArgs obj)
        {
            // SingleOrDefault -> now we can have null ids (new friends), and this method returns null if id does not exist, unlike Single which throws an exception.
            switch (obj.ViewModelname)
            {
                case nameof(FriendDetailViewModel):
                {
                    var lookupItem = Friends.SingleOrDefault(l => l.Id == obj.Id);
                    if (lookupItem == null)
                    {
                        Friends.Add(new NavigationItemViewModel(obj.Id, obj.DisplayMember, eventAggregator,
                            nameof(FriendDetailViewModel)));
                    }
                    else
                    {
                        lookupItem.DisplayMember = obj.DisplayMember;
                    }
                    break;
                }
            }
        }

        #endregion
    }
}