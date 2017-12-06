using System.Data.Entity;
using System.Threading.Tasks;
using FriendOrganizer.DataAccess;
using FriendOrganizer.Model;

namespace FriendOrganizer.UI.Data.Repositories
{
    public class FriendRepository : GenericRepository<Friend, FriendOrganizerDbContext>, IFriendRepository
    {

        #region Constructors and Destructors

        public FriendRepository(FriendOrganizerDbContext context) : base(context)
        {
        }

        #endregion

        #region Public Methods and Operators

        public override async Task<Friend> GetByIdAsync(int id)
        {
            return await Context.Friends
                .Include(f => f.PhoneNumbers)
                .SingleAsync(f => f.Id == id);
        }

        public void RemovePhoneNumber(FriendPhoneNumber model)
        {
            Context.FriendPhoneNumbers.Remove(model);
        }

        #endregion
    }
}