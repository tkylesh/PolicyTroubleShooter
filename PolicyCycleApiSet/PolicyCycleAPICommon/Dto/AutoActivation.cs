using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BTS.PolicyCycleAPICommon.Dto
{
    public class AutoActivation
    {
        public string ActivationStatus { get; set; }
        public string IsReadyToActivate { get; set; }
        public List<AutoActivationReasons> ReasonForFailure {get; set; }
    }
}