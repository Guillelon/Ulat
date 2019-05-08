using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace GuilleCoin.Models
{
    public class GuilleCoinDB: DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
    }
}