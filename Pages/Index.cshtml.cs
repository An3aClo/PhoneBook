using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PhoneBook.Models;
using System.Data;
using System.Collections;

namespace PhoneBook.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public List<PhoneBookObject> allPhoneBooks { get; set; }

        [BindProperty]
        public List<EntryObject> allPhoneBookEntries { get; set; }

        [BindProperty]
        public PhoneBookObject defaultPhoneBook { get; set; }

        [BindProperty]
        public string searchString { get; set; }
        

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            //Fetch all phone books
            GetPhoneBooks();
            defaultPhoneBook = allPhoneBooks.First();

            //Fetch all contacts of phone books
            GetAllContacts(defaultPhoneBook.PhoneBookID);            
        }

        public void OnPostFetchPhoneBookContact(Guid phoneBookId)
        {
            GetAllContacts(phoneBookId);
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
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void GetAllContacts( Guid phoneBookId)
        {
            allPhoneBookEntries = new List<EntryObject>();
            var parameters = new Hashtable()
            {
                {"@PhoneBookID", phoneBookId }
            };
            try
            {
                DataRowCollection rows = DAL.ExecuteSP("GetEntry", parameters).Tables[0].Rows;
                if (rows.Count > 0)
                {
                    foreach (DataRow row in rows)
                    {
                        var entry = new EntryObject()
                        {
                            EntryName =  row["EntryName"].ToString(),
                            EntryNumber = row["EntryNumber"].ToString()
                        };
                        allPhoneBookEntries.Add(entry);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void OnPostSearch()
        {
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                //filter data
                allPhoneBookEntries = (List<EntryObject>)allPhoneBookEntries.Where(s => s.EntryName.Contains(searchString) || s.EntryNumber.Contains(searchString));
            }
        }
    }
}
