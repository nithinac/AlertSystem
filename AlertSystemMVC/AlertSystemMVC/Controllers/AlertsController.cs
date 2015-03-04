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
    public class AlertsController : Controller
    {
        private AlertSystemMVCContext db = new AlertSystemMVCContext();

        // GET: Alerts
        public async Task<ActionResult> Index()
        {
            return View(await db.Alerts.ToListAsync());
        }

        // GET: Alerts/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Alert alert = await db.Alerts.FindAsync(id);
            if (alert == null)
            {
                return HttpNotFound();
            }
            return View(alert);
        }

        // GET: Alerts/Create
        public ActionResult Create()
        {

            var properties = typeof(ProductionSummary).GetProperties().Where(x => x.PropertyType.IsPrimitive && x.DeclaringType==typeof(ProductionSummary));

            List<string> propertyNames = new List<string>();
            foreach (var p in properties)
                propertyNames.Add(p.Name);

            SelectList propertyList = new SelectList(propertyNames);

            List<Machine> machines = db.Machines.Where(x => x.ActiveFlag).ToList();
            List<Shift> shifts = db.Shifts.Where(x => x.ActiveFlag).ToList();

            return View(new AlertViewModel { MachinesList = machines.ToSelectList("Code"), ShiftsList = shifts.ToSelectList("Description"), PropertyList=propertyList });
        }

        // POST: Alerts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Machine,Name,Description,AlertType,RepeatType,AlertProperty,ThresholdValue,Operation,DailyAlertTime,Shift")] AlertViewModel alertView)
        {
            if (ModelState.IsValid)
            {
                Machine machine = db.Machines.FirstOrDefault(x => x.Code.Equals(alertView.Machine));
                Shift shift = db.Shifts.FirstOrDefault(x => x.Description.Equals(alertView.Shift));
                Alert alert = new Alert { Machines = machine, Name = alertView.Name, Description = alertView.Description, AlertType = alertView.AlertType, RepeatType = alertView.RepeatType, AlertProperty = alertView.AlertProperty, ThresholdValue = alertView.ThresholdValue, Operation = alertView.Operation, DailyAlertTime = alertView.DailyAlertTime, Shift = shift };
                db.Alerts.Add(alert);
                await db.SaveChangesAsync();
                AlertManager.CreateAlert(alert);
                return RedirectToAction("Index");
            }

            return View(alertView);
        }

        // GET: Alerts/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Alert alert = await db.Alerts.FindAsync(id);
            if (alert == null)
            {
                return HttpNotFound();
            }

            

            var properties = typeof(ProductionSummary).GetProperties().Where(x => x.PropertyType.IsPrimitive && x.DeclaringType == typeof(ProductionSummary));

            List<string> propertyNames = new List<string>();
            foreach (var p in properties)
                propertyNames.Add(p.Name);

            SelectList machinesList = db.Machines.ToSelectList<Machine>("Code", SelectedField: alert.Machines);
            SelectList shiftsList = db.Shifts.ToSelectList<Shift>("Description", SelectedField: alert.Shift);
            SelectList propertyList = new SelectList(propertyNames, alert.AlertProperty);
            

            AlertEditViewModel view = new AlertEditViewModel(alert);
            view.MachinesList = machinesList;
            view.ShiftsList = shiftsList;
            view.PropertyList = propertyList;
            return View(view);                                
        }

        // POST: Alerts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Key,Machine,Name,Description,AlertType,RepeatType,AlertProperty,ThresholdValue,Operation,DailyAlertTime,Shift")] AlertEditViewModel alertView)
        {
            if (ModelState.IsValid)
            {
                Alert alert = await db.Alerts.FindAsync(alertView.Key);
                alert.Machines = await db.Machines.FindAsync(alertView.Machine);
                alert.Name = alertView.Name;
                alert.Description = alertView.Description;
                alert.AlertType = alertView.AlertType;
                alert.RepeatType = alertView.RepeatType;
                alert.AlertProperty = alertView.AlertProperty;
                alert.ThresholdValue = alertView.ThresholdValue;
                alert.Operation = alertView.Operation;
                alert.DailyAlertTime = alertView.DailyAlertTime;
                alert.Shift = await db.Shifts.FindAsync(alertView.Shift);
                db.Entry(alert).State = EntityState.Modified;
                await db.SaveChangesAsync();
                AlertManager.UpdateAlert(alert);
                return RedirectToAction("Index");
            }
            return View(alertView);
        }

        // GET: Alerts/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Alert alert = await db.Alerts.FindAsync(id);
            if (alert == null)
            {
                return HttpNotFound();
            }
            return View(alert);
        }

        // POST: Alerts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            Alert alert = await db.Alerts.FindAsync(id);
            db.Alerts.Remove(alert);
            await db.SaveChangesAsync();
            AlertManager.DeleteAlert(alert);
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
