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
    public class GetAllEntries
    {       
        [Fact]
        public void GetAllEntriesTest()
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
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(allEntries);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
