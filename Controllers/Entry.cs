using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using Microsoft.AspNetCore.Http;
using PhoneBook.Models;

namespace PhoneBook.Controllers
{    
    [ApiController]
    [Route("Api/[controller]")]
    public class Entry : ControllerBase
    {
        [HttpGet, Route("GetEntry")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public List<EntryObject> Post([FromBody] PhoneBookDetail phoneBookDetail)
        {
            return GetAllContacts(phoneBookDetail.PhoneBookId);
        }

        public List<EntryObject> GetAllContacts(Guid phoneBookId)
        {
            List<EntryObject> allPhoneBookEntries = new List<EntryObject>();
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
                            EntryName = row["EntryName"].ToString(),
                            EntryNumber = row["EntryNumber"].ToString()
                        };
                        allPhoneBookEntries.Add(entry);
                    }
                }
                return allPhoneBookEntries;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}