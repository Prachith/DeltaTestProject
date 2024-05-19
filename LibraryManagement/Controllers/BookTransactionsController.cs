using LibraryManagement.Extensions;
using LibraryManagement.Models;
using LibraryManagement.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    public class BookTransactionsController : Controller
    {
        private readonly BookTransactionsService _bookTransactionsService;
        private readonly ValidationService _validationService;

        public static BookTransactionModel transactionModel = new BookTransactionModel();
        public static BookReturnModel bookReturnModel = new BookReturnModel();

        public BookTransactionsController(BookTransactionsService bookTransactionsService, ValidationService validationService)
        {
            _bookTransactionsService = bookTransactionsService;
            _validationService = validationService;
        }

        public async Task<IActionResult> Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(SessionVariables.SessionKeyUserId))) return RedirectToAction("Login", "User");
            var loadbooks = await Initialize();
            
            return View(transactionModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(Transactions model) 
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(SessionVariables.SessionKeyUserId))) return RedirectToAction("Login", "User");

            var bookValidation = _validationService.ValidateBook(model, transactionModel);
            if (bookValidation != string.Empty)
            {
                ViewBag.message = bookValidation;
                return View(transactionModel);
            }
            var bookModel = await _bookTransactionsService.AddBook(model);

            if (transactionModel.Transactions != null) transactionModel.Transactions.Add(bookModel);
            else
                transactionModel.Transactions = new List<Transactions> { bookModel };

            return View(transactionModel);

        }

        public IActionResult Delete(int Id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(SessionVariables.SessionKeyUserId))) return RedirectToAction("Login", "User");
            var item = transactionModel.Transactions.Single(x=>x.BookId==Id);
            
            if (item != null)  transactionModel.Transactions.Remove(item);            

            return View("Index",transactionModel);
        }

        public async Task<IActionResult> Complete()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(SessionVariables.SessionKeyUserId))) return RedirectToAction("Login", "User");

            var sessionId = HttpContext.Session.GetInt32(SessionVariables.SessionKeyUserIdKey);
            string response = string.Empty;
            
            var complete = await _bookTransactionsService.AddBookTransaction(transactionModel, (int)sessionId);
            
            ViewBag.message = complete;

            var loadbooks = await Initialize();

            return View("Index", loadbooks);
        }

        private async Task<BookTransactionModel> Initialize()
        {
            var loadbooks = await _bookTransactionsService.LoadBookTransactions();
            transactionModel = loadbooks;
            return loadbooks;
        }

        public async Task<IActionResult> Return()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(SessionVariables.SessionKeyUserId))) return RedirectToAction("Login", "User");

            var sessionId = HttpContext.Session.GetInt32(SessionVariables.SessionKeyUserIdKey);

            var loadTrans = await _bookTransactionsService.LoadUserTransactions((int)sessionId);
            bookReturnModel = loadTrans;

            return View(bookReturnModel);
        }

        public async Task<IActionResult> Load(BookReturnModel model)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(SessionVariables.SessionKeyUserId))) return RedirectToAction("Login", "User");
            var returnTransaction = await _bookTransactionsService.LoadTransaction(model.TransactionId);
            bookReturnModel.BookTransactions = returnTransaction;

            return View("Return", bookReturnModel);
        }

        public async Task<IActionResult> ReturnBook(int transactionId, int Id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(SessionVariables.SessionKeyUserId))) return RedirectToAction("Login", "User");
            var response = await _validationService.ValidateReturns(transactionId, Id);

            ViewBag.message = response;
            
            var returnTransaction = await _bookTransactionsService.LoadTransaction(transactionId);
            bookReturnModel.BookTransactions = returnTransaction;
            return View("Return", bookReturnModel);
            
        }
    }
}
