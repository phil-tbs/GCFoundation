using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Components.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class DateFormatAttribute : Attribute
    {
        public string Format { get; }

        public DateFormatAttribute(string format)
        {
            Format = format;
        }
    }
}
