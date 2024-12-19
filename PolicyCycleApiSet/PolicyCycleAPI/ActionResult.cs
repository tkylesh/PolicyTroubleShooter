using System;
using System.Net;
using System.Web.Http;
using System.Net.Http;
using System.Threading.Tasks;

namespace BTS.PolicyCycleAPI
{
  public class ActionResult : IHttpActionResult
  {
    string _value;
    HttpRequestMessage _request;
    HttpStatusCode _statuscode;

    public ActionResult(string value, HttpRequestMessage request, HttpStatusCode statuscode = HttpStatusCode.OK)
    {
      _value = value;
      _request = request;
      _statuscode = statuscode;
    }

    public Task<HttpResponseMessage> ExecuteAsync(System.Threading.CancellationToken cancellationToken)
    {
      var response = new HttpResponseMessage()
      {
        Content = new StringContent(_value),
        RequestMessage = _request,
        StatusCode = _statuscode
      };
      return Task.FromResult(response);
    }
  }
}