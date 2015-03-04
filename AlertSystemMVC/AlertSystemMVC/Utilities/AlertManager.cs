using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using AlertSystemMVC.Models;
using Hangfire;
using System.Text;

namespace AlertSystemMVC.Utilities
{
    public class AlertManager
    {
        private static int ConvertToIST(int hour)
        {
            return hour - 5;
        }
        private static DateTime ConvertToIST(DateTime time)
        {
            DateTime alertTime = time;
            return alertTime.Subtract(new TimeSpan(5, 30, 00));        
        }
        public static void CreateAlert(Alert alert)
        {
            string CronExpression = null;
            if (alert.RepeatType == RepeatType.DayWise)
            {
                CronExpression = Cron.Daily(ConvertToIST(alert.DailyAlertTime));                
            }
            else
            {
                DateTime alertTime =ConvertToIST(alert.Shift.EndTime.AddMinutes(30));
                CronExpression = alertTime.Minute + " " + alertTime.Hour + " * * *";
            }

            RecurringJob.AddOrUpdate(alert.Name, () => RaiseAlert(alert), CronExpression);
        }

        public static void UpdateAlert(Alert alert)
        {
            CreateAlert(alert);
        }

        public static void DeleteAlert(Alert alert)
        {
            RecurringJob.RemoveIfExists(alert.Name);
        }
        public static void RaiseAlert(Alert alert)
        {
            using (AlertSystemMVCContext db = new AlertSystemMVCContext())
            {
                    List<ProductionSummary> productions;
                    if (alert.RepeatType == RepeatType.DayWise)
                    {
                        productions = db.ProductionSummaries.ToList();
                    }
                    else
                    {
                        productions = db.ProductionSummaries.Where(x => x.Machine.Key == alert.Machines.Key && x.Shift.Key == alert.Shift.Key && x.Day == DateTime.Today.Day).ToList();
                    }
                    StringBuilder builder = new StringBuilder();
                    builder.AppendLine(alert.Name+" triggered! \n\n");
                    bool trigger = false;
                    foreach (var production in productions)
                    {
                        if (alert.AlertType == AlertType.Conditional)
                        {
                            if (IsConditionSatisfied(production, alert.AlertProperty, alert.Operation, alert.ThresholdValue))
                            {
                                trigger = true;
                                var propertyValue = GetStatus(production, alert.AlertProperty);
                                builder.AppendFormat("{0} : {1} {2} {3} for Machine: {4} in Shift: {5} \n", alert.AlertProperty,propertyValue , alert.Operation, alert.ThresholdValue, alert.Machines.Code,production.Shift.Description);                                
                            }
                        }
                        else
                        {
                            trigger = true;
                            var propertyValue = GetStatus(production, alert.AlertProperty);
                            builder.AppendFormat("{0} = {1} in Shift: {2}", alert.AlertProperty, propertyValue,  production.Shift.Description);                            
                        }
                    }
                    if (trigger)
                        Mailer.SendMail("ms@iprings.com", "nithinnayagam@gmail.com", builder.ToString());
            }
        }

        private static bool IsConditionSatisfied(ProductionSummary obj, string property, Expression operation, double value)
        {
            var propertyValue = typeof(ProductionSummary).GetProperty(property).GetValue(obj);
            switch (operation)
            {
                case Expression.Equal:
                    {
                        return propertyValue.Equals(value);
                    }
                case Expression.GreaterThan:
                    {
                        return Convert.ToDouble(propertyValue) > value;
                    }
                case Expression.LesserThan:
                    {
                        return Convert.ToDouble(propertyValue) < value;
                    }
                case Expression.NotEqual:
                    {
                        return !propertyValue.Equals(value);
                    }
                case Expression.GreaterThanOrEqual:
                    {
                        return Convert.ToDouble(propertyValue) >= value;
                    }
                case Expression.LesserThanOrEqual:
                    {
                        return Convert.ToDouble(propertyValue) <= value;
                    }
                default:
                    return false;
            }
        }

        private static string GetStatus(ProductionSummary obj, string property)
        {
            return typeof(ProductionSummary).GetProperty(property).GetValue(obj).ToString();
        }

    }
}