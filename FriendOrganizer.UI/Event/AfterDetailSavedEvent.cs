using Prism.Events;

namespace FriendOrganizer.UI.Event
{
    public class AfterDetailSavedEvent : PubSubEvent<AfterDetailSavedEventArgs>
    {
    }

    public class AfterDetailSavedEventArgs
    {
        #region Public Properties

        public string DisplayMember { get; set; }
        public int Id { get; set; }
        public string ViewModelname { get; set; }

        #endregion
    }
}