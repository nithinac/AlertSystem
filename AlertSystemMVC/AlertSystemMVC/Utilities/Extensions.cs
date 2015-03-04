using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace AlertSystemMVC.Utilities
{
    public static class Extensions
    {
        public static string toDateString(this DateTime dt)
        {
            return dt.Day + " " + dt.ToString("MMM") + ", " + dt.Year;
        }

        public static string toTimeString ( this DateTime dt)
        {
            return dt.Hour + ":" + dt.Minute + ":" + dt.Second;
        }
        public static void ForEach<T>(this IEnumerable<T> input, Action<T> exp)
        {
            foreach (var item in input)
            {
                exp(item);
            }
        }
        public static void ForEach<T>(this ICollection<T> input, Action<T> exp)
        {
            foreach (var item in input)
            {
                exp(item);
            }
        }
        /// <summary>
        /// For each.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input">The input.</param>
        /// <param name="exp">The exp.</param>
        public static void ForEach<T>(this IEnumerable<T> input, Action<T, int> exp)
        {
            int i = 0;
            foreach (var item in input)
            {
                exp(item, i++);
            }
        }
        /// <summary>
        /// For each.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input">The input.</param>
        /// <param name="exp">The exp.</param>
        public static void ForEach<T>(this ICollection<T> input, Action<T, int> exp)
        {
            int i = 0;
            foreach (var item in input)
            {
                exp(item, i++);
            }
        }

        public static object GetPropertyValue(this object value, string propertyName)
        {
            return value.GetType().GetProperties().Single(x => x.Name == propertyName).GetValue(value, null);
        }

        public static SelectListItem DefaultListValue(string DisplayName, bool Selected, string DefaultValue)
        {
            return new SelectListItem() { Selected = Selected, Text = DisplayName, Value = DefaultValue };
        }
        public static SelectList ToSelectList<T>(this IEnumerable<T> List, string ValueField, string DisplayName = "-Select-", string KeyField = "Key", object SelectedField = null, string DefaultValue = "-1")
        {
            var SelectList = new List<SelectListItem>();
            var defaultValue = DefaultListValue(DisplayName, SelectedField == null, DefaultValue);
            SelectList.Add(defaultValue);

            List.ForEach(x =>
            {
                SelectList.Add(new SelectListItem() { Text = (string)x.GetPropertyValue(ValueField), Value = (string)x.GetPropertyValue(KeyField) });
            });

            return new SelectList(SelectList, "Value", "Text", (SelectedField == null ? "-1" : SelectedField), new List<string>() { "-1" });
        }
    }


}