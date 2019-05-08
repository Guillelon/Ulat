using GuilleCoin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GuilleCoin.DAL
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public int SelectedUser { get; set; }
        public decimal AmountToSend { get; set; }
        public Guid WalletId { get; set; }
        public IList<TransactionDTO> Transactions { get; set; }
        public IList<UserDTO> AvailableUsers { get; set; }
        public UserDTO()
        {
            Transactions = new List<TransactionDTO>();
        }
    }

    public class TransactionDTO
    {
        public DateTime CreateDate { get; set; }
        public bool Sender { get; set; }
        public string OtherUser { get; set; }
        public decimal Amount { get; set; }
    }
}