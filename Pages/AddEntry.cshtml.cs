using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PhoneBook.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace PhoneBook.Pages
{
    public class AddEntryModel : PageModel
    {
        [BindProperty]
        public string entryName { get; set; }
        [BindProperty]
        public string entryNumber { get; set; }

        public string errorMessage { get; set; }


        [BindProperty]
        public string selectedPhoneBookId { get; set; }

        public List<PhoneBookObject> allPhoneBooks { get; set; }

        public void OnGet()
        {
            //Fetch all phone books
            GetPhoneBooks();
        }

        public IActionResult OnPost()
        {
            //Insert phone book entry 
            var isentryInserted = InsertEntry();
            if (isentryInserted)
            {
                return Redirect("./Index");
            }
            else
            {
                errorMessage = "Oops! Something went wrong when adding the an entry..";
                return Page();
            }
            //TODO :: Call API here
        }

        public bool InsertEntry()
        {
            var parameters = new Hashtable()
            {
                {"@EntryID", Guid.NewGuid() },
                { "@EntryName", entryName },
                { "@EntryNumber", entryNumber },
                { "@PhoneBookId", selectedPhoneBookId }   
            };
            try
            {
                var entryInserted = DAL.ExecuteScalarSP("InsertEntry", parameters).ToString();
                if (!string.IsNullOrWhiteSpace(entryInserted))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
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
    }
}
