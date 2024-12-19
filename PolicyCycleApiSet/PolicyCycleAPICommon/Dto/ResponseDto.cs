using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BTS.PolicyCycleAPICommon.Dto
{
    public class ResponseDto
    {
        public string AutoActivationStatus { get; set; }
        public string AutoActivationFailureMessage { get; set; }
        public string PolicyIssuanceStatus { get; set; }
        public string PolicyIssuanceFailureMessage { get; set; }
    }
}