using Xunit;
using System.Collections;
using System;
using PhoneBook;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PhoneBookUnitTest
{
    [TestClass]
    public class InsertEntry
    {
        [Fact]
        public void InsertEntryTest()
        {
            var parameters = new Hashtable()
            {
                {"@EntryID", Guid.NewGuid() },
                { "@EntryName", "Unit Test Entry" },
                { "@EntryNumber", "123 123 1233" },
                { "@PhoneBookId", "1DCC9B4A-DBE6-46AC-AB3E-025AEA300DFA" }
            };

            var entryInserted = DAL.ExecuteScalarSP("InsertEntry", parameters).ToString();
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(entryInserted);
        }
    }
}
