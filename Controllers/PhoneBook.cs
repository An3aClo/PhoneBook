using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhoneBook.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Collections;

namespace PhoneBook.Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    public class PhoneBook : ControllerBase
    {
        [HttpGet, Route("GetPhoneBooks")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public List<PhoneBookObject> Post()
        {            
            return GetPhoneBooks();
        }

        [HttpPost, Route("InsertPhoneBook")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public bool InsertPhoneBookPost([FromBody] PhoneBookFormBody phoneBook)
        {
            return InsertPhoneBook(phoneBook.PhoneBookName);
        }

        public bool InsertPhoneBook(string PhoneBookName)
        {
            var parameters = new Hashtable()
            {
                {"@PhoneBookName", PhoneBookName},
                {"@PhoneBookID", Guid.NewGuid()}
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

        public List<PhoneBookObject> GetPhoneBooks()
        {
            List<PhoneBookObject> allPhoneBooks = new List<PhoneBookObject>();
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
                return allPhoneBooks;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
