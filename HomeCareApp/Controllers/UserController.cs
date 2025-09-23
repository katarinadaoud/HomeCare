using Microsoft.AspNetCore.Mvc;
using HomeCareApp.Models;
using HomeCareApp.ViewModels;

namespace HomeCareApp.Controllers;

public class UserController : Controller
{
    private readonly AppDbContext _appDbContext;

    public UserController(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public IActionResult Table()
    {
        List<User> users = _appDbContext.Users.ToList();
        var usersViewModel = new UsersViewModel(users, "Table");
        return View(usersViewModel);
    }

    public IActionResult Grid()
    {
        List<User> users = _appDbContext.Users.ToList();
        var usersViewModel = new UsersViewModel(users, "Grid");
        return View(usersViewModel);
    }
    
    public IActionResult Details(int id)
    {
        List<User> users = _appDbContext.Users.ToList();
        var user = users.FirstOrDefault(i => i.User_id == id);
        if (user == null)
            return NotFound();
        return View(user);
    }
}