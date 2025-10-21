using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HomeCareApp.Models;

namespace HomeCareApp.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        ViewBag.Role = "public"; // frontpage for all users
        return View();
    }

    
}