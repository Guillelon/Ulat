using GuilleCoin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GuilleCoin.DAL
{
    public class GuilleCoinRepository
    {
        private GuilleCoinDB _context;

        public GuilleCoinRepository()
        {
            _context = new GuilleCoinDB();
        }

        public IList<User> GetUsers()
        {
            return _context.User.ToList();
        }

        public bool CheckIfUserExists(string ip)
        {
            return _context.User.Where(u => u.Ip == ip).Any();
        }

        public IList<Transaction> GetAllTransactions()
        {
            return _context.Transaction.ToList();

        }

        public IList<Transaction> TransactionsFrom(int id)
        {
            return _context.Transaction.Where(t => t.SenderId == id).ToList();
        }

        public IList<Transaction> TransactionsTo(int id)
        {
            return _context.Transaction.Where(t => t.ReceiverId == id).ToList();
        }

        public IList<UserDTO> GetAvailableUsers(int id)
        {
            var users = _context.User.Where(u => u.Id != id).ToList();
            var dtos = new List<UserDTO>();
            foreach (var user in users)
            {
                var dto = new UserDTO();
                dto.Name = user.Name;
                dto.Id = user.Id;
                dtos.Add(dto);
            }
            return dtos;
        }

        public User GetUser(int id)
        {
            return _context.User.Where(u => u.Id == id).FirstOrDefault();
        }

        public UserDTO GetUser(string walletId)
        {
            var wallet = Guid.Parse(walletId);
            var user = _context.User.Where(u => u.WalletId == wallet).FirstOrDefault();
            if (user != null)
            {
                var dto = new UserDTO();
                dto.Id = user.Id;
                dto.Name = user.Name;
                dto.Amount = GetBalance(user.Id);
                dto.WalletId = user.WalletId;
                dto.AvailableUsers = GetAvailableUsers(user.Id);
                var transactions = _context.Transaction.Where(t => t.ReceiverId == user.Id || 
                                                                   t.SenderId == user.Id).ToList();
                foreach(var transaction in transactions)
                {
                    var tranDto = new TransactionDTO();
                    tranDto.CreateDate = transaction.CreateDate;
                    tranDto.Amount = transaction.Amount;
                    if(transaction.SenderId == user.Id)
                    {
                        tranDto.Sender = true;
                        tranDto.OtherUser = transaction.Receiver.Name;
                    }
                    else
                    {
                        tranDto.Sender = false;
                        tranDto.OtherUser = transaction.Sender.Name;
                    }
                    dto.Transactions.Add(tranDto);
                }
                dto.Transactions = dto.Transactions.OrderByDescending(t => t.CreateDate).ToList();
                return dto;
            }
            return null;
        }

        public decimal GetBalance(int id)
        {
            var user = _context.User.Where(u => u.Id == id).FirstOrDefault();
            if(user != null)
            {
                var initAmount = user.Amount;
                var sent = _context.Database.SqlQuery<decimal>("SELECT  COALESCE(SUM(AMOUNT),0) FROM Transactions WHERE SENDERID = " + id).First();
                var received = _context.Database.SqlQuery<decimal>("SELECT  COALESCE(SUM(AMOUNT),0) FROM Transactions WHERE ReceiverId = " + id).First();
                
                return (initAmount + received) - sent;
            }
            return 0;
        }
        public User AddUser(string ip, string name)
        {
            var user = new User();
            user.Ip = ip;
            user.Name = name;
            user.WalletId = Guid.NewGuid();
            user.Amount = 25m;
            _context.User.Add(user);
            _context.SaveChanges();
            return user;
        }

        public Transaction MakeTransaction(int senderId, int receiverId, decimal amount)
        {
            var transaction = new Transaction();
            transaction.SenderId = senderId;
            transaction.ReceiverId = receiverId;
            transaction.Amount = amount;
            transaction = _context.Transaction.Add(transaction);
            _context.SaveChanges();
            return transaction;
        }
    }
}