using System;
using FriendOrganizer.Model;

namespace FriendOrganizer.UI.Wrapper
{
    public class MeetingWrapper : ModelWrapper<Meeting>
    {
        #region Constructors and Destructors

        public MeetingWrapper(Meeting model) : base(model)
        {
        }

        #endregion

        #region Public Properties

        public DateTime DateFrom
        {
            get => GetValue<DateTime>();
            set
            {
                SetValue(value);
                if (DateTo < DateFrom)
                {
                    DateTo = DateFrom;
                }
            } 
        }

        public DateTime DateTo
        {
            get => GetValue<DateTime>();
            set
            {
                SetValue(value);
                if (DateTo < DateFrom)
                {
                    DateFrom = DateTo;
                }
            }
        }

        public int Id => Model.Id;

        public string Title
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        #endregion
    }
}