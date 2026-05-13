using BookstoreAdoNetApp.DataAccess;
using BookstoreAdoNetApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace BookstoreAdoNetApp.Controllers
{
    public class BooksController : Controller
    {
        private readonly BookRepository _bookRepository;

        public BooksController(BookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public IActionResult Index()
        {
            var books = _bookRepository.GetAllBooksUsingReader();
            return View(books);
        }

        public IActionResult Details(int id)
        {
            var book = _bookRepository.GetBookById(id);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Book book)
        {
            if (ModelState.IsValid)
            {
                _bookRepository.AddBook(book);
                TempData["Success"] = "Book added successfully";
                return RedirectToAction("Index");
            }

            return View(book);
        }

        public IActionResult Edit(int id)
        {
            var book = _bookRepository.GetBookById(id);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        [HttpPost]
        public IActionResult Edit(Book book)
        {
            if (ModelState.IsValid)
            {
                _bookRepository.UpdateBook(book);
                TempData["Success"] = "Book updated successfully";
                return RedirectToAction("Index");
            }

            return View(book);
        }

        public IActionResult Delete(int id)
        {
            var book = _bookRepository.GetBookById(id);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            _bookRepository.DeleteBook(id);
            TempData["Success"] = "Book deleted successfully";
            return RedirectToAction("Index");
        }

        public IActionResult DataSetDemo()
        {
            DataTable booksTable = _bookRepository.GetBooksUsingDataTable();
            return View(booksTable);
        }
    }
}
