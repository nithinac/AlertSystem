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
using AlertSystemMVC.Utilities;

namespace AlertSystemMVC.Controllers
{
    public class ShiftsController : Controller
    {
        private AlertSystemMVCContext db = new AlertSystemMVCContext();

        // GET: Shifts
        public async Task<ActionResult> Index()
        {
            return View(await db.Shifts.ToListAsync());
        }

        // GET: Shifts/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shift shift = await db.Shifts.FindAsync(id);
            if (shift == null)
            {
                return HttpNotFound();
            }
            return View(shift);
        }

        // GET: Shifts/Create
        public ActionResult Create()
        {
            return View(new ShiftViewModel());
        }

        // POST: Shifts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Description,StartTime,EndTime")] ShiftViewModel shiftView)
        {
            if (ModelState.IsValid)
            {
                Shift shift = new Shift { Description = shiftView.Description, StartTime = DateTime.ParseExact(shiftView.StartTime, "HH:mm:ss", null), EndTime = DateTime.ParseExact(shiftView.EndTime, "HH:mm:ss", null) };
                db.Shifts.Add(shift);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(shiftView);
        }

        // GET: Shifts/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shift shift = await db.Shifts.FindAsync(id);
            if (shift == null)
            {
                return HttpNotFound();
            }
            return View(new ShiftEditViewModel()
            {
                Key = shift.Key,
                Description = shift.Description,
                StartTime = shift.StartTime.toTimeString(),
                EndTime = shift.EndTime.toTimeString()
            });

        }

        // POST: Shifts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Key,Description,StartTime,EndTime")] ShiftEditViewModel shiftView)
        {
            if (ModelState.IsValid)
            {
                Shift shift = await db.Shifts.FindAsync(shiftView.Key);
                shift.Description = shiftView.Description;
                shift.StartTime = DateTime.ParseExact(shiftView.StartTime, "HH:mm:ss", null);
                shift.EndTime = DateTime.ParseExact(shiftView.EndTime, "HH:mm:ss", null);                
                db.Entry(shift).State = EntityState.Modified;
                await db.SaveChangesAsync();
                List<Alert> alerts = db.Alerts.Where(x => x.Shift.Key.Equals(shift.Key)).ToList();
                foreach (var alert in alerts)
                {
                    AlertManager.UpdateAlert(alert);
                }
                return RedirectToAction("Index");
            }
            return View(shiftView);
        }

        // GET: Shifts/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shift shift = await db.Shifts.FindAsync(id);
            if (shift == null)
            {
                return HttpNotFound();
            }
            return View(shift);
        }

        // POST: Shifts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            Shift shift = await db.Shifts.FindAsync(id);
            db.Shifts.Remove(shift);
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
