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
    public class RestBoxesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: RestBoxes
        public async Task<ActionResult> Index()
        {
            var restBoxeses = db.RestBoxeses.Include(r => r.BoxType).Include(r => r.Customer);
            return View(await restBoxeses.ToListAsync());
        }

        // GET: RestBoxes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RestBoxes restBoxes = await db.RestBoxeses.FindAsync(id);
            if (restBoxes == null)
            {
                return HttpNotFound();
            }
            return View(restBoxes);
        }

        // GET: RestBoxes/Create
        public ActionResult Create(int? customerId)
        {
            if (customerId != null)
            {
                var customer = db.Customers.SingleOrDefault(i => i.Id == customerId);
                if (customer == null)
                {
                    return HttpNotFound();
                }
                ViewBag.CustomerId = customerId;
                ViewBag.CustomerName = customer.Name;
            }
            else
            {
                ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name");
            }
            ViewBag.BoxTypeId = new SelectList(db.BoxTypes, "Id", "Name");
            return View();
        }

        // POST: RestBoxes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Count,CustomerId,BoxTypeId")] RestBoxes restBoxes)
        {
            if (ModelState.IsValid)
            {
                db.RestBoxeses.Add(restBoxes);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.BoxTypeId = new SelectList(db.BoxTypes, "Id", "Name", restBoxes.BoxTypeId);
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name", restBoxes.CustomerId);
            return View(restBoxes);
        }

        // GET: RestBoxes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RestBoxes restBoxes = await db.RestBoxeses.FindAsync(id);
            if (restBoxes == null)
            {
                return HttpNotFound();
            }
            ViewBag.BoxTypeId = new SelectList(db.BoxTypes, "Id", "Name", restBoxes.BoxTypeId);
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name", restBoxes.CustomerId);
            return View(restBoxes);
        }

        // POST: RestBoxes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Count,CustomerId,BoxTypeId")] RestBoxes restBoxes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(restBoxes).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.BoxTypeId = new SelectList(db.BoxTypes, "Id", "Name", restBoxes.BoxTypeId);
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name", restBoxes.CustomerId);
            return View(restBoxes);
        }

        // GET: RestBoxes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RestBoxes restBoxes = await db.RestBoxeses.FindAsync(id);
            if (restBoxes == null)
            {
                return HttpNotFound();
            }
            return View(restBoxes);
        }

        // POST: RestBoxes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            RestBoxes restBoxes = await db.RestBoxeses.FindAsync(id);
            db.RestBoxeses.Remove(restBoxes);
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
