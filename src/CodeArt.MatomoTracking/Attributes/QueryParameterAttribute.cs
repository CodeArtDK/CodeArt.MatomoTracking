using System;
using System.Collections.Generic;
using System.Text;

namespace CodeArt.MatomoTracking.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class QueryParameterAttribute : Attribute
    {
        public string Name { get; set; }
        public QueryParameterAttribute(string name)
        {
            Name = name;
        }
    }
}
