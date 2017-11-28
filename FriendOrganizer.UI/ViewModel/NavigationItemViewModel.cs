namespace FriendOrganizer.UI.ViewModel
{
    public class NavigationItemViewModel : ViewModelBase
    {
        #region Fields

        private string displayMember;

        #endregion

        #region Constructors and Destructors

        public NavigationItemViewModel(int id, string displayMember)
        {
            Id = id;
            this.displayMember = displayMember;            
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

        #endregion
    }
}