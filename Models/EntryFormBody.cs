﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneBook.Models
{
    public class EntryFormBody
    {
        public string SelectedPhoneBookId { get; set; }
        public string EntryName { get; set; }
        public string EntryNumber { get; set; }
    }
}
