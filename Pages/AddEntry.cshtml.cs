using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PhoneBook.Models;

namespace PhoneBook.Pages
{
    public class AddEntryModel : PageModel
    {
        [BindProperty]
        public string entryName { get; set; }
        [BindProperty]
        public string entryNumber { get; set; }

        public string errorMessage { get; set; }

        public SelectList listOfPhoneBooks { get; set; }

        [BindProperty]
        public string selectedPhoneBookId { get; set; }

        public List<PhoneBookObject> allPhoneBooks { get; set; }

        public void OnGet()
        {
            //Fetch all phone books
            GetPhoneBooks();
        }

        public void GetPhoneBooks()
        {
            allPhoneBooks = new List<PhoneBookObject>();
            try
            {
                DataRowCollection rows = DAL.ExecuteSP("GetAllPhoneBooks").Tables[0].Rows;
                if (rows.Count > 0)
                {
                    foreach (DataRow row in rows)
                    {
                        var phoneBook = new PhoneBookObject()
                        {
                            PhoneBookID = Guid.Parse(row["PhoneBookID"].ToString()),
                            PhoneBookName = row["PhoneBookName"].ToString(),
                        };
                        allPhoneBooks.Add(phoneBook);
                        listOfPhoneBooks = new SelectList(phoneBook.PhoneBookName, nameof(phoneBook.PhoneBookID));
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
