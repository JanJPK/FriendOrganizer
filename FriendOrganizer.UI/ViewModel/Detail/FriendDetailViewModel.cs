using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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

namespace FriendOrganizer.UI.ViewModel.Detail
{
    /// <summary>
    ///     Displays details of selected friend.
    /// </summary>
    public class FriendDetailViewModel : DetailViewModelBase, IFriendDetailViewModel
    {
        #region Fields

        private readonly IFriendRepository friendRepository;
        private readonly IProgrammingLanguageLookupDataService programmingLanguageLookupDataService;

        private FriendWrapper friend;
        private FriendPhoneNumberWrapper selectedPhoneNumber;

        #endregion

        #region Constructors and Destructors

        public FriendDetailViewModel(IFriendRepository friendRepository,
            IEventAggregator eventAggregator, IMessageDialogService messageDialog,
            IProgrammingLanguageLookupDataService programmingLanguageLookupDataService)
            : base(eventAggregator, messageDialog)
        {
            this.friendRepository = friendRepository;
            this.programmingLanguageLookupDataService = programmingLanguageLookupDataService;

            eventAggregator.GetEvent<AfterCollectionSavedEvent>().Subscribe(AfterCollectionSaved);

            AddPhoneNumberCommand = new DelegateCommand(OnAddPhoneNumberExecute);
            RemovePhoneNumberCommand = new DelegateCommand(OnRemovePhoneNumberExecute, OnRemovePhoneNumberCanExecute);

            ProgrammingLanguages = new ObservableCollection<LookupItem>();
            PhoneNumbers = new ObservableCollection<FriendPhoneNumberWrapper>();
        }

        #endregion

        #region Public Properties

        public ICommand AddPhoneNumberCommand { get; }

        public FriendWrapper Friend
        {
            get => friend;
            private set
            {
                friend = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<FriendPhoneNumberWrapper> PhoneNumbers { get; }

        public ObservableCollection<LookupItem> ProgrammingLanguages { get; }
        public ICommand RemovePhoneNumberCommand { get; }

        public FriendPhoneNumberWrapper SelectedPhoneNumber
        {
            get => selectedPhoneNumber;
            set
            {
                selectedPhoneNumber = value;
                OnPropertyChanged();
                ((DelegateCommand) RemovePhoneNumberCommand).RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region Public Methods and Operators

        public override async Task LoadAsync(int id)
        {
            var friend = id > 0
                ? await friendRepository.GetByIdAsync(id)
                : CreateNewFriend();

            Id = id;

            InitializeFriend(friend);
            InitializeFriendPhoneNumbers(friend.PhoneNumbers);

            await LoadProgrammingLanguagesLookupAsync();
        }

        #endregion

        #region Methods

        protected override async void OnDeleteExecute()
        {
            // Checking if friend is part of a meeting.
            if (await friendRepository.HasMeetingsAsync(Friend.Id))
            {
                MessageDialogService.ShowInfoDialog(
                    $"{Friend.FirstName} {Friend.LastName} attends a meeting and cannot be deleted.");
                return;
            }

            var result = MessageDialogService.ShowOkCancelDialog(
                $"Do you really want to delete the friend {Friend.FirstName} {Friend.LastName}?",
                "Question");
            if (result == MessageDialogResult.OK)
            {
                friendRepository.Remove(Friend.Model);
                await friendRepository.SaveAsync();
                RaiseDetailDeletedEvent(Friend.Id);
            }
        }

        protected override bool OnSaveCanExecute()
        {
            return Friend != null
                   && !Friend.HasErrors
                   && PhoneNumbers.All(p => !p.HasErrors)
                   && HasChanges;
        }

        protected override async void OnSaveExecute()
        {
            await friendRepository.SaveAsync();
            HasChanges = friendRepository.HasChanges();
            Id = Friend.Id;
            RaiseDetailSavedEvent(Friend.Id, $"{Friend.FirstName} {Friend.LastName}");
        }

        private async void AfterCollectionSaved(AfterCollectionSavedEventArgs args)
        {
            if (args.ViewModelName == nameof(ProgrammingLanguageDetailViewModel))
            {
                await LoadProgrammingLanguagesLookupAsync();
            }
        }

        private Friend CreateNewFriend()
        {
            var friend = new Friend();
            friendRepository.Add(friend);
            return friend;
        }

        private void FriendPhoneNumberWrapper_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!HasChanges)
            {
                HasChanges = friendRepository.HasChanges();
            }
            if (e.PropertyName == nameof(FriendPhoneNumberWrapper.HasErrors))
            {
                ((DelegateCommand) SaveCommand).RaiseCanExecuteChanged();
            }
        }

        private void InitializeFriend(Friend friend)
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

                if (e.PropertyName == nameof(Friend.FirstName)
                    || e.PropertyName == nameof(Friend.LastName))
                {
                    SetTitle();
                }
            };
            ((DelegateCommand) SaveCommand).RaiseCanExecuteChanged();
            if (Friend.Id == 0)
            {
                // Triggers the validation.
                Friend.FirstName = "";
            }
            SetTitle();
        }

        private void InitializeFriendPhoneNumbers(ICollection<FriendPhoneNumber> friendPhoneNumbers)
        {
            foreach (var wrapper in PhoneNumbers)
            {
                wrapper.PropertyChanged -= FriendPhoneNumberWrapper_PropertyChanged;
            }

            PhoneNumbers.Clear();

            foreach (var friendPhoneNumber in friendPhoneNumbers)
            {
                var wrapper = new FriendPhoneNumberWrapper(friendPhoneNumber);
                PhoneNumbers.Add(wrapper);
                wrapper.PropertyChanged += FriendPhoneNumberWrapper_PropertyChanged;
            }
        }

        private async Task LoadProgrammingLanguagesLookupAsync()
        {
            ProgrammingLanguages.Clear();
            ProgrammingLanguages.Add(new NullLookupItem {DisplayMember = " - "});
            var lookup = await programmingLanguageLookupDataService.GetProgrammingLanguageLookupAsync();
            foreach (var lookupItem in lookup)
            {
                ProgrammingLanguages.Add(lookupItem);
            }
        }

        private void OnAddPhoneNumberExecute()
        {
            var newNumber = new FriendPhoneNumberWrapper(new FriendPhoneNumber());
            newNumber.PropertyChanged += FriendPhoneNumberWrapper_PropertyChanged;
            PhoneNumbers.Add(newNumber);
            Friend.Model.PhoneNumbers.Add(newNumber.Model);
            newNumber.Number = ""; // Trigger validation.
        }

        private bool OnRemovePhoneNumberCanExecute()
        {
            return SelectedPhoneNumber != null;
        }

        private void OnRemovePhoneNumberExecute()
        {
            SelectedPhoneNumber.PropertyChanged -= FriendPhoneNumberWrapper_PropertyChanged;
            //Friend.Model.PhoneNumbers.Remove(SelectedPhoneNumber.Model);
            friendRepository.RemovePhoneNumber(SelectedPhoneNumber.Model);
            PhoneNumbers.Remove(SelectedPhoneNumber);
            SelectedPhoneNumber = null;
            HasChanges = friendRepository.HasChanges();
            ((DelegateCommand) SaveCommand).RaiseCanExecuteChanged();
        }

        private void SetTitle()
        {
            Title = $"{Friend.FirstName} {Friend.LastName}";
        }

        #endregion
    }
}