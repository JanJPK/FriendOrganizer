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
        private string detailViewModelName;

        #endregion

        #region Constructors and Destructors

        public NavigationItemViewModel(int id, string displayMember, IEventAggregator eventAggregator,
            string detailViewModelName)
        {
            Id = id;
            this.eventAggregator = eventAggregator;
            this.displayMember = displayMember;
            this.detailViewModelName = detailViewModelName;
            OpenDetailViewCommand = new DelegateCommand(OnOpenDetailViewExecute);
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

        public ICommand OpenDetailViewCommand { get; }

        #endregion

        #region Methods

        private void OnOpenDetailViewExecute()
        {
            eventAggregator.GetEvent<OpenDetailViewEvent>().Publish(
                new OpenDetailViewEventArgs
                {
                    Id = Id,
                    ViewModelName = detailViewModelName
                });
        }

        #endregion
    }
}