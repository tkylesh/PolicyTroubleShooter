using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BTS.PolicyCycleAPICommon.Dto;
using BTS.PolicyCycleAPIDAL.Repository.Interface;
using BTS.PolicyCycleAPIDAL.DataAccess;
using log4net;
using System.Net;

namespace BTS.PolicyCycleAPIDAL.Repository.Impl
{
    public class PolicyCycleRepository : IPolicyCycleRepository
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ResponseDto GetPolicyCycleResponse(string quoteID, string companyNumber, string policyPrefix, int policyNumber, string transactionDate)

        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

            ResponseDto response = new ResponseDto();
            try
            {
                response = PolicyCycleDataAccess.GetResponseInformation(quoteID, companyNumber, policyPrefix, policyNumber, transactionDate);
            }
            catch (Exception ex)
            {
                log.Error($"Error processing policy {policyNumber.ToString()}: {ex.Message}  with PolicyCycleAPI", ex);
                throw;
            }
            return response;
        }

        public PolicyMasterDto GetPolicyMasterResponse(int policyNumber)

        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

            PolicyMasterDto response = new PolicyMasterDto();
            try
            {
                response = PolicyCycleDataAccess.GetPolicyMasterInformation(policyNumber);
            }
            catch (Exception ex)
            {
                log.Error($"Error processing policy {policyNumber.ToString()}: {ex.Message}  with PolicyCycleAPI", ex);
                throw;
            }
            return response;
        }


        public AutoActivation GetAutoActivationResponse(int policyNumber)
        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

            AutoActivation response = new AutoActivation();
            AutoActivate activate = new AutoActivate();
            try
            {
                response = activate.GetAutoActivationResponse(policyNumber);
            }
            catch (Exception ex)
            {
                log.Error($"Error auto activating policy {policyNumber.ToString()}: {ex.Message}  with PolicyCycleAPI", ex);
                throw;
            }
            return response;
        }


        public DMSResponseDto GetDMSResponse(int policyNumber, string quoteID, string activity)
        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

            DMSResponseDto response = new DMSResponseDto();
            try
            {
                response = PolicyCycleDataAccess.GetDMSResponseInformation(policyNumber, quoteID, activity);
            }
            catch (Exception ex)
            {
                log.Error($"Error writing file to DMS document server {policyNumber.ToString()}: {ex.Message}  with PolicyCycleAPI", ex);
                throw;
            }
            return response;
        }
    }
}