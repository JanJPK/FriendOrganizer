using System;
using System.Threading.Tasks;
using FriendOrganizer.Model;
using FriendOrganizer.UI.Data.Repositories;
using FriendOrganizer.UI.View.Services;
using FriendOrganizer.UI.Wrapper;
using Prism.Commands;
using Prism.Events;

namespace FriendOrganizer.UI.ViewModel.Detail
{
    public class MeetingDetailViewModel : DetailViewModelBase, IMeetingDetailViewModel
    {
        #region Fields

        private MeetingWrapper meeting;

        private readonly IMeetingRepository meetingRepository;
        private readonly IMessageDialogService messageDialogService;

        #endregion

        #region Constructors and Destructors

        public MeetingDetailViewModel(IEventAggregator eventAggregator) : base(eventAggregator)
        {
        }

        public MeetingDetailViewModel(IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService,
            IMeetingRepository meetingRepository) : base(eventAggregator)
        {
            this.meetingRepository = meetingRepository;
            this.messageDialogService = messageDialogService;
        }

        #endregion

        #region Public Properties

        public MeetingWrapper Meeting
        {
            get => meeting;
            private set
            {
                meeting = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Public Methods and Operators

        public override async Task LoadAsync(int? id)
        {
            var meeting = id.HasValue
                ? await meetingRepository.GetByIdAsync(id.Value)
                : CreateNewMeeting();
            InitializeMeeting(meeting);
        }

        #endregion

        #region Methods

        protected override void OnDeleteExecute()
        {
            var result =
                messageDialogService.ShowOkCancelDialog($"Do you really want to delete the meeting?", "Question");
            if (result == MessageDialogResult.OK)
            {
                meetingRepository.Remove(Meeting.Model);
                meetingRepository.SaveAsync();
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
            RaiseDetailSavedEvent(Meeting.Id, Meeting.Title);
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
            };
            ((DelegateCommand) SaveCommand).RaiseCanExecuteChanged();

            // Triggers validation on newly created meeting.
            if (Meeting.Id == 0)
            {
                Meeting.Title = "";
            }
        }

        #endregion
    }
}