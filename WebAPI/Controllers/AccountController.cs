using OCBC.BusinessEntity;
using OCBC.BusinessService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace WebAPI.Controllers
{
    public class AccountController : Controller
    {
        private OCBCService ocbcSvc;
        public AccountController()
        {
            ocbcSvc = new OCBCService();
        }

        //home page before login
        public ActionResult Index()
        {
            ViewData["login"] = false;
            return View();
        }

        //register page 
        public ActionResult Register()
        {
            ViewData["login"] = false;
            return View();
        }

        //login page before login
        public ActionResult Login()
        {
            ViewData["login"] = false;
            return View();
        }

        //login out and redirect to home page
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            //clear the session
            Session.Abandon();
            ViewData["login"] = false;
            return RedirectToAction("Index");
        }

        /// <summary>
        /// save registration info and redirect to home page
        /// </summary>
        /// <param name="registerDetails">registration detail</param>
        /// <returns>retrun to home page if success</returns>
        [HttpPost]
        public ActionResult SaveRegisterDetails(RegisterViewModel registerDetails)
        {
            ViewData["login"] = false;
            if (ModelState.IsValid)
            {
                //step1: initial the user entity to save into database
                User user = new User()
                {
                    Id = Guid.NewGuid(),
                    FirstName = registerDetails.FirstName,
                    LastName = registerDetails.LastName,
                    Email = registerDetails.Email,
                    PhoneNumber = registerDetails.PhoneNumber,
                    Password = registerDetails.Password,
                    Balance = 0,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    LastLoginDate = DateTime.Now,
                    IsDeleted = false
                };

                //step2: save into database
                var result = ocbcSvc.SaveRegisterDetails(user);
                ViewBag.Message = result;

                //step3: direct to home page if user successfully saved into database
                if (result.Contains("successfully"))
                {
                    ViewData["login"] = true;
                    return View("Index", user);
                }
                else
                {
                    //stay in register page and display the error message returned from server
                    return View("Register", registerDetails);
                }
            }
            else
            {
                //if the validation fails, return the model with errors to the view so can display the error message
                return View("Register", registerDetails);
            }
        }

        /// <summary>
        /// post login page 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            ViewData["login"] = false;
            if (ModelState.IsValid)
            {
                //validate user is existing in database or not.
                var user = ValidateUser(model);

                if (user != null)
                {
                    //redirect to home page if user found in database
                    ViewData["login"] = true;
                    //FormsAuthentication.SetAuthCookie(model.Email, false);
                    return View("Index", user);
                }
                else
                {
                    //stay in login page and display error message
                    ModelState.AddModelError("Failure", "Either Email or Password is Incorrect.");
                    return View();
                }
            }
            else
            {
                //ff model state is invalid, return the model with error message to the login page
                return View(model);
            }
        }

        /// <summary>
        /// transfer money
        /// </summary>
        /// <param name="senderId">the identifier of current user</param>
        /// <param name="recipientEmail">email is treated as the account id of recipient</param>
        /// <param name="transferAmount">money to transfer</param>
        /// <returns>if success, return current user latest account balance to UI</returns>
        [HttpPost]
        public ActionResult TransferMoney(string senderId, string recipientEmail, decimal transferAmount)
        {
            //step1: initial transaction model for inserting into transfer history table
            TransferHistory transfer = new TransferHistory();
            transfer.Id = Guid.NewGuid();
            transfer.SenderId = new Guid(senderId);
            transfer.RecipientEmail = recipientEmail;
            transfer.TransferAmount = transferAmount;

            //step2: save transaction model into database
            var result = ocbcSvc.AddTransactionInfo(transfer);

            //step3: return current user's latest account balance to UI
            if (result.Contains("successfully"))
                return Json(new { HasError = false, Message = result.Split('|')[0], Balance = result.Split('|')[1] }, JsonRequestBehavior.AllowGet);
            return Json(new { HasError = true, Message = result }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// deposit money
        /// </summary>
        /// <param name="userId">the identifier of current user</param>
        /// <param name="amount">money to deposit</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DepositMoney(string userId, decimal amount)
        {
            //deposit the money base on user identifier
            var result = ocbcSvc.DepositMoney(userId, amount);
            //return latest balance to UI if deposit successfully
            if (result.Contains("successfully"))
                return Json(new { HasError = false, Message = result.Split('|')[0], Balance = result.Split('|')[1] }, JsonRequestBehavior.AllowGet);
            return Json(new { HasError = true, Message = result }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// validate if current user exists in data base base on email and password
        /// </summary>
        /// <param name="model">email is treat as account id </param>
        /// <returns></returns>
        public User ValidateUser(LoginViewModel model)
        {
            return ocbcSvc.ValidateUser(model);
        }

        /// <summary>
        ///TODO: display transaction history list
        /// </summary>
        /// <returns></returns>
        public ActionResult RenderTransferHistoryPartialView()
        {
            return PartialView("_TransferHistory", new List<TransferHistory>());
        }
    }
}