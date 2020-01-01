using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NoorEl7abeebCompanyWebApp.Models;
using NoorEl7abeebCompanyWebApp.Models.MyModels;

namespace NoorEl7abeebCompanyWebApp.Controllers
{
    [Authorize]
    public class BoxTypesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: BoxTypes
        public async Task<ActionResult> Index()
        {
            return View(await db.BoxTypes.ToListAsync());
        }

        // GET: BoxTypes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BoxType boxType = await db.BoxTypes.FindAsync(id);
            if (boxType == null)
            {
                return HttpNotFound();
            }
            return View(boxType);
        }

        // GET: BoxTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BoxTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name")] BoxType boxType)
        {
            if (ModelState.IsValid)
            {
                db.BoxTypes.Add(boxType);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(boxType);
        }

        // GET: BoxTypes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BoxType boxType = await db.BoxTypes.FindAsync(id);
            if (boxType == null)
            {
                return HttpNotFound();
            }
            return View(boxType);
        }

        // POST: BoxTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name")] BoxType boxType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(boxType).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(boxType);
        }

        // GET: BoxTypes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BoxType boxType = await db.BoxTypes.FindAsync(id);
            if (boxType == null)
            {
                return HttpNotFound();
            }
            return View(boxType);
        }

        // POST: BoxTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            BoxType boxType = await db.BoxTypes.FindAsync(id);
            db.BoxTypes.Remove(boxType);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
