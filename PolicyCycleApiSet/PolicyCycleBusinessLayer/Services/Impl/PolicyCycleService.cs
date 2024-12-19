using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using BTS.PolicyCycleBusinessLayer.Service.Interface;
using BTS.PolicyCycleAPICommon.Dto;
using BTS.PolicyCycleAPIDAL.Repository.Impl;
using System.Net;

namespace BTS.PolicyCycleBusinessLayer.Services.Impl
{
    public class PolicyCycleService : IPolicyCycleService
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ResponseDto GetPolicyCycleResponse (string quoteID, int policyNumber, string transactionDate)
        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

            ResponseDto response = new ResponseDto();
            ResponseDto activateResponse = new ResponseDto();
            PolicyMasterDto master = new PolicyMasterDto();
            AutoActivation activate = new AutoActivation();
            DMSResponseDto DMSResponse = new DMSResponseDto();
            var repo = new PolicyCycleRepository();

            master = repo.GetPolicyMasterResponse(policyNumber);

            if (!master.PolicyNumber.Equals(0))
            {
                if (master.PolicyStatus.Equals("E"))
                {
                    try
                    {
                        activate = repo.GetAutoActivationResponse(policyNumber);
                    }
                    catch (Exception ex)
                    {
                        log.Error($"Error activating policy {policyNumber.ToString()}:    {ex.Message} with PolicyCycleAPI", ex);
                    }
                }

                if ((activate.ActivationStatus != null && activate.ActivationStatus.Equals("Success")) || (!master.PolicyStatus.Equals("E") && !master.PolicyActivity.Equals("")))
                    {
                        activateResponse.AutoActivationStatus = activate.ActivationStatus;
                        var entdate = DateTime.Parse(transactionDate);
                        DateTime dt = new DateTime((int)(entdate.Year), (int)entdate.Month, (int)entdate.Day);
                        response = repo.GetPolicyCycleResponse(quoteID, master.CompanyNumber, master.PolicyPrefix, policyNumber, transactionDate);
                        response.AutoActivationStatus = activateResponse.AutoActivationStatus;
                        response.AutoActivationFailureMessage = activateResponse.AutoActivationFailureMessage;
                        if (response.PolicyIssuanceStatus.TrimEnd().Equals("Success"))
                        {
                            DMSResponse = repo.GetDMSResponse(policyNumber, quoteID, master.PolicyActivity);
                        }
                        else
                        {
                            response.PolicyIssuanceStatus = "Failure";
                            if(master.PolicyStatus.Equals("E")) response.PolicyIssuanceFailureMessage = "Policy failed to issue";
                            if (!master.PolicyStatus.Equals("E")) response.PolicyIssuanceFailureMessage = "Policy failed to process";
                        }
                    }
                    else
                    {
                        response.AutoActivationStatus = "Failure";
                        if (activate.ReasonForFailure != null)
                        {
                            response.AutoActivationFailureMessage = activate.ReasonForFailure[0].ReasonDesc;
                            log.Error($"Policy {policyNumber.ToString()} did not auto activate due to: {response.AutoActivationFailureMessage} in PolicyCycleAPI");
                         }
                         else
                         {
                            response.AutoActivationFailureMessage = "Error returned from auto activation api";
                            log.Error($"Policy {policyNumber.ToString()} did not auto activate due to: {response.AutoActivationFailureMessage} in PolicyCycleAPI");
                         }
                    }

              }

            return response;
        }
    }
}
