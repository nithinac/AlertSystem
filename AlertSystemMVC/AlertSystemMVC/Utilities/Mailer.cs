using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Postal;
using AlertSystemMVC.Models;
namespace AlertSystemMVC.Utilities
{
    public class Mailer
    {
        public static void SendMail(string from,string to,string content)
        {
            var email = new AlertEmail("AlertEmail");
            email.From = from;
            email.To = to;
            email.AlertContent = content;
            email.SendAsync();
        }
    }
}