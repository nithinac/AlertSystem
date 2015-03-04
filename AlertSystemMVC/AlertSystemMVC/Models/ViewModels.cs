using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlertSystemMVC.Models
{
    public class MachineViewModel
    {        
        public string Code { get; set; }
        public string Description { get; set; }
        public MachineTypes MachineType { get; set; }
        public double MinLoad { get; set; }
        public double MaxLoad { get; set; }
    }

    public class MachineEditViewModel : MachineViewModel
    {
        public string Key { get; set; }
    }

     

    public class ShiftViewModel
    {        
        public string Description { get; set; }        
        public string StartTime { get; set; }        
        public string EndTime { get; set; }
    }

    public class ShiftEditViewModel : ShiftViewModel
    {
        public string Key { get; set; }
    }

    public class AlertViewModel
    {        
        public string Machine { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public AlertType AlertType { get; set; }
        public RepeatType RepeatType { get; set; }
        public string AlertProperty { get; set; }
        public double ThresholdValue { get; set; }
        public Expression Operation { get; set; }
        public int DailyAlertTime { get; set; }
        public string Shift { get; set; }
        public System.Web.Mvc.SelectList MachinesList { get; set; }
        public System.Web.Mvc.SelectList ShiftsList { get; set; }
        public System.Web.Mvc.SelectList PropertyList { get; set; }
    }

    public class AlertEditViewModel :AlertViewModel
    {
        public string Key { get; set; }
        public AlertEditViewModel() { }
        public AlertEditViewModel(Alert alert)
        {
            Key = alert.Key;
            Machine = alert.Machines.Code;
            Name = alert.Name;
            Description = alert.Description;
            AlertType = alert.AlertType;
            RepeatType = alert.RepeatType;
            AlertProperty = alert.AlertProperty;
            ThresholdValue = alert.ThresholdValue;
            Operation = alert.Operation;
            DailyAlertTime = alert.DailyAlertTime;
            Shift = alert.Shift.Description;
        }
    }
    public class ProductionSummaryViewModel
    {
        public string Machine { get; set; }
        public string Shift { get; set; }
        public int Day { get; set; }
        public int ProductionCount { get; set; }
        public int Downtime { get; set; }
        public int Rejected { get; set; }
        public System.Web.Mvc.SelectList MachinesList { get; set; }
        public System.Web.Mvc.SelectList ShiftsList { get; set; }

    }

    public class ProductionSummaryEditViewModel
    {
        public string Key { get; set; }
    }
}