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
        var item = new List<Item>();
        var item1 = new Item();
        item1.FullName = "John Doe";
        item1.Description = "This is a sample item description.";
    
        
        item.Add(item1);
        
        ViewBag.CurrentViewName = "List of Items";  
        return View(item);
    }
}