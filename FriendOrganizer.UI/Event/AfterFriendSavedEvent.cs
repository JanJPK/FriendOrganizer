using Prism.Events;

namespace FriendOrganizer.UI.Event
{
    public class AfterFriendSavedEvent : PubSubEvent<AfterFriendSavedEventArgs>
    {
    }

    public class AfterFriendSavedEventArgs
    {
        #region Public Properties

        public string DisplayMember { get; set; }
        public int Id { get; set; }

        #endregion
    }
}