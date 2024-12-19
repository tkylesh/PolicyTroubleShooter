using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using Newtonsoft.Json;
using log4net;
using BTS.PolicyCycleBusinessLayer.Services.Impl;
using BTS.PolicyCycleAPICommon.Dto;



namespace BTS.PolicyCycleAPI.Controllers
{
    // Allow CORS for all origins.(Caution!) will change when origins, headers and methods allowed are more defined
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("policycycle")]
    public class PolicyCycleController : ApiController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [Route("response")]  // policycycle/response?quoteid=11111111111,policynumber=123456789&effectivedate=6/6/2016&transactiontype
        public IHttpActionResult GetPolicyCycleResponse(string quoteID, int policyNumber, string effectiveDate)
        {
            try
            {
                var service = new PolicyCycleService();
                var response = new ResponseDto();

                //IdDescriptorPairDto dto = service.ValidateToken(token);
                //if (dto.Value != HttpStatusCode.OK.ToString())
                //    return new ActionResult(dto.Description, Request, HttpStatusCode.Unauthorized);

                response = service.GetPolicyCycleResponse(quoteID, policyNumber, effectiveDate);
                return Ok(response);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                return new ActionResult(ex.Message, Request, HttpStatusCode.BadRequest);
            }
        }

    }
}