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
    public class ExportsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Exports
        public async Task<ActionResult> Index()
        {
            var exports = db.Exports.Include(e => e.BoxType).Include(e => e.Customer);
            return View(await exports.ToListAsync());
        }

        // GET: Exports/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Export export = await db.Exports.FindAsync(id);
            if (export == null)
            {
                return HttpNotFound();
            }
            return View(export);
        }

        // GET: Exports/Create
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

        // POST: Exports/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Date,Quantity,Notes,CustomerId,BoxTypeId")] Export export)
        {
            if (ModelState.IsValid)
            {
                db.Exports.Add(export);
                var c = db.Customers.Find(export.CustomerId);
                var rest = c.RestBoxeses.FirstOrDefault(r => r.BoxTypeId == export.BoxTypeId);
                if (rest != null)
                {
                    rest.Count += export.Quantity;
                }
                else
                {
                    db.RestBoxeses.Add(new RestBoxes
                    {
                        CustomerId = export.CustomerId,
                        BoxTypeId = export.BoxTypeId,
                        Count = export.Quantity
                    });
                }
                await db.SaveChangesAsync();
                if (Request.IsAjaxRequest())
                { 
                    var result = new { Quantity = export.Quantity, BoxType = db.BoxTypes.Find(export.BoxTypeId).Name, Customer = c.Name };
                    return Json(result,JsonRequestBehavior.AllowGet);
                }
                return RedirectToAction("Index");
            }

            ViewBag.BoxTypeId = new SelectList(db.BoxTypes, "Id", "Name", export.BoxTypeId);
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name", export.CustomerId);
            return View(export);
        }

        public ActionResult ReturnEmptyBoxes(int? customerId)
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ReturnEmptyBoxes([Bind(Include = "Id,Count,CustomerId,BoxTypeId")] RestBoxes restBoxes)
        {
            if (ModelState.IsValid)
            {
                var c = db.Customers.Find(restBoxes.CustomerId);
                var rest = c.RestBoxeses.FirstOrDefault(r => r.BoxTypeId == restBoxes.BoxTypeId);
                if (rest != null)
                {
                    rest.Count -= restBoxes.Count;
                }
                else
                {
                    restBoxes.Count *= -1;
                    db.RestBoxeses.Add(restBoxes);
                }
                db.Exports.Add(new Export
                {
                    BoxTypeId = restBoxes.BoxTypeId,
                    CustomerId = restBoxes.CustomerId,
                    Date = DateTime.Now,
                    Quantity = restBoxes.Count<0?restBoxes.Count:(restBoxes.Count*-1)
                });
                await db.SaveChangesAsync();
                if (Request.IsAjaxRequest())
                {
                    var result = new { Quantity = restBoxes.Count, BoxType = db.BoxTypes.Find(restBoxes.BoxTypeId).Name, Customer = c.Name };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                return RedirectToAction("Index");
            }

            ViewBag.BoxTypeId = new SelectList(db.BoxTypes, "Id", "Name", restBoxes.BoxTypeId);
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name", restBoxes.CustomerId);
            return View(restBoxes);
        }
        
        // GET: Exports/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Export export = await db.Exports.FindAsync(id);
            if (export == null)
            {
                return HttpNotFound();
            }
            ViewBag.BoxTypeId = new SelectList(db.BoxTypes, "Id", "Name", export.BoxTypeId);
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name", export.CustomerId);
            return View(export);
        }

        // POST: Exports/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Date,Quantity,Notes,CustomerId,BoxTypeId")] Export export)
        {
            if (ModelState.IsValid)
            {
                var exportInDbQuantity = db.Exports.Find(export.Id).Quantity;
                db.Entry(export).State = EntityState.Modified;
                var c = db.Customers.Find(export.CustomerId);
                var rest = c.RestBoxeses.FirstOrDefault(r => r.BoxTypeId == export.BoxTypeId);
                if (rest != null)
                {
                    rest.Count -= exportInDbQuantity;
                    rest.Count += export.Quantity;
                }
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.BoxTypeId = new SelectList(db.BoxTypes, "Id", "Name", export.BoxTypeId);
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name", export.CustomerId);
            return View(export);
        }

        // GET: Exports/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Export export = await db.Exports.FindAsync(id);
            if (export == null)
            {
                return HttpNotFound();
            }
            return View(export);
        }

        // POST: Exports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Export export = await db.Exports.FindAsync(id);
            var rest = export.Customer.RestBoxeses.FirstOrDefault(r => r.BoxTypeId == export.BoxTypeId);
            if (rest != null)
            {
                rest.Count -= export.Quantity;
            }
            db.Exports.Remove(export);
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
        public ActionResult RestBoxesForPeriod()
        {
            ViewBag.BoxTypeId = new SelectList(db.BoxTypes, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RestBoxesForPeriod(int BoxTypeId)
        {
            var box = db.BoxTypes.Include(b => b.Imports).FirstOrDefault(b => b.Id == BoxTypeId);
            List<RestBoxes> result;
            if (box != null)
            {
                result = box.RestBoxeses.ToList();
            }
            else
            {
                result = db.RestBoxeses.ToList();
            }
            return PartialView("_RestBoxes", result);
        }
    }
}
