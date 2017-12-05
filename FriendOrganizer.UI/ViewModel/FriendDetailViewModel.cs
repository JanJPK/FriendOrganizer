using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using FriendOrganizer.Model;
using FriendOrganizer.UI.Data.Lookups;
using FriendOrganizer.UI.Data.Repositories;
using FriendOrganizer.UI.Event;
using FriendOrganizer.UI.View.Services;
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

        private readonly IEventAggregator eventAggregator;

        private readonly IFriendRepository friendRepository;
        private readonly IMessageDialogService messageDialog;
        private readonly IProgrammingLanguageLookupDataService programmingLanguageLookupDataService;

        private FriendWrapper friend;
        private bool hasChanges;

        #endregion

        #region Constructors and Destructors

        public FriendDetailViewModel(IFriendRepository friendRepository,
            IEventAggregator eventAggregator, IMessageDialogService messageDialog,
            IProgrammingLanguageLookupDataService programmingLanguageLookupDataService)
        {
            this.friendRepository = friendRepository;
            this.eventAggregator = eventAggregator;
            this.messageDialog = messageDialog;
            this.programmingLanguageLookupDataService = programmingLanguageLookupDataService;

            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
            DeleteCommand = new DelegateCommand(OnDeleteExecute);
            ProgrammingLanguages = new ObservableCollection<LookupItem>();
        }

        #endregion

        #region Public Properties

        public ICommand DeleteCommand { get; }

        public FriendWrapper Friend
        {
            get => friend;
            private set
            {
                friend = value;
                OnPropertyChanged();
            }
        }

        public bool HasChanges
        {
            get => hasChanges;
            set
            {
                if (hasChanges != value)
                {
                    hasChanges = value;
                    OnPropertyChanged();
                    ((DelegateCommand) SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public ObservableCollection<LookupItem> ProgrammingLanguages { get; }

        public ICommand SaveCommand { get; }

        #endregion

        #region Public Methods and Operators

        public async Task LoadAsync(int? id)
        {
            var friend = id.HasValue
                ? await friendRepository.GetByIdAsync(id.Value)
                : CreateNewFriend();

            InitializeFriends(friend);

            await LoadProgrammingLanguagesLookupAsync();
        }

        #endregion

        #region Methods

        private Friend CreateNewFriend()
        {
            var friend = new Friend();
            friendRepository.Add(friend);
            return friend;
        }

        private void InitializeFriends(Friend friend)
        {
            Friend = new FriendWrapper(friend);
            Friend.PropertyChanged += (s, e) =>
            {
                if (!HasChanges)
                {
                    HasChanges = friendRepository.HasChanges();
                }
                if (e.PropertyName == nameof(Friend.HasErrors))
                {
                    ((DelegateCommand) SaveCommand).RaiseCanExecuteChanged();
                }
            };
            ((DelegateCommand) SaveCommand).RaiseCanExecuteChanged();
            if (Friend.Id == 0)
            {
                // Triggers the validation.
                Friend.FirstName = "";
            }
        }

        private async Task LoadProgrammingLanguagesLookupAsync()
        {
            ProgrammingLanguages.Clear();
            ProgrammingLanguages.Add(new NullLookupItem { DisplayMember = " - "});
            var lookup = await programmingLanguageLookupDataService.GetProgrammingLanguageLookupAsync();
            foreach (var lookupItem in lookup)
            {
                ProgrammingLanguages.Add(lookupItem);
            }
        }

        private async void OnDeleteExecute()
        {
            var result = messageDialog.ShowOkCancelDialog(
                $"Do you really want to delete the friend {Friend.FirstName} {Friend.LastName}?",
                "Question");
            if (result == MessageDialogResult.OK)
            {
                friendRepository.Remove(Friend.Model);
                await friendRepository.SaveAsync();
                eventAggregator.GetEvent<AfterFriendDeletedEvent>().Publish(Friend.Id);
            }
        }

        private bool OnSaveCanExecute()
        {
            return Friend != null && !Friend.HasErrors && HasChanges;
        }

        private async void OnSaveExecute()
        {
            await friendRepository.SaveAsync();
            HasChanges = friendRepository.HasChanges();
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