using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections;

namespace PhoneBook.Pages
{
    public class AddPhoneBookModel : PageModel
    {
        [BindProperty]
        public string phoneBookName {get; set;}
        public string errorMessage { get; set; }

        public IActionResult OnPost()
        {
            //Insert phone book
            var isBookInserted = InsertPhoneBook();
            if (isBookInserted)
            {
                return Redirect("./Index");
            }
            else
            {
                errorMessage = "Oops! Something went wrong when adding a phone book.";
                return Page();
            }
            //TODO :: Call API here            
        }

        public bool InsertPhoneBook()
        {
            var parameters = new Hashtable()
            {
                {"@PhoneBookName", phoneBookName },
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
