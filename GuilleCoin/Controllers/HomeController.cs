using GuilleCoin.DAL;
using GuilleCoin.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GuilleCoin.Controllers
{
    public class HomeController : Controller
    {
        private GuilleCoinRepository _repo;

        public HomeController()
        {
            _repo = new GuilleCoinRepository();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AllTransactions()
        {
            return View();
        }

        public string GetAllTransactions()
        {
            var transactions = _repo.GetAllTransactions().Select(t => new { t.Id, t.CreateDate, Receiver = t.Receiver.Name, Sender= t.Sender.Name, Amount = ("Ǥ"+ t.Amount.ToString("N2"))  }).ToList();
            
            try
            {
                var json = JsonConvert.SerializeObject(transactions);
                return json;
            }
            catch (Exception e)
            {
                var hola = 2;
            }
            return null;
        }

        public ActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateUser(User user)
        {
            if(user.Name != null && user.Name.Length > 2)
            {
                var remoteIpAddress = Request.UserHostAddress;
                user = _repo.AddUser(remoteIpAddress, user.Name);
                return RedirectToAction("SaveUserInLocalStorage", new { @WalletId = user.WalletId.ToString() });
            }
            return View(user);
        }

        public ActionResult SaveUserInLocalStorage(string WalletId)
        {
            var user = new User();
            user.WalletId = Guid.Parse(WalletId);
            return View(user);
        }

        public ActionResult UserProfile(string walletId)
        {
            var dto = _repo.GetUser(walletId);
            if (TempData["SuccessMessage"] != null)
                ViewBag.SuccessMessage = TempData["SuccessMessage"];
            else if (TempData["ErrorMessage"] != null)
                ViewBag.ErrorMessage = TempData["ErrorMessage"];
            return View(dto);
        }

        [HttpPost]
        public ActionResult MakeTransaction(UserDTO user)
        {
            var userModel = _repo.GetUser(user.Id);
            if (user.AmountToSend > 0 && user.SelectedUser > 0)
            {
                var userBalance = _repo.GetBalance(user.Id);
                if(userBalance > user.AmountToSend)
                {
                    _repo.MakeTransaction(user.Id, user.SelectedUser, user.AmountToSend);
                    TempData["SuccessMessage"] = "La transacción se envió con éxito!";
                }
                else
                    TempData["ErrorMessage"] = "No tiene suficiente saldo para realizar esa transacción.";
            }
            else
            {
                TempData["ErrorMessage"] = "Debe seleccionar un receptor y colocar un monto mayor a 0.";
            }

            return RedirectToAction("UserProfile", new { @walletId = userModel.WalletId.ToString() });
        }
    }
}