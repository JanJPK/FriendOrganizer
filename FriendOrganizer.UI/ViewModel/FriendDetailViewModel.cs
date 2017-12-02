using System.Threading.Tasks;
using System.Windows.Input;
using FriendOrganizer.UI.Data;
using FriendOrganizer.UI.Event;
using FriendOrganizer.UI.Wrapper;
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

        private FriendWrapper friend;

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

        public FriendWrapper Friend
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
            var friend = await dataService.GetByIdAsync(id);
            Friend = new FriendWrapper(friend);
            Friend.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(Friend.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            };
        }

        #endregion

        #region Methods

        private async void OnOpenFriendDetailView(int id)
        {
            await LoadAsync(id);
        }

        private bool OnSaveCanExecute()
        {
            return Friend != null && !Friend.HasErrors;
        }

        private async void OnSaveExecute()
        {
            await dataService.SaveAsync(Friend.Model);
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