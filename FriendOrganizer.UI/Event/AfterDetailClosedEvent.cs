﻿using Prism.Events;

namespace FriendOrganizer.UI.Event
{
    public class AfterDetailClosedEvent : PubSubEvent<AfterDetailClosedEventArgs>
    {
    }

    public class AfterDetailClosedEventArgs
    {
        #region Public Properties

        public int Id { get; set; }
        public string ViewModelName { get; set; }

        #endregion
    }
}