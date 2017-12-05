using System;
using System.Threading.Tasks;
using System.Windows.Input;
using FriendOrganizer.UI.Event;
using FriendOrganizer.UI.View.Services;
using Prism.Commands;
using Prism.Events;

namespace FriendOrganizer.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region Fields

        private readonly IEventAggregator eventAggregator;
        private readonly Func<IFriendDetailViewModel> friendDetailViewModelCreator;
        private readonly IMessageDialogService messageDialogService;
        private IFriendDetailViewModel friendDetailViewModel;

        #endregion

        #region Constructors and Destructors

        public MainViewModel(INavigationViewModel navigationViewModel,
            Func<IFriendDetailViewModel> friendDetailViewModelCreator,
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService)
        {
            this.messageDialogService = messageDialogService;
            this.friendDetailViewModelCreator = friendDetailViewModelCreator;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.GetEvent<OpenFriendDetailViewEvent>().Subscribe(OnOpenFriendDetailView);
            this.eventAggregator.GetEvent<AfterFriendDeletedEvent>().Subscribe(AfterFriendDeleted);
            CreateNewFriendCommand = new DelegateCommand(OnCreateNewFriendExecute);
            NavigationViewModel = navigationViewModel;
        }

        #endregion

        #region Public Properties

        public ICommand CreateNewFriendCommand { get; }

        public IFriendDetailViewModel FriendDetailViewModel
        {
            get => friendDetailViewModel;
            private set
            {
                friendDetailViewModel = value;
                OnPropertyChanged();
            }
        }

        public INavigationViewModel NavigationViewModel { get; }

        #endregion

        #region Public Methods and Operators

        public async Task LoadAsync()
        {
            await NavigationViewModel.LoadAsync();
        }

        #endregion

        #region Methods

        private void AfterFriendDeleted(int id)
        {
            FriendDetailViewModel = null;
        }

        private void OnCreateNewFriendExecute()
        {
            OnOpenFriendDetailView(null);
        }

        private async void OnOpenFriendDetailView(int? id)
        {
            if (FriendDetailViewModel != null && FriendDetailViewModel.HasChanges)
            {
                // MessageBox here is a bad practice - it prevents unit testing.
                //var result = MessageBox.Show("You've made changes. Navigate away and lose them?", "Question",
                //    MessageBoxButton.OKCancel);
                var result =
                    messageDialogService.ShowOkCancelDialog("You've made changes. Navigate away and lose them?",
                        "Question");
                if (result == MessageDialogResult.Cancel)
                {
                    return;
                }
            }
            FriendDetailViewModel = friendDetailViewModelCreator();
            await FriendDetailViewModel.LoadAsync(id);
        }

        #endregion
    }
}