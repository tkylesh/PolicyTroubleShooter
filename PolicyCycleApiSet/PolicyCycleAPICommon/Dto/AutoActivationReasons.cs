using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BTS.PolicyCycleAPICommon.Dto
{
    public class AutoActivationReasons
    {
        public string ReasonDesc { get; set; }
        public string CurrentValue { get; set; }
        public string PolicyNoteWritten { get; set; }
    }
}