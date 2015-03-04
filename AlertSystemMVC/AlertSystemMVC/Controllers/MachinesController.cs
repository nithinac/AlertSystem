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
    public class MachinesController : Controller
    {
        private AlertSystemMVCContext db = new AlertSystemMVCContext();

        // GET: Machines
        public async Task<ActionResult> Index()
        {
            return View(await db.Machines.ToListAsync());
        }

        // GET: Machines/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Machine machine = await db.Machines.FindAsync(id);
            if (machine == null)
            {
                return HttpNotFound();
            }
            return View(machine);
        }

        // GET: Machines/Create
        public ActionResult Create()
        {
            return View(new MachineViewModel());
        }

        // POST: Machines/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Code,Description,MachineType,MinLoad,MaxLoad")] MachineViewModel machineView)
        {
            if (ModelState.IsValid)
            {
                Machine machine = new Machine
                {
                    Code = machineView.Code,
                    Description = machineView.Description,
                    MachineType = machineView.MachineType,
                    MinLoad = machineView.MinLoad,
                    MaxLoad = machineView.MaxLoad
                };
                db.Machines.Add(machine);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(machineView);
        }

        // GET: Machines/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Machine machine = await db.Machines.FindAsync(id);
            if (machine == null)
            {
                return HttpNotFound();
            }
            return View(new MachineEditViewModel()
            {
                Key = machine.Key,
                Code = machine.Code,
                Description = machine.Description,
                MachineType = machine.MachineType,
                MinLoad = machine.MinLoad,
                MaxLoad = machine.MaxLoad
            });
        }

        // POST: Machines/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Key,Code,Description,MachineType,MinLoad,MaxLoad")] MachineEditViewModel machineView)
        {
            if (ModelState.IsValid)
            {
                Machine machine = await db.Machines.FindAsync(machineView.Key);
                if (machine == null)
                    return HttpNotFound();
                machine.Code = machineView.Code;
                machine.Description = machineView.Description;
                machine.MachineType = machineView.MachineType;
                machine.MinLoad = machineView.MinLoad;
                machine.MaxLoad = machineView.MaxLoad;
                db.Entry(machine).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(machineView);
        }

        // GET: Machines/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Machine machine = await db.Machines.FindAsync(id);
            if (machine == null)
            {
                return HttpNotFound();
            }
            return View(machine);
        }

        // POST: Machines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            Machine machine = await db.Machines.FindAsync(id);
            List<Alert> alerts = db.Alerts.Where(x => x.Machines.Key.Equals(machine.Key)).ToList();
            db.Alerts.RemoveRange(alerts);
            List<ProductionSummary> productions = db.ProductionSummaries.Where(x => x.Machine.Key.Equals(machine.Key)).ToList();
            db.ProductionSummaries.RemoveRange(productions);
            db.Machines.Remove(machine);
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
