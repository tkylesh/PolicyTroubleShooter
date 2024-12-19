using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BTS.PolicyCycleAPICommon.Dto;

namespace BTS.PolicyCycleBusinessLayer.Service.Interface
{
    public interface IPolicyCycleService
    {
        ResponseDto GetPolicyCycleResponse(string quoteID, int policyNumber, string transactionDate);
    }
}