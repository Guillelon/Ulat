using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GuilleCoin.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Ip { get; set; }
        public string Name { get; set; }
        public Guid WalletId { get; set; }
        public decimal Amount { get; set; }
    }
}