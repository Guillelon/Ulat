using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GuilleCoin.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreateDate { get; set; }

        public virtual User Receiver { get; set; }
        public virtual User Sender { get; set; }

        public int ReceiverId { get; set; }
        public int SenderId { get; set; }

        public Transaction()
        {
            Amount = 0m;
            CreateDate = DateTime.Now;
        }
    }
}