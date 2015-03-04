using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AlertSystemMVC.Models;

namespace AlertSystemMVC.Controllers
{
    public class ProductionSummariesController : Controller
    {
        private AlertSystemMVCContext db = new AlertSystemMVCContext();

        // GET: ProductionSummaries
        public async Task<ActionResult> Index()
        {
            return View(await db.ProductionSummaries.ToListAsync());
        }

        // GET: ProductionSummaries/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductionSummary productionSummary = await db.ProductionSummaries.FindAsync(id);
            if (productionSummary == null)
            {
                return HttpNotFound();
            }
            return View(productionSummary);
        }

        // GET: ProductionSummaries/Create
        public ActionResult Create()
        {
            List<string> machines = new List<string>();
            foreach (var m in db.Machines.ToList())
                machines.Add(m.Code);

            List<string> shifts = new List<string>();
            foreach (var s in db.Shifts.ToList())
                shifts.Add(s.Description);

            SelectList machinesList = new SelectList(machines);
            SelectList shiftsList = new SelectList(shifts);

            return View(new ProductionSummaryViewModel { MachinesList = machinesList, ShiftsList = shiftsList });
        }

        // POST: ProductionSummaries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Machine,Shift,Day,ProductionCount,Downtime,Rejected")] ProductionSummaryViewModel productionSummaryView)
        {
            if (ModelState.IsValid)
            {
                Machine machine = db.Machines.FirstOrDefault(x => x.Code.Equals(productionSummaryView.Machine));
                Shift shift = db.Shifts.FirstOrDefault(x => x.Description.Equals(productionSummaryView.Shift));
                ProductionSummary productionSummary = new ProductionSummary { Machine = machine, Shift = shift, Day = productionSummaryView.Day, ProductionCount = productionSummaryView.ProductionCount, Downtime = productionSummaryView.Downtime, Rejected = productionSummaryView.Rejected };
                db.ProductionSummaries.Add(productionSummary);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(productionSummaryView);
        }

        // GET: ProductionSummaries/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductionSummary productionSummary = await db.ProductionSummaries.FindAsync(id);
            if (productionSummary == null)
            {
                return HttpNotFound();
            }
            return View(productionSummary);
        }

        // POST: ProductionSummaries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Key,Day,ProductionCount,Downtime,Rejected,CreatedAt,UpdatedAt,TimeStamp,ActiveFlag")] ProductionSummary productionSummary)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productionSummary).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(productionSummary);
        }

        // GET: ProductionSummaries/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductionSummary productionSummary = await db.ProductionSummaries.FindAsync(id);
            if (productionSummary == null)
            {
                return HttpNotFound();
            }
            return View(productionSummary);
        }

        // POST: ProductionSummaries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            ProductionSummary productionSummary = await db.ProductionSummaries.FindAsync(id);
            db.ProductionSummaries.Remove(productionSummary);
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
