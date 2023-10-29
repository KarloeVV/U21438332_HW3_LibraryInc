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
    public class typesController : Controller
    {
        private LibraryEntities1 db = new LibraryEntities1();

        // GET: types
        public async Task<ActionResult> Index()
        {
            // Display a list of types.
            return View(await db.types.ToListAsync());
        }

        // GET: types/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Retrieve and display type details by ID.
            types types = await db.types.FindAsync(id);
            if (types is null)
            {
                return HttpNotFound();
            }
            return View(types);
        }

        // GET: types/Create
        public ActionResult Create()
        {
            // Display the view for creating a new type.
            return View();
        }

        // POST: types/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "typeId,name")] types types)
        {
            if (ModelState.IsValid)
            {
                // If the data provided for the new type is valid, add it to the database.
                db.types.Add(types);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            // If the provided data is not valid, return to the creation view with validation errors.
            return View(types);
        }

        // GET: types/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Retrieve type details by ID for editing.
            types types = await db.types.FindAsync(id);
            if (types is null)
            {
                return HttpNotFound();
            }
            return View(types);
        }

        // POST: types/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "typeId,name")] types types)
        {
            if (ModelState.IsValid)
            {
                // If the data provided for editing is valid, mark the type as modified and save changes.
                db.Entry(types).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            // If the provided data is not valid, return to the edit view with validation errors.
            return View(types);
        }

        // GET: types/Delete
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Retrieve type details by ID for deletion confirmation.
            types types = await db.types.FindAsync(id);
            if (types is null)
            {
                return HttpNotFound();
            }
            return View(types);
        }

        // POST: types/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            // Retrieve the type to be deleted by ID, remove it, and save changes.
            types types = await db.types.FindAsync(id);
            db.types.Remove(types);
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

