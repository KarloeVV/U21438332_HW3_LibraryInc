using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LibraryInc.Models;

namespace LibraryInc.Controllers
{
    public class borrowsController : Controller
    {
        private LibraryEntities1 db = new LibraryEntities1();

        // GET: borrows
        public async Task<ActionResult> Index()
        {
            // Retrieve a list of borrows from the database, including related books and students, and display them in a view.
            var borrows = db.borrows.Include(b => b.books).Include(b => b.students);
            return View(await borrows.ToListAsync());
        }

        // GET: borrows/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                // If the ID is not provided, return a "Bad Request" status.
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Retrieve borrow details by ID from the database.
            borrows borrows = await db.borrows.FindAsync(id);

            if (borrows == null)
            {
                // If the borrow is not found, return a "Not Found" status.
                return HttpNotFound();
            }

            // Display the borrow details.
            return View(borrows);
        }

        // GET: borrows/Create
        public ActionResult Create()
        {
            // Provide a view for creating a new borrow, and populate dropdowns with book and student options.
            ViewBag.bookId = new SelectList(db.books, "bookId", "name");
            ViewBag.studentId = new SelectList(db.students, "studentId", "name");
            return View();
        }

        // POST: borrows/Create
        // To protect from overposting attacks, specify the properties that can be bound to.
        // For more details, see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "borrowId,studentId,bookId,takenDate,broughtDate")] borrows borrows)
        {
            if (ModelState.IsValid)
            {
                // If the data provided for the new borrow is valid, add it to the database.
                db.borrows.Add(borrows);
                await db.SaveChangesAsync();

                // Redirect to the borrow list after a successful creation.
                return RedirectToAction("Index");
            }

            // If the provided data is not valid, return to the creation view with validation errors.
            ViewBag.bookId = new SelectList(db.books, "bookId", "name", borrows.bookId);
            ViewBag.studentId = new SelectList(db.students, "studentId", "name", borrows.studentId);
            return View(borrows);
        }

        // GET: borrows/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                // If the ID is not provided, return a "Bad Request" status.
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Retrieve borrow details by ID from the database for editing.
            borrows borrows = await db.borrows.FindAsync(id);

            if (borrows == null)
            {
                // If the borrow is not found, return a "Not Found" status.
                return HttpNotFound();
            }

            // Provide a view for editing the borrow, and populate dropdowns with book and student options.
            ViewBag.bookId = new SelectList(db.books, "bookId", "name", borrows.bookId);
            ViewBag.studentId = new SelectList(db.students, "studentId", "name", borrows.studentId);
            return View(borrows);
        }

        // POST: borrows/Edit/5
        // To protect from overposting attacks, specify the properties that can be bound to.
        // For more details, see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "borrowId,studentId,bookId,takenDate,broughtDate")] borrows borrows)
        {
            if (ModelState.IsValid)
            {
                // If the data provided for editing is valid, mark the borrow as modified and save changes.
                db.Entry(borrows).State = EntityState.Modified;
                await db.SaveChangesAsync();

                // Redirect to the borrow list after a successful edit.
                return RedirectToAction("Index");
            }

            // If the provided data is not valid, return to the edit view with validation errors.
            ViewBag.bookId = a SelectList(db.books, "bookId", "name", borrows.bookId);
            ViewBag.studentId = a SelectList(db.students, "studentId", "name", borrows.studentId);
            return View(borrows);
        }

        // GET: borrows/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                // If the ID is not provided, return a "Bad Request" status.
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Retrieve borrow details by ID from the database for deletion.
            borrows borrows = await db.borrows.FindAsync(id);

            if (borrows == null)
            {
                // If the borrow is not found, return a "Not Found" status.
                return HttpNotFound();
            }

            // Display the borrow details for deletion confirmation.
            return View(borrows);
        }

        // POST: borrows/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            // Retrieve the borrow to be deleted by ID from the database, remove it, and save changes.
            borrows borrows = await db.borrows.FindAsync(id);
            db.borrows.Remove(borrows);
            await db.SaveChangesAsync();

            // Redirect to the borrow list after a successful deletion.
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
