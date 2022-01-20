using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;
using PhoneBook.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace PhoneBook.Pages
{
    public class AddPhoneBookModel : PageModel
    {
        [BindProperty]
        public string PhoneBookName {get; set;}
        public string ErrorMessage { get; set; }

        public IActionResult OnPost()
        { 
            //Insert phone book
            var phoneBookDetail = new PhoneBookFormBody
            {
                PhoneBookName = PhoneBookName
            };
            var isBookInserted = Convert.ToBoolean(InsertPhoneBook(phoneBookDetail).Result);
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

        /// <summary>
        /// This method makes and API call to insert a phone book in the database
        /// </summary>
        /// <param name="phoneBookDetail"></param>
        /// <returns>string</returns>
        public async Task<string> InsertPhoneBook(PhoneBookFormBody phoneBookDetail)
        {
            var payload = JsonConvert.SerializeObject(phoneBookDetail);
            HttpMethod method = HttpMethod.Post;
            var URL = DAL.GetEnviromentKey() + "/Api/PhoneBook/InsertPhoneBook"; 
            var response = string.Empty;

            var httpClientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
                {
                    return true;
                }
            };
            using (var client = new HttpClient(httpClientHandler))
            using (var request = new HttpRequestMessage(method, URL))
            {
                request.Content = new StringContent(payload, Encoding.UTF8, "application/json");

                using (HttpResponseMessage res = client.SendAsync(request).Result)
                {
                    using (HttpContent content = res.Content)
                    {
                        response = await content.ReadAsStringAsync();
                    }
                }
            }
            return response;
        }
    }
}
