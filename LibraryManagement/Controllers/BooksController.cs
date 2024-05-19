using LibraryManagement.Models;
using LibraryManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    public class BooksController : Controller
    {
        private readonly BookService _bookService;

        public BooksController(BookService bookService)
        {
            _bookService = bookService;
        }

        public async Task<IActionResult> Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(SessionVariables.SessionKeyUserId))) return RedirectToAction("Login", "User");

            var listBooks = await _bookService.GetAllBooks();

            return View(listBooks);
        }

        public async Task<IActionResult> AddBooks(Book book)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            if (string.IsNullOrEmpty(HttpContext.Session.GetString(SessionVariables.SessionKeyUserId))) return RedirectToAction("Login", "User");

            var sessionId = HttpContext.Session.GetInt32(SessionVariables.SessionKeyUserIdKey);

            var newBook = await _bookService.AddBook(book, (int)sessionId);

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int Id)
        {
            var book = _bookService.GetBook(Id);
            return View(book);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Book book) 
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(SessionVariables.SessionKeyUserId))) return RedirectToAction("Login", "User");

            var sessionId = HttpContext.Session.GetInt32(SessionVariables.SessionKeyUserIdKey);

            var newBook = await _bookService.AddBook(book, (int)sessionId);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(SessionVariables.SessionKeyUserId))) return RedirectToAction("Login", "User");

            var sessionId = HttpContext.Session.GetInt32(SessionVariables.SessionKeyUserIdKey);

            var response = await _bookService.DeleteBook(id, (int)sessionId);
            return RedirectToAction("Index");
        }
    }
}
