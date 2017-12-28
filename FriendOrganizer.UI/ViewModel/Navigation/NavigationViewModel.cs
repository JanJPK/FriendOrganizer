using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using FriendOrganizer.UI.Data.Lookups;
using FriendOrganizer.UI.Event;
using FriendOrganizer.UI.ViewModel.Detail;
using Prism.Events;

namespace FriendOrganizer.UI.ViewModel.Navigation
{
    /// <summary>
    ///     Displays the list of items. Publishes an event in event aggregator that FriendDetailVieModel is subscribed to.
    /// </summary>
    public class NavigationViewModel : ViewModelBase, INavigationViewModel
    {
        #region Fields

        private readonly IEventAggregator eventAggregator;

        private readonly IFriendLookupDataService friendLookupService;
        private readonly IMeetingLookupDataService meetingLookupService;

        #endregion

        #region Constructors and Destructors

        public NavigationViewModel(IFriendLookupDataService friendLookupService,
            IMeetingLookupDataService meetingLookupService, IEventAggregator eventAggregator)
        {
            this.friendLookupService = friendLookupService;
            this.meetingLookupService = meetingLookupService;
            this.eventAggregator = eventAggregator;

            Friends = new ObservableCollection<NavigationItemViewModel>();
            Meetings = new ObservableCollection<NavigationItemViewModel>();

            eventAggregator.GetEvent<AfterDetailSavedEvent>().Subscribe(AfterDetailSaved);
            eventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(AfterDetailDeleted);
        }

        #endregion

        #region Public Properties

        // This will be bound to the UI to display the friends.
        public ObservableCollection<NavigationItemViewModel> Friends { get; }

        public ObservableCollection<NavigationItemViewModel> Meetings { get; }

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

            lookup = await meetingLookupService.GetMeetingLookupAsync();
            Meetings.Clear();
            foreach (var item in lookup)
            {
                Meetings.Add(new NavigationItemViewModel(item.Id, item.DisplayMember, eventAggregator,
                    nameof(MeetingDetailViewModel)));
            }
        }

        #endregion

        #region Methods

        private void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            switch (args.ViewModelName)
            {
                case nameof(FriendDetailViewModel):
                {
                    AfterDetailDeleted(Friends, args);
                    break;
                }

                case nameof(MeetingDetailViewModel):
                {
                    AfterDetailDeleted(Meetings, args);
                    break;
                }
            }
        }

        private void AfterDetailDeleted(ObservableCollection<NavigationItemViewModel> items,
            AfterDetailDeletedEventArgs args)
        {
            var item = items.SingleOrDefault(f => f.Id == args.Id);
            if (item != null)
            {
                items.Remove(item);
            }
        }

        private void AfterDetailSaved(AfterDetailSavedEventArgs args)
        {
            // SingleOrDefault -> now we can have null ids (new friends), and this method returns null if id does not exist, unlike Single which throws an exception.
            switch (args.ViewModelName)
            {
                case nameof(FriendDetailViewModel):
                {
                    AfterDetailSaved(Friends, args);
                    break;
                }

                case nameof(MeetingDetailViewModel):
                {
                    AfterDetailSaved(Meetings, args);
                    break;
                }
            }
        }

        private void AfterDetailSaved(ObservableCollection<NavigationItemViewModel> items,
            AfterDetailSavedEventArgs args)
        {
            var lookupItem = items.SingleOrDefault(l => l.Id == args.Id);
            if (lookupItem == null)
            {
                items.Add(new NavigationItemViewModel(args.Id, args.DisplayMember, eventAggregator,
                    args.ViewModelName));
            }
            else
            {
                lookupItem.DisplayMember = args.DisplayMember;
            }
        }

        #endregion
    }
}