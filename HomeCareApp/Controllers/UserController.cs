using Microsoft.AspNetCore.Mvc;
using HomeCareApp.Models;
using HomeCareApp.ViewModels;

namespace HomeCareApp.Controllers;

public class UserController : Controller
{
    private readonly UserDbContext _userDbContext;

    public UserController(UserDbContext userDbContext)
    {
        _userDbContext = userDbContext;
    }

    public IActionResult Table()
    {
        List<User> users = _userDbContext.Users.ToList();
        var usersViewModel = new UsersViewModel(users, "Table");
        return View(usersViewModel);
    }

    public IActionResult Grid()
    {
        List<User> users = _userDbContext.Users.ToList();
        var usersViewModel = new UsersViewModel(users, "Grid");
        return View(usersViewModel);
    }
    
    public IActionResult Details(int id)
    {
        List<User> users = _userDbContext.Users.ToList();
        var user = users.FirstOrDefault(i => i.User_id == id);
        if (user == null)
            return NotFound();
        return View(user);
    }
}