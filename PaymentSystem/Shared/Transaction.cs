﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentSystem.Shared
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public string SourceUsername { get; set; }
        public string DestinationUsername { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }
}


