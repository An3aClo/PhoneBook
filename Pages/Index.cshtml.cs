using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PhoneBook.Models;
using System.Data;
using System.Collections;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

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
        public string SearchString { get; internal set; }

        [BindProperty]
        public String SearchTerm { get; set; }

        /// <summary>
        /// This is the method which gets executed on page load
        /// </summary>
        public void OnGet()
        {
            //Fetch all phone books 
            AllPhoneBooks = GetPhoneBooksAPICall().Result;
            
            if (AllPhoneBooks.Count>0)
            {
                //Set default PhoneBook
                SelectedPhoneBook = AllPhoneBooks.First();
                //Fetch all contacts of phone books
                var phoneBookDetail = new PhoneBookDetail
                {
                    PhoneBookId = SelectedPhoneBook.PhoneBookID
                };
                AllPhoneBookEntries = GetAllEntriesForPhoneBookAPICall(phoneBookDetail).Result;
            }
        }

        /// <summary>
        /// This method execute every time when one of the phone book buttons are clicked to get the new set of entries for a phone book
        /// </summary>
        /// <param name="phoneBookId"></param>
        public void OnPostFetchPhoneBookContact(Guid phoneBookId)
        {
            //Fetch all the phone books
            AllPhoneBooks = GetPhoneBooksAPICall().Result;
            //Make the chosen phone book the selected book
            SelectedPhoneBook = AllPhoneBooks.Find(b => b.PhoneBookID == phoneBookId);
            //Fetch all contacts of phone books
            var phoneBookDetail = new PhoneBookDetail
            {
                PhoneBookId = SelectedPhoneBook.PhoneBookID
            };
            AllPhoneBookEntries = GetAllEntriesForPhoneBookAPICall(phoneBookDetail).Result;
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

        /// <summary>
        /// This method makes an API call to get all the entries for a specific phone book
        /// </summary>
        /// <param name="phoneBookDetail">This is an object containing the id of the phone book</param>
        /// <returns>List<EntryObject></returns>
        public async Task<List<EntryObject>> GetAllEntriesForPhoneBookAPICall (PhoneBookDetail phoneBookDetail)
        {
            var payload = JsonConvert.SerializeObject(phoneBookDetail);
            HttpMethod method = HttpMethod.Get;
            //var URL = _configuration.GetSection("EnvironmentEndPoint").Value + "/createLead";  //ToDo - Andrea - Move this to reference a dynamic variable in AWS Parameter Store
            var URL = "https://localhost:44338/Api/Entry/GetEntry";  //TODO:: - Andrea - Move this to reference a dynamic variable in AWS Parameter Store
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
            return JsonConvert.DeserializeObject<List<EntryObject>>(response);
        }

        public async Task<List<EntryObject>> GetAllEntriesAPICall()
        {
            HttpMethod method = HttpMethod.Get;
            //var URL = _configuration.GetSection("EnvironmentEndPoint").Value + "/createLead";  //ToDo - Andrea - Move this to reference a dynamic variable in AWS Parameter Store
            var URL = "https://localhost:44338/Api/Entry/GetAllEntry";  //TODO:: - Andrea - Move this to reference a dynamic variable in AWS Parameter Store
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
            return JsonConvert.DeserializeObject<List<EntryObject>>(response);
        }

        public void OnPost()
        {
            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                //Fetch All entries
                List<EntryObject> allEntries = GetAllEntriesAPICall().Result;

                //Filter data
                AllPhoneBookEntries = (List<EntryObject>)allEntries.FindAll(s => s.EntryName.Contains(SearchTerm) || s.EntryNumber.Contains(SearchTerm));

                //Fetch all phone books 
                AllPhoneBooks = GetPhoneBooksAPICall().Result;
            }
        }
    }
}
