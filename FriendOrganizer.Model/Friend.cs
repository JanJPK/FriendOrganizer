using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace FriendOrganizer.Model
{
    public class Friend
    {
        #region Constructors and Destructors

        public Friend()
        {
            PhoneNumbers = new Collection<FriendPhoneNumber>();
        }

        #endregion

        #region Public Properties

        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }

        public ProgrammingLanguage FavoriteLanguage { get; set; }

        public int? FavoriteLanguageId { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        public int Id { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        public ICollection<FriendPhoneNumber> PhoneNumbers { get; set; }

        #endregion
    }
}