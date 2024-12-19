using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BTS.PolicyCycleAPICommon.Dto
{
    public class AS400Parms
    {
        public string Server { get; set; }
        public string Database { get; set; }
        public string UserID { get; set; }
        public string Password { get; set; }
        public string DataQueueLibrary { get; set; }
        public string DataQueueIn { get; set; }
        public string DataQueueOut { get; set; }
    }
}