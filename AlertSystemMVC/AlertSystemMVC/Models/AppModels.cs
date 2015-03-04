using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using AlertSystemMVC.Utilities;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlertSystemMVC.Models
{

    public class DateTimeStamp
    {
        [Required, DataType(DataType.Date), ScaffoldColumn(false)]
        public DateTime CreatedAt { get; set; }
        [Required, DataType(DataType.Date), ScaffoldColumn(false)]
        public DateTime UpdatedAt { get; set; }
        [Timestamp, ScaffoldColumn(false)]
        public Byte[] TimeStamp { get; set; }
        [Required, ScaffoldColumn(false)]
        public bool ActiveFlag { get; set; }
        public string Modified
        {
            get
            {
                return UpdatedAt.toDateString();
            }
        }
        public string Created
        {
            get
            {
                //BUG: return UpdatedAt.toDateString();
                //FIX: Created should return CreatedAt 31/01/2015
                return CreatedAt.toDateString();
            }
        }
    }
    public class AppBaseStamp : DateTimeStamp
    {
        [Key, Column(Order = 1), Required, MaxLength(40), ScaffoldColumn(false)]
        public string Key { get; set; }        
    }

    public class DivisionBase : AppBaseStamp
    {
        //[Required]
        //public virtual Division Division { get; set; }
    }

    public class EntityDivisionBase : DivisionBase
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public string Description { get; set; }
    }
    public enum MachineTypes { Machine = 1, Furnace = 2 };
    public class Machine :EntityDivisionBase
    {
        [Required]
        public MachineTypes MachineType { get; set; }
        public double MinLoad { get; set; }
        public double MaxLoad { get; set; }
    }

    public class Shift : DivisionBase
    {
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public DateTime EndTime { get; set; }

        [NotMapped]
        public string StartTimeString { get { return StartTime.toTimeString(); } }
        [NotMapped]
        public string EndTimeString { get { return EndTime.toTimeString(); } }
        //public bool IsDayLight
        //{
        //    get
        //    {
        //        return EndTime.TimeOfDay > StartTime.TimeOfDay;
        //    }
        //}
        //public DateTime CurrentDay
        //{
        //    get
        //    {
        //        return IsDayLight ? (DateTime.Today.Date) : (DateTime.Now.TimeOfDay >= DateTime.Today.TimeOfDay ? DateTime.Now.AddDays(-1).Date : DateTime.Today.Date);
        //    }
        //}
        //public DateTime StartTimeOfTheDay
        //{
        //    get
        //    {
        //        return IsDayLight ? (DateTime.Now.Date + StartTime.TimeOfDay) : ((DateTime.Now.TimeOfDay <= EndTime.TimeOfDay) ? (DateTime.Now.AddDays(-1).Date + StartTime.TimeOfDay) : (DateTime.Now.Date + StartTime.TimeOfDay));
        //    }
        //}
        //public DateTime EndTimeOfTheDay
        //{
        //    get
        //    {
        //        return IsDayLight ? (DateTime.Now.Date + EndTime.TimeOfDay) : ((DateTime.Now.TimeOfDay <= EndTime.TimeOfDay) ? (DateTime.Now.Date + EndTime.TimeOfDay) : (DateTime.Now.AddDays(1).Date + EndTime.TimeOfDay));
        //    }
        //}
    }

    public enum AlertType { Conditional, Status };
    public enum RepeatType { DayWise, ShiftWise };
    public enum Expression { GreaterThan, Equal, LesserThan, NotEqual, GreaterThanOrEqual, LesserThanOrEqual };
    
    public class Alert :AppBaseStamp
    {     
        public virtual Machine Machines { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public AlertType AlertType { get; set; }
        public RepeatType RepeatType { get; set; }
        public string AlertProperty { get; set; }
        public double ThresholdValue { get; set; }
        public Expression Operation { get; set; }
        public int DailyAlertTime { get; set; }
        public virtual Shift Shift { get; set; }
    }

    public class ProductionSummary :AppBaseStamp
    {
        public virtual Machine Machine { get; set; }
        public virtual Shift Shift { get; set; }
        public int Day { get; set; }
        public int ProductionCount { get; set; }
        public int Downtime { get; set; }
        public int Rejected { get; set; }    
    }
}



