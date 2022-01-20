using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections;

namespace PhoneBook.Pages
{
    public class AddPhoneBookModel : PageModel
    {
        [BindProperty]
        public string PhoneBookName {get; set;}
        public string ErrorMessage { get; set; }

        public IActionResult OnPost()
        {
            //TODO :: Call API here  
            //Insert phone book
            var isBookInserted = InsertPhoneBook();
            if (isBookInserted)
            {
                return Redirect("./Index");
            }
            else
            {
                ErrorMessage = "Oops! Something went wrong when adding a phone book.";
                return Page();
            }                      
        }

        //TODO :: Move to API  
        public bool InsertPhoneBook()
        {
            var parameters = new Hashtable()
            {
                {"@PhoneBookName", PhoneBookName },
                { "@PhoneBookID", Guid.NewGuid() }
            };
            try
            {                
                var phoneBookInserted = DAL.ExecuteScalarSP("InsertPhoneBook", parameters).ToString();
                if (!string.IsNullOrWhiteSpace(phoneBookInserted))
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
    }
}
