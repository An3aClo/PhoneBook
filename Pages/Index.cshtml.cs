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
using Newtonsoft.Json;
using System.Net.Http;

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
            //Fetch all phone books 
            AllPhoneBooks = GetPhoneBooksAPICall().Result;
            
            if (AllPhoneBooks.Count>0)
            {
                SelectedPhoneBook = AllPhoneBooks.First();
                //Fetch all contacts of phone books
                GetAllContacts(SelectedPhoneBook.PhoneBookID);
            }
        }

        public void OnPostFetchPhoneBookContact(Guid phoneBookId)
        {
            AllPhoneBooks = GetPhoneBooksAPICall().Result;
            SelectedPhoneBook = AllPhoneBooks.Find(b => b.PhoneBookID == phoneBookId);
            GetAllContacts(phoneBookId);
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
