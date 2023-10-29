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
    public class authorsController : Controller
    {
        private LibraryEntities1 db = new LibraryEntities1();

        // GET: authors
        public async Task<ActionResult> Index()
        {
            // Retrieve a list of authors from the database and display them in a view.
            return View(await db.authors.ToListAsync());
        }

        // GET: authors/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                // If the ID is not provided, return a "Bad Request" status.
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Retrieve author details by ID from the database.
            authors authors = await db.authors.FindAsync(id);

            if (authors == null)
            {
                // If the author is not found, return a "Not Found" status.
                return HttpNotFound();
            }

            // Display the author details.
            return View(authors);
        }

        // GET: authors/Create
        public ActionResult Create()
        {
            // Display a view for creating a new author.
            return View();
        }

        // POST: authors/Create
        // To protect from overposting attacks, specify the properties that can be bound to.
        // For more details, see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "authorId, name, surname")] authors authors)
        {
            if (ModelState.IsValid)
            {
                // If the data provided for the new author is valid, add it to the database.
                db.authors.Add(authors);
                await db.SaveChangesAsync();

                // Redirect to the author list after a successful creation.
                return RedirectToAction("Index");
            }

            // If the provided data is not valid, return to the creation view with validation errors.
            return View(authors);
        }

        // GET: authors/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                // If the ID is not provided, return a "Bad Request" status.
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Retrieve author details by ID from the database for editing.
            authors authors = await db.authors.FindAsync(id);

            if (authors == null)
            {
                // If the author is not found, return a "Not Found" status.
                return HttpNotFound();
            }

            // Display the author details for editing.
            return View(authors);
        }

        // POST: authors/Edit/5
        // To protect from overposting attacks, specify the properties that can be bound to.
        // For more details, see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "authorId, name, surname")] authors authors)
        {
            if (ModelState.IsValid)
            {
                // If the data provided for editing is valid, mark the author as modified and save changes.
                db.Entry(authors).State = EntityState.Modified;
                await db.SaveChangesAsync();

                // Redirect to the author list after a successful edit.
                return RedirectToAction("Index");
            }

            // If the provided data is not valid, return to the edit view with validation errors.
            return View(authors);
        }

        // GET: authors/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                // If the ID is not provided, return a "Bad Request" status.
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Retrieve author details by ID from the database for deletion.
            authors authors = await db.authors.FindAsync(id);

            if (authors == null)
            {
                // If the author is not found, return a "Not Found" status.
                return HttpNotFound();
            }

            // Display the author details for deletion confirmation.
            return View(authors);
        }

        // POST: authors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            // Retrieve the author to be deleted by ID from the database, remove it, and save changes.
            authors authors = await db.authors.FindAsync(id);
            db.authors.Remove(authors);
            await db.SaveChangesAsync();

            // Redirect to the author list after a successful deletion.
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
