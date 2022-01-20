using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneBook.Models
{
    public class Search : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public bool SearchEntries(string searchString)
        {
            return true;
        }
    }
}
