using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notifications.Model
{
    public class EmailMessageRequest
    {
        public string EmailAdress { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }
}
