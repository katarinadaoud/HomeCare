using HomeCareApp.Models;

namespace HomeCareApp.ViewModels
{
    public class UsersViewModel
    {
        public IEnumerable<User> Users;
        public string? CurrentViewName;

        public UsersViewModel(IEnumerable<User> users, string? currentViewName)
        {
            Users = users;
            CurrentViewName = currentViewName;
        }
    }
}