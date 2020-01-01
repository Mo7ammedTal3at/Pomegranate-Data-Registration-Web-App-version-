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
    public class PaymentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Payments
        public async Task<ActionResult> Index()
        {
            var payments = db.Payments.Include(p => p.Customer);
            return View(await payments.ToListAsync());
        }

        // GET: Payments/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment payment = await db.Payments.FindAsync(id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            return View(payment);
        }

        // GET: Payments/Create
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
            return View();
        }

        // POST: Payments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Money,Date,Notes,CustomerId")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                db.Payments.Add(payment);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name", payment.CustomerId);
            return View(payment);
        }

        // GET: Payments/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment payment = await db.Payments.FindAsync(id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name", payment.CustomerId);
            return View(payment);
        }

        // POST: Payments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Money,Date,Notes,CustomerId")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(payment).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name", payment.CustomerId);
            return View(payment);
        }

        // GET: Payments/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment payment = await db.Payments.FindAsync(id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            return View(payment);
        }

        // POST: Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Payment payment = await db.Payments.FindAsync(id);
            db.Payments.Remove(payment);
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

        public ActionResult PaymentsForPeriod()
        {
            ViewBag.BoxTypeId = new SelectList(db.BoxTypes, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PaymentsForPeriod(Period period)
        {
            var result = db.Payments.Where(i =>
             i.Date.CompareTo(period.From) >= 0
            && i.Date.CompareTo(period.To) <= 0
            ).ToList();
            return PartialView("_CustomerPayments", result);
        }

        public ActionResult CustomersPayments()
        {
            var result = db.Customers.Include(c => c.Imports).Include(c => c.Payments).Select(c => new CustomerPaymentsReportVM
            {
                CustomerName = c.Name,
                PaidForHim = (double?)c.Payments.Sum(p => p.Money)??0,
                RestPaymentsForHim = (double?)(c.Imports.Sum(i => i.TotalPrice) - c.Payments.Sum(p => p.Money))??0,
                TotalPayments = (double?)c.Imports.Sum(i => i.TotalPrice)??0
            }).ToList();
            return View(result);
        }
    }
}
