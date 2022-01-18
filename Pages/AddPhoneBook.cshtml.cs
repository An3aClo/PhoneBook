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
    public class AddPhoneBookModel : PageModel
    {
        [BindProperty]
        public string phoneBookName {get; set;}

        public void OnGet()
        {

        }

        public void OnPost()
        {
            InsertPhoneBook();
            //Call API here
            //Insert phone book
        }

        public void InsertPhoneBook()
        {
            var parameters = new Hashtable()
            {
                {"@PhoneBookName", phoneBookName },
                { "@PhoneBookID", new Guid() }
            };
            try
            {
                var insertedPhoneBook = DAL.ExecuteScalarSP("InsertPhoneBook", parameters).ToString(); 
              
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
