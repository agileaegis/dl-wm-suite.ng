using System;
using System.Threading.Tasks;
using dl.wm.suite.common.dtos.Vms.Points;
using Microsoft.AspNetCore.Mvc;

namespace dl.wm.suite.fleet.api.Controllers.API.V1
{
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = "Bearer")]
    public class PointsController : ControllerBase
    {

        /// <summary>
        /// Create TrackingPoint and check Geofences
        /// </summary>
        /// <param name="point">The tracking point details</param>
        /// <returns>Ok response</returns>
        // POST api/v1/points
        [HttpPost(Name = "PostPointRoute")]
        public async Task<IActionResult> PostPointAsync([FromBody]TrackingPointForCreationUiModel point)
        {
            return Ok();
        }

        /// <summary>
        /// Create multiple TrackingPoints and check Geofences
        /// </summary>
        /// <param name="points">List of TrackingPoint details</param>
        /// <returns>Ok response</returns>
        // POST api/v1/points/batch
        [HttpPost("batch", Name = "PostPointsRoute")]
        public async Task<IActionResult> PostPointsAsync([FromBody]TrackingPointForCreationUiModel[] points)
        {
            return Ok();
        }
    }
}
