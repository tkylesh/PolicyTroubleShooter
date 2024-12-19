After installing, add the following reference and code in the WebApiConfig.cs file:

using BTS.ApiVersioning;

In the "Register" method, add the following code after creating an instance of a new JsonMediaTypeFormatter:

//Use custom ControllerSelector
config.Services.Replace(typeof(IHttpControllerSelector), new ApiVersioningSelector(config));

How to version a controller:
************************************************************
[RoutePrefix("rating/policytypes")]
public class PolicyTypesV2Controller : ApiController

*************************************************************
The version of controller to select will be set in the "Accept" header: Ex.: application/json;version=2

GET http://localhost.:52637/rating/policytypes/tn HTTP/1.1
Host: localhost.:52637
Accept: application/json;version=2

*************************************************************
If "Accept" header is not set, the controller selected will be the one without a version in its name. Ex.:PolicyTypesController