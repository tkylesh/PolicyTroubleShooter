using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTS.PolicyCycleAPICommon.Dto;

namespace BTS.PolicyCycleAPIDAL.Repository.Interface
{
    public interface IPolicyCycleRepository
    {
        ResponseDto GetPolicyCycleResponse(string quoteID, string companyNumber, string policyPrefix, int policyNumber, string transactionDate);
        PolicyMasterDto GetPolicyMasterResponse(int policyNumber);
        AutoActivation GetAutoActivationResponse(int policyNumber);
        DMSResponseDto GetDMSResponse(int policyNumber, string QuoteID, string activity);
    }
}
