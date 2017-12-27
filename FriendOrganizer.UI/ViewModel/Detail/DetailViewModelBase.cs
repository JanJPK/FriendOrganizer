using System.Threading.Tasks;
using System.Windows.Input;
using FriendOrganizer.UI.Event;
using Prism.Commands;
using Prism.Events;

namespace FriendOrganizer.UI.ViewModel.Detail
{
    public abstract class DetailViewModelBase : ViewModelBase, IDetailViewModel
    {
        #region Fields

        protected readonly IEventAggregator EventAggregator;
        private bool hasChanges;

        #endregion

        #region Constructors and Destructors

        public DetailViewModelBase(IEventAggregator eventAggregator)
        {
            EventAggregator = eventAggregator;
            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
            DeleteCommand = new DelegateCommand(OnDeleteExecute);
        }

        #endregion

        #region Public Properties

        public ICommand DeleteCommand { get; }

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

        public ICommand SaveCommand { get; }

        #endregion

        #region Public Methods and Operators

        public abstract Task LoadAsync(int? id);

        #endregion

        #region Methods

        protected abstract void OnDeleteExecute();
        protected abstract bool OnSaveCanExecute();
        protected abstract void OnSaveExecute();

        protected virtual void RaiseDetailDeletedEvent(int modelId)
        {
            EventAggregator.GetEvent<AfterDetailDeletedEvent>()
                .Publish(new AfterDetailDeletedEventArgs
                {
                    Id = modelId,
                    ViewModelName = GetType().Name
                });
        }

        protected virtual void RaiseDetailSavedEvent(int modelId, string displayMember)
        {
            EventAggregator.GetEvent<AfterDetailSavedEvent>()
                .Publish(new AfterDetailSavedEventArgs
                {
                    Id = modelId,
                    DisplayMember = displayMember,
                    ViewModelname = GetType().Name
                });
        }

        #endregion
    }
}