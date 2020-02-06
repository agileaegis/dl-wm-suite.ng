using System.Linq;
using System.Security.Claims;
using dl.wm.suite.cms.api.Redis.Models.VirtualEarths;
using dl.wm.suite.common.infrastructure.Exceptions.Controllers.Maps;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;

namespace dl.wm.suite.cms.api.Controllers.API.Base
{
    public abstract class BaseController : ControllerBase
    {
        protected string GetEmailFromClaims()
        {
            var claimsPrincipal = User as ClaimsPrincipal;
            var email = claimsPrincipal?.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")
                .Value;
            return email;
        }
    }
    public abstract class GeoBaseController : BaseController
    {
        protected string GetLocationByCoordinates(string bingMapsApiKey, double lat, double lng)
        {
            string url =
                $"http://dev.virtualearth.net/REST/v1/Locations/{lat},{lng}?o=json&c=el&key={bingMapsApiKey}";

            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);

            request.AddHeader("Content-Type", "application/json");

            var response = client.Execute(request);
            if (response.IsSuccessful)
            {
                var result = JsonConvert.DeserializeObject<VirtualEarth>(response.Content);
                return $"{result.resourceSets[0]?.resources.FirstOrDefault()?.address.addressLine} - " +
                       $"{result.resourceSets[0]?.resources.FirstOrDefault()?.address.postalCode}";
            }

            throw new GeolocationNotFound();
        }
    }
}