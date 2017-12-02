namespace FriendOrganizer.Model
{
    /// <summary>
    ///     Displayed in the list on the left - displays a short summary of a friend and allows to select one.
    /// </summary>
    public class LookupItem 
    {
        public int Id { get; set; }
        public string DisplayMember { get; set; }
    }
}