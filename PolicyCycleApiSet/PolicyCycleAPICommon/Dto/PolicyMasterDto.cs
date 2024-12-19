using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BTS.PolicyCycleAPICommon.Dto
{
    public class PolicyMasterDto
    {
        public string CompanyNumber { get; set; }
        public string PolicyPrefix { get; set; }
        public int PolicyNumber { get; set; }
        public string PolicyStatus { get; set; }
        public string PolicyActivity { get; set; }
    }
}