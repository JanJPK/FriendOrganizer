using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FriendOrganizer.Model;
using FriendOrganizer.UI.Data;

namespace FriendOrganizer.UI.ViewModel
{
    public class FriendDetailViewModel : ViewModelBase, IFriendDetailViewModel
    {
        public IFriendDataService dataService;

        private Friend friend;

        public Friend Friend
        {
            get { return friend; }
            private set
            {
                friend = value;
                OnPropertyChanged();
            }
        }

        public FriendDetailViewModel(IFriendDataService dataService)
        {
            this.dataService = dataService;
        }

        public async Task LoadAsync(int id)
        {
            Friend = await dataService.GetByIdAsync(id);
        }
    }
}
