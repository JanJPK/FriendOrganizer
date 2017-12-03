using System.Windows.Input;
using FriendOrganizer.UI.Event;
using Prism.Commands;
using Prism.Events;

namespace FriendOrganizer.UI.ViewModel
{
    public class NavigationItemViewModel : ViewModelBase
    {
        #region Fields

        private readonly IEventAggregator eventAggregator;
        private string displayMember;

        #endregion

        #region Constructors and Destructors

        public NavigationItemViewModel(int id, string displayMember, IEventAggregator eventAggregator)
        {
            Id = id;
            this.eventAggregator = eventAggregator;
            this.displayMember = displayMember;
            OpenFriendDetailViewCommand = new DelegateCommand(OnOpenFriendDetailView);
        }

        #endregion

        #region Public Properties

        public string DisplayMember
        {
            get => displayMember;
            set
            {
                displayMember = value;
                OnPropertyChanged();
            }
        }

        public int Id { get; }

        public ICommand OpenFriendDetailViewCommand { get; }

        #endregion

        #region Methods

        private void OnOpenFriendDetailView()
        {
            eventAggregator.GetEvent<OpenFriendDetailViewEvent>().Publish(Id);
        }

        #endregion
    }
}