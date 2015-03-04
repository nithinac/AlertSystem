using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Postal;
namespace AlertSystemMVC.Models
{
    public class GenericEmail : Email
    {
        public GenericEmail(string viewName) : base(viewName) { }
        public string To { get; set; }
        public string From { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
    }
    public class AlertEmail :GenericEmail
    {
        public AlertEmail(string viewName) : base(viewName) { }
        public string AlertContent { get; set; }
    }
}