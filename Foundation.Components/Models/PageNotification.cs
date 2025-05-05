using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation.Components.Enum;

namespace Foundation.Components.Models
{
    public class PageNotification
    {
        public required string Title { get; set; }

        public required string Message { get; set; }

        public AlertType AlertType { get; set; } = AlertType.Info;
    }
}
