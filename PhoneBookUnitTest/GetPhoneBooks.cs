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
    public class GetPhoneBooks
    {
        [Fact]
        public void GetPhoneBooksTest()
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
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(allPhoneBooks);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
