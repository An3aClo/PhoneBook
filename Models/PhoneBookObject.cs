using System;

namespace PhoneBook.Models
{
    public class PhoneBookObject
    {
        public Guid PhoneBookID { get; set; }
        public string PhoneBookName { get; set; }
        public EntryObject PhoneBookntries { get; set; }
    }
}
