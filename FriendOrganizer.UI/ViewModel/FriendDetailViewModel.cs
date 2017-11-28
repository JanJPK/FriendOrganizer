using System.Threading.Tasks;
using System.Windows.Input;
using FriendOrganizer.Model;
using FriendOrganizer.UI.Data;
using FriendOrganizer.UI.Event;
using Prism.Commands;
using Prism.Events;

namespace FriendOrganizer.UI.ViewModel
{
    /// <summary>
    ///     Displays details of selected friend.
    /// </summary>
    public class FriendDetailViewModel : ViewModelBase, IFriendDetailViewModel
    {
        #region Fields

        public IFriendDataService dataService;
        public IEventAggregator eventAggregator;

        private Friend friend;

        #endregion

        #region Constructors and Destructors

        public FriendDetailViewModel(IFriendDataService dataService, IEventAggregator eventAggregator)
        {
            this.dataService = dataService;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.GetEvent<OpenFriendDetailViewEvent>().Subscribe(OnOpenFriendDetailView);
            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
        }

        #endregion

        #region Public Properties

        public Friend Friend
        {
            get => friend;
            private set
            {
                friend = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveCommand { get; }

        #endregion

        #region Public Methods and Operators

        public async Task LoadAsync(int id)
        {
            Friend = await dataService.GetByIdAsync(id);
        }

        #endregion

        #region Methods

        private async void OnOpenFriendDetailView(int id)
        {
            await LoadAsync(id);
        }

        private bool OnSaveCanExecute()
        {
            return true;
        }

        private async void OnSaveExecute()
        {
            await dataService.SaveAsync(Friend);
            eventAggregator.GetEvent<AfterFriendSavedEvent>().Publish(
                new AfterFriendSavedEventArgs
                {
                    Id = Friend.Id,
                    DisplayMember = $"{Friend.FirstName} {Friend.LastName}"
                });
        }

        #endregion
    }
}