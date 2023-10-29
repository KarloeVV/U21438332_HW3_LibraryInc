using LibraryInc.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace LibraryInc.Controllers
{
    public class StudentBookController : Controller
    {
        private LibraryEntities1 db = new LibraryEntities1();

        // GET: StudentBook/StudentBook
        public async Task<ActionResult> StudentBook(int? page)
        {
            // Pagination settings
            int pagesize = 6, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;

            // Retrieve a list of students, books, and borrows with pagination.
            var list = db.students.OrderByDescending(x => x.studentId).ToList();
            IPagedList<students> students = list.ToPagedList(pageindex, pagesize);

            var booksList = db.books.Include(b => b.authors).Include(b => b.types).ToList();
            IPagedList<books> books = booksList.ToPagedList(pageindex, pagesize);

            var list3 = db.borrows.Include(b => b.books).Include(b => b.students).ToList();
            IPagedList<borrows> borrows = list3.ToPagedList(pageindex, pagesize);

            var studentbook = new StudentBookViewModel
            {
                // Populate the StudentBookViewModel with data.
                Students = await db.students.ToListAsync(),
                Borrowz = await db.borrows.Include(b => b.books).Include(b => b.students).ToListAsync(),
                Books = books,
                PageStudent = students
            };

            return View(studentbook);
        }

        // GET: StudentBook/StudentIndex
        public async Task<ActionResult> StudentIndex()
        {
            // Display a list of students.
            return View(await db.students.ToListAsync());
        }

        // GET: StudentBook/StudentDetails/5
        public async Task<ActionResult> StudentDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Retrieve and display student details by ID.
            students students = await db.students.FindAsync(id);
            if (students == null)
            {
                return HttpNotFound();
            }
            return View(students);
        }

        // GET: StudentBook/Create
        public ActionResult Create()
        {
            // Display the view for creating a new student.
            return View();
        }

        [ValidateAntiForgeryToken]
        // POST: StudentBook/StudentCreate
        public async Task<ActionResult> StudentCreate([Bind(Include = "studentId,name,surname,birthdate,gender,class,point")] students students)
        {
            if (ModelState.IsValid)
            {
                // If the data provided for the new student is valid, add it to the database.
                db.students.Add(students);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            // If the provided data is not valid, return to the creation view with validation errors.
            return View(students);
        }

        // GET: StudentBook/StudentEdit/5
        public async Task<ActionResult> StudentEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Retrieve student details by ID for editing.
            students students = await db.students.FindAsync(id);
            if (students == null)
            {
                return HttpNotFound();
            }
            return View(students);
        }

        // POST: StudentBook/StudentEdit/5
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> StudentEdit([Bind(Include = "studentId,name,surname,birthdate,gender,class,point")] students students)
        {
            if (ModelState.IsValid)
            {
                // If the data provided for editing is valid, mark the student as modified and save changes.
                db.Entry(students).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            // If the provided data is not valid, return to the edit view with validation errors.
            return View(students);
        }

        // GET: StudentBook/StudentDelete/5
        public async Task<ActionResult> StudentDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Retrieve student details by ID for deletion confirmation.
            students students = await db.students.FindAsync(id);
            if (students == null)
            {
                return HttpNotFound();
            }
            return View(students);
        }

        // POST: StudentBook/StudentDelete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> StudentDeleteConfirmed(int id)
        {
            // Retrieve the student to be deleted by ID, remove it, and save changes.
            students students = await db.students.FindAsync(id);
            db.students.Remove(students);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: StudentBook/BookIndex
        public async Task<ActionResult> BookIndex()
        {
            // Display a list of books including their authors and types.
            var books = db.books.Include(b => b.authors).Include(b => b.types);
            return View(await books.ToListAsync());
        }

        // GET: StudentBook/BookDetails/5
        public async Task<ActionResult> BookDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Retrieve and display book details by ID.
            books books = await db.books.FindAsync(id);
            if (books == null)
            {
                return HttpNotFound();
            }
            return View(books);
        }

        // GET: StudentBook/BookCreate
        public ActionResult BookCreate()
        {
            // Display the view for creating a new book, providing lists of authors and types for selection.
            ViewBag.authorId = new SelectList(db.authors, "authorId", "name");
            ViewBag.typeId = new SelectList(db.types, "typeId", "name");
            return View();
        }

        // POST: StudentBook/BookCreate
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> BookCreate([Bind(Include = "bookId,name,pagecount,point,authorId,typeId")] books books)
        {
            if (ModelState.IsValid)
            {
                // If the data provided for the new book is valid, add it to the database.
                db.books.Add(books);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            // If the provided data is not valid, return to the creation view with validation errors.
            ViewBag.authorId = new SelectList(db.authors, "authorId", "name", books.authorId);
            ViewBag.typeId = a new SelectList(db.types, "typeId", "name", books.typeId);
            return View(books);
        }

        // GET: StudentBook/BookEdit/5
        public async Task<ActionResult> BookEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Retrieve book details by ID for editing, providing lists of authors and types for selection.
            books books = await db.books.FindAsync(id);
            if (books is null)
            {
                return HttpNotFound();
            }
            ViewBag.authorId = new SelectList(db.authors, "authorId", "name", books.authorId);
            ViewBag.typeId = new SelectList(db.types, "typeId", "name", books.typeId);
            return View(books);
        }

        // POST: StudentBook/BookEdit/5
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> BookEdit([Bind(Include = "bookId,name,pagecount,point,authorId,typeId")] books books)
        {
            if (ModelState.IsValid)
            {
                // If the data provided for editing is valid, mark the book as modified and save changes.
                db.Entry(books).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            // If the provided data is not valid, return to the edit view with validation errors.
            ViewBag.authorId = new SelectList(db.authors, "authorId", "name", books.authorId);
            ViewBag.typeId = new SelectList(db.types, "typeId", "name", books.typeId);
            return View(books);
        }

        // GET: StudentBook/BookDelete/5
        public async Task<ActionResult> BookDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Retrieve book details by ID for deletion confirmation.
            books books = await db.books.FindAsync(id);
            if (books is null)
            {
                return HttpNotFound();
            }
            return View(books);
        }

        // POST: StudentBook/BookDelete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> BookDeleteConfirmed(int id)
        {
            // Retrieve the book to be deleted by ID, remove it, and save changes.
            books books = await db.books.FindAsync(id);
            db.books.Remove(books);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Dispose of the database context to release resources.
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
