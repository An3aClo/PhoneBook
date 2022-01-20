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
        public string EntryName { get; set; }
        [BindProperty]
        public string EntryNumber { get; set; }

        public string ErrorMessage { get; set; }

        [BindProperty]
        public string SelectedPhoneBookId { get; set; }

        public List<PhoneBookObject> AllPhoneBooks { get; set; }

        public void OnGet()
        {
            //Fetch all phone books
            GetPhoneBooks();
        }

        public IActionResult OnPost()
        {
            //TODO :: Call API here
            //Insert phone book entry 
            var isentryInserted = InsertEntry();
            if (isentryInserted)
            {
                return Redirect("./Index");
            }
            else
            {
                ErrorMessage = "Oops! Something went wrong when adding the an entry..";
                return Page();
            }            
        }

        //TODO :: Move this method to API
        public bool InsertEntry()
        {
            var parameters = new Hashtable()
            {
                {"@EntryID", Guid.NewGuid() },
                { "@EntryName", EntryName },
                { "@EntryNumber", EntryNumber },
                { "@PhoneBookId", SelectedPhoneBookId }   
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

        //TODO :: Move this method to API
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
    }
}
