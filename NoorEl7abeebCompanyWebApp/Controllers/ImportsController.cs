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
    public class ImportsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Imports
        public async Task<ActionResult> Index()
        {
            var imports = db.Imports.Include(i => i.BoxType).Include(i => i.Customer);
            return View(await imports.ToListAsync());
        }

        // GET: Imports/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Import import = await db.Imports.FindAsync(id);
            if (import == null)
            {
                return HttpNotFound();
            }
            return View(import);
        }

        // GET: Imports/Create
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

        // POST: Imports/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Date,Quantity,Weight,Price,Notes,CustomerId,BoxTypeId")] Import import)
        {
            import.TotalWeight = import.Weight * import.Quantity;
            import.TotalPrice = import.Price * import.TotalWeight;

            if (ModelState.IsValid)
            {
                db.Imports.Add(import);
                var c = db.Customers.Find(import.CustomerId);
                var rest = c.RestBoxeses.FirstOrDefault(r => r.BoxTypeId == import.BoxTypeId);
                if (rest != null)
                {
                    rest.Count -= import.Quantity;
                }
                else
                {
                    db.RestBoxeses.Add(new RestBoxes
                    {
                        CustomerId = import.CustomerId,
                        BoxTypeId = import.BoxTypeId,
                        Count = (import.Quantity * (-1))
                    });
                }
                await db.SaveChangesAsync();
                if (Request.IsAjaxRequest())
                {
                    var result = new { Quantity = import.Quantity, BoxType = db.BoxTypes.Find(import.BoxTypeId).Name, Customer = c.Name };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                return RedirectToAction("Index");
            }

            ViewBag.BoxTypeId = new SelectList(db.BoxTypes, "Id", "Name", import.BoxTypeId);
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name", import.CustomerId);
            return View(import);
        }
        // GET: Imports/ReturnEmptyBoxes
        

        // GET: Imports/Edit/5
            public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Import import = await db.Imports.FindAsync(id);
            if (import == null)
            {
                return HttpNotFound();
            }
            ViewBag.BoxTypeId = new SelectList(db.BoxTypes, "Id", "Name", import.BoxTypeId);
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name", import.CustomerId);
            return View(import);
        }

        // POST: Imports/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Date,Quantity,Weight,Price,Notes,CustomerId,BoxTypeId")] Import import)
        {
            import.TotalWeight = import.Weight * import.Quantity;
            import.TotalPrice = import.Price * import.TotalWeight;
            if (ModelState.IsValid)
            {
                var importInDbQuantity = db.Imports.Find(import.Id).Quantity;
                db.Entry(import).State = EntityState.Modified;
                var c = db.Customers.Find(import.CustomerId);
                var rest = c.RestBoxeses.FirstOrDefault(r => r.BoxTypeId == import.BoxTypeId);
                if (rest != null)
                {
                    rest.Count += importInDbQuantity;
                    rest.Count -= import.Quantity;
                }
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.BoxTypeId = new SelectList(db.BoxTypes, "Id", "Name", import.BoxTypeId);
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name", import.CustomerId);
            return View(import);
        }

        // GET: Imports/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Import import = await db.Imports.FindAsync(id);
            if (import == null)
            {
                return HttpNotFound();
            }
            return View(import);
        }

        // POST: Imports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Import import = await db.Imports.FindAsync(id);
            var rest=import.Customer.RestBoxeses.FirstOrDefault(r => r.BoxTypeId == import.BoxTypeId);
            if (rest != null)
            {
                rest.Count += import.Quantity;
            }
            db.Imports.Remove(import);
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
        public ActionResult ImportsForPeriod()
        {
            ViewBag.BoxTypeId = new SelectList(db.BoxTypes, "Id", "Name");
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ImportsForPeriod(Period period)
        {
            var box = db.BoxTypes.Include(b=>b.Imports).FirstOrDefault(b=>b.Id==period.BoxTypeId);
            List<Import> result;
            if (box != null)
            {
                result = box.Imports.Where(i =>
                 i.Date.CompareTo(period.From) >= 0
                && i.Date.CompareTo(period.To) <= 0
             ).ToList();
            }
            else
            {
                result = db.Imports.Where(i =>
                 i.Date.CompareTo(period.From) >= 0
                && i.Date.CompareTo(period.To) <= 0
             ).ToList();
            }
            return PartialView("_CustomerImports",result);
        }
    }
}
