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
        public List<PhoneBookObject> AllPhoneBooks { get; set; }

        [BindProperty]
        public List<EntryObject> AllPhoneBookEntries { get; set; }

        [BindProperty]
        public PhoneBookObject SelectedPhoneBook { get; set; }

        [BindProperty]
        public string SearchString { get; set; }  

        public void OnGet()
        {
            //TODO :: Call API here  
            //Fetch all phone books
            GetPhoneBooks();
            if (AllPhoneBooks.Count>0)
            {
                SelectedPhoneBook = AllPhoneBooks.First();
                //Fetch all contacts of phone books
                GetAllContacts(SelectedPhoneBook.PhoneBookID);
            }
        }

        public void OnPostFetchPhoneBookContact(Guid phoneBookId)
        {
            GetPhoneBooks();
            SelectedPhoneBook = AllPhoneBooks.Find(b => b.PhoneBookID == phoneBookId);
            GetAllContacts(phoneBookId);
        }

        public void GetPhoneBooks()
        {
            AllPhoneBooks = new List<PhoneBookObject>();
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
                        AllPhoneBooks.Add(phoneBook);
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
            AllPhoneBookEntries = new List<EntryObject>();
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
                        AllPhoneBookEntries.Add(entry);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void OnPostSearch( )
        {
            if (!string.IsNullOrWhiteSpace(SearchString))
            {
                //filter data
                AllPhoneBookEntries = (List<EntryObject>)AllPhoneBookEntries.Where(s => s.EntryName.Contains(SearchString) || s.EntryNumber.Contains(SearchString));
            }
        }
    }
}
