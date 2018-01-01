using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using FriendOrganizer.Model;
using FriendOrganizer.UI.Data.Repositories;
using FriendOrganizer.UI.Event;
using FriendOrganizer.UI.View.Services;
using FriendOrganizer.UI.Wrapper;
using Prism.Commands;
using Prism.Events;

namespace FriendOrganizer.UI.ViewModel.Detail
{
    public class MeetingDetailViewModel : DetailViewModelBase, IMeetingDetailViewModel
    {
        #region Fields

        private readonly IMeetingRepository meetingRepository;
        private List<Friend> allFriends;

        private MeetingWrapper meeting;

        private Friend selectedAddedFriend;

        private Friend selectedAvailableFriend;

        #endregion

        #region Constructors and Destructors

        public MeetingDetailViewModel(IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService,
            IMeetingRepository meetingRepository) : base(eventAggregator, messageDialogService)
        {
            this.meetingRepository = meetingRepository;
            eventAggregator.GetEvent<AfterDetailSavedEvent>().Subscribe(AfterDetailSaved);
            eventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(AfterDetailDeleted);

            AddedFriends = new ObservableCollection<Friend>();
            AvailableFriends = new ObservableCollection<Friend>();
            AddFriendCommand = new DelegateCommand(OnAddFriendExecute, OnAddFriendCanExecute);
            RemoveFriendCommand = new DelegateCommand(OnRemoveFriendExecute, OnRemoveFriendCanExecute);
        }

        #endregion

        #region Public Properties

        public ObservableCollection<Friend> AddedFriends { get; set; }

        public ICommand AddFriendCommand { get; set; }

        public ObservableCollection<Friend> AvailableFriends { get; set; }

        public MeetingWrapper Meeting
        {
            get => meeting;
            private set
            {
                meeting = value;
                OnPropertyChanged();
            }
        }

        public ICommand RemoveFriendCommand { get; set; }

        public Friend SelectedAddedFriend
        {
            get => selectedAddedFriend;
            set
            {
                selectedAddedFriend = value;
                OnPropertyChanged();
                ((DelegateCommand) RemoveFriendCommand).RaiseCanExecuteChanged();
            }
        }

        public Friend SelectedAvailableFriend
        {
            get => selectedAvailableFriend;
            set
            {
                selectedAvailableFriend = value;
                OnPropertyChanged();
                ((DelegateCommand) AddFriendCommand).RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region Public Methods and Operators

        public override async Task LoadAsync(int id)
        {
            var meeting = id > 0
                ? await meetingRepository.GetByIdAsync(id)
                : CreateNewMeeting();

            Id = id;

            InitializeMeeting(meeting);

            allFriends = await meetingRepository.GetAllFriendsAsync();
            SetupPicklist();
        }

        #endregion

        #region Methods

        protected override async void OnDeleteExecute()
        {
            var result =
                await MessageDialogService.ShowOkCancelDialogAsync($"Do you really want to delete the meeting?", "Question");
            if (result == MessageDialogResult.OK)
            {
                meetingRepository.Remove(Meeting.Model);
                await meetingRepository.SaveAsync();
                RaiseDetailDeletedEvent(Meeting.Id);
            }
        }

        protected override bool OnSaveCanExecute()
        {
            return Meeting != null && !Meeting.HasErrors && HasChanges;
        }

        protected override async void OnSaveExecute()
        {
            await meetingRepository.SaveAsync();
            HasChanges = meetingRepository.HasChanges();
            Id = meeting.Id;
            RaiseDetailSavedEvent(Meeting.Id, Meeting.Title);
        }

        private async void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            if (args.ViewModelName == nameof(FriendDetailViewModel))
            {
                allFriends = await meetingRepository.GetAllFriendsAsync();
                SetupPicklist();
            }
        }

        private async void AfterDetailSaved(AfterDetailSavedEventArgs args)
        {
            if (args.ViewModelName == nameof(FriendDetailViewModel))
            {
                await meetingRepository.ReloadFriendAsync(args.Id);
                allFriends = await meetingRepository.GetAllFriendsAsync();
                SetupPicklist();
            }
        }

        private Meeting CreateNewMeeting()
        {
            var meeting = new Meeting
            {
                DateFrom = DateTime.Now,
                DateTo = DateTime.Now
            };
            meetingRepository.Add(meeting);
            return meeting;
        }

        private void InitializeMeeting(Meeting meeting)
        {
            Meeting = new MeetingWrapper(meeting);
            Meeting.PropertyChanged += (s, e) =>
            {
                if (!HasChanges)
                {
                    HasChanges = meetingRepository.HasChanges();
                }

                if (e.PropertyName == nameof(Meeting.HasErrors))
                {
                    ((DelegateCommand) SaveCommand).RaiseCanExecuteChanged();
                }

                if (e.PropertyName == nameof(Meeting.Title))
                {
                    SetTitle();
                }
            };
            ((DelegateCommand) SaveCommand).RaiseCanExecuteChanged();

            // Triggers validation on newly created meeting.
            if (Meeting.Id == 0)
            {
                Meeting.Title = "";
            }
            SetTitle();
        }

        private bool OnAddFriendCanExecute()
        {
            return SelectedAvailableFriend != null;
        }

        private void OnAddFriendExecute()
        {
            var friendtoAdd = SelectedAvailableFriend;

            Meeting.Model.Friends.Add(friendtoAdd);
            AddedFriends.Add(friendtoAdd);
            AvailableFriends.Remove(friendtoAdd);
            HasChanges = meetingRepository.HasChanges();
            ((DelegateCommand) SaveCommand).RaiseCanExecuteChanged();
        }

        private bool OnRemoveFriendCanExecute()
        {
            return SelectedAddedFriend != null;
        }

        private void OnRemoveFriendExecute()
        {
            var friendToRemove = SelectedAddedFriend;

            Meeting.Model.Friends.Remove(friendToRemove);
            AddedFriends.Remove(friendToRemove);
            AvailableFriends.Add(friendToRemove);
            HasChanges = meetingRepository.HasChanges();
            ((DelegateCommand) SaveCommand).RaiseCanExecuteChanged();
        }

        private void SetTitle()
        {
            Title = Meeting.Title;
        }

        private void SetupPicklist()
        {
            var meetingFriendIds = Meeting.Model.Friends.Select(f => f.Id).ToList();
            var addedFriends = allFriends.Where(f => meetingFriendIds.Contains(f.Id)).OrderBy(f => f.Id);
            var availableFriends = allFriends.Except(addedFriends).OrderBy(f => f.FirstName);

            AddedFriends.Clear();
            AvailableFriends.Clear();

            foreach (var addedFriend in addedFriends)
            {
                AddedFriends.Add(addedFriend);
            }

            foreach (var availableFriend in availableFriends)
            {
                AvailableFriends.Add(availableFriend);
            }
        }

        #endregion
    }
}