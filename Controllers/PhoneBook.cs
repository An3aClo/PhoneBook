using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneBook.Controllers
{
    public class PhoneBook : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
