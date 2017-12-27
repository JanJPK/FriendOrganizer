using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Autofac.Features.Indexed;
using FriendOrganizer.UI.Event;
using FriendOrganizer.UI.View.Services;
using FriendOrganizer.UI.ViewModel.Detail;
using FriendOrganizer.UI.ViewModel.Navigation;
using Prism.Commands;
using Prism.Events;

namespace FriendOrganizer.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region Fields

        private readonly IEventAggregator eventAggregator;
        private readonly IMessageDialogService messageDialogService;
        private IDetailViewModel detailViewModel;
        private IIndex<string, IDetailViewModel> detailViewModelCreator;

        #endregion

        #region Constructors and Destructors

        public MainViewModel(INavigationViewModel navigationViewModel,
            IIndex<string, IDetailViewModel> detailViewModelCreator,
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService)
        {
            this.messageDialogService = messageDialogService;
            this.detailViewModelCreator = detailViewModelCreator;

            this.eventAggregator = eventAggregator;
            this.eventAggregator.GetEvent<OpenDetailViewEvent>().Subscribe(OnOpenDetailView);
            this.eventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(AfterDetailDeleted);

            CreateNewDetailCommand = new DelegateCommand<Type>(OnCreateNewDetailExecute);
            NavigationViewModel = navigationViewModel;
        }

        #endregion

        #region Public Properties

        public ICommand CreateNewDetailCommand { get; }

        public IDetailViewModel DetailViewModel
        {
            get => detailViewModel;
            private set
            {
                detailViewModel = value;
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

        private void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            DetailViewModel = null;
        }

        private void OnCreateNewDetailExecute(Type viewModelType)
        {
            //OnOpenDetailView(new OpenDetailViewEventArgs { ViewModelName = nameof(FriendDetailViewModel)});
            // More versatile approach:
            OnOpenDetailView(new OpenDetailViewEventArgs {ViewModelName = viewModelType.Name});
        }

        private async void OnOpenDetailView(OpenDetailViewEventArgs args)
        {
            if (DetailViewModel != null && DetailViewModel.HasChanges)
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

            DetailViewModel = detailViewModelCreator[args.ViewModelName];
            await DetailViewModel.LoadAsync(args.Id);
        }

        #endregion
    }
}