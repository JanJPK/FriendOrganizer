using System;
using System.Collections.Generic;
using FriendOrganizer.Model;

namespace FriendOrganizer.UI.Wrapper
{
    /// <summary>
    ///     Wrapper that allows us to implement interfaces (like INotifyDataErrorInfo) without "polluting" the actual model
    ///     (Friend).
    /// </summary>
    public class FriendWrapper : ModelWrapper<Friend>
    {
        #region Constructors and Destructors

        public FriendWrapper(Friend model) : base(model)
        {
        }

        #endregion

        #region Public Properties

        public string Email
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public int? FavoriteLanguageId
        {
            get => GetValue<int?>();
            set => SetValue(value);
        }

        public string FirstName
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public int Id => Model.Id;

        public string LastName
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        #endregion

        #region Methods

        protected override IEnumerable<string> ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(FirstName):
                {
                    if (string.Equals(FirstName, "Robot", StringComparison.OrdinalIgnoreCase))
                    {
                        yield return "Robots are not valid friends";
                    }
                    break;
                }
            }
        }

        #endregion
    }
}