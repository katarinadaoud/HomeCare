using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HomeCareApp.Models;

namespace HomeCareApp.Controllers;

public class HomeController : Controller
{
    public IActionResult Table()
    {
        var user = new List<User>();
        var user1 = new User();
        user1.Name = "John Doe";
        user1.Phone = "12345678";
    
        
        user.Add(user1);
        
        ViewBag.CurrentViewName = "List of Users";  
        return View(user);
    }
}