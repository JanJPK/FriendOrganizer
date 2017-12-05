namespace FriendOrganizer.Model
{
    /// <summary>
    ///     Displayed in the list on the left - displays a short summary of a friend and allows to select one.
    /// </summary>
    public class LookupItem
    {
        #region Public Properties

        public string DisplayMember { get; set; }
        public int Id { get; set; }

        #endregion
    }

    /// <summary>
    ///     Allows to choose the default (nothing) value im ComboBox after selecting something else earlier.
    /// </summary>
    public class NullLookupItem : LookupItem
    {
        public new int? Id { get { return null; } }
    }
}