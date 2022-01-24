using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xunit;
using System.Collections;
using System;
using PhoneBook;
using PhoneBook.Models;
using System.Collections.Generic;
using System.Data;

namespace PhoneBookUnitTest
{
    [TestClass]
    public class Entry
    {
        [Theory]
        [InlineData("1DCC9B4A-DBE6-46AC-AB3E-025AEA300DFA")]
        public void GetAllEntriesForPhoneBooksTest(string phoneBookId)
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
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(allPhoneBookEntries);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
