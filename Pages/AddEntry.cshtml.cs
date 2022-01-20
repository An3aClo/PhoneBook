using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using PhoneBook.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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
            AllPhoneBooks = GetPhoneBooksAPICall().Result;
        }

        public IActionResult OnPost()
        {
            //Insert phone book entry 
            var entryDetail = new EntryFormBody
            {
                EntryName = EntryName,
                EntryNumber = EntryNumber,
                SelectedPhoneBookId = SelectedPhoneBookId
            };
            var isentryInserted =  Convert.ToBoolean(InsertEntryAPICall(entryDetail).Result);
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

        /// <summary>
        /// This method makes and API call to insert an entry
        /// </summary>
        /// <param name="entryDetail"></param>
        /// <returns>string</returns>
        public async Task<string> InsertEntryAPICall(EntryFormBody entryDetail)
        {
            var payload = JsonConvert.SerializeObject(entryDetail);
            HttpMethod method = HttpMethod.Post;
            //var URL = _configuration.GetSection("EnvironmentEndPoint").Value + "/createLead";  //ToDo - Andrea - Move this to reference a dynamic variable in AWS Parameter Store
            var URL = "https://localhost:44338/Api/Entry/InsertEntry";  //TODO:: - Andrea - Move this to reference a dynamic variable in AWS Parameter Store
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

        /// <summary>
        /// This methods makes and API call to fetch all the phone books
        /// </summary>
        /// <returns>List<PhoneBookObject></returns>
        public async Task<List<PhoneBookObject>> GetPhoneBooksAPICall()
        {
            HttpMethod method = HttpMethod.Get;
            //var URL = _configuration.GetSection("EnvironmentEndPoint").Value + "/createLead";  //ToDo - Andrea - Move this to reference a dynamic variable in AWS Parameter Store
            var URL = "https://localhost:44338/Api/PhoneBook/GetPhoneBooks";  //TODO:: - Andrea - Move this to reference a dynamic variable in AWS Parameter Store
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
                using (HttpResponseMessage res = client.SendAsync(request).Result)
                {
                    using (HttpContent content = res.Content)
                    {
                        response = await content.ReadAsStringAsync();
                    }
                }
            }
            return JsonConvert.DeserializeObject<List<PhoneBookObject>>(response);
        }
    }
}
