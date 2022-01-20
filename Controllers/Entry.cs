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
            return GetAllEntriesForPhoneBooks(phoneBookDetail.PhoneBookId);
        }

        [HttpGet, Route("GetAllEntry")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public List<EntryObject> GetAllEntriesGet()
        {
            return GetAllEntries();
        }

        [HttpPost, Route("InsertEntry")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public bool InsertEntryPost([FromBody] EntryFormBody entry)
        {
            return InsertEntry(entry);
        }

        public List<EntryObject> GetAllEntriesForPhoneBooks(Guid phoneBookId)
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

        public List<EntryObject> GetAllEntries()
        {
            List<EntryObject> allEntries = new List<EntryObject>();            
            try
            {
                DataRowCollection rows = DAL.ExecuteSP("GetAllEntries").Tables[0].Rows;
                if (rows.Count > 0)
                {
                    foreach (DataRow row in rows)
                    {
                        var entry = new EntryObject()
                        {
                            EntryName = row["EntryName"].ToString(),
                            EntryNumber = row["EntryNumber"].ToString()
                        };
                        allEntries.Add(entry);
                    }
                }
                return allEntries;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool InsertEntry(EntryFormBody entry)
        {
            var parameters = new Hashtable()
            {
                {"@EntryID", Guid.NewGuid() },
                { "@EntryName", entry.EntryName },
                { "@EntryNumber", entry.EntryNumber },
                { "@PhoneBookId", entry.SelectedPhoneBookId }
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
    }
}