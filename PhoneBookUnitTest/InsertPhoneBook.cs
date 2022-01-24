using Xunit;
using System.Collections;
using System;
using PhoneBook;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PhoneBookUnitTest
{
    [TestClass]
    public class InsertPhoneBook
    {
        [Fact]
        public void InsertPhoneBookTest(){
            
            var parameters = new Hashtable()
            {
                {"@PhoneBookName", "Unit Test Phone Book"},
                {"@PhoneBookID", Guid.NewGuid()}
            };
            try
            {
                var phoneBookInserted = DAL.ExecuteScalarSP("InsertPhoneBook", parameters).ToString();
                if (!string.IsNullOrWhiteSpace(phoneBookInserted))
                {
                    Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(phoneBookInserted);
                }
                else
                {
                    Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(phoneBookInserted);
                }
            }
            catch (Exception ex)
            {
                throw;
            }}
    }
}
